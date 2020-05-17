using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/World/Item")]
public class SOItem : ScriptableObject
{
    private static SOItem[] allPots = new SOItem[0];
    public int ID;
    public GameObject model;


    public void DebugPrint()
    {
        Debug.Log(GetDebugString());
    }

    public string GetDebugString()
    {
        return "Item | ID=" + ID + " Name=" + name + " Model=" + model.name;
    }

    public string printString()
    {
        return "I:" + name + "(" + ID + ")";
    }

    public static SOItem getByID(int id)
    {
        SOItem[] all = getAll();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].ID == id)
            {
                return all[i];
            }
        }
        return null;
    }

    public static SOItem[] getAll()
    {
        lock (allPots)
        {
            if (allPots.Length == 0)
            {
                string[] potionGUIDs = AssetDatabase.FindAssets("t:SOItem");
                allPots = new SOItem[potionGUIDs.Length];
                for (int i = 0; i < potionGUIDs.Length; i++)
                {
                    allPots[i] = (SOItem)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(potionGUIDs[i]), typeof(SOItem));
                }
            }
        }
        return allPots;
    }
}
