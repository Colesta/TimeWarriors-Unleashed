using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    public Stats s;        
    public SetStats ss;
    public Moves m;
    public Score sc;
   

    public GameObject BattleScreen;
    public GameObject ResultScreen;
    public GameObject LoseScreen;
    public GameObject WinScreen;

    void Start()
    {
        StartCoroutine(BattleSequence());

        

    }

    //Lets the Battle sequence happen every 3 seconds, allwoing the enemies to automatically take their own "turns"
    IEnumerator BattleSequence()
    {
        while (!ss.AllEnemyDead())
        {
            int target = GenerateRandomTarget();
            int damage = GenerateRandomEnemyAttack();

            //Attack a randomly selected target by a random amount of damage every 3 seconds
            yield return new WaitForSeconds(3); 
            ss.DamageHero(target, damage);
           


            if (ss.AllEnemyDead())
            {
                //When all enmies die, the battle screen goes away and the amount of enemies defeated is increased and saved to permanent storage
                //If you havnt finished all levels, result screen is brought up, if you have, then you see the win screen
                BattleScreen.SetActive(false);

                sc.UpdateUserStats();
                sc.EnemiesDefeated +=4;


                if (s.CurrentLevel == 2) 
                {
                    WinScreen.SetActive(true);
                    s.CurrentLevel = 1;
                }
                else
                {
                    ResultScreen.SetActive(true);
                }
            }

            //If all heroes died, then you see the lose screen

            if (ss.AllHeroDead())
            {
                BattleScreen.SetActive(false);
                LoseScreen.SetActive(true);

            }
        }
    }

    IEnumerator DamageHeroWithDelay(int target, int damage)
    {
        yield return new WaitForSeconds(5); // Wait for 5 seconds
        ss.DamageHero(target, damage);
    }


    //Randoly select between the 4 heroes
    public int GenerateRandomTarget()
    {
        int minRange = 1;
        int maxRange = 4;
        int randomValue = UnityEngine.Random.Range(minRange, maxRange);
        return randomValue;

    }

    //randomly select how what damge will be done
    public int GenerateRandomEnemyAttack()
    {
        int minRange = 1;
        int maxRange = 4;
        int randomValue = UnityEngine.Random.Range(minRange, maxRange);


        return EnemyAttack(randomValue);


    }
    // The actual value of said damage 
    public int EnemyAttack(int num)
    {
        switch (num)
        {
            case 1:
                return 25;
            case 2:
                return 30;
            case 3:
                return 45;
            case 4:
                return 70;
        }



        return 0;
    }
}


