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
        sc = GetComponent<Score>() ?? FindObjectOfType<Score>();
        if (sc == null)
        {
            Debug.LogError("Score component not found!");
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

        // Log the total time before saving it to the Score class
        Debug.Log($"Total Time in GameTimer before saving: {totalTime}");

        // Save the total time to Score class
        if (sc != null)
        {
            Score.Instance.CurrentTime += (int)totalTime; // Update totalTime
            Score.Instance.UpdateCurrentScore(); // Save new data
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
    // Ensure all relevant updates are done before application quits
    Debug.Log("Updating score before quitting...");
    Score.Instance.UpdateCurrentScore(); // Update scores first

    // Now stop the timer, ensuring the right values are preserved
    StopTimer();
}

}
