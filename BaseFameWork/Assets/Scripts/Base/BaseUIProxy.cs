using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// uiProxy的基类，负责记录系统当前最新的ui面板
/// </summary>
public abstract class BaseUIProxy
{
    public static List<BaseUIProxy> openingModuleList = new List<BaseUIProxy>();
    public static BaseUIProxy currentOpeningModule
    {
        get
        {
            if (openingModuleList.Count <= 0)
                return null;

            return openingModuleList[openingModuleList.Count - 1];
        }
    }
    
    public static void Clear()
    {
        while (currentOpeningModule != null)
        {
            currentOpeningModule.GetView().Hide();
            openingModuleList.Remove(currentOpeningModule);
        }
    }
    
    public abstract void Show();
    public abstract void Hide();
    public abstract UIBaseView GetView();
}

