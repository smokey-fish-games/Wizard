using System;
using UnityEngine;

public class FreeStandingIngredientController : Interactable
{
    // Start is called before the first frame update
    FillerRenderer fr;
    ContainerFiller thisCF;

    public override bool setProperty(string property, string value)
    {
        if (property.Trim() == CONSTANTS.CONTENTS_STRING)
        {
            if (Int32.TryParse(value.Trim(), out int ingredientID))
            {
                ContainerFiller newCon = ContainerFiller.GetByID(ingredientID);
                if (newCon == null)
                {
                    return false;
                }
                else
                {
                    if(newCon.thistype != ContainerFiller.INGREDIENTTYPE.SOLID)
                    {
                        return false;
                    }
                    thisCF = newCon;
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

    public override string getPropertyValue(string property)
    {
        if (property.Trim() == CONSTANTS.CONTENTS_STRING)
        {
            if (thisCF != null)
            {
                return thisCF.ID.ToString();
            }
            else
            {
                return "";
            }
        }
        return "";
    }

    public override bool PickupObject()
    {
        return true;
    }

    public override bool UseObject(IEffectable user)
    {
        return false;
    }

    public void refreshContentGraphic()
    {
        if(thisCF != null)
        {
            fr.setContents(thisCF.color, thisCF.texture);
        }
            
        fr.showContents(fr != null);
    }

    private void Awake()
    {
        fr = GetComponent<FillerRenderer>();
        if (fr == null)
        {
            Debug.LogError("FILLER RENDERED NULL FOR " + gameObject.name);
        }

        canBeUsedInHand = false;
        canBeUsedInWorld = false;
        canBePickedUp = true;
        container = false;
    }

    public override bool UseObjectOnObject(Interactable target)
    {
        if(thisCF == null)
        {
            return false;
        }
        if(target.IsContainer())
        {
            Container caaa = (Container)target;
            if (caaa.AddToContainer(thisCF))
            {
                // it went in time to go bye bye
                Destroy(transform.root.gameObject);
                return true;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public ContainerFiller getContents()
    {
        return thisCF;
    }

    private void OnDestroy()
    {
        ItemController.itemDestroyed(this.uniqueID);
    }
}
