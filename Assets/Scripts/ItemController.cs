using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    int currentID = 1;

    GameController gc;

    static Dictionary<int, GameObject> trackedObjects = new Dictionary<int, GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        gc = GetComponent<GameController>();
        // register dev commands
        DeveloperConsole.instance.RegisterCommand("spawn", "[ID] (posistion) (rotation) Spawns an item of given ID.", devConsoleSpawn);
        DeveloperConsole.instance.RegisterCommand("edit", "[ID] [property] [value] Edits a specific spawned items property to the new value.", devConsoleEdit);
        DeveloperConsole.instance.RegisterCommand("delete", "[ID] Deletes a specific spawned item.", devConsoleDelete);

        // Look for items not intialized here?

    }

    public bool devConsoleSpawn(string[] parameters)
    {
        SOItem tospawn;

        Vector3 pos = gc.getCurrentCharPos();

        Quaternion rot = gc.getCurrentCharRot();

        switch(parameters.Length)
        {
            case 3:
                String[] rotcoms = parameters[2].Split(',');
                if(rotcoms.Length != 4)
                {
                    DeveloperConsole.instance.writeError("Incorrect rotation parameter. Expected x,x,x,x !");
                    return false;
                }
                int[] rotcomsint = new int[rotcoms.Length];
                for (int i = 0; i < rotcoms.Length; i++)
                {
                    if(!Int32.TryParse(rotcoms[i], out int temp))
                    {
                        DeveloperConsole.instance.writeError("Incorrect rotation parameter. Expected x,x,x,x !");
                        return false;
                    }
                    else
                    {
                        rotcomsint[i] = temp;
                    }
                }

                rot = new Quaternion(rotcomsint[0], rotcomsint[1], rotcomsint[2], rotcomsint[3]);
                //fallthrough
                goto case 2;
            case 2:
                String[] poscoms = parameters[1].Split(',');
                if (poscoms.Length != 3)
                {
                    DeveloperConsole.instance.writeError("Incorrect posistion parameter. Expected x,x,x !");
                    return false;
                }
                int[] poscomsint = new int[poscoms.Length];
                for (int i = 0; i < poscoms.Length; i++)
                {
                    if (!Int32.TryParse(poscoms[i], out int temp2))
                    {
                        DeveloperConsole.instance.writeError("Incorrect posistion parameter. Expected x,x,x !");
                        return false;
                    }
                    else
                    {
                        poscomsint[i] = temp2;
                    }
                }

                pos = new Vector3(poscomsint[0], poscomsint[1], poscomsint[2]);
                //fallthrough
                goto case 1;
            case 1:
                if (!Int32.TryParse(parameters[0], out int temp3))
                {
                    DeveloperConsole.instance.writeError("Incorrect ID parameter. Expected x!");
                    return false;
                }
                else
                {
                    tospawn = SOItem.GetByID(temp3);
                    if(tospawn == null)
                    {
                        DeveloperConsole.instance.writeError("Unknown ID parameter!");
                        return false;
                    }
                }
                break;
            
            default:
                DeveloperConsole.instance.writeError("Incorrect parameters!");
                return false;

        }

        int id = SpawnItem(tospawn, pos, rot);
        
        if(id != 0)
        {
            DeveloperConsole.instance.writeMessage("Spawned item " + tospawn.name +  " with ID " + id);
            return true;
        }
        else
        {
            DeveloperConsole.instance.writeError("Failed to spawn item!");
            return false;
        }
    }

    public bool devConsoleDelete(string[] parameters)
    {
        if(parameters.Length == 0)
        {
            DeveloperConsole.instance.writeError("Missing required ID parameter");
            return false;
        }

        if (!Int32.TryParse(parameters[0].Trim(), out int ID))
        {
            DeveloperConsole.instance.writeError("Unknown ID " + parameters[0]);
            return false;
        }
        else
        {
            if (!trackedObjects.ContainsKey(ID))
            {
                DeveloperConsole.instance.writeError("Unknown ID " + parameters[0]);
                return false;
            }

            return destroyItem(ID);
        }
    }

    public bool devConsoleEdit(string[] parameters)
    {
        if (parameters.Length != 3)
        {
            DeveloperConsole.instance.writeError("Missing required parameter(s)");
            return false;
        }

        if (!Int32.TryParse(parameters[0].Trim(), out int ID))
        {
            DeveloperConsole.instance.writeError("Unknown ID " + parameters[0]);
            return false;
        }
        else
        {
            if (!trackedObjects.ContainsKey(ID))
            {
                DeveloperConsole.instance.writeError("Unknown ID " + parameters[0]);
                return false;
            }

            return changeitemProperty(ID, parameters[1], parameters[2]);
        }
    }

    // Update items via console
    public bool changeitemProperty(int itemtochange, string property, string value)
    {
        property = property.Trim();
        value = value.Trim();

        if (property == "" || value == "")
        {
            return false;
        }

        if (!trackedObjects.ContainsKey(itemtochange))
        {
            return false;
        }

        IItem i = trackedObjects[itemtochange].GetComponentInChildren<IItem>();
        if(i == null)
        {
            return false;
        }
        else
        {
            i.setProperty(property, value);
        }
        return true;
    }

    // destroy items
    public static bool destroyItem(int idtodetroy)
    {
        if(!trackedObjects.ContainsKey(idtodetroy))
        {
            return false;
        }

        GameObject.Destroy(trackedObjects[idtodetroy]);
        trackedObjects.Remove(idtodetroy);
        return true;
    }

    public static void itemDestroyed(int itemgone)
    {
        trackedObjects.Remove(itemgone);
    }

    // Spawn items
    public int SpawnItem(SOItem toSpawn, Vector3 pos)
    {
        return SpawnItem(toSpawn, pos, new Quaternion(0, 0, 0, 0), new Dictionary<string, string>());
    }

    public int SpawnItem(SOItem toSpawn, Vector3 pos, Quaternion rot)
    {
        return SpawnItem(toSpawn, pos, rot, new Dictionary<string, string>());
    }

    public int SpawnItem(SOItem toSpawn, Vector3 pos, Dictionary<string, string> startingProperties)
    {
        return SpawnItem(toSpawn, pos, new Quaternion(0, 0, 0, 0), startingProperties);
    }
    public int SpawnItem(SOItem toSpawn, Vector3 pos, Quaternion rot, Dictionary<string,string> startingProperties)
    {
        int rc = 0;
        if(toSpawn == null)
        {
            Debug.LogWarning("tospawn null");
            return rc;
        }

        // one check
        if(toSpawn.ID == CONSTANTS.ITEM_FREESTAND_INGREDIENT)
        {
            if(!startingProperties.ContainsKey(CONSTANTS.CONTENTS_STRING))
            {
                Debug.LogError("Cannot create a Freestanding ingredient without contents");
                DeveloperConsole.instance.writeError("Cannot create a Freestanding ingredient without contents");
                return rc;
            }
        }

        GameObject GO = Instantiate(toSpawn.model, pos, rot);
        IItem i = GO.GetComponentInChildren<IItem>();
        if (i != null)
        {
            i.uniqueID = currentID;
            string dictionaryString = "";
            foreach (string s in startingProperties.Keys)
            {
                dictionaryString += s + "=" + startingProperties[s] + " ";
                i.setProperty(s, startingProperties[s]);
            }
            Debug.Log("Spawned Item " + toSpawn.name + " with ID " + i.uniqueID + " at " + pos + " with rotation " + rot + " and properties " + dictionaryString);
            rc = currentID;
        }
        trackedObjects.Add(currentID, GO);
        currentID++;
        return rc;
    }
}
