using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {

    public Text ItemName;
    public Image ItemImage;

    public void updateItem(string name) {
        ItemName.text = name;
    }

    public void updateImage(string imagepath) {

        Sprite spr = new Sprite();
        ItemImage.sprite = spr;
    }

}
