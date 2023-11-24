using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlTank : BaseTank
{
    //上一次发送同步信息的时间
    private float lastSendSyncTime = 0;
    //同步帧率
    public static float syncInterval = 0.01f;

    new void Update()
    {
        base.Update();
        if (IsDie())
        {
            return;
        }
        MoveUpdate();
        TurretUpdate();
        FireUpdate();
        //发送同步信息
        SyncUpdate();
    }

    //移动控制
    public void MoveUpdate()
    {
        //已经死亡
        if (IsDie())
        {
            return;
        }
        float x = Input.GetAxis("Horizontal");
        transform.Rotate(0, x * steer * Time.deltaTime, 0);
        float y = Input.GetAxis("Vertical");
        Vector3 s = y * transform.forward * speed * Time.deltaTime;
        transform.position += s;
    }

    //炮塔控制
    public void TurretUpdate()
    {
        //已经死亡
        if (IsDie())
        {
            return;
        }
        //或者轴向
        float axisX = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            axisX = -1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            axisX = 1;
        }

        //获取鼠标滚轮滚动量
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        float minAngle = 8f;
        float maxAngle = 345f;
        float m = 20f;

        //旋转角度
        Vector3 le = turret.localEulerAngles;
        Vector3 ge = gun.localEulerAngles;

        le.y += axisX * Time.deltaTime * turretSpeed;
        turret.localEulerAngles = le;
        //限制炮塔旋转角度的范围
        if (gun.localEulerAngles.x > minAngle && gun.localEulerAngles.x < maxAngle)
        {
            if (gun.localEulerAngles.x >= minAngle && gun.localEulerAngles.x < minAngle + 10f)
            {
                ge.x = minAngle;
            }
            else if (gun.localEulerAngles.x <= maxAngle && gun.localEulerAngles.x > maxAngle - 10f)
            {
                ge.x = maxAngle;
            }
        }
        //根据鼠标滚轮滚动量旋转炮塔
        ge.x += scrollDelta * turretSpeed * Time.deltaTime * m;
        gun.localEulerAngles = ge;
    }

    public void FireUpdate()
    {
        //已经死亡
        if (IsDie())
        {
            return;
        }
        if (!Input.GetMouseButton(0))
        {
            return;
        }
        if (Time.time - lastFireTime < fireCd)
        {
            return;
        }
        lastFireTime=Time.time;
        Bullet bullet = Fire();
        //发射同步协议
        MsgFire msg = new MsgFire();
        msg.x = bullet.transform.position.x;
        msg.y = bullet.transform.position.y;
        msg.z = bullet.transform.position.z;
        msg.ex = bullet.transform.eulerAngles.x;
        msg.ey = bullet.transform.eulerAngles.y;
        msg.ez = bullet.transform.eulerAngles.z;
        NetManager.Send(msg);
    }

    //发送同步信息
    public void SyncUpdate()
    {
        //时间间隔判断
        if (Time.time - lastSendSyncTime < syncInterval)
        {
            return;
        }
        lastSendSyncTime = Time.time;
        //发送同步协议
        MsgSyncTank msg = new MsgSyncTank();
        msg.x = transform.position.x;
        msg.y = transform.position.y;
        msg.z = transform.position.z;
        msg.ex = transform.eulerAngles.x;
        msg.ey = transform.eulerAngles.y;
        msg.ez = transform.eulerAngles.z;
        msg.turretY = turret.localEulerAngles.y;
        msg.gunX = gun.localEulerAngles.x;
        NetManager.Send(msg);
    }
}
