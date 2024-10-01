using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    private float startTime;
    private float totalTime;
    private bool isTiming;

     private Score sc;
    

    private void Awake()
    {
        sc = GetComponent<Score>();
        // Ensure only one instance of GameTimer exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This makes the GameTimer persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy any additional instances
        }
    }

    // Call this method to start the timer
    public void StartTimer()
    {
        if (!isTiming) // Prevent restarting the timer if it's already running
        {
            startTime = Time.time; // Capture the start time
            isTiming = true;
            Debug.Log("Timer started.");
        }
    }

    // Call this method to stop the timer
    public void StopTimer()
    {
        if (isTiming)
        {
            totalTime += Time.time - startTime; // Calculate elapsed time
            isTiming = false;
            Debug.Log($"Timer stopped. Total time played: {totalTime} seconds.");

            // Save the total time to Score class
           
           
                sc.totalTime += (int)totalTime; // Add to totalTime
                sc.UpdateCurrentScore(); // Save new data
            
        }
    }

    // Automatically called when the application quits
    private void OnApplicationQuit()
    {
        StopTimer(); // Call StopTimer when the application is closing
        Debug.Log("Application is quitting. Timer stopped.");
    }
}
