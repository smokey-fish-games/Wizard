using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Potion/Recipe")]
public class SORecipe : ScriptableObject
{
    public int ID;
    public ContainerFiller result;
    public ContainerFiller[] ingredients;

    private static SORecipe[] allRecipes = new SORecipe[0];


    public string GetDebugString()
    {
        string printer = "Recipe | ID=" + ID + " Result=" + result.PrintString() + " Ingredients=[";
        for(int i = 0; i < ingredients.Length; i++)
        {
            if(i != 0)
            {
                printer += ", ";
            }
            printer += ingredients[i].PrintString();
        }
        printer += "]";

        return printer;
    }

    public string PrintString()
    {
        return "R:" + name + "(" + ID + ")";
    }

    public static SORecipe GetByID(int id)
    {
        SORecipe[] all = GetAll();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].ID == id)
            {
                return all[i];
            }
        }
        return null;
    }

    public static SORecipe[] GetAll()
    {
        lock (allRecipes)
        {
            if (allRecipes.Length == 0)
            {
                string[] potionGUIDs = AssetDatabase.FindAssets("t:SORecipe");
                allRecipes = new SORecipe[potionGUIDs.Length];
                for (int i = 0; i < potionGUIDs.Length; i++)
                {
                    allRecipes[i] = (SORecipe)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(potionGUIDs[i]), typeof(SORecipe));
                }
            }
        }
        return allRecipes;
    }
}
