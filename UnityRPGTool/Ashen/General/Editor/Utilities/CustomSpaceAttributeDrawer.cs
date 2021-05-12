using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace EditorUtilities
{
    [DrawerPriority(DrawerPriorityLevel.SuperPriority)]
    public class CustomSpaceAttributeDrawer : OdinAttributeDrawer<CustomSpaceAttribute>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (this.Attribute.spaceBefore)
            {
                EditorGUILayout.Space();
            }
            CallNextDrawer(label);
            if (this.Attribute.spaceAfter)
            {
                EditorGUILayout.Space();
            }
        }
    }
}