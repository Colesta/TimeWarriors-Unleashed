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
        // Try to find the Score component either in the same GameObject or in the scene
        s = GetComponent<Score>() ?? FindObjectOfType<Score>();
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

    [Serializable]
    private class User
    {
        public string username;
        public string password;
    }

    [Serializable]
    private class UserArray
    {
        public User[] users;
    }

    // Get username and password input from UI fields
    public void GetLoginInputValue()
    {
        CurrentUser = InputUser.text;
        CurrentPass = InputPass.text;
    }

    // Validate the login credentials
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
                // Set the username globally after successful login
                UserSession.SetCurrentUser(CurrentUser);

                s.NewRun();  // Assuming 's' is set up
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


    // Register a new user
    public void Register()
    {
        try
        {
            GetLoginInputValue();

            if (string.IsNullOrEmpty(CurrentUser) || string.IsNullOrEmpty(CurrentPass))
            {
                Error.text = "Username or Password is empty.";
                return;
            }

            if (UsernameExists())
            {
                Error.text = "Username already exists.";
                return;
            }

            if (CurrentUser.Length < 5)
            {
                Error.text = "Username must be at least 5 characters.";
                return;
            }

            if (CurrentPass.Length < 8 || !PasswordLowerCheck() || !PasswordUpperCheck() || !PasswordDigitCheck())
            {
                Error.text = "Password needs to be 8 characters, have a Lowercase, Uppercase and a Digit.";
                return;
            }

            User newUser = new User
            {
                username = CurrentUser,
                password = CurrentPass
            };

            string path = Application.dataPath + usersPath;
            UserArray userArray;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                userArray = JsonUtility.FromJson<UserArray>(json) ?? new UserArray { users = new User[0] };
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

            if (s != null)
            {
                s.NewRun();
            }
            else
            {
                Debug.LogError("Score component is null.");
            }

            DismissLogin();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception occurred: {ex.Message}");
        }
    }

    // Return the current username
    public string GetCurrentUser()
    {
        return CurrentUser;
    }

    // Dismiss login screen and show the main menu
    public void DismissLogin()
    {
        if (LoginScreen == null || MainMenu == null || UserPlaying == null)
        {
            Debug.LogError("One or more UI elements are not assigned.");
            return;
        }

        LoginScreen.SetActive(false);
        MainMenu.SetActive(true);
        UserPlaying.text = "Current User Playing: \n" + CurrentUser;
    }

    // Check if the username already exists
    public bool UsernameExists()
    {
        string path = Application.dataPath + usersPath;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            User[] usersData = JsonUtility.FromJson<UserArray>(json).users;

            foreach (User user in usersData)
            {
                if (user.username == CurrentUser)
                {
                    return true; // Username exists
                }
            }
        }
        return false; // Username doesn't exist
    }

    // Check if the password contains a lowercase letter
    public bool PasswordLowerCheck()
    {
        foreach (char letter in CurrentPass)
        {
            if (char.IsLower(letter)) return true;
        }
        return false;
    }

    // Check if the password contains an uppercase letter
    public bool PasswordUpperCheck()
    {
        foreach (char letter in CurrentPass)
        {
            if (char.IsUpper(letter)) return true;
        }
        return false;
    }

    // Check if the password contains a digit
    public bool PasswordDigitCheck()
    {
        foreach (char letter in CurrentPass)
        {
            if (char.IsDigit(letter)) return true;
        }
        return false;
    }
}
