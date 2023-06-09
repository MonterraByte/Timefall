using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManagerScript : MonoBehaviour
{
    public GameObject player;
    public Vector3 respawnPoint;
    public float respawnDelay = 3f;

    public void StartRespawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        // Deactivate player
        player.SetActive(false);

        // Wait for the respawn delay
        yield return new WaitForSeconds(respawnDelay);

        // Respawn player at the respawn point
        player.transform.position = respawnPoint;
        player.SetActive(true);
    }
}
