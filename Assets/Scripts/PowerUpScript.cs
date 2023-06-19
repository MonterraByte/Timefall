using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    public static event Action OnDestroyedBoots;
    public static event Action OnDestroyedFlame;
    public static event Action OnDestroyedBoomerang;

    private PanelController panel;
    // Start is called before the first frame update
    void Start()
    {
        panel = FindObjectOfType<PanelController>();
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
            switch(tag)
            {
                case "Boots":
                    OnDestroyedBoots?.Invoke();
                    panel.ShowPanel(2, 10);
                    break;

                case "Flame":
                    OnDestroyedFlame?.Invoke();
                    panel.ShowPanel(1, 10);
                    break;

                case "Boomerang":
                    OnDestroyedBoomerang?.Invoke();
                    panel.ShowPanel(0, 10);
                    break;

                case "Claws":
                    panel.ShowPanel(3, 10);
                    break;

                case "HookShot":
                    panel.ShowPanel(4, 10);
                    break;
            }
        }
    }
}
