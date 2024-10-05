using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    private float startTime;
    private float totalTime;
    private bool isTiming;

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
            
        }

        StartTimer();
        
    }

    // Call this method to start the timer
    public void StartTimer()
    {
        if (!isTiming) // Prevent restarting the timer if it's already running
        {
            startTime = Time.time; // Capture the start time
            isTiming = true;
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


        // Save the total time to Score class
       Score.Instance.CurrentTime += (int)totalTime; // Update totalTime
            Score.Instance.UpdateCurrentScore(); // Save new data
    }
    else
    {
        Debug.LogWarning("StopTimer called, but timer is not running.");
    }
}


    // Automatically called when the application quits
    private void OnApplicationQuit()
{
    Score.Instance.UpdateCurrentScore(); // Update scores first

    // Now stop the timer, ensuring the right values are preserved
    StopTimer();
}

}
