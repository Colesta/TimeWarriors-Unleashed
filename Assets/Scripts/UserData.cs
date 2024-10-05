using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objects representing the data in the User Json File
[System.Serializable]
public class Users
{
    public string username;
    public string password;
}

//Objects representing the data in the Score Json File
[System.Serializable]
public class UserData
{
    public string username;
    public int money;
    public int enemiesDefeated;
    public int totalTime;
    public int distanceRan;


    
}


