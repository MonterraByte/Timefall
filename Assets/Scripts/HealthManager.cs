using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{

    public GameObject gameOver;
    public GameObject heart1, heart2, heart3;
    public GameObject shield;
    public int Lives { get; private set; } = 3;
    public int Shields { get; private set; }

    void Start()
    {
        heart1.gameObject.SetActive(true);
        heart2.gameObject.SetActive(true);
        heart3.gameObject.SetActive(true);
        shield.gameObject.SetActive(false);
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

    public void gainHeart(){
        Lives++;
    }

    public void takeShield(){
        Shields--;
        shield.gameObject.SetActive(false);
    }

    public void gainShield(){
        Shields++;
        shield.gameObject.SetActive(true);
    }
}
