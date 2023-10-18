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
        if(IsDie())
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
        float axis = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            axis = -1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            axis = 1;
        }
        //旋转角度
        Vector3 le = turret.localEulerAngles;
        le.y += axis * Time.deltaTime * turretSpeed;
        turret.localEulerAngles = le;
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
