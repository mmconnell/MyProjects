﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using System;
using Ashen.DeliverySystem;

namespace Ashen.EquationSystem
{
    public class Equation : I_Equation, I_Cacheable
    {
        //public static readonly Dictionary<string, I_EquationArgument> EMPTY_EQUATION_ARGUMENTS = new Dictionary<string, I_EquationArgument>();

        [ShowInInspector, PropertyOrder(0), DelayedProperty, HideLabel]
        public string equation
        {
            get
            {
                return ToString();
            }
            set
            {
                BuildFromString(value);
            }
        }

        [HideInInspector]
        public bool testData;

        private bool notTestData
        {
            get
            {
                return !testData;
            }
        }

        [OdinSerialize, HideInInspector]
        private bool validEquation;

        public bool ValidEquation
        {
            get
            {
                return validEquation;
            }
        }
        [OdinSerialize, HideInInspector]
        private string reason;

        public string Reason
        {
            get
            {
                return reason;
            }
        }

        [OdinSerialize, HideInInspector]
        private string parseReason;

        public string ParseReason
        {
            get
            {
                return parseReason;
            }
        }

        [PropertyOrder(1), ShowIf("testData")]
        public List<I_EquationComponent> equationComponents;

        [HideInInspector]
        public bool newCalculation;
        [HideInInspector]
        public bool keepGoing;
        [HideInInspector]
        public int currentIndex;

        public Equation()
        {
            currentIndex = 0;
            newCalculation = true;
            keepGoing = true;
            calculated = new HashSet<I_DeliveryTool>();
        }
        [ShowInInspector, ReadOnly, ShowIf("testData")]
        bool requiresSource;
        [ReadOnly, ShowIf("testData")]
        public bool canBeCached;
        [NonSerialized, ShowInInspector, ShowIf("testData")]
        bool evaluated = false;

        [NonSerialized, ShowInInspector, ShowIf("testData")]
        float? lastCalculation;

        private Dictionary<I_DeliveryTool, List<InvalidationPack>> invalidationListeners;
        private HashSet<I_DeliveryTool> calculated;

        [NonSerialized, ShowInInspector, ReadOnly, ShowIf("testData")]
        private Dictionary<I_DeliveryTool, float> sources = new Dictionary<I_DeliveryTool, float>();
        private Dictionary<I_DeliveryTool, float> Sources
        {
            get
            {
                if (sources == null)
                {
                    sources = new Dictionary<I_DeliveryTool, float>();
                }
                return sources;
            }
        }

        private bool IsCached(I_DeliveryTool source)
        {
            if (!evaluated)
            {
                return false;
            }
            if (!canBeCached)
            {
                return false;
            }
            if (!requiresSource)
            {
                if (lastCalculation == null)
                {
                    return false;
                }
                return true;
            }
            return Sources.ContainsKey(source);
        }

        private void Evaluate()
        {
            for (int x = 0; x < equationComponents.Count; x++)
            {
                I_EquationComponent component = equationComponents[x];
                if (!component.IsCachable())
                {
                    canBeCached = false;
                    evaluated = true;
                    return;
                }
                if (component.RequiresCaching())
                {
                    requiresSource = true;
                }
            }
            canBeCached = true;
            evaluated = true;
        }

        public void Recalculate(I_DeliveryTool toolManager, EquationArgumentPack extraArguments)
        {
            Sources.Remove(toolManager);
            Calculate(toolManager, extraArguments);
            AlertListeners(toolManager);
        }

        private float Calculate(I_DeliveryTool source, I_DeliveryTool target, bool firstRun, EquationArgumentPack extraArguments)
        {
            if (IsCached(source))
            {
                if (requiresSource)
                {
                    return Sources[source];
                }
                if (lastCalculation != null)
                {
                    return (float)lastCalculation;
                }
            }
            if (equationComponents == null)
            {
                equationComponents = new List<I_EquationComponent>();
            }
            if (!evaluated)
            {
                Evaluate();
            }
            if (equationComponents.Count == 0)
            {
                return 0;
            }
            if (!validEquation)
            {
                return 0;
            }
            currentIndex = 0;
            float result = InnerCalculate(source, target, extraArguments);
            if (canBeCached)
            {
                if (requiresSource)
                {
                    if (Sources.ContainsKey(source))
                    {
                        Sources[source] = result;
                    }
                    else
                    {
                        Sources.Add(source, result);
                    }
                    for (int x = 0; x < equationComponents.Count; x++)
                    {
                        I_EquationComponent component = equationComponents[x];
                        if (firstRun)
                        {
                            component.Cache(source, this);
                        }
                    }
                }
                else
                {
                    lastCalculation = result;
                }
            }
            firstRun = false;
            return result;
        }

        public float Calculate(I_DeliveryTool source)
        {
            DeliveryArgumentPacks deliveryArguments = PoolManager.Instance.deliveryArgumentsPool.GetObject();
            EquationArgumentPack equationArguments = deliveryArguments.GetPack<EquationArgumentPack>();
            float result = Calculate(source, equationArguments);
            deliveryArguments.Disable();
            return result;
        }

