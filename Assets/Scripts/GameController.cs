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

        // Spawn all cauldrons
        ContainerFiller[] liquids = ContainerFiller.GetAllByType(ContainerFiller.INGREDIENTTYPE.LIQUID);

        float offset = -3f;
        float z = 3f;

        //spawn an empty default one
        ic.SpawnItem(SOItem.GetByID(1), new Vector3(11.54006f, 1.28f, z));
        z += offset;

        for (int i = 0; i < liquids.Length; i++)
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("contents", liquids[i].ID.ToString());
            // this one has set contents not random
            ic.SpawnItem(SOItem.GetByID(1), new Vector3(11.54006f, 1.28f, z), di);
            z += offset;
        }

        // Spawn all bottles
        offset = 0.5f;
        z = 7f;

        //spawn an empty default one
        ic.SpawnItem(SOItem.GetByID(0), new Vector3(z, 1.264f, 6.338f));
        z += offset;
        for (int i = 0; i < liquids.Length; i++)
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("contents", liquids[i].ID.ToString());
            // this one has set contents not random
            ic.SpawnItem(SOItem.GetByID(0), new Vector3(z, 1.264f, 6.338f), di);
            z += offset;
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

    public GameObject GetCharacter()
    {
        return character;
    }


    void PrintSOs()
    {
        string printString = "ScriptableObjects:\n--ContainerFillers--\n";
        ContainerFiller[] toListi = ContainerFiller.GetAll();
        foreach (ContainerFiller s in toListi)
        {

            printString += s.GetDebugString() + "\n";
        }

        printString += "--Effects--\n";
        SOEffect[] toListe = SOEffect.GetAll();
        foreach (SOEffect s in toListe)
        {
            printString += s.GetDebugString() + "\n";
        }

        printString += "--Items--\n";
        SOItem[] toListit = SOItem.GetAll();
        foreach (SOItem s in toListit)
        {
            printString += s.GetDebugString() + "\n";
        }

        printString += "--Recipes--\n";
        SORecipe[] toListre = SORecipe.GetAll();
        foreach (SORecipe s in toListre)
        {
            printString += s.GetDebugString() + "\n";
        }
        printString += "End of ScriptableObjects";

        Debug.Log(printString);
    }

    void TestSOs()
    {
        //test ContainerFiller
        {
            ContainerFiller[] toCheck = ContainerFiller.GetAll();
            Dictionary<int, string> potionIDs = new Dictionary<int, string>();
            foreach (ContainerFiller s in toCheck)
            {
                if (potionIDs.ContainsKey(s.ID))
                {
                    Debug.LogError("ERROR: ContainerFiller " + s.name + " has the same ID as " + potionIDs[s.ID] + " = " + s.ID);
                }
                else
                {
                    potionIDs.Add(s.ID, s.name);
                }
            }
        }

        {
            //test effects
            SOEffect[] toCheck = SOEffect.GetAll();
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
            SOItem[] toCheck = SOItem.GetAll();
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

        {
            //test Recipes
            SORecipe[] toCheck = SORecipe.GetAll();
            Dictionary<int, string> itemIDs = new Dictionary<int, string>();
            foreach (SORecipe s in toCheck)
            {
                if (itemIDs.ContainsKey(s.ID))
                {
                    Debug.LogError("ERROR: SORecipe " + s.name + " has the same ID as " + itemIDs[s.ID] + " = " + s.ID);
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
