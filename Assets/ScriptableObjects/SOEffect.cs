using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Potion/Effect")]
public class SOEffect : ScriptableObject
{
    public int ID;

    public void DebugPrint()
    {
        Debug.Log(GetDebugString());
    }

    public string GetDebugString()
    {
        return "Effect | ID=" + ID + " Name=" + name;
    }

    public string printString()
    {
        return "E:" + name + "(" + ID + ")";
    }

}
