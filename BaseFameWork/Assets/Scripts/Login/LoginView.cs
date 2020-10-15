using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;


public class LoginView : UIBaseView
{
    public Button loginBtn;
    public InputField user;
    public InputField password;
    public Text tip;
    private void Start()
    {
        loginBtn = transform.Find("loginBtn").GetComponent<Button>();
        user = transform.Find("user").GetComponent<InputField>();
        password = transform.Find("password").GetComponent<InputField>();
        tip = transform.Find("tip").GetComponent<Text>();
        loginBtn.onClick.AddListener(OnClick);
    }
    
    //单机版
    private void OnClick()
    {
        if (user.text == "admin" && password.text == "123456")
        {
            Addressables.LoadSceneAsync("TabelDemo");
            Hide();
        }
        else
        {
            tip.text = "用户名或密码错误！";
            StopCoroutine(SuperTool.Delay(1,() => tip.text = ""));
            StartCoroutine(SuperTool.Delay(1,() => tip.text = ""));
        }
    }
}
