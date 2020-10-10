using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public abstract class PureSingleton<T> where T: PureSingleton<T>,new ()
{
    private static T instance;
    public static T Instance => instance ?? new T();

    public virtual void Init()
    {
    }
}
