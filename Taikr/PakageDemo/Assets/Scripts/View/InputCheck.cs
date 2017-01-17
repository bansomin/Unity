using UnityEngine;
using System.Collections;

public class InputCheck : MonoBehaviour {

	// Update is called once per frame
	void Update () {
	    //鼠标中键
        if (Input.GetMouseButtonDown(2)) {
            int index = Random.Range(0, 10);            
            KnapsackUIManager.Instance.StoreItems(index);
        }
	}
}
