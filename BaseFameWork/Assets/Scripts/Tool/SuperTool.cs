using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;
using System.Globalization;

public static partial class SuperTool 
{
    private static List<string> tagList = new List<string>();

    #region TagRelated
    public static  void InitTags()
    {
        InitAddTag();
        SerializedObject tagManager= new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        tagsProp.ClearArray();
        tagManager.ApplyModifiedProperties();
        for (int i = 0; i < tagList.Count; i++)
        {
            tagsProp.InsertArrayElementAtIndex(i);
            SerializedProperty sp = tagsProp.GetArrayElementAtIndex(i);
            sp.stringValue = tagList[i];
            tagManager.ApplyModifiedProperties();
        }
    }

    public static void AddTag(string tagName)
    {
        if (!tagList.Contains(tagName))
        {
            tagList.Add(tagName);
            RefreshTags();
        }
        else
        {
            Debug.Log("该标签已经存在！");
        }
    }
    private static void InitAddTag()
    {
        tagList.Add("UI");
    }

    private static void RefreshTags()
    {
        InitTags();
    }
    

    #endregion

    public static string GetNowTime()
    {
        return DateTime.Now.ToString(CultureInfo.CurrentCulture);
    }
    
    public static IEnumerator Delay(float time,Action action = null)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }

}
