
/*************************BEGIN**************************************************** 
    			Created by HAO on 2017/1/16
    BRIEF	: 	拖拽UI
    VERSION	: 
    HISTORY	:
***************************END****************************************************/
using UnityEngine;
using System.Collections;

public class DragItemUI : ItemUI {

    //显示提示
    public void showDragUI() {
        gameObject.SetActive(true);
    }

    //隐藏提示
    public void hideDragUI() {
        gameObject.SetActive(false);
    }

    //设置位置
    public void setLocalPosition(Vector2 position) {
        transform.localPosition = position;
    }
}
