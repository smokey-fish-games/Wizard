using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject characterPrefab;
    public Transform characterspawner;
    GameObject character;
    public GameObject todelete;
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
    }

    void respawnCharacter()
    {
        if(character != null)
        {
            GameObject.Destroy(character);
        }

        Debug.Log("Spawning character on " + characterspawner.position);
        character = Instantiate(characterPrefab, characterspawner.transform);
    }


    void PrintSOs()
    {
        //Tests
        string printString = "ScriptableObjects:\n--Ingredients--\n";
        string[] ingreGUID = AssetDatabase.FindAssets("t:SOIngredient");
        foreach (string s in ingreGUID)
        {
            string path = AssetDatabase.GUIDToAssetPath(s);
            SOIngredient soi = (SOIngredient)AssetDatabase.LoadAssetAtPath(path, typeof(SOIngredient));
            printString += soi.GetDebugString() + " " + path + "\n";
        }

        printString += "--Effects--\n";
        string[] effectGUID = AssetDatabase.FindAssets("t:SOEffect");
        foreach (string s in effectGUID)
        {
            string path = AssetDatabase.GUIDToAssetPath(s);
            SOEffect soi = (SOEffect)AssetDatabase.LoadAssetAtPath(path, typeof(SOEffect));
            printString += soi.GetDebugString() + " " + path + "\n";
        }

        printString += "--Potions--\n";
        string[] potionGUIDs = AssetDatabase.FindAssets("t:SOPotion");
        foreach (string s in potionGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(s);
            SOPotion soi = (SOPotion)AssetDatabase.LoadAssetAtPath(path, typeof(SOPotion));
            printString += soi.GetDebugString() + " " + path + "\n";
        }
        printString += "End of ScriptableObjects";
        Debug.Log(printString);
    }

    void TestSOs()
    {
        //test potions
        {
            string[] potionGUIDs = AssetDatabase.FindAssets("t:SOPotion");
            Dictionary<int, string> potionIDs = new Dictionary<int, string>();
            foreach (string s in potionGUIDs)
            {
                SOPotion soi = (SOPotion)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(s), typeof(SOPotion));
                if (potionIDs.ContainsKey(soi.ID))
                {
                    Debug.LogError("ERROR: SOPotion " + soi.name + " has the same ID as " + potionIDs[soi.ID] + " = " + soi.ID);
                }
                else
                {
                    potionIDs.Add(soi.ID, soi.name);
                }
            }
        }


        //test ingredients
        { 
            string[] ingreGUID = AssetDatabase.FindAssets("t:SOIngredient");
            Dictionary<int, string> ingrIDs = new Dictionary<int, string>();
            foreach (string s in ingreGUID)
            {
                SOIngredient soi = (SOIngredient)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(s), typeof(SOIngredient));
                if (ingrIDs.ContainsKey(soi.ID))
                {
                    Debug.LogError("ERROR: SOIngredient " + soi.name + " has the same ID as " + ingrIDs[soi.ID] + " = " + soi.ID);
                }
                else
                {
                    ingrIDs.Add(soi.ID, soi.name);
                }
            }
        }

        {
            //test effects
            string[] effectGUID = AssetDatabase.FindAssets("t:SOEffect");
            Dictionary<int, string> effectIDs = new Dictionary<int, string>();
            foreach (string s in effectGUID)
            {
                SOEffect soi = (SOEffect)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(s), typeof(SOEffect));
                if (effectIDs.ContainsKey(soi.ID))
                {
                    Debug.LogError("ERROR: SOEffect " + soi.name + " has the same ID as " + effectIDs[soi.ID] + " = " + soi.ID);
                }
                else
                {
                    effectIDs.Add(soi.ID, soi.name);
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
