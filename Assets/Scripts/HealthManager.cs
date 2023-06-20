using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public const int MaxLives = 3;
    public const int MaxShields = 1;

    public GameObject gameOver;
    public GameObject heart1, heart2, heart3;
    public GameObject shield;

    private int _lives;
    public int Lives {
        get => _lives;
        set {
            _lives = Math.Clamp(value, 0, MaxLives);
            UpdateUi();
        }
    }

    private int _shields;
    public int Shields {
        get => _shields;
        set {
            _shields = Math.Clamp(value, 0, MaxShields);
            UpdateUi();
        }
    }

    void Start() {
        Lives = 3;
    }

    public void GameOverMainMenu()

    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOverRetry()

    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateUi() {
        heart1.gameObject.SetActive(Lives >= 1);
        heart2.gameObject.SetActive(Lives >= 2);
        heart3.gameObject.SetActive(Lives >= 3);
        shield.gameObject.SetActive(Shields >= 1);
        gameOver.gameObject.SetActive(Lives <= 0);
    }
}
