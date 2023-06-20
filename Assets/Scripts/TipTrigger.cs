using UnityEngine;

public class TipTrigger : MonoBehaviour
{
    public GameObject tipObject;

    private bool tipShown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !tipShown)
        {
            // Show the tip object
            tipObject.SetActive(true);
            tipShown = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide the tip object
            tipObject.SetActive(false);
            tipShown = false;
        }
    }
}
