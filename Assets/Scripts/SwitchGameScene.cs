using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchGameScene : MonoBehaviour
{
    public Button playButton;

    void Start()
    {
        playButton.onClick.AddListener(() => LoadSceneByName("Overworld"));
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    
}
