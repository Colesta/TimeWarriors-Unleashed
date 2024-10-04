using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    // Singleton instance to make LevelSystem persistent
    public static LevelSystem Instance;

    private Stats s;
    private Score sc;

    // Current level 
    private int CurrentLevel = 1;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy any duplicate instances
        }

        s = GetComponent<Stats>();
        sc = GetComponent<Score>();
    }

    void Start()
    {
        UpdateBackground();
    }

    // Updates the background sprite based on the current level (can be extended as needed)
    void UpdateBackground()
    {
        // Add background update logic here, e.g.:
        // if (CurrentLevel == 1) { Background.sprite = Level1Background; }
    }

    // Increase level count by one
    public void AddLevel()
    {
        CurrentLevel += 1;
        UpdateBackground(); // Update background when level changes
    }

    public int getCurrentLevel()
    {
        return CurrentLevel;
    }

    // When the button is pressed on the lose screen, update your scores
    public void LoseScreen()
    {
        Score.Instance.UpdateCurrentScore();
    }

    // When the button is pressed on the win screen, update your scores and reset the level
    public void WinScreen()
    {
        Score.Instance.UpdateCurrentScore();
      
    }
}
