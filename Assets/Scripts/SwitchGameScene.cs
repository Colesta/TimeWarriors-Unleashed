using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchGameScene : MonoBehaviour
{
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
