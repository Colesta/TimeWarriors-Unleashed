using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; } // Singleton instance

    public int Money = 0;
    public int EnemiesDefeated = 0;
    public int CurrentTime = 0; 
    public float CurrentDistance = 0F; 

    private string usersDataPath = "/Resources/Score.json"; //File Path

    [Serializable]
    public class UserData
    {
        public string username; 
        public int money;
        public int enemiesDefeated; 
        public int totalTime; 
        public float distanceRan; 
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
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }

        Instance = this; // Assign the singleton instance
        DontDestroyOnLoad(gameObject); // Preserve this GameObject across scenes
    }

    //When a Player registers or Signs in, this method will be called to create a new Game Session in the Score Json File
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


            UserData newUserData = new UserData
            {
                username = username,
                money = Money,
                enemiesDefeated = EnemiesDefeated,
                totalTime = CurrentTime,
                distanceRan = CurrentDistance
            };

            string path = Application.dataPath + usersDataPath;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                userArray = JsonUtility.FromJson<UserDataArray>(json) ?? new UserDataArray();
            }
            else
            {
                Debug.Log("File not found, creating a new one.");
                userArray = new UserDataArray();
            }

            userArray.score.Add(newUserData);

            string newJson = JsonUtility.ToJson(userArray, true);
            File.WriteAllText(path, newJson);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception in NewRun(): {ex.Message}");
        }
    }

    //Will be called throughout the code to assign changed Score varibales to the Json File
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
                    // Update currentUserData with the latest session values
                    currentUserData.money = Money; // Set the latest money
                    currentUserData.enemiesDefeated = EnemiesDefeated; // Set the latest enemies defeated
                    currentUserData.totalTime = CurrentTime; // Save the current time as total time
                    currentUserData.distanceRan = CurrentDistance; // Save the current distance as distance ran


                    string updatedJson = JsonUtility.ToJson(userArray, true);
                    File.WriteAllText(path, updatedJson);
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

