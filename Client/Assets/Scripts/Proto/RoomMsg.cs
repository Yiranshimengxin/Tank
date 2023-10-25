using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//查询成绩
public class MsgGetAchieve : MsgBase
{
    public MsgGetAchieve()
    {
        protoName = "MsgGetAchieve";
    }

    //服务端回
    public int win = 0;
    public int lost = 0;
}

[System.Serializable]
//房间信息
public class RoomInfo
{
    //房间id
    public int id = 0;
    //人数
    public int count = 0;
    //状态 0-准备中 1-战斗中
    public int status = 0;
}

//请求房间列表
public class MsgGetRoomList : MsgBase
{
    public MsgGetRoomList()
    {
        protoName = "MsgGetRoomList";
    }

    //服务端回
    public RoomInfo[] rooms;
}

//创建房间
public class MsgCreateRoom : MsgBase
{
    public MsgCreateRoom()
    {
        protoName = "MsgCreateRoom";
    }

    //服务端回
    public int result = 0;
}

//进入房间
public class MsgEnterRoom : MsgBase
{
    public MsgEnterRoom()
    {
        protoName = "MsgEnterRoom";
    }

    //服务端发
    public int id = 0;
    //服务端回
    public int result = 0;
}

//玩家信息
[System.Serializable]
public class PlayerInfo
{
    //账号
    public string id = "czr";
    //阵营
    public int camp = 0;
    //胜利数
    public int win = 0;
    //失败数
    public int lost = 0;
    //是否是新房主
    public int isOwner = 0;
}

//获取房间信息
public class MsgGetRoomInfo : MsgBase
{
    public MsgGetRoomInfo()
    {
        protoName = "MsgGetRoomInfo";
    }

    //服务端回
    public PlayerInfo[] players;
}

//离开房间
public class MsgLeaveRoom : MsgBase
{
    public MsgLeaveRoom()
    {
        protoName = "MsgLeaveRoom";
    }

    //服务端回
    public int result = 0;
}

//开战
public class MsgStartBattle : MsgBase
{
    public MsgStartBattle()
    {
        protoName = "MsgStartBattle";
    }

    //服务端回
    public int result = 0;
}