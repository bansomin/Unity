
/*************************BEGIN**************************************************** 
    			Created by HAO on 2017/1/13
    BRIEF	: 	继承自Item，有回血、回蓝
    VERSION	: 
    HISTORY	:
***************************END****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Consumable : Item {

    public int BackHp {
        get;
        private set;
    }

    public int BackMp {
        get;
        private set;
    }

    public Consumable(int id, string name, string des, int buyp, int sellp, string image, int backhp, int backmp) : base(id, name, des, buyp, sellp, image) {

        this.BackHp = backhp;
        this.BackMp = backmp;
        base.ItemType = "Consumable";
    }

}

