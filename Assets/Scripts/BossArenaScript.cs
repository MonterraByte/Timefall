using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class BossArenaScript : MonoBehaviour {
    public GameObject boss;
    public CinemachineCamera defaultCamera;
    public CinemachineCamera bossFightCamera;
    public GameObject horizontalDoor;

    private Boss bossScript;
    private Vector3 bossInitialPosition;
    private Vector3 horizontalDoorInitialPosition;

    void Start() {
        bossScript = boss.GetComponent<Boss>();
        bossScript.enabled = false;
        bossInitialPosition = boss.transform.position;
        horizontalDoorInitialPosition = horizontalDoor.transform.position;
    }

    private void OnTriggerEnter(Collider other) {
        var player = other.gameObject.GetComponent<PlayerScript>();
        if (player == null) {
            return;
        }

        player.SetRespawnPoint(new Vector3(117f, 76f, -0.5f));

        bossFightCamera.gameObject.SetActive(true);
        defaultCamera.gameObject.SetActive(false);
        bossScript.enabled = true;
        StartCoroutine(CloseHorizontalDoor());
        Debug.Log("Start boss fight");
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponent<PlayerScript>() == null) {
            return;
        }

        bossScript.enabled = false;
        bossScript.StopAllCoroutines();
        boss.transform.position = bossInitialPosition;
        defaultCamera.gameObject.SetActive(true);
        bossFightCamera.gameObject.SetActive(false);
        horizontalDoor.transform.position = horizontalDoorInitialPosition;
    }

    private IEnumerator CloseHorizontalDoor() {
        yield return new WaitForSeconds(1f);
        var target = horizontalDoorInitialPosition.x + 5f;
        while (horizontalDoor.transform.position.x < target) {
            horizontalDoor.transform.position += Vector3.right * (5.0f * Time.deltaTime);
            yield return null;
        }
    }
}
