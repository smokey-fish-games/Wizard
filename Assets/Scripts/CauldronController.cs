using System;
using UnityEditor;
using UnityEngine;

public class CauldronController : MonoBehaviour, IItem
{
    public int uniqueID { get; set; } = 0;
    public Renderer r;
    public SOPotion contents;
    // Start is called before the first frame update
    void Start()
    {
        //Set random potion contents
        if (contents == null)
        {
            contents = SOPotion.getByID(0);
        }
        setProperty("contents", contents.ID.ToString());
    }

    public SOPotion getContents()
    {
        return contents;
    }

    public void refreshContentGraphic()
    {
        r.material.SetColor("_potionColor", contents.color);
        r.enabled = (contents.ID != 0); // 0 = empty
    }

    public bool isEmpty()
    {
        return (contents.ID == 0);
    }

    // Interface functions
    public bool setProperty(string property, string value)
    {
        if(property.Trim() == "contents")
        {
            int potionID;
            if(Int32.TryParse(value.Trim(), out potionID))
            {
                SOPotion newCon =  SOPotion.getByID(potionID);
                if(newCon == null)
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
        if (with.GetType() is IItem)
        {
            // What are you pushing onto this cauldron?
            Debug.LogWarning("Interaction for " + this.name + " and " + with.name + " detected!");
            throw new System.NotImplementedException();
        }
        else
        {
            //Unknown
            Debug.LogError("Unknown interaction for " + this.name + " with " + with.name);
        }
        return false ;
    }

    public bool isPickupable()
    {
        return false;
    }

    public string getPropertyValue(string property)
    {
        if (property.Trim() == "contents")
        {
            return contents.ID.ToString();
        }
        return "";
    }
}
