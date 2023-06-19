using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{

    public GameObject gameOver;
    public GameObject heart1, heart2, heart3;
    public int Lives { get; private set; } = 3;

    void Start()
    {
        heart1.gameObject.SetActive(true);
        heart2.gameObject.SetActive(true);
        heart3.gameObject.SetActive(true);
        gameOver.gameObject.SetActive(false);
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

    public void takeHeart()
    {
        Lives--;
        heart1.gameObject.SetActive(Lives >= 1);
        heart2.gameObject.SetActive(Lives >= 2);
        heart3.gameObject.SetActive(Lives >= 3);
        gameOver.gameObject.SetActive(Lives <= 0);
    }
}
