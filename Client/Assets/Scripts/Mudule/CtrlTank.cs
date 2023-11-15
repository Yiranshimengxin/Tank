using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CtrlTank : BaseTank
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
        Fire();
    }
}
