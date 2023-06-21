using Unity.Cinemachine;
using UnityEngine;

public class BossArenaScript : MonoBehaviour {
    public GameObject boss;
    public CinemachineCamera defaultCamera;
    public CinemachineCamera bossFightCamera;

    private Boss bossScript;

    void Start() {
        bossScript = boss.GetComponent<Boss>();
        bossScript.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        bossScript.enabled = true;
        bossFightCamera.gameObject.SetActive(true);
        defaultCamera.gameObject.SetActive(false);
        Debug.Log("Start boss fight");
    }

    private void OnTriggerExit(Collider other) {
        bossScript.enabled = false;
        defaultCamera.gameObject.SetActive(true);
        bossFightCamera.gameObject.SetActive(false);
    }
}
