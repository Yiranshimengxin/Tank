using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //战场中的坦克
    public static Dictionary<string, BaseTank> tanks = new Dictionary<string, BaseTank>();

    //初始化
    public static void Init()
    {
        NetManager.AddMsgListener("MsgEnterBattle", OnMsgEnterBattle);
        NetManager.AddMsgListener("MsgBattleResult", OnMsgBattleResult);
        NetManager.AddMsgListener("MsgLeaveBattle", OnMsgLeaveBattle);

        NetManager.AddMsgListener("MsgSyncTank", OnMsgSyncTank);
        NetManager.AddMsgListener("MsgFire", OnMsgFire);
        NetManager.AddMsgListener("MsgHit", OnMsgHit);
    }

    //收到击中协议
    private static void OnMsgHit(MsgBase msgBase)
    {
        MsgHit msg = (MsgHit)msgBase;
        //查找坦克
        BaseTank tank = GetTank(msg.targetId);
        if (tank == null)
        {
            return;
        }
        //开火
        tank.Attacked(msg.damage);
    }

    //收到开火协议
    private static void OnMsgFire(MsgBase msgBase)
    {
        MsgFire msg = (MsgFire)msgBase;
        //不同步自己
        if (msg.id == MainGame.id)
        {
            return;
        }
        //查找坦克
        SyncTank tank = (SyncTank)GetTank(msg.id);
        if (tank == null)
        {
            return;
        }
        //开火
        tank.SyncFire(msg);
    }

    //收到同步协议
    private static void OnMsgSyncTank(MsgBase msgBase)
    {
        MsgSyncTank msg = (MsgSyncTank)msgBase;
        //不同步自己
        if (msg.id == MainGame.id)
        {
            return;
        }
        //查找坦克
        SyncTank tank = (SyncTank)GetTank(msg.id);
        if (tank == null)
        {
            return;
        }
        //移动同步
        tank.SyncPos(msg);
    }

    //收到玩家退出协议
    private static void OnMsgLeaveBattle(MsgBase msgBase)
    {
        MsgLeaveBattle msg = (MsgLeaveBattle)msgBase;
        //查找坦克
        BaseTank tank = GetTank(msg.id);
        if (tank == null)
        {
            return;
        }
        //删除坦克
        RemoveTank(msg.id);
        Destroy(tank.gameObject);
    }

    //收到战斗结束协议
    private static void OnMsgBattleResult(MsgBase msgBase)
    {
        MsgBattleResult msg = (MsgBattleResult)msgBase;
        //判断胜利还是失败
        bool isWin = false;
        BaseTank tank = GetCtrlTank();
        if (tank != null && tank.camp == msg.winCamp)
        {
            isWin = true;
        }
        //显示界面
        PanelManager.Open<ResultPanel>(isWin);
    }

    //收到进入战斗协议
    public static void OnMsgEnterBattle(MsgBase msgBase)
    {
        MsgEnterBattle msg = (MsgEnterBattle)msgBase;
        EnterBattle(msg);
    }

    public static void EnterBattle(MsgEnterBattle msg)
    {
        //重置
        BattleManager.Reset();
        //关闭界面
        PanelManager.Close("RoomPanel");
        PanelManager.Close("ResultPanel");
        //产生坦克
        for (int i = 0; i < msg.tanks.Length; i++)
        {
            GenerateTank(msg.tanks[i]);
        }
    }

    //产生坦克
    private static void GenerateTank(TankInfo tankInfo)
    {
        //GameObject
        string objName = "Tank_" + tankInfo.id;
        GameObject tankObj = new GameObject(objName);
        BaseTank tank = null;
        if (tankInfo.id == MainGame.id)
        {
            tank = tankObj.AddComponent<CtrlTank>();
            tankObj.AddComponent<CameraFollow>();
        }
        else
        {
            tank = tankObj.AddComponent<SyncTank>();
        }
        //属性
        tank.camp = tankInfo.camp;
        tank.id = tankInfo.id;
        tank.hp = tankInfo.hp;
        //位置和旋转
        Vector3 pos = new Vector3(tankInfo.x, tankInfo.y, tankInfo.z);
        Vector3 rot = new Vector3(tankInfo.ex, tankInfo.ey, tankInfo.ez);
        tank.transform.position = pos;
        tank.transform.eulerAngles = rot;
        //Init
        if (tankInfo.camp == 1)
        {
            tank.Init("Tank1");
        }
        else
        {
            tank.Init("Tank2");
        }
        //列表
        AddTank(tankInfo.id, tank);
    }

    //添加坦克
    public static void AddTank(string id, BaseTank tank)
    {
        tanks[id] = tank;
    }

    //删除坦克
    public static void RemoveTank(string id)
    {
        if (tanks.ContainsKey(id))
        {
            tanks.Remove(id);
        }
    }

    //获取坦克
    public static BaseTank GetTank(string id)
    {
        if (tanks.ContainsKey(id))
        {
            return tanks[id];
        }
        return null;
    }

    //获取玩家控制的坦克
    public static BaseTank GetCtrlTank()
    {
        return GetTank(MainGame.id);
    }

    //重置战场
    public static void Reset()
    {
        //场景
        foreach (BaseTank tank in tanks.Values)
        {
            Destroy(tank.gameObject);
        }
        //列表
        tanks.Clear();
    }
}
