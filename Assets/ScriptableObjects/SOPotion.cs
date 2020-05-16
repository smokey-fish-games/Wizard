using UnityEngine;

[CreateAssetMenu(fileName ="Potion", menuName = "ScriptableObjects/Potion/Potion")]
public class SOPotion : ScriptableObject
{
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
}
