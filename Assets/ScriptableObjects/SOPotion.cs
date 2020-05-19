using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="Potion", menuName = "ScriptableObjects/Potion/Potion")]
public class SOPotion : ScriptableObject
{
    private static SOPotion[] allPots = new SOPotion[0];

    public int ID;
    public Color color;
    public SOEffect onDrinkEffect;

    public SOIngredient[] ingredients;


    public void DebugPrint()
    {
        Debug.Log(GetDebugString());
    }
    public string GetDebugString()
    {
        string printer = "Potion | ID=" + ID + " Name=" + name + " Ingredients=[";
        int isa = 0;
        foreach (SOIngredient i in ingredients)
        {
            if (isa != 0)
            {
                printer += ", ";
            } else
            {
                isa++;
            }
            printer += i.printString();
        }
        printer += "] Color=" +color + " OnDrinkEffect=" + onDrinkEffect.printString();
        return printer;
    }

    public string printString()
    {
        return "P:" + name + "(" + ID + ")";
    }

    public static SOPotion getByID(int id)
    {
        SOPotion[] all = getAll();
        for(int i = 0; i < all.Length; i++)
        {
            if(all[i].ID == id)
            {
                return all[i];
            }
        }
        return null;
    }

    public static SOPotion[] getAll()
    {
        lock (allPots)
        {
            if (allPots.Length == 0)
            {
                string[] potionGUIDs = AssetDatabase.FindAssets("t:SOPotion");
                allPots = new SOPotion[potionGUIDs.Length];
                for (int i = 0; i < potionGUIDs.Length; i++)
                {
                    allPots[i] = (SOPotion)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(potionGUIDs[i]), typeof(SOPotion));
                }
            }
        }
        return allPots;
    }
}
