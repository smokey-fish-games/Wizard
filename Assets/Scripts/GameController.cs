using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject characterPrefab;
    public Transform characterspawner;
    GameObject character;
    public GameObject todelete;

    ItemController ic;
    // Start is called before the first frame update
    void Start()
    {
        if(characterspawner == null)
        {
            Debug.LogError("SPAWNER IS NULL!!!");
            Application.Quit(1);
        }

        if (characterPrefab == null)
        {
            Debug.LogError("CHARACTER PREFAB IS NULL!!!");
            Application.Quit(1);
        }
                PrintSOs();
        TestSOs();
        GameObject.Destroy(todelete);
        respawnCharacter();

        GameEvents.current.onPlayerDeath += onPlayerDeath;

        DeveloperConsole.instance.RegisterCommand("kill", "Kills the current player dead.", killPlayer);
        ic = GetComponent<ItemController>();

        // Spawn 6 cauldrons
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("contents", "0");
            // this one has set contents not random
            ic.SpawnItem(SOItem.getByID(1), new Vector3(11.54006f, 1.28f, 3f), di);
        }
        
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("contents", "1");
            // this one has set contents not random
            ic.SpawnItem(SOItem.getByID(1), new Vector3(11.54006f, 1.28f, 0f), di);
        }

        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("contents", "2");
            // this one has set contents not random
            ic.SpawnItem(SOItem.getByID(1), new Vector3(11.54006f, 1.28f, -3f), di);
        }

        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("contents", "3");
            // this one has set contents not random
            ic.SpawnItem(SOItem.getByID(1), new Vector3(11.54006f, 1.28f, -6f), di);
        }
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("contents", "4");
            // this one has set contents not random
            ic.SpawnItem(SOItem.getByID(1), new Vector3(11.54006f, 1.28f, -9f), di);
        }
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("contents", "5");
            // this one has set contents not random
            ic.SpawnItem(SOItem.getByID(1), new Vector3(11.54006f, 1.28f, -12f), di);
        }


        // Spawn 6 bottles
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("contents", "0");
            // this one has set contents not random
            ic.SpawnItem(SOItem.getByID(0), new Vector3(7f, 1.264f, 6.338f), di);
        }
        
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("contents", "1");
            // this one has set contents not random
            ic.SpawnItem(SOItem.getByID(0), new Vector3(7.5f, 1.264f, 6.338f), di);
        }
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("contents", "2");
            // this one has set contents not random
            ic.SpawnItem(SOItem.getByID(0), new Vector3(8f, 1.264f, 6.338f), di);
        }
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("contents", "3");
            // this one has set contents not random
            ic.SpawnItem(SOItem.getByID(0), new Vector3(8.5f, 1.264f, 6.338f), di);
        }
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("contents", "4");
            // this one has set contents not random
            ic.SpawnItem(SOItem.getByID(0), new Vector3(9f, 1.264f, 6.338f), di);
        }
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("contents", "5");
            // this one has set contents not random
            ic.SpawnItem(SOItem.getByID(0), new Vector3(9.5f, 1.264f, 6.338f), di);
        }

    }

    public Vector3 getCurrentCharPos()
    {
        return character.transform.position + character.transform.forward * 3;
    }

    public Quaternion getCurrentCharRot()
    {
        return character.transform.rotation;
    }

    void respawnCharacter()
    {
        if(character != null)
        {
            GameObject.Destroy(character);
        }

        Debug.Log("Spawning character on " + characterspawner.position);
        character = Instantiate(characterPrefab, characterspawner.position, new Quaternion(0,0,0,0));
    }


    void PrintSOs()
    {
        string printString = "ScriptableObjects:\n--Ingredients--\n";
        SOIngredient[] toListi = SOIngredient.getAll();
        foreach (SOIngredient s in toListi)
        {
            printString += s.GetDebugString() + "\n";
        }

        printString += "--Effects--\n";
        SOEffect[] toListe = SOEffect.getAll();
        foreach (SOEffect s in toListe)
        {
            printString += s.GetDebugString() + "\n";
        }

        printString += "--Potions--\n";
        SOPotion[] toListp = SOPotion.getAll();
        foreach (SOPotion s in toListp)
        {
            printString += s.GetDebugString() + "\n";
        }

        printString += "--Items--\n";
        SOItem[] toListit = SOItem.getAll();
        foreach (SOItem s in toListit)
        {
            printString += s.GetDebugString() + "\n";
        }
        printString += "End of ScriptableObjects";

        Debug.Log(printString);
    }

    void TestSOs()
    {
        //test potions
        {
            SOPotion[] toCheck = SOPotion.getAll();
            Dictionary<int, string> potionIDs = new Dictionary<int, string>();
            foreach (SOPotion s in toCheck)
            {
                if (potionIDs.ContainsKey(s.ID))
                {
                    Debug.LogError("ERROR: SOPotion " + s.name + " has the same ID as " + potionIDs[s.ID] + " = " + s.ID);
                }
                else
                {
                    potionIDs.Add(s.ID, s.name);
                }
            }
        }


        //test ingredients
        { 
            SOIngredient[] toCheck = SOIngredient.getAll();
            Dictionary<int, string> ingrIDs = new Dictionary<int, string>();
            foreach (SOIngredient s in toCheck)
            {
                if (ingrIDs.ContainsKey(s.ID))
                {
                    Debug.LogError("ERROR: SOIngredient " + s.name + " has the same ID as " + ingrIDs[s.ID] + " = " + s.ID);
                }
                else
                {
                    ingrIDs.Add(s.ID, s.name);
                }
            }
        }

        {
            //test effects
            SOEffect[] toCheck = SOEffect.getAll();
            Dictionary<int, string> effectIDs = new Dictionary<int, string>();
            foreach (SOEffect s in toCheck)
            {
                if (effectIDs.ContainsKey(s.ID))
                {
                    Debug.LogError("ERROR: SOEffect " + s.name + " has the same ID as " + effectIDs[s.ID] + " = " + s.ID);
                }
                else
                {
                    effectIDs.Add(s.ID, s.name);
                }
            }
        }

        {
            //test items
            SOItem[] toCheck = SOItem.getAll();
            Dictionary<int, string> itemIDs = new Dictionary<int, string>();
            foreach (SOItem s in toCheck)
            {
                if (itemIDs.ContainsKey(s.ID))
                {
                    Debug.LogError("ERROR: SOItem " + s.name + " has the same ID as " + itemIDs[s.ID] + " = " + s.ID);
                }
                else
                {
                    itemIDs.Add(s.ID, s.name);
                }
            }
        }
    }

    public bool killPlayer(string[] noop)
    {
        character.GetComponent<characterControllerScript>().kill();
        return true;
    }

    void onPlayerDeath()
    {
        respawnCharacter();
    }
}
