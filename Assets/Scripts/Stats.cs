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
    public int Damage;

    public bool Ultimate1 = false;
    public bool Ultimate2 = false;
    public bool Ultimate3 = false;
    public bool Ultimate4 = false;

    public int HealthPotions = 0;
    public int ManaPotions = 0;
    
    

   
    public static string FireType = "Fire";
    public static string IceType = "Ice";
    public static string WindType = "Wind";
    public static string ThunderType = "Thunder";
    public static string EclipseType = "Eclipse";
    public static string SolarType = "Solar";

    public int CurrentLevel = 1;

   

}





