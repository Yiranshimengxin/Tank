using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    //账号输入框
    private InputField inputId;
    //密码输入框
    private InputField inputPw;
    //登录按钮
    private Button btnLogin;
    //注册按钮
    private Button btnRegister;


    //初始化
    public override void OnInit()
    {
        skinPath = "LoginPanel";
        layer = PanelManager.Layer.Panel;
    }

    //显示
    public override void OnShow(params object[] para)
    {
        //寻找组件
        inputId = skin.transform.Find("InputId").GetComponent<InputField>();
        inputPw = skin.transform.Find("InputPw").GetComponent<InputField>();
        btnLogin = skin.transform.Find("BtnLogin").GetComponent<Button>();
        btnRegister = skin.transform.Find("BtnRegister").GetComponent<Button>();

        //寻找组件监听
        btnLogin.onClick.AddListener(OnLoginClick);
        btnRegister.onClick.AddListener(OnRegisterClick);

        //网络协议监听
        NetManager.AddMsgListener("MsgLogin", OnMsgLogin);
        //网络事件监听
        NetManager.AddEventListener(NetEvent.ConnectSuccess, OnConnectSuccess);
        NetManager.AddEventListener(NetEvent.ConnectFail, OnConnectFail);
        NetManager.Connect("127.0.0.1", 8888);
    }

    private void OnMsgLogin(MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin)msgBase;
        if (msg.result == 0)
        {
            Debug.Log("登录成功！");
            //设置id
            MainGame.id = msg.id;
            //进入游戏

            //添加坦克
            GameObject tankObj = new GameObject("MyTank");
            tankObj.transform.position = Vector3.zero + new Vector3(10, 1, 10);
            CtrlTank tank = tankObj.AddComponent<CtrlTank>();
            tank.Init("Tank1");
            //设置相机
            tankObj.AddComponent<CameraFollow>();
            //关闭界面
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("登录失败！");
        }
    }


    private void OnConnectFail(string err)
    {
        Debug.Log("失败");
    }

    private void OnConnectSuccess(string err)
    {
        Debug.Log("成功");
    }

    public override void OnClose()
    {
        //网络协议监听
        NetManager.RemoveMsgListener("MsgLogin", OnMsgLogin);
        //网络事件监听
        NetManager.RemoveEventListener(NetEvent.ConnectSuccess, OnConnectSuccess);
        NetManager.RemoveEventListener(NetEvent.ConnectFail, OnConnectFail);

    }

    //当按下登录按钮
    public void OnLoginClick()
    {
        //用户名密码为空
        if (inputId.text == "" || inputPw.text == "")
        {
            PanelManager.Open<TipPanel>("用户名和密码不能为空！");
            return;
        }
        //发送
        MsgLogin msgLogin = new MsgLogin();
        msgLogin.id = inputId.text;
        msgLogin.pw = inputPw.text;
        NetManager.Send(msgLogin);
    }

    //当按下注册按钮
    public void OnRegisterClick()
    {
        PanelManager.Open<RegisterPanel>();
    }
}
