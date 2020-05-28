using System.Linq;
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

    public string PrintString()
    {
        return "I:" + name + "(" + ID + ")";
    }

    public static SOItem GetByID(int id)
    {
        SOItem[] all = GetAll();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].ID == id)
            {
                return all[i];
            }
        }
        return null;
    }

    public static SOItem[] GetAll()
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
                allPots = allPots.OrderBy(co => co.ID).ToArray();
            }

        }
        return allPots;
    }
}
