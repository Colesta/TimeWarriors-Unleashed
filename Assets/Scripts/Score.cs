using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class Score : MonoBehaviour
{
    public GameObject LoginScreen;
    public GameObject MainMenu;

    public TMP_InputField InputUser;
    public TMP_InputField InputPass;

    public TextMeshProUGUI Error;
    public TextMeshProUGUI UserPlaying;

    private string CurrentUser;
    private string CurrentPass;

    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI EnemiesDefeatedText;
    public TextMeshProUGUI TotalRunsText;
    public TextMeshProUGUI CompletedRunsText;

    public int Money = 250;
    public int EnemiesDefeated = 0;
    public int TotalRuns = 0;
    public int CompletedRuns = 0;
    public int HealthPotions = 0;
    public int ManaPotions = 0;

    public string usersDataPath = "Resources/Scores.json";

    public void GetLoginInputValue()
    {
        CurrentUser = InputUser.text;
        CurrentPass = InputPass.text;
    }

    public void ValidateLogin()
    {
        GetLoginInputValue();
        string path = Application.dataPath + "/" + usersDataPath;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            UserData[] usersData = JsonUtility.FromJson<UserDataArray>(json).users;

            foreach (UserData user in usersData)
            {
                if (string.Equals(user.username, CurrentUser, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(user.password, CurrentPass))
                {
                    Money = user.money;
                    EnemiesDefeated = user.enemiesDefeated;
                    TotalRuns = user.totalRuns;
                    CompletedRuns = user.completedRuns;
                    HealthPotions = user.healthPotions;
                    ManaPotions = user.manaPotions;

                    SetMenuScore();
                    DismissLogin();

                    return;
                }
            }

            Error.text = "Incorrect Username or Password";
        }
        else
        {
            Error.text = "File Doesn't Exist";
        }
    }

    [System.Serializable]
    private class UserDataArray
    {
        public UserData[] users;
    }

    public string getCurrentUser()
    {
        return CurrentUser;
    }

    public void Register()
    {
        GetLoginInputValue();
        Debug.Log("Registering new user: " + CurrentUser);

        UserData newUser = new UserData
        {
            username = CurrentUser,
            password = CurrentPass,
            money = 250,
            enemiesDefeated = 0,
            totalRuns = 0,
            completedRuns = 0,
            healthPotions = 0,
            manaPotions = 0
        };

        string path = Application.dataPath + "/" + usersDataPath;
        Debug.Log("Path to JSON file: " + path);

        UserDataArray userDataArray = new UserDataArray();
        if (File.Exists(path))
        {
            Debug.Log("File exists. Reading existing data.");
            string json = File.ReadAllText(path);
            userDataArray = JsonUtility.FromJson<UserDataArray>(json);
        }
        else
        {
            Debug.Log("File does not exist. Creating new user data array.");
            userDataArray.users = new UserData[0];
        }

        Debug.Log("Current user count: " + userDataArray.users.Length);

        Array.Resize(ref userDataArray.users, userDataArray.users.Length + 1);
        userDataArray.users[userDataArray.users.Length - 1] = newUser;

        string newJson = JsonUtility.ToJson(userDataArray, true); // Pretty-print JSON for readability
        File.WriteAllText(path, newJson);

        Debug.Log("New user registered and data saved. Total users now: " + userDataArray.users.Length);

        SetMenuScore();
        DismissLogin();
    }

    public void UpdateUserStats()
    {
        string username = getCurrentUser();
        string path = Application.dataPath + "/" + usersDataPath;

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

    public void DismissLogin()
    {
        LoginScreen.SetActive(false);
        MainMenu.SetActive(true);
        UserPlaying.text = "Current User Playing: \n" + CurrentUser;
    }

    public void UpdateCurrentScore()
    {
        string path = Application.dataPath + "/" + usersDataPath;

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
        string path = Application.dataPath + "/" + usersDataPath;

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
