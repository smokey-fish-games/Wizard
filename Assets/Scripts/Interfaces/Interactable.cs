using UnityEngine;
using UnityEngine.Assertions;

public abstract class Interactable : IItem
{
    public bool canBeUsedInHand = false;
    public bool canBeUsedInWorld = false;
    public bool usedOnWorldObject = false;

    public abstract bool UseObject(IEffectable user);
    public abstract bool UseObjectOnObject(Interactable target);
    public abstract bool PickupObject();

    public bool IsHandUseable()
    {
        return canBeUsedInHand;
    }

    public bool IsWorldUseable()
    {
        return canBeUsedInWorld;
    }

    public bool IsUsedOnWorldObjects()
    {
        return usedOnWorldObject;
    }

    private void Start()
    {
        Assert.IsFalse(canBeUsedInWorld && canBeUsedInHand);
    }

   public bool IsContainer()
   {
       return container;
   }
}
