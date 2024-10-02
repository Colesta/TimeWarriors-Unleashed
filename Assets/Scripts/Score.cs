using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; } // Singleton instance

    public int Money = 0;
    public int EnemiesDefeated = 0;
    public int CurrentTime = 0; // Corrected variable name
    public float CurrentDistance = 0F; // Corrected variable name

    private string usersDataPath = "/Resources/Score.json"; // Updated path

    [Serializable]
    public class UserData
    {
        public string username; // Changed to public
        public int money; // Changed to public
        public int enemiesDefeated; // Changed to public
        public int totalTime; // Changed to public
        public float distanceRan; // Changed to public
    }

    [Serializable]
    public class UserDataArray
    {
        public List<UserData> score = new List<UserData>();
    }

    private UserDataArray userArray = new UserDataArray();

    private void Awake()
    {
        // Check if instance already exists
        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroying duplicate Score instance.");
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }

        Instance = this; // Assign the singleton instance
        DontDestroyOnLoad(gameObject); // Preserve this GameObject across scenes
        Debug.Log("Score instance created and set to persist.");
    }

    public void NewRun()
    {
        try
        {
            string username = UserSession.CurrentUser;
            if (string.IsNullOrEmpty(username))
            {
                Debug.LogError("Username is null or empty.");
                return;
            }

            Debug.Log($"Current user: {username} - Starting a new run.");

            UserData newUserData = new UserData
            {
                username = username,
                money = Money,
                enemiesDefeated = EnemiesDefeated,
                totalTime = CurrentTime,
                distanceRan = CurrentDistance
            };

            string path = Application.dataPath + usersDataPath;
            Debug.Log($"Path for Score.json: {path}");

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                userArray = JsonUtility.FromJson<UserDataArray>(json) ?? new UserDataArray();
                Debug.Log($"Loaded existing data: {json}");
            }
            else
            {
                Debug.Log("File not found, creating a new one.");
                userArray = new UserDataArray();
            }

            userArray.score.Add(newUserData);
            Debug.Log($"New User Data: {JsonUtility.ToJson(newUserData)}"); // Log new user data
            Debug.Log($"Current User Array Count: {userArray.score.Count}"); // Log count

            string newJson = JsonUtility.ToJson(userArray, true);
            File.WriteAllText(path, newJson);
            Debug.Log("User data updated and saved.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception in NewRun(): {ex.Message}");
        }
    }

    public void UpdateCurrentScore()
    {
        try
        {
            string path = Application.dataPath + usersDataPath;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                userArray = JsonUtility.FromJson<UserDataArray>(json);

                string username = UserSession.CurrentUser;
                if (string.IsNullOrEmpty(username))
                {
                    Debug.LogError("Username is null or empty.");
                    return;
                }

                // Loop through the score list in reverse to find the latest entry
                UserData currentUserData = null;
                for (int i = userArray.score.Count - 1; i >= 0; i--)
                {
                    if (userArray.score[i].username == username)
                    {
                        currentUserData = userArray.score[i];
                        break; // Exit the loop after finding the latest instance
                    }
                }

                if (currentUserData != null)
                {
                    // Log current values before saving
                    Debug.Log($"Before update - Money: {Money}, Enemies Defeated: {EnemiesDefeated}, Total Time: {CurrentTime}, Distance Ran: {CurrentDistance}");

                    // Update currentUserData with the latest session values
                    currentUserData.money = Money; // Set the latest money
                    currentUserData.enemiesDefeated = EnemiesDefeated; // Set the latest enemies defeated
                    currentUserData.totalTime = CurrentTime; // Save the current time as total time
                    currentUserData.distanceRan = CurrentDistance; // Save the current distance as distance ran

                    // Log updated user data before saving
                    Debug.Log($"UserData before save - Total Time: {currentUserData.totalTime}, Distance Ran: {currentUserData.distanceRan}");

                    string updatedJson = JsonUtility.ToJson(userArray, true);
                    File.WriteAllText(path, updatedJson);
                    Debug.Log("User data updated and saved after score update.");
                }
                else
                {
                    Debug.LogWarning("Current user data not found in score file.");
                }
            }
            else
            {
                Debug.LogError("Score file does not exist at path: " + path);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception in UpdateCurrentScore(): {ex.Message}");
            Debug.LogError($"Stack Trace: {ex.StackTrace}");
        }
    }
}

