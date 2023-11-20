using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //移动速度
    public float speed = 100f;
    //发射者
    public BaseTank tank;
    //炮弹模型
    private GameObject skin;
    //物理
    Rigidbody rigidbody;

    public void Init()
    {
        //皮肤
        GameObject skinRes = ResManager.LoadPrefab("Bullet");
        skin = Instantiate(skinRes);
        skin.transform.parent = transform;
        skin.transform.localPosition = Vector3.zero;
        skin.transform.localEulerAngles = Vector3.zero;
        //物理

    }

    private void Update()
    {
        //向前移动
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    //碰撞
    private void OnCollisionEnter(Collision collision)
    {
        //打到的坦克
        GameObject collObj = collision.gameObject;
        BaseTank hitTank = collObj.GetComponent<BaseTank>();
        //不能打自己
        if (hitTank == tank)
        {
            return;
        }
        //攻击其他坦克
        if (hitTank != null)
        {
            SendMsgHit(tank, hitTank);
        }
        //显示爆炸效果
        GameObject explode = ResManager.LoadPrefab("Exp");
        GameObject go = Instantiate(explode, transform.position, transform.rotation);
        //摧毁自身
        Destroy(go, 2);
        Destroy(gameObject);
    }

    //发送伤害协议
    void SendMsgHit(BaseTank tank, BaseTank hitTank)
    {
        if (hitTank == null || tank == null)
        {
            return;
        }
        //不是自己发出的炮弹
        if (tank.id != MainGame.id)
        {
            return;
        }
        MsgHit msg = new MsgHit();
        msg.targetId = hitTank.id;
        msg.id = tank.id;
        msg.x = transform.position.x;
        msg.y = transform.position.y;
        msg.z = transform.position.z;
        NetManager.Send(msg);
    }
}