        public float Calculate(I_DeliveryTool source, EquationArgumentPack extraArguments)
        {
            if (IsCalculated(source))
            {
                return Calculate(source, source, false, extraArguments);
            }
            calculated.Add(source);
            return Calculate(source, source, true, extraArguments);
        }

        public float Calculate(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack extraArguments)
        {
            if (IsCalculated(source))
            {
                return Calculate(source, source, false, extraArguments);
            }
            calculated.Add(source);
            return Calculate(source, source, true, extraArguments);
        }

        private bool IsCalculated(I_DeliveryTool source)
        {
            if (calculated == null)
            {
                calculated = new HashSet<I_DeliveryTool>();
                return false;
            }
            return calculated.Contains(source);
        }

        public float Calculate(I_DeliveryTool source, I_DeliveryTool target, int currentIndex, EquationArgumentPack extraArguments)
        {
            if (equationComponents == null)
            {
                equationComponents = new List<I_EquationComponent>();
            }
            if (equationComponents.Count == 0)
            {
                return 0;
            }
            this.currentIndex = currentIndex;
            return InnerCalculate(source, target, extraArguments);
        }

        private float InnerCalculate(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack extraArguments)
        {
            keepGoing = true;
            float total = 0;
            if (currentIndex >= equationComponents.Count || currentIndex < 0)
            {
                return total;
            }
            while (keepGoing)
            {
                I_EquationComponent component = equationComponents[currentIndex];
                total = component.Calculate(this, source, target, total, extraArguments);
                if (keepGoing)
                {
                    currentIndex++;
                }
                if (currentIndex >= equationComponents.Count)
                {
                    keepGoing = false;
                }
            }
            return total;
        }

        public void AddInvalidationListener(I_DeliveryTool toolManager, I_InvalidationListener listener, string key)
        {
            if (invalidationListeners == null)
            {
                invalidationListeners = new Dictionary<I_DeliveryTool, List<InvalidationPack>>();
            }
            if (invalidationListeners.TryGetValue(toolManager, out List<InvalidationPack> listeners))
            {
                listeners.Add(new InvalidationPack
                {
                    invalidationListener = listener,
                    key = key
                });
            }
            else
            {
                listeners = new List<InvalidationPack>
                {
                    new InvalidationPack
                    {
                        invalidationListener = listener,
                        key = key
                    }
                };
                invalidationListeners.Add(toolManager, listeners);
            }
        }

        private void AlertListeners(I_DeliveryTool toolManager)
        {
            if (invalidationListeners != null)
            {
                if (invalidationListeners.TryGetValue(toolManager, out List<InvalidationPack> listeners))
                {
                    for (int x = 0; x < listeners.Count; x++)
                    {
                        listeners[x].invalidationListener.Invalidate(toolManager, listeners[x].key);
                    }
                }
            }
        }

        public void BuildFromString(string equation)
        {
            reason = null;
            List<I_EquationComponent> components = EquationParser.Instance.ParseEquationString(equation, out List<string> errorReasons);
            if (components == null || errorReasons.Count > 0)
            {
                if (errorReasons.Count == 0)
                {
                    parseReason = "Unknown Error";
                }
                else
                {
                    bool first = true;
                    foreach (string errorReason in errorReasons)
                    {
                        if (first)
                        {
                            parseReason = errorReason;
                            first = false;
                        }
                        else
                        {
                            parseReason += "\n" + errorReason;
                        }
                    }
                }
                validEquation = false;
                equationComponents = components;
            }
            else
            {
                equationComponents = components;
                VerifyEquation();
            }
        }

        public void VerifyEquation()
        {
            parseReason = null;
            if (equationComponents == null)
            {
                reason = "Empty Equation";
                validEquation = false;
                return;
            }
            if (VerifyEquation(0, false) == -1)
            {
                return;
            }
            validEquation = true;
        }

        private int VerifyArgumentOperation(int start)
        {
            I_EquationComponent component = equationComponents[start];
            A_ArgumentOperation operation = component as A_ArgumentOperation;
            int argCount = 0;
            int result = start;
            while (component != (I_EquationComponent)Operations.Instance.CLOSEPARAM)
            {
                argCount++;
                result = VerifyEquation(result + 1, true);
                if (result == -1)
                {
                    return -1;
                }
                component = equationComponents[result];
            }
            if (operation.minimumArguments > 0 && argCount < operation.minimumArguments)
            {
                reason = "Too few arguments in operation at " + start;
                validEquation = false;
                return -1;
            }
            if (operation.maximumArguments > 0 && argCount > operation.maximumArguments)
            {
                reason = "Too many arguments in operation at " + start;
                validEquation = false;
                return -1;
            }
            return result;
        }

