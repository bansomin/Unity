
/*************************BEGIN**************************************************** 
    			Created by HAO on 2017/1/10
    BRIEF	: 	Buz的攻击
    VERSION	: 
    HISTORY	:
***************************END****************************************************/

using UnityEngine;
using System.Collections;

public class Buz_Attack : MonoBehaviour {

    private Transform player;           //玩家
    private Vector3 zapNoise = Vector3.zero;
    private float rechargeTimer = 1.5f; //冷却时间
    private bool isCanAttact = false;  //是否可以进行攻击
    private AudioSource audio;           //攻击音效

    public Buz_Movement movement;
    public LineRenderer electriArc;     //闪电 

    void Awake() {
        //找到敌人的目标（玩家）
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audio = this.GetComponent<AudioSource>();
    }

	// Update is called once per frame
	void Update () {

        //向玩家方向移动
        movement.moveTarget = player.position;

        Vector3 dir = player.position - transform.position;
        //isCanAttact = (dir.magnitude < 2.0f);

        isCanAttact = false;
        if (dir.magnitude<2.0f) {
            isCanAttact = true;
            //向ZERO方向移动
            movement.moveTarget = Vector3.zero;
        }

        rechargeTimer -= Time.deltaTime;
        if ((rechargeTimer<=0) && isCanAttact) {
            
            //闪电测试
            //x,z
            zapNoise = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)) * 0.5f;
            StartCoroutine(DoElectricArc());

            //重置冷却时间
            rechargeTimer = Random.Range(1.0f, 2.0f);
        }
   
	}

    //协程
    IEnumerator DoElectricArc() {

        if (electriArc.enabled) {
            yield return 0;
        }

        //Show
        electriArc.enabled = true;

        //播放音效
        audio.Play();
        
        zapNoise = transform.rotation * zapNoise;
        float stopTime = Time.time + 0.2f;

        while (Time.time < stopTime) {
            electriArc.SetPosition(0, electriArc.transform.position);
            electriArc.SetPosition(1, player.position + zapNoise);
            yield return 0;
        }

        //Hide
        electriArc.enabled = false;
    }

}
