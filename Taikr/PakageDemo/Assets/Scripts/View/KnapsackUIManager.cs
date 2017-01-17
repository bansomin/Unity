/*************************BEGIN**************************************************** 
    			Created by HAO on 2017/1/13
    BRIEF	: 	存、取、扔、交换
    VERSION	: 
    HISTORY	:
***************************END****************************************************/

using UnityEngine;
using System.Collections;

using System.Text;
using System.Collections.Generic;

public class KnapsackUIManager : MonoBehaviour {

    //单例
    private static KnapsackUIManager _instance;
    public static KnapsackUIManager Instance {get {return _instance;} }
    private Dictionary<int, Item> ItemList = new Dictionary<int, Item>();

    //空格子
    public GridPanelUI gridPanelUI;

    //提示信息
    public TipUI tipui;
    private bool isShow = false;

    //拖拽
    public DragItemUI dragui;
    private bool isDrag = false;

    void Awake() {

        //单例
        _instance = this;
        
        //取数据
        load();

        //注册事件
        GridUI.OnEnter += GridUI_OnEnter;
        GridUI.OnExit += GridUI_OnExit;
        GridUI.OnLeftBeginDrag += GridUI_OnLeftBeginDrag;
        GridUI.OnLeftEndDrag += GridUI_OnLeftEndDrag;

        dragui.hideDragUI();
        tipui.hideTipUI();
    }

    //更新提示信息相关数据 
    void Update() {

        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GameObject.Find("KnapsackUI").transform as RectTransform, 
            Input.mousePosition, 
            null, 
            out position
        );
        
        //拖拽
        if (isDrag) {
            dragui.showDragUI();
            dragui.setLocalPosition(position);
        }

