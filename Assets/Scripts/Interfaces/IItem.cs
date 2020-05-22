using UnityEngine;

public interface IItem 
{
    bool setProperty(string property, string value);

    string getPropertyValue(string property);
    int uniqueID { get; set; }
}
