using UnityEngine;
public class DrainController : Container
{
    public override int uniqueID { get; set; }
    public ContainerFiller.INGREDIENTTYPE[] acceptsTypes;

    // Start is called before the first frame update
    void Awake()
    {
        if (acceptsTypes == null || acceptsTypes.Length == 0)
        {
            Debug.LogError("Accepting TYPE NOT SET! " + this.name);
        }
        MaxCapacity = 1;
        canBeUsedInHand = false;
        canBeUsedInWorld = false;
        canBePickedUp = false;
        container = true;
        usedOnWorldObject = false;
    }


    public override bool EmptyContent(ContainerFiller item)
    {
        return true;
    }

    public override bool AddToContainer(ContainerFiller item)
    {
        // Just takes it if it's a liquid
        bool good = false;
        foreach(ContainerFiller.INGREDIENTTYPE c in acceptsTypes)
        {
            if(c == item.thistype)
            {
                good = true;
                break;
            }
        }
        if (!good)
        {
            return false;
        }
        return true;
    }

    public override ContainerFiller[] GetContents()
    {
        return new ContainerFiller[0];
    }

    public override bool IsEmpty()
    {
        // Always empty and ready for more
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

    public override bool IsFull()
    {
        // Can never be full as it deletes objects
        return false;
    }

    public override bool setProperty(string property, string value)
    {
        // Nothing
        return false;
    }

    public override string getPropertyValue(string property)
    {
        // Nothing
        return "";
    }

    public override bool UseObjectOnObject(Interactable target)
    {
        return false;
    }
}
