using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    public string targetSceneName = "MainMenu";
    public List<GameObject> singletons;

    void Start()
    {
        foreach (GameObject g in singletons)
        {
            DontDestroyOnLoad(g);
        }

        SceneManager.LoadScene(targetSceneName);
    }

 
}
