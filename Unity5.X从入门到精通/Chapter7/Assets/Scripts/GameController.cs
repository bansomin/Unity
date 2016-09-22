using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    private GameObject newball;
    public GameObject myBall;         //BowlingBall
    private float maxWidth;
    private float time = 2;

    // Use this for initialization
    void Start () {

        //将屏幕的宽度转换成世界坐标
        Vector3 screenPos = new Vector3(Screen.width, 0, 0);
        Vector3 movewidth = Camera.main.ScreenToWorldPoint(screenPos);

        //获取保龄球自身的宽度
        float ballwidth = myBall.GetComponent<Renderer>().bounds.extents.x;

        //计算球实例化位置的宽度
        maxWidth = movewidth.x - ballwidth;
	}

    void FixedUpdate() {

        time -= Time.deltaTime;
        if (time < 0) {

            //产生一个随机数,代表实例化下一个保龄球所需的时间
            time = Random.Range(1.5f, 2f);

            //保龄球实例化位置的宽度内产生一个随机数，来控制实例化的保龄球的位置
            float posX = Random.Range(-maxWidth, maxWidth);
            Vector3 position = new Vector3(posX, transform.position.y, 0);

            //实例化保龄球，10s后销毁
            newball = (GameObject)Instantiate(myBall, position, Quaternion.identity);
            Destroy(newball, 10);


        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
