using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SkipToMainMenu : MonoBehaviour {
    public InputActionReference skipAction;

    void Start() {
        skipAction.action.performed += _ => SceneManager.LoadScene("MainMenu");
        skipAction.action.Enable();
    }
}
