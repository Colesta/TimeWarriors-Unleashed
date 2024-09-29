using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserSession
{
    public static string CurrentUser { get; private set; }

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


