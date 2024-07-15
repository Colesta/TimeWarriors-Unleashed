using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    private class UsersArray
    {
        public Users[] users;
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
            UserData[] usersData = JsonUtility.FromJson<UserDataArray>(json).users;

            foreach (UserData user in usersData)
            {
                if (string.Equals(user.username, CurrentUser, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(user.password, CurrentPass))
                {
                    s.SetMenuScore();
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

        UserData newUser = new UserData
        {
            username = CurrentUser,
            password = CurrentPass,
           
        };

        string path = Application.dataPath + usersPath;
        

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

        Array.Resize(ref userDataArray.users, userDataArray.users.Length + 1);
        userDataArray.users[userDataArray.users.Length - 1] = newUser;

        string newJson = JsonUtility.ToJson(userDataArray, true); 
        File.WriteAllText(path, newJson);

        s.SetMenuScore();
        DismissLogin();
    }

    public String getCurrentUser(){
        return this.CurrentUser;
    }
    


    public void DismissLogin()
    {
        LoginScreen.SetActive(false);
        MainMenu.SetActive(true);
        UserPlaying.text = "Current User Playing: \n" + CurrentUser;
    }

}