        //是否显示
        if (isShow) {
            tipui.showTipUI();
            tipui.setLocalPosition(position);
        }
    }

    //模拟读取数据的过程
    private void load() {

        ItemList = new Dictionary<int, Item>();

        Weapon w1 = new Weapon(0, "牛刀", "牛B的刀！", 20, 10, "", 100);
        Weapon w2 = new Weapon(1, "羊刀", "杀羊刀。", 15, 10, "", 20);
        Weapon w3 = new Weapon(2, "宝剑", "大宝剑！", 120, 50, "", 500);
        Weapon w4 = new Weapon(3, "军枪", "可以对敌人射击，很厉害的一把枪。", 1500, 125, "", 720);

        Consumable c1 = new Consumable(4, "红瓶", "加血", 25, 11, "", 20, 0);
        Consumable c2 = new Consumable(5, "蓝瓶", "加蓝", 39, 19, "", 0, 20);

        Armor a1 = new Armor(6, "头盔", "保护脑袋！", 128, 83, "", 5, 40, 1);
        Armor a2 = new Armor(7, "护肩", "上古护肩，锈迹斑斑。", 1000, 0, "", 15, 40, 11);
        Armor a3 = new Armor(8, "胸甲", "皇上御赐胸甲。", 153, 0, "", 25, 30, 11);
        Armor a4 = new Armor(9, "护腿", "预防风寒，从腿做起", 999, 60, "", 19, 30, 51);

        ItemList.Add(w1.Id, w1);
        ItemList.Add(w2.Id, w2);
        ItemList.Add(w3.Id, w3);
        ItemList.Add(w4.Id, w4);
        ItemList.Add(c1.Id, c1);
        ItemList.Add(c2.Id, c2);
        ItemList.Add(a1.Id, a1);
        ItemList.Add(a2.Id, a2);
        ItemList.Add(a3.Id, a3);
        ItemList.Add(a4.Id, a4);
    }

    //保存物品
    public void StoreItems(int id) {

        if (!ItemList.ContainsKey(id)) {
            return;
        }
        else {
   
            //指定格子
            Transform empty_grid = gridPanelUI.findEmptyGrid();

            if (empty_grid == null) {
                Debug.LogWarning("背包已满");
                return;
            }
            else {
                //Debug.LogWarning("背包未满");

                Item item = ItemList[id];
                createNewItem(item, empty_grid);
                ////动态加载方法
                //GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Item");
                //itemPrefab.GetComponent<ItemUI>().updateItem(item.Name);    //更新名称
                ////itemPrefab.GetComponent<ItemUI>().updateImage(item.Image);  //更新图片

                //GameObject obj = GameObject.Instantiate(itemPrefab);    //创建

                //obj.transform.SetParent(empty_grid);

                //obj.transform.localPosition = Vector3.zero;
                //obj.transform.localScale = Vector3.one;

            }
        }
    }

    #region 事件回调
    private void GridUI_OnEnter(Transform gridTransform) {
    
        //攻取格子信息
        Item item = ItemModel.GetItemByKey(gridTransform.name);
        string text = GetTipText(item);
        tipui.updateTip(text);
        isShow = true;
    }

    private void GridUI_OnExit() {

        tipui.hideTipUI();
        isShow = false;
    }

    private void GridUI_OnLeftBeginDrag(Transform gridTransform) {

        if (gridTransform.childCount == 0) {
            return;
        }
        else {
            Item item = ItemModel.GetItemByKey(gridTransform.name);
            Debug.Log("Name :" + item.Name);
            dragui.updateItem(item.Name);
            //删除原平的
            Destroy(gridTransform.GetChild(0).gameObject);
            //消除数据
            //ItemModel.DeleteItem(gridTransform.name);
            isDrag = true;
        }
    }

    private void GridUI_OnLeftEndDrag(Transform fromTransform, Transform enterTransform) {
        Debug.Log("End");

        dragui.hideDragUI();
        isDrag = false;

        //UI的外部，扔掉装备
        if (enterTransform == null) {
            //删除原来数据
            ItemModel.DeleteItem(fromTransform.name);
            Debug.Log("删除信息成功");
        }
        else {
            //拖拽操作是否有效
            if (enterTransform.tag == "Grid") { //有效（进行赋值或交换 ）
                if (enterTransform.childCount == 0) {   //直接赋值
                    //从源格子取出数据
                    Item item = ItemModel.GetItemByKey(fromTransform.name);
                    this.createNewItem(item, enterTransform);
                    //删除原来数据
                    ItemModel.DeleteItem(fromTransform.name);                                 
                }
                else {  //两装备交换 
                    //获取
                    Item from   = ItemModel.GetItemByKey(fromTransform.name);
                    Item to     = ItemModel.GetItemByKey(enterTransform.name);
                    //删除原来的数据
                    ItemModel.DeleteItem(fromTransform.name);
                    ItemModel.DeleteItem(enterTransform.name);
                    //交换
                    this.createNewItem(from, enterTransform);
                    this.createNewItem(to, fromTransform);
                }
            }
            else {  //无效（复原）---拖到UI的其他部分
                //从源格子取出数据
                Item item = ItemModel.GetItemByKey(fromTransform.name);               
                this.createNewItem(item, fromTransform);
            }
        }

    }
    #endregion

    private string GetTipText(Item item) {
        if (item == null)
            return "";
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("<color=red>{0}</color>\n\n", item.Name);
        switch (item.ItemType) {
            case "Armor":
                Armor armor = item as Armor;
                sb.AppendFormat("力量:{0}\n防御:{1}\n敏捷:{2}\n\n", armor.Power, armor.Defend, armor.Agility);
                break;
            case "Consumable":
                Consumable consumable = item as Consumable;
                sb.AppendFormat("HP:{0}\nMP:{1}\n\n", consumable.BackHp, consumable.BackMp);
                break;
            case "Weapon":
                Weapon weapon = item as Weapon;
                sb.AppendFormat("攻击:{0}\n\n", weapon.Damage);
                break;
            default:
                break;
        }
        sb.AppendFormat("<size=25><color=white>购买价格：{0}\n出售价格：{1}</color></size>\n\n<color=yellow><size=20>描述：{2}</size></color>", item.BuyPrice, item.SellPrice, item.Descriptionn);
        return sb.ToString();
    }

    private void createNewItem(Item item, Transform parent) {

        if (item == null) {
            return;
        }

        //动态加载方法
        GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Item");
 
        GameObject obj = GameObject.Instantiate(itemPrefab);            //创建
        //itemPrefab.GetComponent<ItemUI>().updateItem(item.Name);      //更新名称
        //itemPrefab.GetComponent<ItemUI>().updateImage(item.Image);    //更新图片
        obj.GetComponent<ItemUI>().updateItem(item.Name);               //更新名称

        obj.transform.SetParent(parent);

        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;

        //保存信息
        ItemModel.StoreItem(parent.name, item);
        Debug.LogWarning("Name : " + item.Name + " Des : " + item.Descriptionn);
    }

}
