using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/Potion/Ingredient")]
public class SOIngredient : ScriptableObject
{
    public int ID;

    public void DebugPrint()
    {
        Debug.Log(GetDebugString());
    }

    public string GetDebugString()
    {
        return "Ingredient | ID=" + ID + " Name=" + name;
    }

    public string printString()
    {
        return "I:" + name + "(" + ID + ")";
    }
}