        private int VerifyEquation(int start, bool isArgument)
        {
            int openParam = 0;
            bool lastIsValue = false;
            bool lastIsClose = false;
            bool lastIsBasicOperation = false;
            bool lastIsArgumentOperation = false;
            for (int x = start; x < equationComponents.Count; x++)
            {
                I_EquationComponent component = equationComponents[x];
                if (component == null)
                {
                    reason = "Null component: " + x;
                    validEquation = false;
                    return -1;
                }
                if (component.InvalidComponent())
                {
                    reason = "Component is invalid: " + x;
                    validEquation = false;
                    return -1;
                }
                if (!component.IsOperation())
                {
                    if (lastIsValue)
                    {
                        reason = "Double value: " + x;
                        validEquation = false;
                        return -1;
                    }
                    if (lastIsClose)
                    {
                        reason = "Close param followed by invalid token: " + x;
                        validEquation = false;
                        return -1;
                    }
                    if (lastIsArgumentOperation)
                    {
                        reason = "Argument operation followed by invalid token: " + x;
                        validEquation = false;
                        return -1;
                    }
                    lastIsValue = true;
                    lastIsBasicOperation = false;
                }
                else
                {
                    if (component.IsArgumentOperation())
                    {
                        if (lastIsValue)
                        {
                            reason = "Argument following value: " + x;
                            validEquation = false;
                            return -1;
                        }
                        if (lastIsClose)
                        {
                            reason = "Argument following closing param: " + x;
                            validEquation = false;
                            return -1;
                        }
                        if (lastIsArgumentOperation)
                        {
                            reason = "Double argument operation: " + x;
                            validEquation = false;
                            return -1;
                        }
                        x = VerifyArgumentOperation(x);
                        if (x == -1)
                        {
                            return x;
                        }
                        lastIsBasicOperation = false;
                        lastIsArgumentOperation = true;
                    }
                    else
                    {
                        if (component == (I_EquationComponent)Operations.Instance.OPENPARAM)
                        {
                            if (lastIsValue)
                            {
                                reason = "Value followed by open param without an operation: " + x;
                                validEquation = false;
                                return -1;
                            }
                            if (lastIsClose)
                            {
                                reason = "Close param followed by invalid token: " + x;
                                validEquation = false;
                                return -1;
                            }
                            lastIsArgumentOperation = false;
                            lastIsBasicOperation = false;
                            openParam++;
                        }
                        else if (component == (I_EquationComponent)Operations.Instance.CLOSEPARAM)
                        {
                            if (openParam == 0)
                            {
                                if (isArgument)
                                {
                                    return x;
                                }
                                reason = "Extra closing param found: " + x;
                                validEquation = false;
                                return -1;
                            }
                            if (!lastIsValue)
                            {
                                reason = "Close param following an operation: " + x;
                                validEquation = false;
                                return -1;
                            }
                            openParam--;
                            lastIsValue = false;
                            lastIsClose = true;
                        }
                        else if (component == (I_EquationComponent)Operations.Instance.ARGUMENT_SEPARATOR)
                        {
                            if (openParam != 0)
                            {
                                reason = "Open param with no closing param: " + x;
                                validEquation = false;
                                return -1;
                            }
                            if (isArgument)
                            {
                                return x;
                            }
                            else
                            {
                                reason = component.Representation() + " outside an argument: " + x;
                                validEquation = false;
                                return -1;
                            }
                        }
                        else
                        {
                            if (!lastIsValue && !lastIsClose)
                            {
                                if (component != (I_EquationComponent)Operations.Instance.SUBTRACT)
                                {
                                    reason = "Operation following another operation: " + x;
                                    validEquation = false;
                                    return -1;
                                }
                            }
                            lastIsClose = false;
                            lastIsValue = false;
                            lastIsBasicOperation = true;
                        }
                    }
                }
            }
            if (openParam != 0)
            {
                reason = "Open param with no closing param";
                validEquation = false;
                return -1;
            }
            if (lastIsBasicOperation || lastIsArgumentOperation)
            {
                reason = "Equation ended on invalid component: " + (equationComponents.Count - 1);
                validEquation = false;
                return -1;
            }
            return 0;
        }

        public override string ToString()
        {
            string value = "";
            if (equationComponents == null)
            {
                return "null";
            }
            foreach (I_EquationComponent component in equationComponents)
            {
                if (component != null)
                {
                    value += component.Representation();
                }
            }
            return value;
        }

        public float GetLow(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack extraArguments)
        {
            return Calculate(source, target, extraArguments);
        }

        public float GetHigh(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack extraArguments)
        {
            return Calculate(source, target, extraArguments);
        }

        public I_Equation Rebuild(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack equationArgumentPack)
        {
            Equation equation = new Equation
            {
                equationComponents = new List<I_EquationComponent>()
            };
            foreach (I_EquationComponent component in equationComponents)
            {
                equation.equationComponents.Add(component.Rebuild(source, target, equationArgumentPack));
            }
            equation.VerifyEquation();
            return equation;
        }

        public bool RequiresRebuild(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack equationArgumentPack)
        {
            if (equationComponents == null)
            {
                return false;
            }
            foreach (I_EquationComponent component in equationComponents)
            {
                if (component.RequiresRebuild())
                {
                    return true;
                }
            }
            return false;
        }
    }
}