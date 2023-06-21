using UnityEngine;

public class TilingScript : MonoBehaviour
{
    public Vector2 tiling = new(1, 1);

    private new Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        this.renderer = GetComponent<Renderer>();
        this.renderer.material.mainTextureScale = tiling;
    }
}
