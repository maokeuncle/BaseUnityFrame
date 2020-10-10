using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingleton: PureSingleton<TestSingleton>
{
    public override void Init()
    {
        base.Init();
        Debug.Log("this is a func from TestSingleton!");
        TestMonoSingleton.Instance.Test();
    }
}
