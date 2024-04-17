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

    public string usersDataPath = "/Resources/Scores.txt";


    //Get String values inputted in Text Areas
    public void GetLoginInputValue()
    {
        CurrentUser = InputUser.text;
        CurrentPass = InputPass.text;

    }

   
    //Check if the username and password correlate to any username and password in permanent storage, if so then initialize all the variables above to the values saved in permanent storge that belong to
    //that specific user
    public void ValidateLogin()
    {
        GetLoginInputValue();    

        string path = Application.dataPath + usersDataPath;

        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                string[] values = line.Split('#');
                string storedUsername = values[0];
                string storedPassword = values[1];

                if (string.Equals(storedUsername, CurrentUser, StringComparison.OrdinalIgnoreCase) && string.Equals(storedPassword, CurrentPass))
                {
                    Money = int.Parse(values[2]);
                    EnemiesDefeated = int.Parse(values[3]);
                    TotalRuns = int.Parse(values[4]);
                    CompletedRuns = int.Parse(values[5]);
                    HealthPotions = int.Parse(values[6]);
                    ManaPotions = int.Parse(values[7]); ;

                    SetMenuScore();
                    DismissLogin();


                    break; 
                }
                else
                {
                    Error.text = "Incorrect Username or Password";
                }
            }
        }
        else
        {
            Error.text = "File Doesnt Exist";
        }
    }

    //return the name of the current user
    public string getCurrentUser()
    {
        return CurrentUser;
    }

    //Adds the username and passowrd of a new user into permanent storage and adds default values into each variable
    public void Register()
    {
        GetLoginInputValue();
        
        int money = 250;
        int enemiesDefeated = 0;
        int TotalRuns = 0;
        int CompletedRuns = 0;
        int HealthPotions = 0;
        int ManaPotions = 0;

        string userData = $"{CurrentUser}#{CurrentPass}#{money}#{enemiesDefeated}#{TotalRuns}#{CompletedRuns}#{HealthPotions}#{ManaPotions}";

        string path = Application.dataPath + usersDataPath;

        using (StreamWriter writer = new StreamWriter(path, true))
        {
            writer.WriteLine(userData);
        }

        SetMenuScore();
        DismissLogin();
    }

    //Takes the current values of the variables stored in secondary storage for the user and saves it into permanent storage
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
                    // Update the user's data
                    userData[2] = Money.ToString();
                    userData[3] = EnemiesDefeated.ToString();
                    userData[4] = TotalRuns.ToString();
                    userData[5] = CompletedRuns.ToString();
                    userData[6] = HealthPotions.ToString();
                    userData[7] = ManaPotions.ToString();

                    // Join the updated user data back into a single string
                    lines[i] = string.Join("#", userData);
                    userFound = true;
                    break;
                }
            }

            if (userFound)
            {
                // Now, you should write the modified data back to the file
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
            Debug.LogWarning("File Doesnt Exist");
        }
    }

    //set the score you see on the main menu to that relaring to current user
    public void SetMenuScore()
    {
        MoneyText.text = "Money: " + Money;
        EnemiesDefeatedText.text = "Enemies Defeated: " + EnemiesDefeated;
        TotalRunsText.text = "Total Runs: " + TotalRuns;
        CompletedRunsText.text = "Runs Completed: " + CompletedRuns;
    }

    //make the login screen go away, make the main menu screen active, and dislay who is currently playing
    public void DismissLogin()
    {
        LoginScreen.SetActive(false);
        MainMenu.SetActive(true);
        UserPlaying.text = "Current User Playing: \n" + CurrentUser;
    }

    //Sets the variables stored in this class to those that correlate to the current user
    public void UpdateCurrentScore()
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
                    ManaPotions = int.Parse(values[7]); ;
                    break;
                }
            }
        }
    }

}
