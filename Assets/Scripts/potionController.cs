using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class potionController : MonoBehaviour, IItem
{
    public int uniqueID { get; set; } = 0;
    public Renderer r;
    public SOPotion contents;
    // Start is called before the first frame update
    void Start()
    {
        if (contents == null)
        {
            //Set random potion contents
            SOPotion[] allpos = SOPotion.getAll();
            int chosen = UnityEngine.Random.Range(0, allpos.Length);
            setProperty("contents", allpos[chosen].ID.ToString());
            Debug.Log("Auto setting potion for " + this.gameObject.name + " to: " + contents.name);
        }
        else
        {
            setProperty("contents", contents.ID.ToString());
        }
    }

    public void emptyBottle()
    {
        setProperty("contents", "0");
    }

    public bool isEmpty()
    {
        return (contents.ID == 0);
    }

    public void refreshContentGraphic()
    {
        r.material.SetColor("_potionColor", contents.color);
        r.enabled = (contents.ID != 0); // 0 = empty
    }

    // Interface functions
    public bool setProperty(string property, string value)
    {
        if (property.Trim() == "contents")
        {
            int potionID;
            if (Int32.TryParse(value.Trim(), out potionID))
            {
                SOPotion newCon = SOPotion.getByID(potionID);
                if (newCon == null)
                {
                    return false;
                }
                else
                {
                    contents = newCon;
                    refreshContentGraphic();
                }
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public bool interact(GameObject with)
    {

        throw new System.NotImplementedException();
    }

    public bool isPickupable()
    {
        return true;
    }

    public string getPropertyValue(string property)
    {
        throw new System.NotImplementedException();
    }
}
