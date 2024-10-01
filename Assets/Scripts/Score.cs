using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int Money = 0;
    public int EnemiesDefeated = 0;
    public int totalTime = 0;
    public float distanceRan = 0F; // Ensure this is a float

    private string usersDataPath = "/Resources/Score.json";

    [Serializable]
    public class UserData
    {
        public string username;
        public int money;
        public int enemiesDefeated;
        public int totalTime;
        public float distanceRan; // Change to float
    }

    [Serializable]
    public class UserDataArray
    {
        public List<UserData> score = new List<UserData>();
    }

    private UserDataArray userArray = new UserDataArray();

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

            Debug.Log($"Current user: {username}");
            Debug.Log($"Money: {Money}, EnemiesDefeated: {EnemiesDefeated}, TotalTime: {totalTime}, DistanceRan: {distanceRan}");

            UserData newUserData = new UserData
            {
                username = username,
                money = Money,
                enemiesDefeated = EnemiesDefeated,
                totalTime = totalTime,
                distanceRan = distanceRan // Use float
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
            }

            userArray.score.Add(newUserData);

            string newJson = JsonUtility.ToJson(userArray, true);
            File.WriteAllText(path, newJson);
            Debug.Log("User data updated and saved.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception in NewRun(): {ex.Message}");
            Debug.LogError($"Stack Trace: {ex.StackTrace}");
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

                string username = UserSession.CurrentUser; // Assuming you have a UserSession class
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
                    Money = currentUserData.money;
                    EnemiesDefeated = currentUserData.enemiesDefeated;
                    totalTime = currentUserData.totalTime;
                    distanceRan = currentUserData.distanceRan; // Update distanceRan
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
