using UnityEditor;
using UnityEngine;

public class CauldronController : MonoBehaviour
{
    public Renderer r;
    public SOPotion contents;
    bool empty = true;
    // Start is called before the first frame update
    void Start()
    {
        //Set random potion contents
        if (contents == null)
        {
            string[] potionGUIDs = AssetDatabase.FindAssets("t:SOPotion");
            int chosen = UnityEngine.Random.Range(0, potionGUIDs.Length);
            contents = (SOPotion)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(potionGUIDs[chosen]), typeof(SOPotion));

            Debug.Log("Setting potion for " + this.gameObject.name + " to: " + contents.name);
        }
        fillContents(contents);
    }

    public void fillContents(SOPotion newcontents)
    {
        if (empty)
        {
            contents = newcontents;
            r.material.SetColor("_potionColor", contents.color);
            r.enabled = true;
            empty = false;
        }
        else
        {
            // TODO potion mixing
        }
    }

    public SOPotion getContents()
    {
        return contents;
    }
}
