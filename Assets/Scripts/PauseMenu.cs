using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour {
    private bool _isPaused;
    public bool IsPaused {
        get => _isPaused;
        set {
            _isPaused = value;
            pauseMenuUI.SetActive(_isPaused);
            Time.timeScale = _isPaused ? 0.0f : 1.0f;
        }
    }

    public GameObject pauseMenuUI;
    public InputActionReference pauseAction;

    private void Start() {
        pauseAction.action.started += ctx => {
            if (ctx.started) {
                IsPaused = !IsPaused;
            }
        };
    }

    private void OnEnable() {
        pauseAction.action.Enable();
    }

    private void OnDisable() {
        pauseAction.action.Disable();
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
