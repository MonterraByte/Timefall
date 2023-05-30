using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStory : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("Spaceship Level", LoadSceneMode.Single);
        
    }
}
