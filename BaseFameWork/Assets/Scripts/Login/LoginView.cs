using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            tip.text = "登录成功！";
            StartCoroutine(SuperTool.Delay(1,() => tip.text = ""));   
        }
        else
        {
            tip.text = "用户名或密码错误！";
            StartCoroutine(SuperTool.Delay(1,() => tip.text = ""));
        }
    }
}
