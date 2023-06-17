using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManagerScript : MonoBehaviour
{
    public GameObject player;
    public GameObject spawner;
    public float respawnDelay = 3f;

    private bool firstTime = true;

    public void StartRespawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    public void setSpawnPoint(GameObject spawn)
    {
     
        if (firstTime)
        {
            this.firstTime = false;
            this.spawner = spawn;
            this.spawner.SetActive(false);
        }
        else
        {
            this.spawner.SetActive(true);
            this.spawner = spawn;
            this.spawner.SetActive(false);
        }
    } 

    private IEnumerator RespawnCoroutine()
    {
        // Deactivate player
        player.SetActive(false);

        // Wait for the respawn delay
        yield return new WaitForSeconds(respawnDelay);

        // Respawn player at the respawn point
        if (firstTime)
        {
            player.transform.position = new Vector3(-8.75f, 24.0f, -0.5f);
        }
        else
        {
            player.transform.position = this.spawner.transform.position;
        }

        player.SetActive(true);
    }
}
