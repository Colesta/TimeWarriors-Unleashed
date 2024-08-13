using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{

    private Score s;
    void Awake()
    {
        s = GetComponent<Score>();
    }

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
                    //s.SetMenuScore();
                    s.NewRun();
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
    try
    {
        GetLoginInputValue();
        Debug.Log($"CurrentUser: {CurrentUser}, CurrentPass: {CurrentPass}");

        if (string.IsNullOrEmpty(CurrentUser) || string.IsNullOrEmpty(CurrentPass))
        {
            Debug.LogError("Username or Password is empty.");
            if (Error != null)
            {
                Error.text = "Username or Password cannot be empty.";
            }
            else
            {
                Debug.LogError("Error text component is not assigned.");
            }
            return;
        }

        User newUser = new User
        {
            username = CurrentUser,
            password = CurrentPass,
        };

        string path = Application.dataPath + usersPath;
        Debug.Log($"Path to user data: {path}");
        UserArray userArray;

        if (File.Exists(path))
        {
            Debug.Log("File exists, reading user data.");
            string json = File.ReadAllText(path);
            userArray = JsonUtility.FromJson<UserArray>(json) ?? new UserArray { users = new User[0] };
            Debug.Log("User data read and parsed.");
        }
        else
        {
            Debug.Log("File does not exist, creating new user array.");
            userArray = new UserArray();
            userArray.users = new User[0];
        }

        Debug.Log("Resizing user array.");
        Array.Resize(ref userArray.users, userArray.users.Length + 1);
        userArray.users[userArray.users.Length - 1] = newUser;

        string newJson = JsonUtility.ToJson(userArray, true);
        File.WriteAllText(path, newJson);
        Debug.Log("User registered and data saved.");

        // Check if 's' is assigned before calling its methods
        if (s != null)
        {
            s.NewRun();
            Debug.Log("s.NewRun() called.");
            //s.SetMenuScore();
            
        }
        else
        {
            Debug.LogError("s is not assigned.");
        }

        // Add additional logs before and after calling DismissLogin()
        Debug.Log("Attempting to call DismissLogin().");
        DismissLogin();
        Debug.Log("DismissLogin() called.");
    }
    catch (Exception ex)
    {
        Debug.LogError("Exception occurred: " + ex.Message);
        Debug.LogError("Stack Trace: " + ex.StackTrace);
    }
}



    public string GetCurrentUser()
    {
        return this.CurrentUser;
    }

    public void DismissLogin()
{
    Debug.Log("DismissLogin method started.");

    if (LoginScreen == null || MainMenu == null || UserPlaying == null)
    {
        Debug.LogError("One or more UI elements are not assigned.");
        return;
    }

    LoginScreen.SetActive(false);
    MainMenu.SetActive(true);
    UserPlaying.text = "Current User Playing: \n" + CurrentUser;

    Debug.Log("DismissLogin method finished.");
}
}
