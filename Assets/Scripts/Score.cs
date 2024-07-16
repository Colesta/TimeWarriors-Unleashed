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

    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI EnemiesDefeatedText;
    public TextMeshProUGUI TotalRunsText;
    public TextMeshProUGUI CompletedRunsText;
    public TextMeshProUGUI test;

    public int Money = 0;
    public int EnemiesDefeated = 0;
    public int TotalRuns = 0;
    public int CompletedRuns = 0;

    private string usersDataPath = "/Resources/Score.json";

    [System.Serializable]
    public class User
    {
        public string username;
        public int money;
        public int enemiesDefeated;
        public int totalRuns;
        public int completedRuns;
    }

    [System.Serializable]
    public class UserArray
    {
        public User[] users;
    }

    private UserArray userArray;

    public void newRun()
    {
        //will add a new run to score file
    }

    public void UpdateUserStats()
    {
        string username = lo.getCurrentUser();
        string path = Application.dataPath + usersDataPath;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            userArray = JsonUtility.FromJson<UserArray>(json);
            bool userFound = false;

            foreach (var user in userArray.users)
            {
                if (user.username == username)
                {
                    user.money = Money;
                    user.enemiesDefeated = EnemiesDefeated;
                    user.totalRuns = TotalRuns;
                    user.completedRuns = CompletedRuns;

                    userFound = true;
                    break;
                }
            }

            if (userFound)
            {
                string updatedJson = JsonUtility.ToJson(userArray);
                File.WriteAllText(path, updatedJson);
                Debug.Log("User data updated successfully.");
            }
            else
            {
                Debug.LogWarning("User not found. Update failed.");
            }
        }
        else
        {
            Debug.LogWarning("File doesn't exist.");
        }
    }

    public void SetMenuScore()
    {
        MoneyText.text = "Money: " + Money;
        EnemiesDefeatedText.text = "Enemies Defeated: " + EnemiesDefeated;
        TotalRunsText.text = "Total Runs: " + TotalRuns;
        CompletedRunsText.text = "Runs Completed: " + CompletedRuns;
    }

    public void UpdateCurrentScore()
    {
        string path = Application.dataPath + usersDataPath;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            userArray = JsonUtility.FromJson<UserArray>(json);

            foreach (var user in userArray.users)
            {
                if (user.username == lo.getCurrentUser())
                {
                    Money = user.money;
                    EnemiesDefeated = user.enemiesDefeated;
                    TotalRuns = user.totalRuns;
                    CompletedRuns = user.completedRuns;
                    break;
                }
            }
        }
    }

    public void SaveUserData(User userData)
    {
        string path = Application.dataPath + usersDataPath;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            userArray = JsonUtility.FromJson<UserArray>(json);
        }
        else
        {
            userArray = new UserArray();
            userArray.users = new User[0];
        }

        bool userExists = false;
        for (int i = 0; i < userArray.users.Length; i++)
        {
            if (userArray.users[i].username == userData.username)
            {
                userArray.users[i] = userData;
                userExists = true;
                break;
            }
        }

        if (!userExists)
        {
            Array.Resize(ref userArray.users, userArray.users.Length + 1);
            userArray.users[userArray.users.Length - 1] = userData;
        }

        string newJson = JsonUtility.ToJson(userArray, true);
        File.WriteAllText(path, newJson);
    }
}
