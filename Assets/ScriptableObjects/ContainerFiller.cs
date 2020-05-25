using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/Potion/Ingredient")]
public class ContainerFiller : ScriptableObject
{
    public int ID;
    public INGREDIENTTYPE thistype;
    public enum INGREDIENTTYPE { LIQUID, GAS, SOLID };
    public Color color;
    public SOEffect onConsumeEffect;
    public GameObject onGroundModel;
    public Texture2D texture;
    public bool potion = false;
    public bool ingredient = false;

    private static ContainerFiller[] allPots = new ContainerFiller[0];

    public static ContainerFiller GetByID(int id)
    {
        ContainerFiller[] all = GetAll();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].ID == id)
            {
                return all[i];
            }
        }
        return null;
    }

    public static ContainerFiller[] GetAll()
    {
        lock (allPots)
        {
            if (allPots.Length == 0)
            {
                string[] potionGUIDs = AssetDatabase.FindAssets("t:ContainerFiller");
                allPots = new ContainerFiller[potionGUIDs.Length];
                for (int i = 0; i < potionGUIDs.Length; i++)
                {
                    allPots[i] = (ContainerFiller)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(potionGUIDs[i]), typeof(ContainerFiller));
                }
            }
        }
        return allPots;
    }

    public static ContainerFiller[] GetAllByType(INGREDIENTTYPE typetoget)
    {
        ContainerFiller[] all = GetAll();
        List<ContainerFiller> toreturn = new List<ContainerFiller>();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].thistype == typetoget)
            {
                toreturn.Add(all[i]);
            }
        }
        return toreturn.ToArray();
    }

    public static ContainerFiller[] GetAllIngredients()
    {
        ContainerFiller[] all = GetAll();
        List<ContainerFiller> toreturn = new List<ContainerFiller>();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].isIngredient())
            {
                toreturn.Add(all[i]);
            }
        }
        return toreturn.ToArray();
    }

    public static ContainerFiller[] GetAllPotions()
    {
        ContainerFiller[] all = GetAll();
        List<ContainerFiller> toreturn = new List<ContainerFiller>();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].isPotion())
            {
                toreturn.Add(all[i]);
            }
        }
        return toreturn.ToArray();
    }

    public static ContainerFiller[] GetAllIngredientsByType(INGREDIENTTYPE typetoget)
    {
        ContainerFiller[] all = GetAll();
        List<ContainerFiller> toreturn = new List<ContainerFiller>();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].isIngredient() && all[i].thistype == typetoget)
            {
                toreturn.Add(all[i]);
            }
        }
        return toreturn.ToArray();
    }

    public static ContainerFiller[] GetAllPotionsByType(INGREDIENTTYPE typetoget)
    {
        ContainerFiller[] all = GetAll();
        List<ContainerFiller> toreturn = new List<ContainerFiller>();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].isPotion() && all[i].thistype == typetoget)
            {
                toreturn.Add(all[i]);
            }
        }
        return toreturn.ToArray();
    }




    public void DebugPrint()
    {
        Debug.Log(GetDebugString());
    }

    public string GetDebugString()
    {
        return "ContainerFiller | ID=" + ID + " Name=" + name + " INGREDIENTTYPE=" + thistype + " Color =" + color + " OnConsumeEffect=" + onConsumeEffect.PrintString();
    }

    public string PrintString()
    {
        return "CF:" + name + "(" + ID + ")";
    }

    public bool isPotion()
    {
        return potion;
    }

    public bool isIngredient()
    {
        return ingredient;
    }
}

