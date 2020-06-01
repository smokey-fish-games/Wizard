using System;
using UnityEngine;

public class ingredientProcessorController : Container
{
    ContainerFiller content = null;
    public bool canHandleDryable = false;
    public bool canHandleGrindable = false;
    void Awake()
    {
        MaxCapacity = 1;
        canBeUsedInHand = false;
        canBeUsedInWorld = false;
        canBePickedUp = false;
        container = true;
        usedOnWorldObject = false;

        if(!canHandleDryable && !canHandleGrindable)
        {
            Debug.LogError("Error this item is useless! ");
        }
        if (canHandleDryable && canHandleGrindable)
        {
            Debug.LogError("Error this item can do too much stuff!");
        }
    }
    public override bool AddToContainer(ContainerFiller item)
    {
        if(!IsEmpty())
        {
            return false;
        }
        if(canHandleDryable && item.isDryable())
        {
            content = item.onDryProcess;
        }
        else if(canHandleGrindable && item.isGrindable())
        {
            content = item.onGrindProcess;
        }
        else
        {
            return false;
        }
        
        return true;
    }

    public override bool EmptyContent(ContainerFiller item)
    {
        if (IsEmpty())
        {
            return false;
        }
        else if(item.ID != content.ID)
        {
            return false;
        }
        content = null;
        return true;
    }

    public override ContainerFiller[] GetContents()
    {
        if (IsEmpty())
        {
            return new ContainerFiller[0];
        }
        else
        {
            return new ContainerFiller[1] { content };
        }
    }

    public override bool IsEmpty()
    {
        return (content == null);
    }

    public override bool IsFull()
    {
        return (content != null);
    }

    public override bool PickupObject()
    {
        return false;
    }
    public override bool UseObject(IEffectable user)
    {
        return false;
    }
    public override bool UseObjectOnObject(Interactable target)
    {
        // Can't be picked up so shrug
        return false;
    }

    public override string getPropertyValue(string property)
    {
        if (property.Trim() == CONSTANTS.CONTENTS_STRING)
        {
            return content.ID.ToString();
        }
        return "";
    }

    public override bool setProperty(string property, string value)
    {
        if (property.Trim() == CONSTANTS.CONTENTS_STRING)
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
                    content = null;
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

    
}
