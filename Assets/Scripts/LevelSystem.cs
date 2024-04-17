using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public Stats s;
    public Score sc;

    public Image Background;
    public Sprite Level1;
    public Sprite Level2;
    

    void Start()
    {

        //Sets the background of the battle screen to diffenret images bsed on the current level

        if (s.CurrentLevel == 1)
        {
            Background.sprite = Level1;
        }
        else if (s.CurrentLevel == 2)
        {
            Background.sprite = Level2;
        }
       

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
        sc.TotalRuns += 1;
        s.CurrentLevel = 1;
    }


    //When button is pressed on win scrreen, will run this code. Updates your scores
    public void WinScreen()
    {
        sc.UpdateCurrentScore();
        sc.TotalRuns += 1;
        sc.CompletedRuns += 1;
        s.CurrentLevel = 1;
    }



}
