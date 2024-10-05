using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchGameScene : MonoBehaviour
{
    //Used by the Scene Controller Class to change the current scene of the game to something else
    public Button playButton;
    public string currentSceneName;
    public string targetSceneName;

    void Start()
    {
        playButton.onClick.AddListener(() => LoadScene(currentSceneName, targetSceneName));
    }

    public void LoadScene(string currentScene, string targetScene)
    {
        if (SceneManager.GetActiveScene().name == currentScene)
        {
            SceneManager.LoadScene(targetScene);
        }
    }
}
