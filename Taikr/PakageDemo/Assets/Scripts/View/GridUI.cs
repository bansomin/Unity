
/*************************BEGIN**************************************************** 
    			Created by HAO on 2017/1/16
    BRIEF	: 	鼠标指向、显示详细信息
    VERSION	: 
    HISTORY	:
***************************END****************************************************/
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class GridUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    //提示显示
    #region Enter&Exit 
    public static Action<Transform> OnEnter;
    public static Action OnExit;

    public void OnPointerEnter(PointerEventData eventData) {

        if (eventData.pointerEnter.tag == "Grid") {
            //Debug.Log("In");
            if (OnEnter != null) {
                OnEnter(transform);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {

        if (eventData.pointerEnter.tag == "Grid") {
            //Debug.Log("Out");
            if (OnEnter != null) {
                OnExit();
            }
        }
    }
    #endregion

    //拖拽
    public static Action<Transform> OnLeftBeginDrag;
    public static Action<Transform, Transform> OnLeftEndDrag;   //从X到X

    public void OnBeginDrag(PointerEventData eventData) {

        if (eventData.button == PointerEventData.InputButton.Left) {    //左键按下
            if (OnLeftBeginDrag != null) {
                OnLeftBeginDrag(transform);
            }
        }
    }

    public void OnDrag(PointerEventData eventData) {
    }

    public void OnEndDrag(PointerEventData eventData) {

        if (eventData.button == PointerEventData.InputButton.Left) {    //左键松开
            if (OnLeftEndDrag != null) {
                if (eventData.pointerEnter == null) {
                    OnLeftEndDrag(transform, null);
                }
                else {
                    OnLeftEndDrag(transform ,eventData.pointerEnter.transform);
                }
            }
        }
    }
}

