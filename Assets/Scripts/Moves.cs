using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime;
using System.Security.Cryptography;

public class Moves : MonoBehaviour
{
    public GameObject Hero1Single;
    public GameObject Hero1Multiple;
    public GameObject Hero2Single;
    public GameObject Hero2Multiple;
    public GameObject Hero3Single;
    public GameObject Hero3Multiple;
    public GameObject Hero4Single;
    public GameObject Hero4Multiple;

    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public GameObject Enemy4;

    public Sprite Tombstone;


    public GameObject BackgroundImage;

    public GameObject Buttons;

    public GameObject OptionButton;

    public GameObject Items;

    public TextMeshProUGUI currentHeroText;


    public Stats s;
    public SetStats ss;
    public Score sc;

    public DropdownHandler dh;

    private int CurrentPlayer = 1;


    public TextMeshProUGUI NumHealthP;
    public TextMeshProUGUI NumManaP;


    void Awake()
    {

    }

    void Start()
    {
        //initilaize your inventory so you can use in battle, as well as the current hero
        sc.UpdateCurrentScore();
        NumHealthP.text = "x" +  sc.HealthPotions;
        NumManaP.text = "x" + sc.ManaPotions;
        currentHeroText.text = "Current Hero: " + CurrentPlayer;
    }

    //Makes it the next players turn by increasing current player by 1, and if its 4 setting it back to 1
    public void NextPlayersTurn()
    {
        CurrentPlayer += 1;
        if (CurrentPlayer > 4)
        {
            CurrentPlayer = 1;
        }
        currentHeroText.text = "Current Hero: " + CurrentPlayer;
        Debug.Log(CurrentPlayer + " NextPlayerTurn Method");
    }

   

    //When the single button is pressed and this method is called, diffrent moves will be made visible depedning on the current hero
    public void Single()
    {
        switch (CurrentPlayer)
        {
            case 1:

                Hero1Single.SetActive(true);
                BackgroundImage.SetActive(true);
                OptionButton.SetActive(false);
                break;

            case 2:

                Hero2Single.SetActive(true);
                BackgroundImage.SetActive(true);
                OptionButton.SetActive(false);

                break;

            case 3:

                Hero3Single.SetActive(true);
                BackgroundImage.SetActive(true);
                OptionButton.SetActive(false);

                break;

            case 4:

                Hero4Single.SetActive(true);
                BackgroundImage.SetActive(true);
                OptionButton.SetActive(false);

                break;


        }

    }

    //Same logic as single

    public void Multiple()
    {
        switch (CurrentPlayer)
        {
            case 1:

                Hero1Multiple.SetActive(true);
                BackgroundImage.SetActive(true);
                OptionButton.SetActive(false);
                break;

            case 2:

                Hero2Multiple.SetActive(true);
                BackgroundImage.SetActive(true);
                OptionButton.SetActive(false);

                break;

            case 3:

                Hero3Multiple.SetActive(true);
                BackgroundImage.SetActive(true);
                OptionButton.SetActive(false);

                break;

            case 4:

                Hero4Multiple.SetActive(true);
                BackgroundImage.SetActive(true);
                OptionButton.SetActive(false);

                break;


        }

    }

    //All moves the heroes can do, with move 1 being a single attack wherre you use a drop down menu to select a target and then attack, decreasing the enemies health and decreasing your mana,
    //while mana does the same exect all enemies healths are being decreased. After every move the NextPlayersTurn method is called automatically mkaing it the next players turn. If the current her is dead, 
    //Instead of doing damage the next hero will be able to go

    public void Hero1Move1()
    {
        if (ss.CheckIfHeroDead(1))
        {
            NextPlayersTurn();
            return;
        }

        int damage = 75;
        int target = dh.target;
        int mana = 15;
        ss.DamageEnemy(target, damage);

        ss.RemoveMana(1, mana);

        ss.UpdateStats();

        Hero1Single.SetActive(false);
        BackgroundImage.SetActive(false);

        NextPlayersTurn();

        Buttons.SetActive(true);

        ChangeEnemyOnDeath();

      
    }


    public void Hero1Move2()
    {
        if (ss.CheckIfHeroDead(1))
        {
            NextPlayersTurn();
            return;
        }

        NextPlayersTurn();
        currentHeroText.text = "Current Hero: " + CurrentPlayer;


        int damage = 50;
        int mana = 30;

        ss.RemoveMana(1, mana);


        ss.DamageEnemy(1, damage);
        ss.DamageEnemy(2, damage);
        ss.DamageEnemy(3, damage);
        ss.DamageEnemy(4, damage);

        ss.UpdateStats();
        Hero1Multiple.SetActive(false);
        BackgroundImage.SetActive(false);

        Buttons.SetActive(true);


    }

    public void Hero2Move1()
    {
        if (ss.CheckIfHeroDead(2))
        {
            NextPlayersTurn();
            return;
        }

        NextPlayersTurn();
        currentHeroText.text = "Current Hero: " + CurrentPlayer;


        int damage = 75;
        int mana = 15;

        ss.RemoveMana(2, mana);

        int target = dh.target;
        ss.DamageEnemy(target, damage);
        ss.UpdateStats();
        Hero2Single.SetActive(false);
        BackgroundImage.SetActive(false);

        Buttons.SetActive(true);

        ChangeEnemyOnDeath();




    }

