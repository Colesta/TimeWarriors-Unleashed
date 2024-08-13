using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    private Stats s;
    private Score sc;

    void Awake()
    {
        s = GetComponent<Stats>();
        sc = GetComponent<Score>();
    }


    public Image Level1Background;
    public Image Level2Background;
    public Image Level3Background;
    
    
    

    void Start()
    {

        //Sets the background of the battle screen to diffenret images bsed on the current level

        // if (s.CurrentLevel == 1)
        // {
        //     Background.sprite = Level1;
        // }
        // else if (s.CurrentLevel == 2)
        // {
        //     Background.sprite = Level2;
        // }
       

    }

    //Increase level count by one

    public void AddLevel()
    {
        s.CurrentLevel += 1;
    }


    //When button is pressed on lose scrreen, will run this code. Updates your scores
    public void LoseScreen()
    {
        sc.UpdateCurrentScore();
       
    }


    //When button is pressed on win scrreen, will run this code. Updates your scores
    public void WinScreen()
    {
        sc.UpdateCurrentScore();
       
        s.CurrentLevel = 1;
    }



}
