using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    public InputField idInput;
    public InputField pwInput;
    public InputField textInput;

    public GameObject tankHead;

    void Start()
    {
        PanelManager.Init();
        PanelManager.Open<LoginPanel>();

        //GameObject tankObj2 = new GameObject("EnemyTank");
        //tankObj2.transform.position = Vector3.zero + new Vector3(20, 1, 20);
        //BaseTank tank2 = tankObj2.AddComponent<BaseTank>();
        //tank2.Init("Tank1");

        NetManager.AddMsgListener("MsgRegister", OnMsgRegister);
        NetManager.AddMsgListener("MsgKick", OnMsgKick);
        NetManager.AddMsgListener("MsgGetText", OnMsgGetText);
        NetManager.AddMsgListener("MsgSaveText", OnMsgSaveText);
    }

    //发送注册协议
    public void OnRegisterClick()
    {
        print("注册按钮");
        MsgRegister msg = new MsgRegister();
        msg.id = idInput.text;
        msg.pw = pwInput.text;
        NetManager.Send(msg);
    }

    //收到注册协议
    public void OnMsgRegister(MsgBase msgBase)
    {
        MsgRegister msg = (MsgRegister)msgBase;
        if (msg.result == 0)
        {
            Debug.Log("注册成功！");
        }
        else
        {
            Debug.Log("注册失败");
        }
    }

    //发送登录协议
    public void OnLoginClick()
    {
        MsgLogin msg = new MsgLogin();
        msg.id = idInput.text;
        msg.pw = pwInput.text;
        NetManager.Send(msg);
    }

    //收到登录协议
    private void OnMsgLogin(MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin)msgBase;
        if (msg.result == 0)
        {
            Debug.Log("登录成功！");
            //请求记事本文本
            MsgGetText msgGetText = new MsgGetText();
            NetManager.Send(msgGetText);
        }
        else
        {
            Debug.Log("登录失败！");
        }
    }

    private void OnMsgKick(MsgBase msgBase)
    {
        Debug.Log("被踢下线！");
    }

    public void OnMsgGetText(MsgBase msgBase)
    {
        MsgGetText msg = new MsgGetText();
        textInput.text = msg.text;
    }

    public void OnSaveClick()
    {
        MsgSaveText msg = new MsgSaveText();
        msg.text = textInput.text;
        NetManager.Send(msg);
    }

    public void OnMsgSaveText(MsgBase msgBase)
    {
        MsgSaveText msg = new MsgSaveText();
        if (msg.result == 0)
        {
            Debug.Log("保存成功！");
        }
        else
        {
            Debug.Log("保存失败！");
        }
    }

    void Update()
    {
        NetManager.Update();
    }

    public void OnSendMove()
    {
        MsgMove msg = new MsgMove();
        msg.x = 100;
        NetManager.Send(msg);
    }

    void OnMove(MsgBase msg)
    {
        print("Move");
    }
}
