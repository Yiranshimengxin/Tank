using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel
{
    //开战按钮
    private Button startButton;
    //退出按钮
    private Button closeButton;
    //列表容器
    private Transform content;
    //玩家信息物体
    private GameObject playerObj;

    //初始化
    public override void OnInit()
    {
        skinPath = "RoomPanel";
        layer = PanelManager.Layer.Panel;
    }

    //显示
    public override void OnShow(params object[] para)
    {
        startButton = skin.transform.Find("StartButton").GetComponent<Button>();
        closeButton = skin.transform.Find("CloseButton").GetComponent<Button>();
        content = skin.transform.Find("ListPanel/Scroll/Viewport/Content");
        playerObj = skin.transform.Find("Player").gameObject;
        //不激活玩家信息
        playerObj.SetActive(false);
        //按钮事件
        startButton.onClick.AddListener(OnStartClick);
        closeButton.onClick.AddListener(OnCloseClick);

        //协议监听
        NetManager.AddMsgListener("MsgGetRoomInfo", OnMsgGetRoomInfo);
        NetManager.AddMsgListener("MsgLeaveRoom", OnMsgLeaveRoom);
        NetManager.AddMsgListener("MsgStartBattle", OnMsgStartBattle);
        //发送查询
        MsgGetRoomInfo msg = new MsgGetRoomInfo();
        NetManager.Send(msg);
    }

    //关闭
    public override void OnClose()
    {
        //协议监听
        NetManager.RemoveMsgListener("MsgGetRoomInfo", OnMsgGetRoomInfo);
        NetManager.RemoveMsgListener("MsgLeaveRoom", OnMsgLeaveRoom);
        NetManager.RemoveMsgListener("MsgStartBattle", OnMsgStartBattle);
    }

    //收到玩家列表协议
    private void OnMsgGetRoomInfo(MsgBase msgBase)
    {
        MsgGetRoomInfo msg = (MsgGetRoomInfo)msgBase;
        //清除玩家列表
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            GameObject go = content.GetChild(i).gameObject;
            Destroy(go);
        }
        //重新生成列表
        if (msg.players == null)
        {
            return;
        }
        print("msg.players.Length "+msg.players.Length);
        for (int i = 0; i < msg.players.Length; i++)
        {
            GeneratePlayerInfo(msg.players[i]);
        }
    }

    //创建一个玩家单元
    public void GeneratePlayerInfo(PlayerInfo playerInfo)
    {
        print("创建一个玩家单元！");
        //创建物体
        GameObject go = Instantiate(playerObj);
        go.transform.SetParent(content);
        go.SetActive(true);
        go.transform.localScale = Vector3.one;
        //获取组件
        Transform t = go.transform;
        Text idText = t.Find("IdText").GetComponent<Text>();
        Text campText = t.Find("CampText").GetComponent<Text>();
        Text scoreText = t.Find("ScoreText").GetComponent<Text>();
        //填充信息
        idText.text = playerInfo.id;
        if (playerInfo.camp == 1)
        {
            campText.text = "红";
        }
        else
        {
            campText.text = "蓝";
        }
        if (playerInfo.isOwner == 1)
        {
            campText.text = campText.text + "！";
        }
        scoreText.text = playerInfo.win + "胜" + playerInfo.lost + "负";
    }

    //收到退出房间协议
    public void OnMsgLeaveRoom(MsgBase msgBase)
    {
        MsgLeaveRoom msg = (MsgLeaveRoom)msgBase;
        //成功退出房间
        if (msg.result == 0)
        {
            PanelManager.Open<TipPanel>("退出房间！");
            PanelManager.Open<RoomListPanel>();
            Close();
        }
        //退出房间失败
        else
        {
            PanelManager.Open<TipPanel>("退出房间失败！");
        }
    }

    //收到开战返回
    public void OnMsgStartBattle(MsgBase msgBase)
    {
        MsgStartBattle msg = (MsgStartBattle)msgBase;
        //开战
        if (msg.result == 0)
        {
            //关闭界面
            Close();
        }
        //开战失败
        else
        {
            PanelManager.Open<TipPanel>("开战失败！两队至少都需要一名玩家，只有对战可以开始战斗！");
        }
    }

    //点击退出按钮
    public void OnCloseClick()
    {
        MsgLeaveRoom msg = new MsgLeaveRoom();
        NetManager.Send(msg);
    }

    //点击开战按钮
    public void OnStartClick()
    {
        MsgStartBattle msg = new MsgStartBattle();
        NetManager.Send(msg);
    }
}
