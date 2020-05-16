using UnityEditor;
using UnityEngine;

public class objectController : MonoBehaviour
{

    public Renderer r;
    bool empty = true;
    public SOPotion currentPotion;
    // Start is called before the first frame update
    void Start()
    {
        if (currentPotion == null)
        {
            //Set random potion contents
            string[] potionGUIDs = AssetDatabase.FindAssets("t:SOPotion");
            int chosen = UnityEngine.Random.Range(0, potionGUIDs.Length);
            currentPotion = (SOPotion)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(potionGUIDs[chosen]), typeof(SOPotion));
            Debug.Log("Auto setting potion for " + this.gameObject.name + " to: " + currentPotion.name);
        }
        fillBottle(currentPotion);
    }

    public void emptyBottle()
    {
        r.enabled = false;
        empty = true;
    }

    public void fillBottle(SOPotion fillingPotion)
    {
        if(empty)
        {
            currentPotion = fillingPotion;
            r.material.SetColor("_potionColor", currentPotion.color);
            r.enabled = true;
            empty = false;
        }
        else
        {
            // TODO potion mixing
        }
    }
}
