using System.Collections;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    private Stats s;        
    private GameManager ss;
    private HeroManager hm;

    void Awake()
    {
        s = GetComponent<Stats>();
        ss = GetComponent<GameManager>();
        hm = GetComponent<HeroManager>();

        // Null checks to prevent errors if components are missing
        if (s == null)
            Debug.LogError("Stats component missing on this GameObject.");
        if (ss == null)
            Debug.LogError("GameManager component missing on this GameObject.");
        if (hm == null)
            Debug.LogError("HeroManager component missing on this GameObject.");
    }

    public GameObject BattleScreen;
    public GameObject ResultScreen;
    public GameObject LoseScreen;
    public GameObject WinScreen;


    // Lets the battle sequence happen every 3 seconds, allowing the enemies to automatically take their own "turns"
    public IEnumerator BattleSequence()
    {
        while (!ss.AllEnemyDead())
        {
            int target = GenerateRandomTarget();
            int damage = GenerateRandomEnemyAttack();

            yield return new WaitForSeconds(GetEnemyAttackTime(Difficulty.Instance.GetCurrentDifficulty()));
            ss.DamageHero(target, damage);

            // Check if all enemies are dead
            if (ss.AllEnemyDead())
            {
                // Deactivate the battle screen, update score, and show result/win screen
                BattleScreen.SetActive(false);
                ResultScreen.SetActive(true);

                if (LevelSystem.Instance.getCurrentLevel() == 3)
                {
                    WinScreen.SetActive(true);
                }
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
        int[] possibleDamageValues = {
            ss.getEnemyDamage(1),
            ss.getEnemyDamage(2),
            ss.getEnemyDamage(3),
            ss.getEnemyDamage(4)
        };
        int randomIndex = UnityEngine.Random.Range(0, possibleDamageValues.Length);
        return possibleDamageValues[randomIndex];
    }

    // Coroutine for delayed hero damage
    private int GetEnemyAttackTime(string difficulty)
    {
        switch (difficulty)
        {
            case "Easy":
                return 7;
            case "Medium":
                return 5;
            case "Hard":
                return 2;
            default:
                return 1;  // Return a default time
        }
    }
}
