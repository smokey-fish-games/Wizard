using System;
using System.Collections.Generic;
using UnityEngine;

public class potionController : Container
{
    public Renderer r;
    public List<ContainerFiller> contents = new List<ContainerFiller>();

    public override int uniqueID { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        MaxCapacity = 1;
        canBeUsedInHand = true;
        canBeUsedInWorld = false;
        canBePickedUp = true;
        container = true;
        refreshContentGraphic();
    }

    void checkContentsNotNull()
    {
        contents.RemoveAll(item => item == null);
    }

    public void refreshContentGraphic()
    {
        checkContentsNotNull();
        if (!IsEmpty())
        {
            r.material.SetColor("_potionColor", contents[0].color);
        }
        r.enabled = !IsEmpty();
    }

    public override bool EmptyContent(ContainerFiller item)
    {
        contents.Remove(item);
        refreshContentGraphic();
        return true;
    }

    public override bool AddToContainer(ContainerFiller item)
    {
        if(IsFull())
        {
            return false;
        }
        if (item.thistype != ContainerFiller.INGREDIENTTYPE.LIQUID)
        {
            return false;
        }
        contents.Add(item);
        refreshContentGraphic();
        return true; 
    }

    public override ContainerFiller[] GetContents()
    {
        return contents.ToArray();
    }

    public override bool IsEmpty()
    {
        return (contents.Count == 0);
    }

    public override bool UseObject(IEffectable user)
    {
        if (!IsEmpty())
        {
            contents[0].onConsumeEffect.onEffect(user);
            EmptyContent(contents[0]);
        }
        return true;
    }

    public override bool PickupObject()
    {
        return true;
    }

    public override bool IsFull()
    {
        return (contents.Count == MaxCapacity);
    }

    void wipeContents()
    {
        contents.Clear();
        refreshContentGraphic();
    }

    public override bool setProperty(string property, string value)
    {
        if (property.Trim() == "contents")
        {
            if (Int32.TryParse(value.Trim(), out int potionID))
            {
                ContainerFiller newCon = ContainerFiller.GetByID(potionID);
                if (newCon == null)
                {
                    return false;
                }
                else
                {
                    if (!IsEmpty())
                    {
                        wipeContents();
                    }
                    return AddToContainer(newCon);
                }
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public override string getPropertyValue(string property)
    {
        if (property.Trim() == "contents")
        {
            return contents[0].ID.ToString();
        }
        return "";
    }
}
