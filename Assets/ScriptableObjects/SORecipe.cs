using UnityEngine;
using UnityEditor;
using System.Linq;

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
                    allRecipes[i].ingredients = allRecipes[i].ingredients.OrderBy(x => x.ID).ToArray();
                }
                allRecipes = allRecipes.OrderBy(co => co.ID).ToArray();
            }
        }
        return allRecipes;
    }


    public static SORecipe findMatchingRecipe(ContainerFiller[] ingredients)
    {
        if(ingredients == null)
        {
            return null;
        }
        int inglen = ingredients.Length;
        if(inglen < 2)
        {
            return null;
        }

        // Sort by ID as the recipes ingredients list will have been sorted as such
        ingredients = ingredients.OrderBy(co => co.ID).ToArray();
        
        foreach (SORecipe s in GetAll())
        {
            if(inglen == s.ingredients.Length)
            {
                //maybe
                bool match = true;
                for(int i = 0; i < inglen; i++)
                {
                    if(s.ingredients[i].ID != ingredients[i].ID)
                    {
                        // nope
                        match = false;
                        break;
                    }
                }
                if(match)
                {
                    return s;
                }
            }
        }
        return null;
    }
}
