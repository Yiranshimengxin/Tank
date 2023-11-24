using System.Collections;
using System.Collections.Generic;

//同步坦克信息
public class MsgSyncTank : MsgBase
{
    public MsgSyncTank()
    {
        protoName = "MsgSyncTank";
    }
    //位置、旋转、炮塔旋转
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float ex = 0;
    public float ey = 0;
    public float ez = 0;
    public float turretY = 0;
    public float gunX = 0;
    //服务端补充
    public string id = "";  //哪个坦克
}

//开火
public class MsgFire : MsgBase
{
    public MsgFire()
    {
        protoName = "MsgFire";
    }
    //位置、旋转、炮塔旋转
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float ex = 0;
    public float ey = 0;
    public float ez = 0;
    public float turretY = 0;
    public float gunX = 0;
    //服务端补充
    public string id = "";  //哪个坦克
}

//击中
public class MsgHit : MsgBase
{
    public MsgHit()
    {
        protoName = "MsgHit";
    }
    //击中谁
    public string targetId = "";
    //击中点
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    //服务端补充
    public string id = "";  //哪个坦克
    public int hp = 0;  //被击中坦克血量
    public int damage = 0;  //受到的伤害
}
