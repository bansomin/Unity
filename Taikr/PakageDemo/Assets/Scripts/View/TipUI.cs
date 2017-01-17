using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TipUI : MonoBehaviour {

    public Text outLineText;
    public Text contentText;

    //更新提示
    public void updateTip(string text) {
        outLineText.text = text;
        contentText.text = text;
    }

    //显示提示
    public void showTipUI() {
        gameObject.SetActive(true);
    }

    //隐藏提示
    public void hideTipUI() {
        gameObject.SetActive(false);
    }

    //设置位置
    public void setLocalPosition(Vector2 position) {
        transform.localPosition = position;
    }

}
