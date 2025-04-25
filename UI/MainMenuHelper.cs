using Riders.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A small utility script that exposes some functions to the MainMenu's Event System
/// </summary>
public class MainMenuHelper : MonoBehaviour
{
    void Start()
    {
        RaceController.LoadSceneAndStartRace(new RaceMetadataToken(RaceMetadataToken.RaceMode.BOT, "what"), UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void StartDaily()
    {
        RaceController.LoadSceneAndStartRace(new RaceMetadataToken(RaceMetadataToken.RaceMode.SOLO, "what"), UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void Quit()
    {
        Debug.Log("Player quit the game!");
        Application.Quit();
    }
}
