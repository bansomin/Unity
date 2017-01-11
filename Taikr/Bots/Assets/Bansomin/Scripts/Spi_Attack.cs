/*************************BEGIN**************************************************** 
    			Created by HAO on 2017/1/10
    BRIEF	: 	Spi的攻击 
    VERSION	: 
    HISTORY	:
***************************END****************************************************/

using UnityEngine;
using System.Collections;

public class Spi_Attack : MonoBehaviour {

    private Transform player;           //玩家

    public Spi_Movement movement;
    public GameObject Spi_bot;          //动画

    public float damageRadius = 5.0f;   //爆炸范围
    public GameObject explosionPrefab;  //爆炸预制体

    void Awake() {
        //找到敌人的目标（玩家）
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update () {

        //在attack中更新moveDirection（moveDirection是Spi的目标位置）
        //计算与目标之间的距离 
        movement.moveDirection = (player.position - transform.position);

        Vector3 dir = player.position - transform.position;
        if (dir.magnitude < 2.0f) {

            movement.moveDirection = Vector3.zero;
            Spi_bot.GetComponent<Animation>().Stop();   //停止动画
            //播放爆炸
            Explode();
        }
        else {
            Spi_bot.GetComponent<Animation>().Play("forward");  //播放行进动画
        }
        
    }

    void Explode() {

        float damageFraction = 1 - (Vector3.Distance(player.position, transform.position)/damageRadius);
        player.GetComponent<Rigidbody>().AddExplosionForce(10, transform.position, damageRadius, 0.0f, ForceMode.Impulse);
        //实例化
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
