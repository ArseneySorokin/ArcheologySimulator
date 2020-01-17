using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustCleaner : MonoBehaviour
{
    public InformationManager informationManager;

    public int brushSize = 5;
    public LayerMask raycastLayers;

    public float completionPercent = 0.75f;

    private Camera mainCamera;
    private bool started;
    private bool open;

    private int cleanedPixels = 0;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }
    public void OpenTool(bool open)
    {
        this.open = open;
    }

    // Update is called once per frame
    void Update()
    {
        if (!open)
            return;

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10, raycastLayers))
            {
                //Debug.Log(hit.collider.gameObject.tag);
                if (hit.collider.gameObject.tag == "Dust")
                {
                    if(!started)
                    {
                        Texture2D oldTex = (Texture2D)hit.collider.GetComponent<MeshRenderer>().material.mainTexture;
                        Texture2D newTex = new Texture2D(oldTex.width, oldTex.height, TextureFormat.ARGB32, false);
                        newTex.SetPixels32(oldTex.GetPixels32());
                        newTex.Apply();
                        hit.collider.GetComponent<MeshRenderer>().material.mainTexture = newTex;
                        started = true;
                    }
                    Renderer renderer = hit.collider.GetComponent<MeshRenderer>();
                    Texture2D texture2D = renderer.material.mainTexture as Texture2D;
                    Vector2 pCoord = hit.textureCoord;
                    pCoord.x *= texture2D.width;
                    pCoord.y *= texture2D.height;

                    Vector2 tiling = renderer.material.mainTextureScale;
                    BrushTransparent(texture2D, Mathf.FloorToInt(pCoord.x * tiling.x), Mathf.FloorToInt(pCoord.y * tiling.y));
                }
            }
        }
    }

    public void BrushTransparent(Texture2D texture, int x, int y)
    {
        float rSquared = brushSize * brushSize;

        for (int u = x - brushSize; u < x + brushSize + 1; u++)
            for (int v = y - brushSize; v < y + brushSize + 1; v++)
                if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared)
                {
                    if(texture.GetPixel(u,v) != Color.clear)
                    {
                        cleanedPixels++;
                        if(completionPercent < (float)cleanedPixels / (texture.width * texture.height))
                            informationManager.UpdateText();
                    }
                    texture.SetPixel(u, v, Color.clear);
                }
        texture.Apply();
    }
}
