using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public Login lo;

    // public TextMeshProUGUI MoneyText;
    // public TextMeshProUGUI EnemiesDefeatedText;
    // public TextMeshProUGUI totalTimeText;
    // public TextMeshProUGUI distanceRanText;
    

    public int Money = 0;
    public int EnemiesDefeated = 0;
    public int totalTime = 0;
    public int distanceRan = 0;

    private string usersDataPath = "/Resources/Score.json";

    [System.Serializable]
    public class UserData
    {
        public string username;
        public int money;
        public int enemiesDefeated;
        public int totalTime;
        public int distanceRan;
    }

    [System.Serializable]
    public class UserDataArray
    {
        public UserData[] score;
    }

    private UserDataArray userArray;

    //will add a new run to score file
    public void newRun()
{
    try
    {
        // Check if 'lo' is null
        if (lo == null)
        {
            Debug.LogError("lo is not assigned.");
            return;
        }

        // Get current user
        string username = lo.GetCurrentUser();
        Debug.Log($"Current user: {username}");

        // Check if username is null or empty
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("Username is null or empty.");
            return;
        }

        // Check other variables
        Debug.Log($"Money: {Money}, EnemiesDefeated: {EnemiesDefeated}, totalTime: {totalTime}, distanceRan: {distanceRan}");

        // Create new user data entry
        UserData newUserData = new UserData
        {
            username = username,
            money = Money,
            enemiesDefeated = EnemiesDefeated,
            totalTime = totalTime,
            distanceRan = distanceRan
        };

        // Check if usersDataPath is null or empty
        if (string.IsNullOrEmpty(usersDataPath))
        {
            Debug.LogError("usersDataPath is null or empty.");
            return;
        }

        // Load existing data
        string path = Application.dataPath + usersDataPath;
        Debug.Log($"Path to user data: {path}");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            userArray = JsonUtility.FromJson<UserDataArray>(json) ?? new UserDataArray { score = new UserData[0] };
        }
        else
        {
            Debug.Log("File not found, creating a new one.");
            userArray = new UserDataArray();
            userArray.score = new UserData[0];
        }

        // Add new user data to the array
        Array.Resize(ref userArray.score, userArray.score.Length + 1);
        userArray.score[userArray.score.Length - 1] = newUserData;

        // Save updated data
        string newJson = JsonUtility.ToJson(userArray, true);
        File.WriteAllText(path, newJson);
        Debug.Log("User data updated and saved.");
    }
    catch (Exception ex)
    {
        Debug.LogError("Exception in newRun(): " + ex.Message);
        Debug.LogError("Stack Trace: " + ex.StackTrace);
    }
}


    // public void UpdateUserStats()
    // {
    //     string username = lo.GetCurrentUser();
    //     string path = Application.dataPath + usersDataPath;

    //     if (File.Exists(path))
    //     {
    //         string json = File.ReadAllText(path);
    //         userArray = JsonUtility.FromJson<UserDataArray>(json);
    //         bool userFound = false;

    //         foreach (var user in userArray.score)
    //         {
    //             if (user.username == username)
    //             {
    //                 user.money = Money;
    //                 user.enemiesDefeated = EnemiesDefeated;
                     

    //                 userFound = true;
    //                 break;
    //             }
    //         }

    //         if (userFound)
    //         {
    //             string updatedJson = JsonUtility.ToJson(userArray);
    //             File.WriteAllText(path, updatedJson);
    //             Debug.Log("User data updated successfully.");
    //         }
    //         else
    //         {
    //             Debug.LogWarning("User not found. Update failed.");
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogWarning("File doesn't exist.");
    //     }
    // }

    public void SetMenuScore()
    {
        // MoneyText.text = "Money: " + Money;
        // EnemiesDefeatedText.text = "Enemies Defeated: " + EnemiesDefeated;
       
    }

    public void UpdateCurrentScore()
    {
        string path = Application.dataPath + usersDataPath;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            userArray = JsonUtility.FromJson<UserDataArray>(json);

            foreach (var user in userArray.score)
            {
                if (user.username == lo.GetCurrentUser())
                {
                    Money = user.money;
                    EnemiesDefeated = user.enemiesDefeated;
                    
                    break;
                }
            }
        }
    }

    public void SaveUserData(UserData userData)
    {
        string path = Application.dataPath + usersDataPath;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            userArray = JsonUtility.FromJson<UserDataArray>(json);
        }
        else
        {
            userArray = new UserDataArray();
            userArray.score = new UserData[0];
        }

        bool userExists = false;
        for (int i = 0; i < userArray.score.Length; i++)
        {
            if (userArray.score[i].username == userData.username)
            {
                userArray.score[i] = userData;
                userExists = true;
                break;
            }
        }

        if (!userExists)
        {
            Array.Resize(ref userArray.score, userArray.score.Length + 1);
            userArray.score[userArray.score.Length - 1] = userData;
        }

        string newJson = JsonUtility.ToJson(userArray, true);
        File.WriteAllText(path, newJson);
    }
}