    public void Hero2Move2()
    {
        if (ss.CheckIfHeroDead(2))
        {
            NextPlayersTurn();
            return;
        }

        NextPlayersTurn();
        currentHeroText.text = "Current Hero: " + CurrentPlayer;


        int damage = 50;
        int mana = 20;

        ss.RemoveMana(2, mana);

        ss.DamageEnemy(1, damage);
        ss.DamageEnemy(2, damage);
        ss.DamageEnemy(3, damage);
        ss.DamageEnemy(4, damage);

        ss.UpdateStats();
        Hero2Multiple.SetActive(false);
        BackgroundImage.SetActive(false);

        Buttons.SetActive(true);
    }

    public void Hero3Move1()
    {
        if (ss.CheckIfHeroDead(3))
        {
            NextPlayersTurn();
            return;
        }

        NextPlayersTurn();
        currentHeroText.text = "Current Hero: " + CurrentPlayer;


        int damage = 75;
        int mana = 15;

        ss.RemoveMana(3, mana);

        int target = dh.target;
        ss.DamageEnemy(target, damage);
        ss.UpdateStats();
        Hero3Single.SetActive(false);
        BackgroundImage.SetActive(false);

        Buttons.SetActive(true);

        ChangeEnemyOnDeath();

    }

    public void Hero3Move2()
    {
        if (ss.CheckIfHeroDead(3))
        {
            NextPlayersTurn();
            return;
        }

        NextPlayersTurn();
        currentHeroText.text = "Current Hero: " + CurrentPlayer;


        int damage = 50;
        int mana = 30;

        ss.RemoveMana(3, mana);


        ss.DamageEnemy(1, damage);
        ss.DamageEnemy(2, damage);
        ss.DamageEnemy(3, damage);
        ss.DamageEnemy(4, damage);

        ss.UpdateStats();
        Hero3Multiple.SetActive(false);
        BackgroundImage.SetActive(false);

        Buttons.SetActive(true);
    }

    public void Hero4Move1()
    {
        if (ss.CheckIfHeroDead(4))
        {
            NextPlayersTurn();
            return;
        }

        NextPlayersTurn();
        currentHeroText.text = "Current Hero: " + CurrentPlayer;


        int damage = 75;
        int mana = 15;

        ss.RemoveMana(4, mana);

        int target = dh.target;
        ss.DamageEnemy(target, damage);
        ss.UpdateStats();
        Hero4Single.SetActive(false);
        BackgroundImage.SetActive(false);

        Buttons.SetActive(true);

        ChangeEnemyOnDeath();

    }

    public void Hero4Move2()
    {
        if (ss.CheckIfHeroDead(4))
        {
            NextPlayersTurn();
            return;
        }

        NextPlayersTurn();
        currentHeroText.text = "Current Hero: " + CurrentPlayer;


        int damage = 50;
        int mana = 30;

        ss.RemoveMana(4, mana);

        ss.DamageEnemy(1, damage);
        ss.DamageEnemy(2, damage);
        ss.DamageEnemy(3, damage);
        ss.DamageEnemy(4, damage);

        ss.UpdateStats();
        Hero4Multiple.SetActive(false);
        BackgroundImage.SetActive(false);

        Buttons.SetActive(true);
    }

    //Potions will increase either your health or mana for all players, and decrease the amount of that item you have in your inventory by 1

    public void HealthPotion()
    {
        int health = 50;
        ss.HealHero(health);
        NumHealthP.text = "x" +  sc.HealthPotions;
        sc.UpdateCurrentScore();

        Items.SetActive(false);
        Buttons.SetActive(true);
        NextPlayersTurn();

    }

    public void ManaPotion()
    {
        int mana = 50;
        ss.AddMana(mana);
        NumManaP.text = "x" +  sc.HealthPotions;
        sc.UpdateCurrentScore();

        Items.SetActive(false);
        Buttons.SetActive(true);
        NextPlayersTurn();

    }


    //Turn the enemies into little skulls if they die
    private void ChangeEnemyOnDeath()
    {
        if (ss.CheckIfEnemyDead(1))
        {
            SpriteRenderer renderer1 = Enemy1.GetComponent<SpriteRenderer>();
            if (renderer1 == null)
            {
                renderer1 = Enemy1.AddComponent<SpriteRenderer>();
            }
            renderer1.sprite = Tombstone;
        }
        if (ss.CheckIfEnemyDead(2))
        {
            SpriteRenderer renderer2 = Enemy2.GetComponent<SpriteRenderer>();
            if (renderer2 == null)
            {
                renderer2 = Enemy2.AddComponent<SpriteRenderer>();
            }
            renderer2.sprite = Tombstone;
        }
        if (ss.CheckIfEnemyDead(3))
        {
            SpriteRenderer renderer3 = Enemy3.GetComponent<SpriteRenderer>();
            if (renderer3 == null)
            {
                renderer3 = Enemy3.AddComponent<SpriteRenderer>();
            }
            renderer3.sprite = Tombstone;
        }
        if (ss.CheckIfEnemyDead(4))
        {
            SpriteRenderer renderer4 = Enemy4.GetComponent<SpriteRenderer>();
            if (renderer4 == null)
            {
                renderer4 = Enemy4.AddComponent<SpriteRenderer>();
            }
            renderer4.sprite = Tombstone;
        }
    }




}
