using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CustomEventManager.AddListener("OnPress_A",OnPressA);
        CustomEventManager.AddListener("OnPress_A",OnPressA1);
        CustomEventManager.AddListener("OnPress_A",OnPressA2);
        CustomEventManager.RemoveListener("OnPress_A",OnPressA1);
    }

    private void OnPressA2(CusEventArgs e)
    {
        /***********
         Do Something
         ************/
        print("第三个监听者执行了");
    }
    private void OnPressA1(CusEventArgs e)
    {
        print("第二个监听者执行了");
    }

    private void OnPressA(CusEventArgs e)
    {
        print("第一个监听者执行了");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            CustomEventManager.DispatchEvent(new CusEventArgs("OnPress_A"));
    }
}
