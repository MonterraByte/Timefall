using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public int health = 100;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            this.health -= 10;
           
        }
    }

}