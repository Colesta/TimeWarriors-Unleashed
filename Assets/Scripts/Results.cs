using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Results : MonoBehaviour
{

    public TextMeshProUGUI CompletionText;
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI Dialouge;

    private Score sc;
    private LevelSystem ls;

    void Awake()
    {
        sc = GetComponent<Score>();
        ls = GetComponent<LevelSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {

        //Adds the money you earned into permanent storage as well as Dislay it on screen
        CompletionText.text = "Level " + ls.getCurrentLevel() + " Completed";
        MoneyText.text = "Money:        " + CalculateMoney();
        Dialouge.text = "Good Job";

        sc.UpdateCurrentScore();
        ls.AddLevel();
        sc.Money += CalculateMoney();



    }

    //Randomly select an amount of money to be rewarded
    public int CalculateMoney()
    {
        int minRange = 75;
        int maxRange = 300;
        int randomValue = UnityEngine.Random.Range(minRange, maxRange);


        return randomValue;

    }


}
