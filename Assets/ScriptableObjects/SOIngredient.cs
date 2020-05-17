using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/Potion/Ingredient")]
public class SOIngredient : ScriptableObject
{
    public enum INGREDIENTTYPE { LIQUID, SOLID, GAS };
    public int ID;
    public INGREDIENTTYPE ingredientType;
    private static SOIngredient[] allPots = new SOIngredient[0];

    public void DebugPrint()
    {
        Debug.Log(GetDebugString());
    }

    public string GetDebugString()
    {
        return "Ingredient | ID=" + ID + " Name=" + name + "Type=" + ingredientType;
    }

    public string printString()
    {
        return "I:" + name + "(" + ID + ")";
    }

    public static SOIngredient getByID(int id)
    {
        SOIngredient[] all = getAll();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].ID == id)
            {
                return all[i];
            }
        }
        return null;
    }

    public static SOIngredient[] getAll()
    {
        lock (allPots)
        {
            if (allPots.Length == 0)
            {
                string[] potionGUIDs = AssetDatabase.FindAssets("t:SOIngredient");
                allPots = new SOIngredient[potionGUIDs.Length];
                for (int i = 0; i < potionGUIDs.Length; i++)
                {
                    allPots[i] = (SOIngredient)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(potionGUIDs[i]), typeof(SOIngredient));
                }
            }
        }

        return allPots;
    }
}
