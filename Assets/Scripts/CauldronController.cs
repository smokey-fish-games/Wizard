using System;
using System.Collections.Generic;
using UnityEngine;

public class CauldronController : Container
{
    public List<ContainerFiller> contents = new List<ContainerFiller>();
    FillerRenderer fr;
    public Texture2D potionTexture;
    public override int uniqueID { get; set; }
    // Start is called before the first frame update
    void Awake()
    {
        fr = GetComponent<FillerRenderer>();
        if (fr == null)
        {
            Debug.LogError("FILLER RENDERED NULL FOR " + gameObject.name);
        }

        if(potionTexture == null)
        {
            Debug.LogError("POTION TEXTURE NULL FOR " + gameObject.name);
        }

        MaxCapacity = 10;
        canBeUsedInHand = false;
        canBeUsedInWorld = false;
        canBePickedUp = false;
        container = true;
        usedOnWorldObject = false;
        refreshContentGraphic();
    }

    public void refreshContentGraphic()
    {
        checkContentsNotNull();
        if (!IsEmpty())
        {
            Color mix = new Color();
            bool first = true;
            foreach(ContainerFiller cooo in contents)
            {
                if(first)
                {
                    mix = cooo.color;
                    first = false;
                }
                else
                {
                    mix += cooo.color;
                }
            }

            fr.setContents(contents[0].color, potionTexture);
        }
        fr.showContents(!IsEmpty());
    }
    void checkContentsNotNull()
    {
        contents.RemoveAll(item => item == null);
    }

    public override bool AddToContainer(ContainerFiller item)
    {
        if(item == null)
        {
            return false;
        }
        if (IsFull())
        {
            return false;
        }
        switch(item.thistype)
        {
            case ContainerFiller.INGREDIENTTYPE.LIQUID:
                contents.Add(item);
                break;
            case ContainerFiller.INGREDIENTTYPE.SOLID:
                if (IContainLiquid())
                {
                    // can add a solid if liquid is there already;
                    contents.Add(item);
                }
                else
                {
                    return false;
                }
                break;
            default:
                return false;
        }

        refreshContentGraphic();
        return true;
    }

    public bool IContainLiquid()
    {
        foreach(ContainerFiller c in contents)
        {
            if(c.thistype == ContainerFiller.INGREDIENTTYPE.LIQUID)
            {
                return true;
            }
        }
        return false;
    }

    public override ContainerFiller[] GetContents()
    {
        return contents.ToArray();
    }

    public override bool IsEmpty()
    {
        return (contents.Count == 0);
    }

    public override bool IsFull()
    {
        return (contents.Count >= MaxCapacity);
    }

    void wipeContents()
    {
        contents.Clear();
        refreshContentGraphic();
    }

    public override string getPropertyValue(string property)
    {
        if (property.Trim() == CONSTANTS.CONTENTS_STRING)
        {
            string toGoback = "";
            bool first = true;
            foreach(ContainerFiller c in contents)
            {
                if(!first)
                {
                    toGoback += ",";
                    first = true;
                }
                toGoback += c.ID.ToString();
            }
            return toGoback;
        }
        return "";
    }
    public override bool setProperty(string property, string value)
    {
        if (property.Trim() == CONSTANTS.CONTENTS_STRING)
        {
            //It might be #,#,#,#
            string[] splitted = value.Trim().Split(',');
            if (splitted.Length == 0)
            {
                DeveloperConsole.instance.writeError("Unable to parse value to set property to!");
                return false;
            }
            bool firstgo = true;
            for (int i = 0; i < splitted.Length; i++)
            {
                if (Int32.TryParse(splitted[i], out int potionID))
                {
                    ContainerFiller newCon = ContainerFiller.GetByID(potionID);
                    if (newCon == null)
                    {
                        return false;
                    }
                    else
                    {
                        if (firstgo)
                        {
                            wipeContents();
                            firstgo = false;
                        }
                        if (!AddToContainer(newCon))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    public override bool EmptyContent(ContainerFiller item)
    {
        contents.Remove(item);
        refreshContentGraphic();
        return true;
    }

    public override bool UseObject(IEffectable user)
    {
        return false;
    }

    public override bool PickupObject()
    {
        return false;
    }

    public override bool UseObjectOnObject(Interactable target)
    {
        // Can't be picked up so shrug
        return false;
    }
}
