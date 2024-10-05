using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserSession
{
    public static string CurrentUser { get; private set; }
    //Used to store and save the Current User playing to be accesible througout all scripts and classes

    // Method to set the username after login
    public static void SetCurrentUser(string username)
    {
        CurrentUser = username;
    }

    // Optional: A method to clear the current user (for logout, etc.)
    public static void ClearUser()
    {
        CurrentUser = null;
    }
}


