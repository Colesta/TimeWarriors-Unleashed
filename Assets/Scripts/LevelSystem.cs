using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    private Stats s;
    private Score sc;

    void Awake()
    {
        s = GetComponent<Stats>();
        sc = GetComponent<Score>();
    }

    // Reference to the UI Image component for the background
    public Image Background;

    // Sprites for different level backgrounds
    public Sprite Level1Background;
    public Sprite Level2Background;
    public Sprite Level3Background;

    void Start()
    {
        // Sets the background of the battle screen to different images based on the current level
        UpdateBackground();
    }

    // Updates the background sprite based on the current level
    void UpdateBackground()
    {
        if (s.CurrentLevel == 1)
        {
            Background.sprite = Level1Background;
        }
        else if (s.CurrentLevel == 2)
        {
            Background.sprite = Level2Background;
        }
        else if (s.CurrentLevel == 3)
        {
            Background.sprite = Level3Background;
        }
    }

    // Increase level count by one
    public void AddLevel()
    {
        s.CurrentLevel += 1;
        UpdateBackground(); // Update background when level changes
    }

    // When the button is pressed on lose screen, update your scores
    public void LoseScreen()
    {
        sc.UpdateCurrentScore();
    }

    // When the button is pressed on win screen, update your scores and reset level
    public void WinScreen()
    {
        sc.UpdateCurrentScore();
        s.CurrentLevel = 1; // Reset level to 1
        UpdateBackground();  // Reset background as well
    }
}
