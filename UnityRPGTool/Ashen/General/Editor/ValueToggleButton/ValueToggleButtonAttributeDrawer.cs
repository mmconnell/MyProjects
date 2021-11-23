using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Linq;
using System.Collections;
using Sirenix.OdinInspector.Editor.ValueResolvers;

public class ValueToggleButtonAttributeDrawer<T> : OdinAttributeDrawer<ValueToggleButtonAttribute, T>
{
    private ValueResolver<object> rawGetter;
    private string error;
    private Func<IEnumerable<ValueDropdownItem<T>>> getValues;
    private int initial = 0;

    private Context context;

    private class Context
    {

        public GUIContent[] Names;
        public T[] Values;
        public float[] NameSizes;
        public List<int> ColumnCounts;
        public float PreviousControlRectWidth;
    }

    protected override void Initialize()
    {
        initial = 0;
        this.rawGetter = ValueResolver.Get<object>(this.Property, this.Attribute.MemberName);
        this.error = this.rawGetter.ErrorMessage;
        this.getValues = () =>
        {
            object value = this.rawGetter.GetValue();

            return value == null ? null : (value as IEnumerable)
                .Cast<object>()
                .Where(x => x != null)
                .Select(x =>
                {
                    if (x is ValueDropdownItem<T>)
                    {
                        return (ValueDropdownItem<T>)x;
                    }

                    if (x is IValueDropdownItem)
                    {
                        var ix = x as IValueDropdownItem;
                        return new ValueDropdownItem<T>(ix.GetText(), (T)ix.GetValue());
                    }

                    if (x is UnityEngine.Object)
                    {
                        UnityEngine.Object ix = x as UnityEngine.Object;
                        return new ValueDropdownItem<T>(ix.name, (T)x);
                    }
                        
                    return new ValueDropdownItem<T>(x.ToString(), (T)x);
                });
        };
    }

    private Context BuildContext()
    {
        IEnumerable<ValueDropdownItem<T>> valueDropdownItems = this.getValues();
        List<string> enumNames = new List<string>();
        context = new Context();
        context.Values = new T[valueDropdownItems.Count()];
        context.Names = new GUIContent[valueDropdownItems.Count()];
        int i = 0;
        foreach (ValueDropdownItem<T> item in valueDropdownItems)
        {
            enumNames.Add(item.Text);
            context.Values[i] = item.Value;
            context.Names[i] = new GUIContent(item.Text);
            i++;
        }

        // Calculate the default sizes for each button
        context.NameSizes = new float[context.Names.Length];
        for (int x = 0; x < context.NameSizes.Length; x++) 
        {
            GUIContent content = context.Names[x];
            context.NameSizes[x] = SirenixGUIStyles.MiniButtonMid.CalcSize(content).x + 3;
        }

        // Assume there is one row with the number of options in columns
        context.ColumnCounts = new List<int>() { context.NameSizes.Length };
        return context;
    }

    protected override void DrawPropertyLayout(GUIContent label)
    {
        if (initial < 2)
        {
            initial++;
            base.CallNextDrawer(label);
            return;
        }
            
        IPropertyValueEntry<T> entry = this.ValueEntry;

        Type t = entry.WeakValues[0].GetType();

        if (Event.current.type == EventType.Layout)
        {
            if (ShouldResetContext(context))
            {
                context = BuildContext();
                GUIHelper.RequestRepaint();
            }
        }

        if (context == null)
        {
            context = BuildContext();
            GUIHelper.RequestRepaint();
        }

        T value = entry.SmartValue;

        Rect controlRect = new Rect();
            
        for (int j = 0, i = 0; j < context.ColumnCounts.Count; j++)
        {
            SirenixEditorGUI.GetFeatureRichControlRect(j == 0 ? label : GUIContent.none, out int id, out bool hasFocus, out Rect rect);
                
            if (j == 0)
            {
                controlRect = rect;
            }
            else
            {
                rect.xMin = controlRect.xMin;
            }

            float xMax = rect.xMax;
            rect.width /= context.ColumnCounts[j];
            rect.width = (int)rect.width;
            int from = i;
            int to = i + context.ColumnCounts[j];
            for (; i < to; i++)
            {
                bool selected;
                    
                selected = context.Values[i].Equals(value);

                GUIStyle style;
                Rect btnRect = rect;
                if (i == from && i == to - 1)
                {
                    style = selected ? SirenixGUIStyles.MiniButtonSelected : SirenixGUIStyles.MiniButton;
                    btnRect.x -= 1;
                    btnRect.xMax = xMax + 1;
                }
                else if (i == from)
                {
                    style = selected ? SirenixGUIStyles.MiniButtonLeftSelected : SirenixGUIStyles.MiniButtonLeft;
                }
                else if (i == to - 1)
                {
                    style = selected ? SirenixGUIStyles.MiniButtonRightSelected : SirenixGUIStyles.MiniButtonRight;
                    btnRect.xMax = xMax;
                }
                else
                {
                    style = selected ? SirenixGUIStyles.MiniButtonMidSelected : SirenixGUIStyles.MiniButtonMid;
                }

                if (GUI.Button(btnRect, context.Names[i], style))
                {
                    GUIHelper.RemoveFocusControl();
                        
                    entry.WeakSmartValue = context.Values[i];

                    GUIHelper.RequestRepaint();
                }

                rect.x += rect.width;
            }
        }

        if (Event.current.type == EventType.Repaint && context.PreviousControlRectWidth != controlRect.width)
        {
            context.PreviousControlRectWidth = controlRect.width;

            float maxBtnWidth = 0;
            int row = 0;
            context.ColumnCounts.Clear();
            context.ColumnCounts.Add(0);
            for (int i = 0; i < context.NameSizes.Length; i++)
            {
                float btnWidth = context.NameSizes[i];
                context.ColumnCounts[row]++;
                int columnCount = context.ColumnCounts[row];
                float columnWidth = controlRect.width / columnCount;

                maxBtnWidth = Mathf.Max(btnWidth, maxBtnWidth);

                if (maxBtnWidth > columnWidth && columnCount > 1)
                {
                    context.ColumnCounts[row]--;
                    context.ColumnCounts.Add(1);
                    row++;
                    maxBtnWidth = btnWidth;
                }
            }
        }
    }

    private bool ShouldResetContext(Context value)
    {
        if (value == null)
        {
            return true;
        }
        IEnumerable<ValueDropdownItem<T>> valueDropdownItems = this.getValues();
        int count = 0;
        foreach (ValueDropdownItem<T> item in valueDropdownItems)
        {
            if (value.Values.Length <= count || !value.Values[count].Equals(item.Value))
            {
                return true;
            }
            count++;
        }
        if (count != value.Names.Length)
        {
            return true;
        }
        return false;
    }
}