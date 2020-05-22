using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Potion/Effect")]
public class SOEffect : ScriptableObject
{
    private static SOEffect[] allPots = new SOEffect[0];
    public int ID;
    public Func<IEffectable, bool> onEffect;

    public void DebugPrint()
    {
        Debug.Log(GetDebugString());
    }

    public string GetDebugString()
    {
        return "Effect | ID=" + ID + " Name=" + name;
    }

    public string PrintString()
    {
        return "E:" + name + "(" + ID + ")";
    }

    public static SOEffect GetByID(int id)
    {
        SOEffect[] all = GetAll();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].ID == id)
            {
                return all[i];
            }
        }
        return null;
    }

    public static SOEffect[] GetAll()
    {
        lock (allPots)
        {
            if (allPots.Length == 0)
            {
                string[] potionGUIDs = AssetDatabase.FindAssets("t:SOEffect");
                allPots = new SOEffect[potionGUIDs.Length];
                for (int i = 0; i < potionGUIDs.Length; i++)
                {
                    allPots[i] = (SOEffect)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(potionGUIDs[i]), typeof(SOEffect));
                }
            }
        }

        return allPots;
    }
}
