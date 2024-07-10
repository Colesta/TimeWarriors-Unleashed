using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class Score : MonoBehaviour
{
    

    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI EnemiesDefeatedText;
    public TextMeshProUGUI TotalRunsText;
    public TextMeshProUGUI CompletedRunsText;
    public TextMeshProUGUI test;


    public int Money = 250;
    public int EnemiesDefeated = 0;
    public int TotalRuns = 0;
    public int CompletedRuns = 0;
    public int HealthPotions = 0;
    public int ManaPotions = 0;

    private string usersDataPath = "/Resources/Score.json";



    [System.Serializable]
    private class UserDataArray
    {
        public UserData[] usersData;
    }



    public void UpdateUserStats()
    {
        string username = getCurrentUser();
        string path = Application.dataPath + usersDataPath;

        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            bool userFound = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] userData = lines[i].Split('#');

                if (userData.Length >= 4 && userData[0] == username)
                {
                    userData[2] = Money.ToString();
                    userData[3] = EnemiesDefeated.ToString();
                    userData[4] = TotalRuns.ToString();
                    userData[5] = CompletedRuns.ToString();
                    userData[6] = HealthPotions.ToString();
                    userData[7] = ManaPotions.ToString();

                    lines[i] = string.Join("#", userData);
                    userFound = true;
                    break;
                }
            }

            if (userFound)
            {
                File.WriteAllLines(path, lines);
                Debug.Log("User data updated successfully.");
            }
            else
            {
                Debug.LogWarning("User not found. Update failed.");
            }
        }
        else
        {
            Debug.LogWarning("File Doesn't Exist");
        }
    }

    public void SetMenuScore()
    {
        MoneyText.text = "Money: " + Money;
        EnemiesDefeatedText.text = "Enemies Defeated: " + EnemiesDefeated;
        TotalRunsText.text = "Total Runs: " + TotalRuns;
        CompletedRunsText.text = "Runs Completed: " + CompletedRuns;
    }

   
    public void AddScore()
    {
        string path = Application.dataPath + usersDataPath;

        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                string[] values = line.Split('#');
                string storedUsername = values[0];
                string storedPassword = values[1];

                if (storedUsername == getCurrentUser())
                {
                    Money = int.Parse(values[2]);
                    EnemiesDefeated = int.Parse(values[3]);
                    TotalRuns = int.Parse(values[4]);
                    CompletedRuns = int.Parse(values[5]);
                    HealthPotions = int.Parse(values[6]);
                    ManaPotions = int.Parse(values[7]);
                    break;
                }
            }
        }
    }

    public void SaveUserData(UserData userData)
    {
        string path = Application.dataPath + usersDataPath;

        UserDataArray userDataArray = new UserDataArray();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            userDataArray = JsonUtility.FromJson<UserDataArray>(json);
        }
        else
        {
            userDataArray.users = new UserData[0];
        }

        bool userExists = false;
        for (int i = 0; i < userDataArray.users.Length; i++)
        {
            if (userDataArray.users[i].username == userData.username)
            {
                userDataArray.users[i] = userData;
                userExists = true;
                break;
            }
        }

        if (!userExists)
        {
            Array.Resize(ref userDataArray.users, userDataArray.users.Length + 1);
            userDataArray.users[userDataArray.users.Length - 1] = userData;
        }

        string newJson = JsonUtility.ToJson(userDataArray, true);
        File.WriteAllText(path, newJson);
    }
}
