using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemModel {

    private static Dictionary<string, Item> GridItem = new Dictionary<string, Item>();

    //存
    public static void StoreItem(string key, Item value) {

        Debug.Log("存储：" + key + " & " + value);
        if (GridItem.ContainsKey(key)) {    //如果已经存在===删除、更新
            DeleteItem(key);
        }
        GridItem.Add(key, value);
    }

    //取
    public static Item GetItemByKey(string key) {
        if (GridItem.ContainsKey(key)) {
            return GridItem[key];
        }
        else {
            return null;
        }
    }

    //删除
    public static void DeleteItem(string key) {
        if (GridItem.ContainsKey(key)) {
            GridItem.Remove(key);
        }
    }

}
