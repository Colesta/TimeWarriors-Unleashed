using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    // Singleton instance to make LevelSystem persistent
    public static LevelSystem Instance;

    // Current level 
    private int CurrentLevel = 1;

    
        void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}
    

    // Increase level count by one
    public void AddLevel()
    {
        this.CurrentLevel += 1;
        Debug.Log("Current level increased: " + CurrentLevel); 
    }

   //return the Current level
    public int getCurrentLevel()
    {
        return CurrentLevel;
    }

    
    
}
