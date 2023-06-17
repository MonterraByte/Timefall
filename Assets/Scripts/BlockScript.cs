using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public float moveSpeed = 20.0f;
    public float limitLow = 0.0f;
    public int powerUp = 1;

    private bool start = false;
    // Start is called before the first frame update
    void Start()
    {
        switch(powerUp)
        {
            case 1:
                PowerUpScript.OnDestroyedBoots += HandleObjectDestroyed;
                break;

            case 2:
                PowerUpScript.OnDestroyedBoomerang += HandleObjectDestroyed;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            this.transform.position += Vector3.down * this.moveSpeed * Time.deltaTime;

            if (this.transform.position.y <= this.limitLow)
            {
                this.start = false;

                switch(powerUp)
                {
                    case 1:
                        PowerUpScript.OnDestroyedBoots += HandleObjectDestroyed;
                        break;

                    case 2:
                        PowerUpScript.OnDestroyedBoomerang -= HandleObjectDestroyed;
                        break;
                }
            }
        }
    }

    private void HandleObjectDestroyed()
    {
        this.start = true;
    }
}
