using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Player
{
    //id
    public string id = "";
    //指向ClientState
    public ClientState state;
    //坐标和旋转
    public int x;
    public int y;
    public int z;
    public float ex;
    public float ey;
    public float ez;
    //在哪个房间
    public int roomId = -1;
    //阵营
    public int camp = 1;
    //坦克生命值
    public int hp = 100;
    //数据库数据
    public PlayerData data;


    //构造函数
    public Player(ClientState state)
    {
        this.state = state;
    }
    //发送信息
    public void Send(MsgBase msgBase)
    {
        NetManager.Send(state, msgBase);
    }
}
