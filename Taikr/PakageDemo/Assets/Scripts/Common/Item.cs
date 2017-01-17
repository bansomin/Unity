using UnityEngine;
using System.Collections;

public class Item {

    public int Id {
                get;
        private set;
    }

    public string Name {
                get;
        private set;
    }

    public string Descriptionn {
                get;
        private set;
    }

    public int BuyPrice {
                get;
        private set;
    }

    public int SellPrice {
                get;
        private set;
    }

    public string Image {
                get;
        private set;
    }

    public string ItemType {
        get;
        protected set;
    }

    //构造函数
    public Item(int id, string name, string des, int buyp, int sellp, string image) {

        this.Id             = id;
        this.Name           = name;
        this.Descriptionn   = des;
        this.BuyPrice       = buyp;
        this.SellPrice      = sellp;
        this.Image          = image;
    }
}
