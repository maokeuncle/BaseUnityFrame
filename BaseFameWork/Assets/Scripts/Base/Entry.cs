using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SuperTool.InitTags();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadUI());
    }

    IEnumerator LoadUI()
    {
        UI.Instance.loadUI();//首先执行loadUI，先把ui资源加载进来
        while (true)
        {
            yield return 0;
            if (UI.Instance.loadFinshed)
                break;
        }
        Proxy.InitAsset();//实例化已经加载到内存中的ui资源
        
        StartCoroutine(SuperTool.Delay(2,() => Proxy.LoginProxy.Show()));  
    }
}
