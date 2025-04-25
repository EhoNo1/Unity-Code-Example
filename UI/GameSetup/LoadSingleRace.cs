using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSingleRace : MonoBehaviour
{
    [SerializeField] private string level;


    public void Start()
    {
        Debug.LogWarning("LoadSingleRace script does not have a level defined!");
    }


    public void LoadLevel()
    {
        SceneManager.LoadScene(level);
    }
}
