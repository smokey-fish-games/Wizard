using UnityEngine;

public class FillerRenderer : MonoBehaviour
{
    public Renderer target;

    private void Awake()
    {
        if (target == null)
        {
            Debug.LogError("Target renderer for " + gameObject.name + " is null");
        }
    }

    public void showContents(bool yesno)
    {
        target.enabled = yesno;
    }

    public void setContentsColor(Color c)
    {
        
        setContents(c, getContentsTex());
    }

    public void setContentsTexture(Texture2D t)
    {
        setContents(target.material.color, t);
    }

    public void setContents(Color c, Texture2D t)
    {
        target.material.color = c;
        target.material.SetColor(CONSTANTS.RENDERER_BASECOLOR_VAR, c);
        target.material.mainTexture = t;
        target.material.SetTexture(CONSTANTS.RENDERER_TEXTURE2D_VAR, t);
    }

    public Color getContentsColor()
    {
        return target.material.GetColor(CONSTANTS.RENDERER_BASECOLOR_VAR);
    }

    public Texture2D getContentsTex()
    {

        return (Texture2D)target.material.GetTexture(CONSTANTS.RENDERER_TEXTURE2D_VAR);
    }
}
