using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel
{
    //账号输入框
    private InputField inputId;
    //密码输入框
    private InputField inputPw;
    //重复输入框
    private InputField inputRep;
    //注册按钮
    private Button btnRegister;
    //关闭按钮
    private Button btnClose;

    //初始化
    public override void OnInit()
    {
        skinPath = "RegisterPanel";
        layer = PanelManager.Layer.Panel;
    }

    //显示
    public override void OnShow(params object[] para)
    {
        //寻找组件
        inputId = skin.transform.Find("InputId").GetComponent<InputField>();
        inputPw = skin.transform.Find("InputPw").GetComponent<InputField>();
        inputRep = skin.transform.Find("InputRep").GetComponent<InputField>();
        btnRegister = skin.transform.Find("BtnRegister").GetComponent<Button>();
        btnClose = skin.transform.Find("BtnClose").GetComponent<Button>();
        //监听
        btnRegister.onClick.AddListener(OnRegisterClick);
        btnClose.onClick.AddListener(OnCloseClick);
        //网络协议监听
        NetManager.AddMsgListener("MsgRegister", OnMsgRegister);
    }

    //关闭
    public override void OnClose()
    {
        //网络协议监听
        NetManager.RemoveMsgListener("MsgRegister", OnMsgRegister);
    }

    //当按下注册按钮
    public void OnRegisterClick()
    {
        //用户名密码为空
        if (inputId.text == "" || inputPw.text == "")
        {
            PanelManager.Open<TipPanel>("用户名和密码不能为空！");
            return;
        }
        //两次密码不同
        if (inputRep.text != inputPw.text)
        {
            PanelManager.Open<TipPanel>("两次输入的密码不同！");
            return;
        }
        //发送
        MsgRegister msgReg = new MsgRegister();
        msgReg.id = inputId.text;
        msgReg.pw = inputPw.text;
        NetManager.Send(msgReg);
    }

    public void OnCloseClick()
    {
        Close();
    }

    //收到注册协议
    public void OnMsgRegister(MsgBase msgBase)
    {
        MsgRegister msg = (MsgRegister)msgBase;
        if (msg.result == 0)
        {
            Debug.Log("注册成功！");
            //提示
            PanelManager.Open<TipPanel>("注册成功！");
            //关闭面板
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("注册失败！");
        }
    }
}
