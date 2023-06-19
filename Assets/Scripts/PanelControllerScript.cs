using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public float panelDuration = 10f; // Default duration in seconds for the panels to remain active
    public float fadeDuration = 1f; // Duration in seconds for the fade-in and fade-out effect

    private GameObject[] panels; // Array to store all the panels
    private GameObject lifes;
    private GameObject manager;

    private void Start()
    {
        // Populate the panels array with all the panels inside the canvas
        panels = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            panels[i] = transform.GetChild(i).gameObject;
            panels[i].SetActive(false); // Set all panels initially inactive
        }

        lifes = GameObject.Find("Lifes");
        manager = FindObjectOfType<HealthManager>().gameObject;
    }

    // Call this method from another script when you want to show a panel
    public void ShowPanel(int panelIndex, float customDuration)
    {
        StartCoroutine(ShowPanelCoroutine(panelIndex, customDuration));
    }

    private IEnumerator ShowPanelCoroutine(int panelIndex, float customDuration)
    {
        //Deactivate health counter
        manager.SetActive(false);
        lifes.SetActive(false);

        // Pause the game
        Time.timeScale = 0f;

        // Set the panel active
        panels[panelIndex].SetActive(true);

        // Get the panel's Image component for the fade effect
        Image panelImage = panels[panelIndex].GetComponent<Image>();
        if (panelImage != null)
        {
            // Set the initial transparency to 0
            panelImage.canvasRenderer.SetAlpha(0f);

            // Fade-in effect
            panelImage.CrossFadeAlpha(1f, fadeDuration, false);
        }

        // Determine the duration based on the custom duration or the default panelDuration
        float duration = customDuration > 0 ? customDuration : panelDuration;

        // Wait for the specified duration
        yield return new WaitForSecondsRealtime(duration);

        // Fade-out effect
        if (panelImage != null)
        {
            panelImage.CrossFadeAlpha(0f, fadeDuration, false);
        }

        // Wait for the fade-out to complete
        yield return new WaitForSecondsRealtime(fadeDuration);

        // Set the panel inactive
        panels[panelIndex].SetActive(false);

        //Activate health counter
        lifes.SetActive(true);
        manager.SetActive(true);

        // Unpause the game
        Time.timeScale = 1f;
    }
}
