using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
/// <summary>
/// 该类为view的代理类，负责管理各自的ui面板，完成各自面板的初始化等操作。
/// </summary>
/// <typeparam name="T"></typeparam>
public class UIProxy <T> : BaseUIProxy where T : UIBaseView
{
    public T view;
    
    /// <summary>
    /// 实例化已经加载到内存中的ui预制体
    /// </summary>
    /// <returns></returns>
    public  UIProxy<T> InitSet()
    {
        String assetName = typeof(T).Name;

        GameObject goUIAsset = UI.Instance.GetUiAsset(assetName);
        if (goUIAsset == null)
        {
            Debug.LogError("ui asset : " + assetName + " is not exist!");
        }
        Addressables.InstantiateAsync("Assets/UIPrefab/"+assetName+".prefab",UI.Instance.uiRoot).Completed += (handle) =>
        {
            handle.Result.name = goUIAsset.name;
            view = handle.Result.AddComponent<T>();
            view.proxy = this;
            
            view.gameObject.SetActive(false);
        };
        AssetInit();
        return this;
    }
    protected virtual void AssetInit() { }

    public sealed override void Show()
    {
        currentOpeningModule?.GetView().Hide();//关闭当前最后打开的一个ui面板
        openingModuleList.Add(this);
        if (view != null) view.Show();
    }

    public sealed override void Hide()
    {
        view.Hide();
        
        if (view is UIBaseView)
        {
            openingModuleList.Remove(this);
            currentOpeningModule?.GetView().Show();//显示当前最后打开的一个ui面板
        }
    }
    
    public sealed override UIBaseView GetView()
    {
        return view;
    }
}
