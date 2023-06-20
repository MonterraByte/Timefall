using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public Button[] buttons;
    public InputActionReference pauseAction;
    public InputActionReference upAction;
    public InputActionReference downAction;
    public InputActionReference confirmAction;

    private int _selectedButton;
    private int SelectedButton {
        get => _selectedButton;
        set {
            _selectedButton = Math.Clamp(value, 0, buttons.Length - 1);
            EventSystem.current.SetSelectedGameObject(buttons[_selectedButton].gameObject);
        }
    }

    private void Start() {
        pauseAction.action.started += _ => { IsPaused = !IsPaused; };
        upAction.action.started += _ => {
            if (IsPaused) {
                SelectedButton--;
            }
        };
        downAction.action.started += _ => {
            if (IsPaused) {
                SelectedButton++;
            }
        };
        confirmAction.action.started += _ => {
            if (IsPaused) {
                buttons[SelectedButton].onClick.Invoke();
            }
        };
    }

    private void OnEnable() {
        pauseAction.action.Enable();
        upAction.action.Enable();
        downAction.action.Enable();
        confirmAction.action.Enable();
    }

    private void OnDisable() {
        pauseAction.action.Disable();
    }

    public void LoadMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
