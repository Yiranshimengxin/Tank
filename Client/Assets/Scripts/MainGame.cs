using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    public static string id = "";

    void Start()
    {
        //初始化
        PanelManager.Init();
        //打开登录面板
        PanelManager.Open<LoginPanel>();
        //网络监听
        NetManager.AddEventListener(NetEvent.Close, OnConnectClose);
        NetManager.AddMsgListener("MsgKick", OnMsgKick);
    }

    private void OnConnectClose(string err)
    {
        Debug.Log("断开连接！");
    }

    private void OnMsgKick(MsgBase msgBase)
    {
        PanelManager.Open<TipPanel>("被踢下线！");
    }


    void Update()
    {
        NetManager.Update();
    }
}
