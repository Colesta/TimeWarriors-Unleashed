using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    // Singleton instance
    public static Difficulty Instance { get; private set; }

    // Difficulty settings
    public bool Easy { get; private set; }
    public bool Medium { get; private set; }
    public bool Hard { get; private set; }

    void Awake()
    {
        // Ensure that there's only one instance of Difficulty (Singleton pattern)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the instance alive across scene changes
        }
        else
        {
            Destroy(gameObject); // Destroys any additional instances
        }
    }

    void Start()
    {
        SetMedium(); // Default difficulty to Easy
    }

    // Set difficulty to Easy
    public void SetEasy()
    {
        Easy = true;
        Medium = false;
        Hard = false;
        Debug.Log("Difficulty set to Easy");
    }

    // Set difficulty to Medium
    public void SetMedium()
    {
        Easy = false;
        Medium = true;
        Hard = false;
        Debug.Log("Difficulty set to Medium");
    }

    // Set difficulty to Hard
    public void SetHard()
    {
        Easy = false;
        Medium = false;
        Hard = true;
        Debug.Log("Difficulty set to Hard");
    }

    // Method to get the current difficulty level
    public string GetCurrentDifficulty()
    {
        if (Easy)
            return "Easy";
        else if (Medium)
            return "Medium";
        else if (Hard)
            return "Hard";
        else
            return "Unknown";
    }
}
