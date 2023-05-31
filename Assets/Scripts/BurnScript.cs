using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnScript : MonoBehaviour
{
    public float health = 100;
    public ParticleSystem fire;
    public Vector3 flamePos;

    private bool burning = false;
    private float currentTime = 0;
    private float burningDuration = 0.5f;
    private ParticleSystem burn;
    // Start is called before the first frame update
    void Start()
    {
        this.burn = Instantiate(fire, Vector3.zero, Quaternion.identity, this.transform);
        this.burn.transform.localPosition = flamePos;
        this.burn.transform.localRotation = Quaternion.identity;
        this.burn.transform.localScale = Vector3.one * 2.5f;
        this.burn.Stop(true);
    
    }

    // Update is called once per frame
    void Update()
    {
        if (burning)
        {
            if (this.currentTime < this.burningDuration)
            {
                this.currentTime += Time.deltaTime;
                this.health -= 25 * Time.deltaTime;
                if (!this.burn.isPlaying)
                {
                    this.burn.Play(true);
                }
            }
            else
            {
                this.burning = false;
                this.burn.Stop(true);
            }
        }

        if (this.health < 0)
        {
            this.burn.Stop(true);
            Destroy(this.gameObject);
        }
    }

    public void Burn()
    {
        this.burning = true;
        this.currentTime = 0;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.layer == 9)
        {
            this.Burn();
        }
    }
}
