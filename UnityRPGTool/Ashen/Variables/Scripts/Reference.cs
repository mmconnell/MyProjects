using EditorUtilities;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ashen.VariableSystem
{
    [Serializable, InlineProperty, HideReferenceObjectPicker, AutoPopulate]
    public class Reference<T>
    {
        [HideInInspector]
        public ReferenceType usingConstant;
        [ShowIf(nameof(usingConstant), ReferenceType.UseObject), CustomContextMenu("Use Constant", nameof(Switch))]
        [OdinSerialize, HideWithContext]
        public A_Variable<T> variable;
        [ShowIf(nameof(usingConstant), ReferenceType.UseConstant), CustomContextMenu("Use Object", nameof(Switch))]
        [OdinSerialize, HideWithContext, CustomSpace(spaceAfter = true)]
        public T constant;

        public T Value
        {
            get
            {
                return (usingConstant == ReferenceType.UseConstant || variable == null) ? constant : variable.value;
            }
        }

        public override string ToString()
        {
            if (Value != null)
            {
                return Value.ToString();
            }
            return "null";
        }

        private void Switch()
        {
            switch (usingConstant)
            {
                case ReferenceType.UseConstant:
                    usingConstant = ReferenceType.UseObject;
                    break;
                case ReferenceType.UseObject:
                    usingConstant = ReferenceType.UseConstant;
                    break;
            }
        }
    }

    public enum ReferenceType
    {
        UseConstant, UseObject
    }
}