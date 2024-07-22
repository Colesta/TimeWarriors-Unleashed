using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{
    public GameObject LoginScreen;
    public GameObject MainMenu;

    public TMP_InputField InputUser;
    public TMP_InputField InputPass;

    public TextMeshProUGUI Error;
    public TextMeshProUGUI UserPlaying;

    private string CurrentUser;
    private string CurrentPass;

    private string usersPath = "/Resources/Users.json";

    [System.Serializable]
    private class User
    {
        public string username;
        public string password;
    }

    [System.Serializable]
    private class UserArray
    {
        public User[] users;
    }

    public void GetLoginInputValue()
    {
        CurrentUser = InputUser.text;
        CurrentPass = InputPass.text;
    }

    public void ValidateLogin()
    {
        Score s = new Score();
        GetLoginInputValue();
        string path = Application.dataPath + usersPath;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            User[] usersData = JsonUtility.FromJson<UserArray>(json).users;


            foreach (User user in usersData)
            {
                if (string.Equals(user.username, CurrentUser, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(user.password, CurrentPass))
                {
                    s.SetMenuScore();
                    s.newRun();
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

    public void Register()
    {
        Score s = new Score();
        GetLoginInputValue();

        User newUser = new User
        {
            username = CurrentUser,
            password = CurrentPass,
        };

        string path = Application.dataPath + usersPath;
        UserArray userArray;

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

        Array.Resize(ref userArray.users, userArray.users.Length + 1);
        userArray.users[userArray.users.Length - 1] = newUser;

        string newJson = JsonUtility.ToJson(userArray, true);
        File.WriteAllText(path, newJson);

        s.SetMenuScore();
        DismissLogin();
    }

    public string GetCurrentUser()
    {
        return this.CurrentUser;
    }

    public void DismissLogin()
    {
        LoginScreen.SetActive(false);
        MainMenu.SetActive(true);
        UserPlaying.text = "Current User Playing: \n" + CurrentUser;
    }
}
