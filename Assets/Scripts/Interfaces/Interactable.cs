using UnityEngine;
using UnityEngine.Assertions;

public abstract class Interactable : MonoBehaviour, IItem
{
    public bool canBeUsedInHand = false;
    public bool canBeUsedInWorld = false;
    public bool canBePickedUp = false;
    public bool container = false;

    public abstract int uniqueID { get; set; }

    public abstract bool UseObject(IEffectable user);
    public abstract bool PickupObject();

    public bool IsPickupable()
    {
        return canBePickedUp;
    }
    
    public bool IsHandUseable()
    {
        return canBeUsedInHand;
    }
    public bool IsWorldUseable()
    {
        return canBeUsedInWorld;
    }
    private void Start()
    {
        Assert.IsFalse(canBeUsedInWorld && canBeUsedInHand);
    }

   public bool IsContainer()
   {
       return container;
   }

    public abstract bool setProperty(string property, string value);
    public abstract string getPropertyValue(string property);
}
