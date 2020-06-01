using System;
using System.Collections.Generic;
using UnityEngine;

public class potionController : Container
{
    public List<ContainerFiller> contents = new List<ContainerFiller>();
    FillerRenderer fr;
    public Texture2D potionTexture;

    // Start is called before the first frame update
    void Awake()
    {
        fr = GetComponent<FillerRenderer>();
        if (fr == null)
        {
            Debug.LogError("FILLER RENDERED NULL FOR " + gameObject.name);
        }

        if (potionTexture == null)
        {
            Debug.LogError("POTION TEXTURE NULL FOR " + gameObject.name);
        }

        MaxCapacity = 1;
        canBeUsedInHand = true;
        canBeUsedInWorld = false;
        canBePickedUp = true;
        container = true;
        usedOnWorldObject = true;
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
            fr.setContents(contents[0].color, potionTexture);
        }
        fr.showContents(!IsEmpty());
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
            contents[0].onConsumeEffect.onEffect(user, contents[0].potency);
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
        if (property.Trim() == CONSTANTS.CONTENTS_STRING)
        {
            return contents[0].ID.ToString();
        }
        return "";
    }


    public override bool UseObjectOnObject(Interactable target)
    {
        if(target == null)
        {
            return false;
        }
        if(target.IsContainer())
        {
            return MoveContents(this, (Container)target);
        }
        return false;
    }
}
