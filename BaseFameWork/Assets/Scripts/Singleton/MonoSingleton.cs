using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T:MonoSingleton<T>
{
    private static T instance;

    public static T Instance => instance;

    private void Awake()
    {
        instance = this as T;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        
    }

}
