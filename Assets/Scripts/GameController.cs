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

    void Awake()
    {
        if (characterspawner == null)
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
    }
    void Start()
    {
        GameObject.Destroy(todelete);
        respawnCharacter();

        GameEvents.current.onPlayerDeath += onPlayerDeath;

        DeveloperConsole.instance.RegisterCommand("kill", "Kills the current player dead.", killPlayer);
        ic = GetComponent<ItemController>();

        // Spawn all cauldrons
        ContainerFiller[] liquidsIngredients = ContainerFiller.GetAllIngredientsByType(ContainerFiller.INGREDIENTTYPE.LIQUID);
        ContainerFiller[] liquidPotions = ContainerFiller.GetAllPotionsByType(ContainerFiller.INGREDIENTTYPE.LIQUID);
        ContainerFiller[] solids = ContainerFiller.GetAllByType(ContainerFiller.INGREDIENTTYPE.SOLID);
        ContainerFiller[] gases = ContainerFiller.GetAllByType(ContainerFiller.INGREDIENTTYPE.GAS);

        float offset = -3f;
        float z = 0f;

        //Spawn cauldrons of potions
        //spawn an empty default one
        ic.SpawnItem(SOItem.GetByID(CONSTANTS.ITEM_CAULDRON), new Vector3(11.54006f, 1.28f, z));
        z += offset;

        for (int i = 0; i < liquidPotions.Length; i++)
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add(CONSTANTS.CONTENTS_STRING, liquidPotions[i].ID.ToString());
            // this one has set contents not random
            ic.SpawnItem(SOItem.GetByID(CONSTANTS.ITEM_CAULDRON), new Vector3(11.54006f, 1.28f, z), di);
            z += offset;
        }

        // Spawn cauldrons of Ingredients
        //spawn an empty default one
        offset = -3f;
        z = 0f;

        ic.SpawnItem(SOItem.GetByID(CONSTANTS.ITEM_CAULDRON), new Vector3(15.4f, 1.28f, z));
        z += offset;

        for (int i = 0; i < liquidsIngredients.Length; i++)
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add(CONSTANTS.CONTENTS_STRING, liquidsIngredients[i].ID.ToString());
            // this one has set contents not random
            ic.SpawnItem(SOItem.GetByID(CONSTANTS.ITEM_CAULDRON), new Vector3(15.4f, 1.28f, z), di);
            z += offset;
        }



        // Spawn all bottles of potions
        offset = 0.5f;
        z = 7f;

        //spawn an empty default one
        ic.SpawnItem(SOItem.GetByID(CONSTANTS.ITEM_POTIONBOTTLE), new Vector3(z, 1.264f, 6.338f));
        z += offset;
        for (int i = 0; i < liquidPotions.Length; i++)
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add(CONSTANTS.CONTENTS_STRING, liquidPotions[i].ID.ToString());
            // this one has set contents not random
            ic.SpawnItem(SOItem.GetByID(CONSTANTS.ITEM_POTIONBOTTLE), new Vector3(z, 1.264f, 6.338f), di);
            z += offset;
        }

        // Spawn all bottles of ingredients
        offset = 0.5f;
        z = 7f;

        //spawn an empty default one
        ic.SpawnItem(SOItem.GetByID(CONSTANTS.ITEM_POTIONBOTTLE), new Vector3(z, 1.264f, 9.41f));
        z += offset;
        for (int i = 0; i < liquidsIngredients.Length; i++)
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add(CONSTANTS.CONTENTS_STRING, liquidsIngredients[i].ID.ToString());
            // this one has set contents not random
            ic.SpawnItem(SOItem.GetByID(CONSTANTS.ITEM_POTIONBOTTLE), new Vector3(z, 1.264f, 9.41f), di);
            z += offset;
        }

        // Spawn piles of ingredients
        offset = 0.5f;
        z = 7.5f;
        for (int i = 0; i < solids.Length; i++)
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add(CONSTANTS.CONTENTS_STRING, solids[i].ID.ToString());
            // this one has set contents not random
            ic.SpawnItem(SOItem.GetByID(CONSTANTS.ITEM_FREESTAND_INGREDIENT), new Vector3(z, 1.5f, 17.4901f), di);
            z += offset;
        }

        // Spawn Bowls with ingredients
        // spawn empty one first
        offset = 0.5f;
        z = 7.5f;
        ic.SpawnItem(SOItem.GetByID(CONSTANTS.ITEM_BOWL), new Vector3(z, 1.124f, 12.96961f));
        z += offset;
        for (int i = 0; i < solids.Length; i++)
        {
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add(CONSTANTS.CONTENTS_STRING, solids[i].ID.ToString());
            // this one has set contents not random
            ic.SpawnItem(SOItem.GetByID(CONSTANTS.ITEM_BOWL), new Vector3(z, 1.124f, 12.96961f), di);
            z += offset;
        }

        // Spawn remaining tools

        // Spawn a bucket
        ic.SpawnItem(SOItem.GetByID(CONSTANTS.ITEM_BUCKET), new Vector3(1.5f, 0.45f, 3.14f));
        // Spawn a drain
        ic.SpawnItem(SOItem.GetByID(CONSTANTS.ITEM_DRAIN), new Vector3(3.181f, 0.2774024f, 3.247f));
        // Spawn a bin
        ic.SpawnItem(SOItem.GetByID(CONSTANTS.ITEM_BIN), new Vector3(2.297608f, 0.4274023f, 3.312145f));

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
                // Check for null parameters
                if(s.color == null)
                {
                    Debug.LogError("ERROR: ContainerFiller " + s.name + " has a NULL Color");
                }
                if (s.onConsumeEffect == null)
                {
                    Debug.LogError("ERROR: ContainerFiller " + s.name + " has a NULL onConsumeEffect");
                }
                if (s.onGroundModel == null)
                {
                    Debug.LogError("ERROR: ContainerFiller " + s.name + " has a NULL onGroundModel");
                }
                if (s.texture == null)
                {
                    Debug.LogError("ERROR: ContainerFiller " + s.name + " has a NULL Texture");
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
                if (s.model == null)
                {
                    Debug.LogError("ERROR: SOItem " + s.name + " has a NULL model");
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
                if (s.result == null)
                {
                    Debug.LogError("ERROR: SORecipe " + s.name + " has a NULL result");
                }
                if (s.ingredients == null)
                {
                    Debug.LogError("ERROR: SORecipe " + s.name + " has a NULL ingredients");
                }
                else if(s.ingredients.Length < 2)
                {
                    Debug.LogError("ERROR: SORecipe " + s.name + " has a less than 2 ingredients");
                }
                else
                {
                    foreach (ContainerFiller c in s.ingredients)
                    {
                        if (c==null)
                        {
                            Debug.LogError("ERROR: SORecipe " + s.name + " contains a NULL ingredient in it's list.");
                            break;
                        }
                    }
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
