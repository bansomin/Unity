/*************************BEGIN**************************************************** 
    			Created by HAO on 2017/1/10
    BRIEF	: 	Spi的移动
    VERSION	: 
    HISTORY	:
***************************END****************************************************/

using UnityEngine;
using System.Collections;

public class Spi_Movement : MonoBehaviour {

    public float walkingSpeed = 5.0f;   //移动速度
    public float oriantationSpeed = 0.3f;

    [HideInInspector]
    public Vector3 moveDirection;       //移动方向
    [HideInInspector]
    public Vector3 faceDirection;       //朝向

    private Transform player;           //玩家
    private Rigidbody rigidbody;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //刚体
        rigidbody = this.GetComponent<Rigidbody>();
    }
	
	void FixedUpdate () {

        //Vector3 targetVelocity = (player.position - transform.position);
        Vector3 targetVelocity = moveDirection;
        targetVelocity.Normalize();
        Vector3 deltaVelocity = targetVelocity * walkingSpeed - rigidbody.velocity;
        rigidbody.AddForce(deltaVelocity, ForceMode.Acceleration);

        Vector3 faceDir = moveDirection;
        if (faceDir == Vector3.zero) {
            rigidbody.angularVelocity = Vector3.zero;
        }
        else { 
            //敌人的旋转
            float rotationAngle = angleAroundAxis(transform.forward, faceDir, Vector3.up);
            rigidbody.angularVelocity = Vector3.up * rotationAngle * oriantationSpeed * 0.2f;
        }
       
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
