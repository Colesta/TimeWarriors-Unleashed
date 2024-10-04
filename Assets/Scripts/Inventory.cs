using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Singleton instance to make Inventory persistent
    public static Inventory Instance;

    // Inventory fields
    public bool Ultimate1 = false;
    public bool Ultimate2 = false;
    public bool Ultimate3 = false;
    public bool Ultimate4 = false;

    public int HealthPotions = 0;
    public int ManaPotions = 0;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy any duplicate instances
        }
    }

    // Method to check if the ultimate move for a specific hero is unlocked
    public bool IsUltimateUnlocked(int heroIndex)
    {
        switch (heroIndex)
        {
            case 1:
                return Ultimate1;
            case 2:
                return Ultimate2;
            case 3:
                return Ultimate3;
            case 4:
                return Ultimate4;
            default:
                Debug.LogError("Invalid hero index: " + heroIndex);
                return false; // Consider ultimate unlocked for invalid index
        }
    }
}