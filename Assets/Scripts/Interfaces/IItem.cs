using UnityEngine;

public interface IItem 
{
    bool setProperty(string property, string value);

    string getPropertyValue(string property);
    bool interact(GameObject with);
    bool isPickupable();
    int uniqueID { get; set; }
}
