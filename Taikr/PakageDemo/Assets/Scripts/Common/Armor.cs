using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Armor : Item {

    public int Power{ //力量
        get;
        private set;
    }  

    public int Defend { //防御
        get;
        private set;
    }

    public int Agility {
        get;
        private set;
    }

    public Armor(int id, string name, string des, int buyp, int sellp, string image, int power, int def, int agil) : base(id, name, des, buyp, sellp, image) {

        this.Power = power;
        this.Defend = def;
        this.Agility = agil;
        base.ItemType = "Armor";
    }

}
    