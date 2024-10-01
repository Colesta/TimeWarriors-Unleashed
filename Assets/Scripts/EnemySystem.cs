using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    private Stats s;        
    private SetStats ss;
    private HeroManager hm;
    private Score sc;

    void Awake()
    {
        s = GetComponent<Stats>();
        ss = GetComponent<SetStats>();
        hm = GetComponent<HeroManager>();
        sc = GetComponent<Score>();

        // Null checks to prevent errors if components are missing
        if (s == null)
            Debug.LogError("Stats component missing on this GameObject.");
        if (ss == null)
            Debug.LogError("SetStats component missing on this GameObject.");
        if (hm == null)
            Debug.LogError("HeroManager component missing on this GameObject.");
        if (sc == null)
            Debug.LogError("Score component missing on this GameObject.");
    }

    public GameObject BattleScreen;
    public GameObject ResultScreen;
    public GameObject LoseScreen;
    public GameObject WinScreen;

    void Start()
    {
        StartCoroutine(BattleSequence());
    }

    // Lets the battle sequence happen every 3 seconds, allowing the enemies to automatically take their own "turns"
    IEnumerator BattleSequence()
    {
        while (!ss.AllEnemyDead())
        {
            int target = GenerateRandomTarget();
            int damage = GenerateRandomEnemyAttack();

            // Attack a randomly selected target by a random amount of damage every 3 seconds
            yield return new WaitForSeconds(1); 
            ss.DamageHero(target, damage);

            // Check if all enemies are dead
            if (ss.AllEnemyDead())
            {
                // Deactivate the battle screen, update score, and show result/win screen
                BattleScreen.SetActive(false);

               
                ResultScreen.SetActive(true);

               
            }

            // Check if all heroes are dead
            if (ss.AllHeroDead())
            {
                BattleScreen.SetActive(false);
                LoseScreen.SetActive(true);
            }
        }
    }

    // Randomly select between the 4 heroes
    public int GenerateRandomTarget()
    {
        int minRange = 1;
        int maxRange = 4;  // Adjusted to reflect inclusive range
        return UnityEngine.Random.Range(minRange, maxRange + 1);
    }

    // Randomly generate how much damage will be done
    public int GenerateRandomEnemyAttack()
    {
        int[] possibleDamageValues = { ss.getEnemyDamage(1), ss.getEnemyDamage(2), ss.getEnemyDamage(3), ss.getEnemyDamage(4)};
        int randomIndex = UnityEngine.Random.Range(0, possibleDamageValues.Length);
        return possibleDamageValues[randomIndex];
    }

    // Coroutine for delayed hero damage
    IEnumerator DamageHeroWithDelay(int target, int damage)
    {
        yield return new WaitForSeconds(5); // Wait for 5 seconds
        ss.DamageHero(target, damage);
    }
}
