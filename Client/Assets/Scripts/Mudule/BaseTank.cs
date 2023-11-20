using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTank : MonoBehaviour
{
    //坦克模型
    GameObject skin;
    //转向速度
    public float steer = 20;
    //移动速度
    public float speed = 8;
    //炮塔旋转速度
    public float turretSpeed = 30f;
    //炮塔
    public Transform turret;
    //炮管
    public Transform gun;
    //发射点
    public Transform firePoint;
    //坦克CD时间
    public float fireCd = 0.5f;
    //上一次发射炮弹的时间
    public float lastFireTime;
    //生命值
    public float hp = 100;
    //属于哪一名玩家
    public string id = "";
    //阵营
    public int camp = 0;
    //刚体
    protected Rigidbody rigidbody;

    public virtual void Init(string skinPath)
    {
        GameObject skinRes = ResManager.LoadPrefab(skinPath);
        skin = Instantiate(skinRes);
        skin.transform.parent = this.transform;
        skin.transform.localPosition = Vector3.zero;
        skin.transform.localEulerAngles = Vector3.zero;
        rigidbody=gameObject.AddComponent<Rigidbody>();

        //炮塔炮管
        turret = skin.transform.Find("Head");
        gun = turret.transform.Find("Cannon");
        firePoint = gun.Find("Point");
    }

    //发射炮弹
    public Bullet Fire()
    {
        //已经死亡
        if (IsDie())
        {
            return null;
        }
        //产生炮弹
        GameObject bulletObj = new GameObject("Bullet");
        bulletObj.AddComponent<Rigidbody>();
        Bullet bullet = bulletObj.AddComponent<Bullet>();
        bullet.Init();
        bullet.tank = this;
        //位置
        bullet.transform.position = firePoint.position;
        bullet.transform.forward = -firePoint.up;
        //更新时间
        lastFireTime = Time.time;
        return bullet;
    }

    //是否死亡
    public bool IsDie()
    {
        return hp <= 0;
    }

    //被攻击
    public void Attacked(float att)
    {
        //已经死亡
        if (IsDie())
        {
            return;
        }
        //扣血
        hp -= att;
        if (IsDie())
        {
            //显示燃烧效果
            GameObject obj = ResManager.LoadPrefab("Fire");
            GameObject exp = Instantiate(obj, transform.position, transform.rotation);
            exp.transform.SetParent(transform);
            Destroy(exp, 5);
            Invoke("DestroyTank", 5);
        }
    }

    public void DestroyTank()
    {
        GameObject obj2 = ResManager.LoadPrefab("Exp2");
        GameObject exp2 = Instantiate(obj2, transform.position, transform.rotation);
        exp2.transform.SetParent(transform);        
        Destroy(exp2, 3);        
        Destroy(gameObject,2f);
    }

    protected void Update()
    {

    }
}
