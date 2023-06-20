using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : MonoBehaviour
{
    public int health = 100;

    private bool isAttacking = false;

    private static readonly BossMove[] moves = (BossMove[])Enum.GetValues(typeof(BossMove));

    private int bulletLayer;

    public void Awake()
    {
        bulletLayer = LayerMask.NameToLayer("Enemy Projectile");
    }


    public enum BossMove
    {
        ConeProjectile,
        AllDirectionsProjectile,
        Charge,
        FlyAndDropProjectiles
    }

    private void chooseAttack()
    {

        // Choose a random attack move
        BossMove randomMove = moves[UnityEngine.Random.Range(0, moves.Length)];

        switch (randomMove)
        {
            case BossMove.ConeProjectile:
                StartCoroutine(ConeProjectileAttack());

                break;
            case BossMove.AllDirectionsProjectile:
                StartCoroutine(AllDirectionsAttack());
                break;
            case BossMove.Charge:
                StartCoroutine(ChargeAttack());
                break;
            case BossMove.FlyAndDropProjectiles:
                StartCoroutine(FlyAndDropAttack());
                break;
        }

    }


    private IEnumerator ConeProjectileAttack()
    {

        chooseAttack();
        yield break;
    }
    private IEnumerator AllDirectionsAttack()
    {
        chooseAttack();
        yield break;
    }
    private IEnumerator ChargeAttack()
    {
        chooseAttack();
        yield break;
    }
    private IEnumerator FlyAndDropAttack()
    {

        chooseAttack();
        yield break;
    }


    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            this.health -= 10;
           
        }
    }

}
