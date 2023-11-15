using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TankInfo
{
    public string id = "";  //玩家id
    public int camp = 0;  //阵营
    public int hp = 0;  //生命值
    public float x = 0;  //位置
    public float y = 0;
    public float z = 0;
    public float ex = 0;  //旋转
    public float ey = 0;
    public float ez = 0;
}
