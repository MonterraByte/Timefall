using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    public static event Action OnDestroyedBoots;
    public static event Action OnDestroyedFlame;
    public static event Action OnDestroyedBoomerang;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            string tag = this.gameObject.tag;
            Destroy(this.gameObject);
            if (tag == "Boots")
            {
                OnDestroyedBoots?.Invoke();
            }
            if (tag == "Flame")
            {
                OnDestroyedFlame?.Invoke();
            }
            if (tag == "Boomerang")
            {
                OnDestroyedBoomerang?.Invoke();
            }
        }
    }
}
