using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public class Room
{
    //id
    public int id = 0;
    //最大玩家数
    public int maxPlayer = 6;
    //玩家列表
    public Dictionary<string, bool> playerIds = new Dictionary<string, bool>();
    //房主id
    public string ownerId = "";
    //状态
    public enum Status
    {
        PREPRAE = 0,
        FIGHT = 1,
    }
    public Status status = Status.PREPRAE;
    //上一次判断结果的时间
    private long lastJudgeTime = 0;

    private static float[,,] birthConfig = new float[2, 3, 6]
    {
        {
            {90,1,10,0,0,0},
            {440,1,900,0,0,0},
            {740,1,900,0,0,0},

        },
        {
            {90,1,30,0,0,0},
            {260,1,-30,0,0,0},
            {440,1,30,0,0,0},
        }
    };

    //初始化位置
    private void SetBirthPos(Player player, int index)
    {
        int camp = player.camp;
        player.x = birthConfig[camp - 1, index, 0];
        player.y = birthConfig[camp - 1, index, 1];
        player.z = birthConfig[camp - 1, index, 2];
        player.ex = birthConfig[camp - 1, index, 3];
        player.ey = birthConfig[camp - 1, index, 4];
        player.ez = birthConfig[camp - 1, index, 5];
    }

    //重置玩家战斗属性
    private void ResetPlayer()
    {
        //位置和旋转
        int count1 = 0;
        int count2 = 0;
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player.camp == 1)
            {
                SetBirthPos(player, count1);
                count1++;
            }
            else
            {
                SetBirthPos(player, count2);
                count2++;
            }
        }
        //生命值
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            player.hp = 100;
        }
    }

    //玩家数据转成TankInfo
    public TankInfo PlayerToTankInfo(Player player)
    {
        TankInfo tankInfo = new TankInfo();
        tankInfo.camp = player.camp;
        tankInfo.id = player.id;
        tankInfo.hp = player.hp;

        tankInfo.x = player.x;
        tankInfo.y = player.y;
        tankInfo.z = player.z;
        tankInfo.ex = player.ex;
        tankInfo.ey = player.ey;
        tankInfo.ez = player.ez;
        return tankInfo;
    }

    //添加玩家
    public bool AddPlayer(string id)
    {
        //获取玩家
        Player player = PlayerManager.GetPlayer(id);
        if (player == null)
        {
            Console.WriteLine("Room.AddPlayer fail, player is null! ");
            return false;
        }
        //房间人数
        if (playerIds.Count >= maxPlayer)
        {
            Console.WriteLine("Room.AddPlayer fail, reach maxPlayer! ");
            return false;
        }
        //准备状态才能加入
        if (status != Status.PREPRAE)
        {
            Console.WriteLine("Room.AddPlayer fail, not prepare! ");
            return false;
        }
        //已经在房间里
        if (playerIds.ContainsKey(id))
        {
            Console.WriteLine("Room.AddPlayer fail, already in this room! ");
            return false;
        }
        //加入列表
        playerIds[id] = true;
        //设置玩家数据
        player.camp = SwitchCamp();
        player.roomId = this.id;
        //设置房主
        if (ownerId == "")
        {
            ownerId = player.id;
        }
        //广播
        Broadcast(ToMsg());
        return true;
    }

    //分配阵营
    public int SwitchCamp()
    {
        //计数
        int count1 = 0;
        int count2 = 0;
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player.camp == 1)
            {
                count1++;
            }
            if (player.camp == 2)
            {
                count2++;
            }
        }
        //选择
        if (count1 <= count2)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    //删除玩家
    public bool RemovePlayer(string id)
    {
        //获取玩家
        Player player = PlayerManager.GetPlayer(id);
        if (player == null)
        {
            Console.WriteLine("Room.RemovePlayer fail, player is null! ");
            return false;
        }
        //没有在房间里
        if (!playerIds.ContainsKey(id))
        {
            Console.WriteLine("Room.RemovePlayer fail, not in this room! ");
            return false;
        }
        //删除列表
        playerIds.Remove(id);
        //设置玩家数据
        player.camp = 0;
        player.roomId = -1;
        //设置房主
        if (IsOwner(player))
        {
            ownerId = SwitchOwner();
        }
        //战斗状态退出
        if (status == Status.FIGHT)
        {
            Update();
            player.data.lost++;
            MsgLeaveBattle msg = new MsgLeaveBattle();
            msg.id = player.id;
            Broadcast(msg);
        }
        //房间为空
        if (playerIds.Count == 0)
        {
            RoomManager.RemoveRoom(this.id);
        }
        //广播
        Broadcast(ToMsg());
        return true;
    }

    //是不是房主
    public bool IsOwner(Player player)
    {
        return player.id == ownerId;
    }

    //选择房主
    public string SwitchOwner()
    {
        //选择第一个玩家
        foreach (string id in playerIds.Keys)
        {
            return id;
        }
        //房间没人
        return "";
    }

    //广播消息
    public void Broadcast(MsgBase msg)
    {
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            player.Send(msg);
        }
    }

    //生成MsgGetRoomInfo协议
    public MsgBase ToMsg()
    {
        MsgGetRoomInfo msg = new MsgGetRoomInfo();
        int count = playerIds.Count;
        msg.players = new PlayerInfo[count];
        //players
        int i = 0;
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            PlayerInfo playerInfo = new PlayerInfo();
            //赋值
            playerInfo.id = player.id;
            playerInfo.camp = player.camp;
            playerInfo.win = player.data.win;
            playerInfo.lost = player.data.lost;
            playerInfo.isOwner = 0;
            if (IsOwner(player))
            {
                playerInfo.isOwner = 1;
            }
            msg.players[i] = playerInfo;
            i++;
        }
        return msg;
    }

    //能否开战
    public bool CanStartBattle()
    {
        //已经是战斗状态
        if (status != Status.PREPRAE)
        {
            return false;
        }
        //统计每个阵营的玩家数
        int count1 = 0;
        int count2 = 0;
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player.camp == 1)
            {
                count1++;
            }
            else
            {
                count2++;
            }
        }
        //每个阵营至少需要1名玩家
        if (count1 < 1 || count2 < 1)
        {
            return false;
        }
        return true;
    }

    //开战
    public bool StartBattle()
    {
        if (!CanStartBattle())
        {
            return false;
        }
        //状态
        status = Status.FIGHT;
        //玩家战斗属性
        ResetPlayer();
        //返回数据
        MsgEnterBattle msg = new MsgEnterBattle();
        msg.mapId = 1;
        msg.tanks = new TankInfo[playerIds.Count];
        int i = 0;
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            msg.tanks[i] = PlayerToTankInfo(player);
            i++;
        }
        Broadcast(msg);
        return true;
    }

    //是否死亡
    public bool IsDie(Player player)
    {
        return player.hp <= 0;
    }

    //胜负判断
    public int Judgement()
    {
        //存活人数
        int count1 = 0;
        int count2 = 0;
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (!IsDie(player))
            {
                if (player.camp == 1)
                {
                    count1++;
                }
                if (player.camp == 2)
                {
                    count2++;
                }
            }
        }
        //判断
        if (count1 <= 0)
        {
            return 2;
        }
        else if (count2 <= 0)
        {
            return 1;
        }
        return 0;
    }

    //定时更新
    public void Update()
    {
        //状态判断
        if (status != Status.FIGHT)
        {
            return;
        }
        //时间判断
        //if (NetManager.GetTimeStamp() - lastJudgeTime < 3f)
        //{
        //    return;
        //}
        lastJudgeTime = NetManager.GetTimeStamp();
        //胜负判断
        int winCamp = Judgement();
        //尚未分出胜负
        if (winCamp == 0)
        {
            return;
        }
        //某一方胜利，结束战斗
        status = Status.PREPRAE;
        //统计信息
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player.camp == winCamp)
            {
                player.data.win++;
            }
            else
            {
                player.data.lost++;
            }
        }
        //发送Result
        MsgBattleResult msg = new MsgBattleResult();
        msg.winCamp = winCamp;
        Broadcast(msg);
    }
}
