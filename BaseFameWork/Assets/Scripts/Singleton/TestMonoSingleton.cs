using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonoSingleton : MonoSingleton<TestMonoSingleton>
{
    public override void Init()
    {
        base.Init();
        print("this is a func from TestMonoSingleton!");
        TestSingleton.Instance.Init();
    }

    public void Test()
    {
        print("mono!");
    }
}
