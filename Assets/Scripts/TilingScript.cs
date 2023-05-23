using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilingScript : MonoBehaviour
{
    public Vector2 tiling = new Vector2 (1, 1);

    private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        this.renderer = GetComponent<Renderer>();
        this.renderer.material.mainTextureScale = tiling;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
