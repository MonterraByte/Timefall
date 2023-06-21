using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthUi : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject heartPrefab;
    public GameObject shieldPrefab;

    public float x;
    public float y;
    private const float offset = 80f;

    private List<GameObject> children = new();

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

    public void UpdateUi(int lives, int shields) {
        Debug.Log($"Updating health UI with {lives} lives and {shields} shields");
        foreach (var obj in children) {
            Destroy(obj);
        }
        children.Clear();

        var currentX = x;
        for (int i = 0; i < lives; i++) {
            var obj = Instantiate(heartPrefab, transform);
            var rect = obj.GetComponent<RectTransform>();
            rect.position = new Vector3(currentX, y, 0f);
            children.Add(obj);
            currentX += offset;
        }

        for (int i = 0; i < shields; i++) {
            var obj = Instantiate(shieldPrefab, transform);
            var rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.position = new Vector3(currentX, y, 0f);
            children.Add(obj);
            currentX += offset;
        }

        if (lives <= 0) {
            gameOver.gameObject.SetActive(true);
        }
    }
}
