using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

public class StaticUtilities 
{
    public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }

    public static string BuildListLabelText(string type, string propertyName)
    {
        return "@" + nameof(StaticUtilities) + "." + nameof(StaticUtilities.BuildStringList) + "<" + type + ">(" + propertyName + ")";
    }

    public const string BEFORE_TYPE = "@" + nameof(StaticUtilities) + "." + nameof(StaticUtilities.BuildStringList) + "<";
    public const string AFTER_TYPE = ">(";
    public const string END = ")";

    public static string BuildStringList<T>(List<T> list) where T : UnityEngine.Object
    {
        if (list == null || list.Count == 0)
        {
            return "EMPTY";
        }
        string returnValue = "";
        foreach (T listItem in list)
        {
            if (listItem == null)
            {
                returnValue += "null";
            }
            else if (returnValue == "")
            {
                returnValue = listItem.name;
            }
            else
            {
                returnValue += ";" + listItem.name;
            }
        }
        return returnValue;
    }

    public static List<UnityEngine.Object> FindAssetsByType(Type type)
    {
        List<UnityEngine.Object> assets = new List<UnityEngine.Object>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", type));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(assetPath, type);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }

    public static bool IsSubclassOf(Type type, Type toCheck)
    {
        bool isGeneric = type.IsGenericTypeDefinition;
        while (toCheck != null && toCheck != typeof(object))
        {
            if (isGeneric)
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (type == cur)
                {
                    return true;
                }
            }
            else if (type == toCheck)
            {
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }

    public static Type GetSublcassOf(Type type, Type toCheck)
    {
        bool isGeneric = type.IsGenericTypeDefinition;
        while (toCheck != null && toCheck != typeof(object))
        {
            if (isGeneric)
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (type == cur)
                {
                    return toCheck;
                }
            }
            else if (type == toCheck)
            {
                return toCheck;
            }
            toCheck = toCheck.BaseType;
        }
        return null;
    }
}
