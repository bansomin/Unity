/*************************BEGIN**************************************************** 
    			Created by HAO on 2017/1/13
    BRIEF	: 	继承自Item，有攻击力
    VERSION	: 
    HISTORY	:
***************************END****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Weapon : Item {

    public int Damage {
        get;
        private set;
    }

    public Weapon(int id, string name, string des, int buyp, int sellp, string image, int damage) : base(id, name, des, buyp, sellp, image) {

        this.Damage = damage;
        base.ItemType = "Weapon";
    }

}
