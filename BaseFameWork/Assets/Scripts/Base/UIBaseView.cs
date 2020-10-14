using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI基类，主要负责每个ui面板的展示与关闭
/// </summary>
public class UIBaseView : MonoBehaviour
{
    public BaseUIProxy  proxy;

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
