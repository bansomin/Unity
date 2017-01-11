/*************************BEGIN**************************************************** 
    			Created by HAO on 2017/1/9
    BRIEF	: 	Buz的移动
    VERSION	: 
    HISTORY	:
***************************END****************************************************/

using UnityEngine;
using System.Collections;

public class Buz_Movement : MonoBehaviour {

    private Transform player;           //玩家
    private Rigidbody rigidbody;        //刚体
    private Vector3 smoothDirection = Vector3.zero; //平滑的飞行

    public Vector3 moveTarget;
    public float flyingSpeed = 5.0f;            //飞行速度
    public float zigZagness = 3.0f;             //左右偏差幅度控制
    public float zigZagSpeed = 2.5f;            //速度
    public float backtrackIntensity = 0.5f;     //回退系数
    public float oriantationMultiplier = 2.5f;  //旋转系数

	void Awake () {

        //找到敌人的目标（玩家）
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //刚体
        rigidbody = this.GetComponent<Rigidbody>();
    }
	
    //力、角速度
	void FixedUpdate () {

        //方向（目标点减去当前点）
        Vector3 direction = moveTarget - transform.position;
        direction.Normalize();  //标准化
        smoothDirection = Vector3.Lerp(smoothDirection, direction, Time.deltaTime*3.0f);
        Vector3 zigzag = transform.right * (Mathf.PingPong(Time.time * zigZagSpeed, 2.0f) - 1.0f) * zigZagness; //值范围(x周，单位偏差) 

        //敌人的前进
        //力
        //Vector3 deltaVelocity = direction - GetComponent<Rigidbody>().velocity;   //rigidbody
        //Vector3 deltaVelocity = direction * flyingSpeed - rigidbody.velocity;
        Vector3 deltaVelocity = (smoothDirection * flyingSpeed + zigzag) - rigidbody.velocity;       //平滑

        float oriantationSpeed = 1.0f;

        if (Vector3.Dot(direction, transform.forward) > 0.8f) {//>0面对面
            rigidbody.AddForce(deltaVelocity, ForceMode.Force);
        }
        else {
            rigidbody.AddForce(-deltaVelocity * backtrackIntensity, ForceMode.Force);
            oriantationSpeed = oriantationMultiplier;
        }
        //敌人的旋转
        float rotationAngle = angleAroundAxis(transform.forward, direction, Vector3.up);
        rigidbody.angularVelocity = Vector3.up * rotationAngle * oriantationSpeed * 0.2f;
    }

    //The angle between dirA and dirB around axis
    static float angleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis) {

        dirA = dirA - Vector3.Project(dirA, axis);
        dirB = dirB - Vector3.Project(dirB, axis);

        //cal angle between a and b
        float angle = Vector3.Angle(dirA, dirB);

        return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
    }

}
