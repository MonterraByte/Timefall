using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    private PlayerInput playerInput;
    public bool isPaused;


    // Start is called before the first frame update
    void Start()
    {
        this.playerInput = GetComponentInParent<PlayerInput>();
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isPaused){
            PauseGame();
        }else{
            ResumeGame();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            if(!isPaused){
                this.isPaused = true;
            }else{
                this.isPaused = false;
            }
        }
    }

    public void PauseGame(){
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
