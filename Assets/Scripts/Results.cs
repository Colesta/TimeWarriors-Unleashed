using System.Runtime.CompilerServices;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Results : MonoBehaviour
{

    public TextMeshProUGUI CompletionText;
    public TextMeshProUGUI MoneyText;



    // Start is called before the first frame update
    void Start()
    {

        //Adds the money you earned into permanent storage as well as Dislay it on screen
        CompletionText.text = "Level " + LevelSystem.Instance.getCurrentLevel() + " Completed";
        MoneyText.text = "Money:        " + CalculateMoney();

        Score.Instance.UpdateCurrentScore();
        LevelSystem.Instance.AddLevel();
        Score.Instance.Money += CalculateMoney();
        Score.Instance.UpdateCurrentScore();



    }

    //Randomly select an amount of money to be rewarded
    public int CalculateMoney()
    {
        int minRange = 150;
        int maxRange = 500;
        int randomValue = UnityEngine.Random.Range(minRange, maxRange);


        return randomValue;

    }


}
