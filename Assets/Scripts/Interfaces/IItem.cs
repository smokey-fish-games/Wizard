using UnityEngine;

public abstract class IItem : MonoBehaviour
{
    public bool canBePickedUp = false;
    public bool container = false;
    public abstract bool setProperty(string property, string value);

    public abstract string getPropertyValue(string property);
    public int uniqueID { get; set; }

    public void setPickupable(bool pickup)
    {
        canBePickedUp = pickup;
    }
    public bool IsPickupable()
    {
        return canBePickedUp;
    }
}
