using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{

    //Stats used by all GameObjects
    public int MaxHP;
    public int CurrentHP;
    public int MaxMana;
    public int CurrentMana;
    public string Type;

   
    public static string FireType = "Fire";
    public static string IceType = "Ice";
    public static string WindType = "Wind";
    public static string ThunderType = "Thunder";
    public static string EclipseType = "Eclipse";
    public static string SolarType = "Solar";

    //Current level equals 0 as if you click either shop or play, the level will be added by 1 automatically, so initialzing the variable with 0 means that the first level will always be 1
    public int CurrentLevel = 0;

   

}





