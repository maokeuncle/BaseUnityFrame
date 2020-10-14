using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// 用于管理所有的ui资源
/// </summary>
public class UI : MonoSingleton<UI>
{
    [HideInInspector] public Transform uiRoot;
    public bool loadFinshed;
    private Dictionary<string, GameObject> uiAssetDic = new Dictionary<string, GameObject>();

    public override void Init()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        uiAssetDic.Clear();
        transform.tag = "UI";
        uiRoot = gameObject.transform;
    }
    
    public void loadUI()
    {
        //需要将ui用到的物体在addressable中添加“ui”标签，方便统一加载
        AssetLabelReference assetLabelReference = new AssetLabelReference {labelString = "ui"};
        Addressables.LoadAssetsAsync<GameObject>(assetLabelReference,null ).Completed += OnResourceloadFinshed;
    }

    private void OnResourceloadFinshed(AsyncOperationHandle<IList<GameObject>> obj)
    {
        IList<GameObject> gos = obj.Result;
        foreach (GameObject go in gos)
        {
            Debug.Log($"loaded ui : {go.name}");
            uiAssetDic.Add(go.name, go);
        }
        loadFinshed = true;
    }
    public GameObject GetUiAsset(string uiAssetName)
    {
        if (!uiAssetDic.ContainsKey(uiAssetName))
        {
            Debug.Log($"{uiAssetName}：该资源不存在！");
            return null;
        }
		
        return uiAssetDic[uiAssetName];//注意：ui预制体需要与view脚本的名称保持一致
    }
}
