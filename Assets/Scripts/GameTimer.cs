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
        // Ensure only one instance of GameTimer exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This makes the GameTimer persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy any additional instances
            Debug.LogWarning("Another instance of GameTimer was destroyed.");
        }

        // Attempt to get the Score component
        sc = GetComponent<Score>();
        if (sc == null)
        {
            Debug.LogError("Score component not found on the GameTimer GameObject!");
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
        else
        {
            Debug.LogWarning("Timer is already running; cannot start it again.");
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
            if (sc != null) // Check if Score component is available
            {
                sc.totalTime += (int)totalTime; // Add to totalTime
                sc.UpdateCurrentScore(); // Save new data
                Debug.Log("Total time updated in Score.");
            }
            else
            {
                Debug.LogError("Cannot update Score because the Score component is null.");
            }
        }
        else
        {
            Debug.LogWarning("StopTimer called, but timer is not running.");
        }
    }

    // Automatically called when the application quits
    private void OnApplicationQuit()
    {
        StopTimer(); // Call StopTimer when the application is closing
        Debug.Log("Application is quitting. Timer stopped.");
    }
}
