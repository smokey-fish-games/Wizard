using System;
using System.Collections.Generic;
using UnityEngine;

public class bucketController : Container
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
        if (potionTexture == null)
        {
            Debug.LogError("POTION TEXTURE NULL FOR " + gameObject.name);
        }

        MaxCapacity = 100;
        canBeUsedInHand = false;
        canBeUsedInWorld = false;
        canBePickedUp = true;
        container = true;
        usedOnWorldObject = true;
        refreshContentGraphic();
    }

    public void refreshContentGraphic()
    {
        checkContentsNotNull();
        if (!IsEmpty())
        {
            Color mix = new Color();
            bool first = true;
            foreach (ContainerFiller cooo in contents)
            {
                if (first)
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
        if (IsFull())
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
            foreach (ContainerFiller c in contents)
            {
                if (!first)
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
        return true;
    }

    public override bool UseObjectOnObject(Interactable target)
    {
        if (target == null)
        {
            return false;
        }
        if (target.IsContainer())
        {
            return MoveContents(this, (Container)target);
        }
        return false;
    }
}
