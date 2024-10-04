using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private Score sc;
    private EnemySystem es;

    void Awake()
    {
        sc = GetComponent<Score>();
        es = GetComponent<EnemySystem>();
    }

    private Stats Hero1;
    private Stats Hero2;
    private Stats Hero3;
    private Stats Hero4;
    private Stats Enemy1;
    private Stats Enemy2;
    private Stats Enemy3;
    private Stats Enemy4;

    public GameObject Hero1UI;
    public GameObject Hero2UI;
    public GameObject Hero3UI;
    public GameObject Hero4UI;
    public GameObject Enemy1UI;
    public GameObject Enemy2UI;
    public GameObject Enemy3UI;
    public GameObject Enemy4UI;

    public Slider HPH1;
    public Slider HPH2;
    public Slider HPH3;
    public Slider HPH4;
    public Slider HPE1;
    public Slider HPE2;
    public Slider HPE3;
    public Slider HPE4;
    public GameObject Background;
    public Sprite BattleScreen1;
    public Sprite BattleScreen2;
    public Sprite BattleScreen3;

    public Animator[] enemyAnimators = new Animator[4]; // Assign the Animator references for Enemies in the Inspector
    public RuntimeAnimatorController[] enemyIdleAnimators = new RuntimeAnimatorController[4]; // Assign idle animations in the Inspector
    public GameObject[] enemyGameObjects = new GameObject[4]; // Assign the GameObjects for Enemies in the Inspector

    [System.Serializable]
    public class Enemy
    {
        public string Species;
        public string Element;
        public int Health;
        public int Damage;
        public string Image;
    }

    [System.Serializable]
    public class EnemyList
    {
        public List<Enemy> Enemies;
    }

    private List<Enemy> enemiesList;

    void Start()
    {
        InitializeHeroes();
        LoadEnemiesFromJson();
        InitializeEnemies();
        InitializeSliders();
        InitializeScreen();
        StartCoroutine(es.BattleSequence());
        
    }

    void Update()
    {
        UpdateStats();
    }

    public void InitializeScreen()
    {
        Image backgroundImage = Background.GetComponent<Image>();

        if (backgroundImage != null)
        {
            if (LevelSystem.Instance.getCurrentLevel() == 1)
            {
                backgroundImage.sprite = BattleScreen1;
            }
            else if (LevelSystem.Instance.getCurrentLevel() == 2)
            {
                backgroundImage.sprite = BattleScreen2;
            }
            else if (LevelSystem.Instance.getCurrentLevel() == 3)
            {
                backgroundImage.sprite = BattleScreen3;
            }
        }
        else
        {
            Debug.LogError("No Image component found on Background GameObject!");
        }
    }

    private void InitializeHeroes()
    {
        Hero1 = gameObject.AddComponent<Stats>();
        Hero2 = gameObject.AddComponent<Stats>();
        Hero3 = gameObject.AddComponent<Stats>();
        Hero4 = gameObject.AddComponent<Stats>();

        // Set hero stats
        SetHeroStats(Hero1);
        SetHeroStats(Hero2);
        SetHeroStats(Hero3);
        SetHeroStats(Hero4);
    }

    private void SetHeroStats(Stats hero)
    {
        hero.MaxHP = 500;
        hero.CurrentHP = 500;
        hero.MaxMana = 150;
        hero.CurrentMana = 150;
        hero.Type = Stats.FireType; // Change this based on the specific hero
    }

    private void LoadEnemiesFromJson()
    {
        Debug.Log("Loading enemies from JSON.");
        // Load JSON data from Resources folder
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Enemies");
        if (jsonTextAsset == null)
        {
            Debug.LogError("Failed to load JSON file.");
            return;
        }

        // Deserialize JSON data into JsonData object
        EnemyList data = JsonUtility.FromJson<EnemyList>(jsonTextAsset.text);
        enemiesList = data.Enemies;

        if (enemiesList == null || enemiesList.Count == 0)
        {
            Debug.LogError("No enemies found in the JSON data.");
        }
    }

    private void InitializeEnemies()
    {
        for (int i = 0; i < 4; i++)
        {
            // Create enemy
            if (i == 0) Enemy1 = CreateRandomEnemy(i);
            else if (i == 1) Enemy2 = CreateRandomEnemy(i);
            else if (i == 2) Enemy3 = CreateRandomEnemy(i);
            else if (i == 3) Enemy4 = CreateRandomEnemy(i);
        }
    }

    private Stats CreateRandomEnemy(int index)
    {
        // Create a new GameObject for the enemy
        GameObject enemyObject = new GameObject("Enemy" + (index + 1));

        // Add the Stats component to the new GameObject
        Stats enemyStats = enemyObject.AddComponent<Stats>();
        enemyGameObjects[index] = enemyObject; // Store the enemy GameObject for reference

        if (enemiesList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, enemiesList.Count);
            Enemy randomEnemy = enemiesList[randomIndex];

            // Set stats based on the selected enemy from JSON
            enemyStats.Species = randomEnemy.Species;
            enemyStats.Type = randomEnemy.Element;
            enemyStats.MaxHP = randomEnemy.Health;
            enemyStats.CurrentHP = randomEnemy.Health;
            enemyStats.Damage = randomEnemy.Damage;

            // Change animator based on species
            enemyAnimators[index] = enemyObject.AddComponent<Animator>();
            enemyAnimators[index].runtimeAnimatorController = GetEnemyAnimator(randomIndex); // Changed from randomIndex to species index
        }
        else
        {
            Debug.LogError("Enemy list is empty!");
        }

        return enemyStats;
    }

    private RuntimeAnimatorController GetEnemyAnimator(int speciesIndex)
    {
        // Validate species index to ensure it is within bounds
        if (speciesIndex < 0 || speciesIndex >= enemyIdleAnimators.Length)
        {
            Debug.LogError("Invalid species index: " + speciesIndex);
            return null; // Return null or handle error appropriately
        }

        // Return the corresponding animator controller for the specified species
        return enemyIdleAnimators[speciesIndex];
    
    }




    private void InitializeSliders()
    {
        if (HPH1 == null || HPH2 == null || HPE1 == null)
        {
            Debug.LogError("Sliders are not assigned in the Inspector.");
            return;
        }
        HPH1.maxValue = returnMaxHeroHP(1);
        HPH1.value = returnCurrentHeroHP(1);
        HPH2.maxValue = returnMaxHeroHP(2);
        HPH2.value = returnCurrentHeroHP(2);
        HPH3.maxValue = returnMaxHeroHP(3);
        HPH3.value = returnCurrentHeroHP(3);
        HPH4.maxValue = returnMaxHeroHP(4);
        HPH4.value = returnCurrentHeroHP(4);
        HPE1.maxValue = returnMaxEnemyHP(1);
        HPE1.value = returnCurrentEnemyHP(1);
        HPE2.maxValue = returnMaxEnemyHP(2);
        HPE2.value = returnCurrentEnemyHP(2);
        HPE3.maxValue = returnMaxEnemyHP(3);
        HPE3.value = returnCurrentEnemyHP(3);
        HPE4.maxValue = returnMaxEnemyHP(4);
        HPE4.value = returnCurrentEnemyHP(4);
    }

    public void ChangeEnemyOnDeath(int enemyIndex)
{
    switch (enemyIndex)
    {
        case 1:
            if (CheckIfEnemyDead(1))
            {
                Enemy1UI.SetActive(false);
                // Additional logic for Enemy 1 death
                // e.g., trigger animations, display messages, etc.
            }
            break;

        case 2:
            if (CheckIfEnemyDead(2))
            {
                Enemy2UI.SetActive(false);
                // Additional logic for Enemy 2 death
            }
            break;

        case 3:
            if (CheckIfEnemyDead(3))
            {
                Enemy3UI.SetActive(false);
                // Additional logic for Enemy 3 death
            }
            break;

        case 4:
            if (CheckIfEnemyDead(4))
            {
                Enemy4UI.SetActive(false);
                // Additional logic for Enemy 4 death
            }
            break;

        default:
            Debug.LogError("Invalid enemy index: " + enemyIndex);
            break;
    }
}

    public void ChangeEnemyOnDeath()
    {
        for (int i = 0; i < enemyGameObjects.Length; i++)
        {
            if (enemyGameObjects[i] != null)
                enemyGameObjects[i].SetActive(false);
        }
    }

    // Method to get the species of a specific enemy
    public string GetEnemySpecies(int enemyIndex)
    {
        switch (enemyIndex)
        {
            case 1: return Enemy1.Species;
            case 2: return Enemy2.Species;
            case 3: return Enemy3.Species;
            case 4: return Enemy4.Species;
            default:
                Debug.LogError("Invalid enemy index: " + enemyIndex);
                return null; // Ensure a return value for invalid indices
        }
    }

    public int returnMaxHeroHP(int index)
    {
        switch (index)
        {
            case 1: return Hero1.MaxHP;
            case 2: return Hero2.MaxHP;
            case 3: return Hero3.MaxHP;
            case 4: return Hero4.MaxHP;
            default:
                Debug.LogError("Invalid hero index: " + index);
                return 0; // or another default value indicating an error
        }
    }
    
    public int returnCurrentHeroHP(int index)
    {
        switch (index)
        {
            case 1: return Hero1.CurrentHP;
            case 2: return Hero2.CurrentHP;
            case 3: return Hero3.CurrentHP;
            case 4: return Hero4.CurrentHP;
            default:
                Debug.LogError("Invalid hero index: " + index);
                return 0; // default value indicating an error
        }
    }

    public int returnMaxEnemyHP(int index)
    {
        switch (index)
        {
            case 1: return Enemy1.MaxHP;
            case 2: return Enemy2.MaxHP;
            case 3: return Enemy3.MaxHP;
            case 4: return Enemy4.MaxHP;
            default:
                Debug.LogError("Invalid enemy index: " + index);
                return 0; // default value indicating an error
        }
    }

    public int returnCurrentEnemyHP(int index)
    {
        switch (index)
        {
            case 1: return Enemy1.CurrentHP;
            case 2: return Enemy2.CurrentHP;
            case 3: return Enemy3.CurrentHP;
            case 4: return Enemy4.CurrentHP;
            default:
                Debug.LogError("Invalid enemy index: " + index);
                return 0; // default value indicating an error
        }
    }

    public void UpdateStats()
    {
        if (HPH1 != null)
            HPH1.value = returnCurrentHeroHP(1);
        if (HPH2 != null)
            HPH2.value = returnCurrentHeroHP(2);
        if (HPH3 != null)
            HPH3.value = returnCurrentHeroHP(3);
        if (HPH4 != null)
            HPH4.value = returnCurrentHeroHP(4);
        if (HPE1 != null)
            HPE1.value = returnCurrentEnemyHP(1);
        if (HPE2 != null)
            HPE2.value = returnCurrentEnemyHP(2);
        if (HPE3 != null)
            HPE3.value = returnCurrentEnemyHP(3);
        if (HPE4 != null)
            HPE4.value = returnCurrentEnemyHP(4);
    }

    public int returnMaxMana(int num)
    {
        switch (num)
        {
            case 1:
                return Hero1.MaxMana;
            case 2:
                return Hero2.MaxMana;
            case 3:
                return Hero3.MaxMana;
            case 4:
                return Hero4.MaxMana;
        }
        return 0;
    }

    public int returnCurrentMana(int num)
    {
        switch (num)
        {
            case 1:
                return Hero1.CurrentMana;
            case 2:
                return Hero2.CurrentMana;
            case 3:
                return Hero3.CurrentMana;
            case 4:
                return Hero4.CurrentMana;
        }
        return 0;
    }

    public string returnEnemyType(int enemyIndex)
    {
        switch (enemyIndex)
        {
            case 1:
                return Enemy1.Type;
            case 2:
                return Enemy2.Type;
            case 3:
                return Enemy3.Type;
            case 4:
                return Enemy4.Type;
          
        }
        return null;
    }

    public int getEnemyDamage(int enemy) 
{
    switch (enemy) 
    {
        case 1:
            if (Enemy1 == null)
            {
                Debug.LogError("Enemy1 is not assigned.");
                return 0; // or handle it another way
            }
            return Enemy1.Damage;
        case 2:
            if (Enemy2 == null)
            {
                Debug.LogError("Enemy2 is not assigned.");
                return 0;
            }
            return Enemy2.Damage;
        case 3:
            if (Enemy3 == null)
            {
                Debug.LogError("Enemy3 is not assigned.");
                return 0;
            }
            return Enemy3.Damage; 
        case 4:
            if (Enemy4 == null)
            {
                Debug.LogError("Enemy4 is not assigned.");
                return 0;
            }
            return Enemy4.Damage;    
        default:
            Debug.LogError("Invalid enemy index: " + enemy);
            return 0; // Return a default value, or you could throw an exception
    }
}



    public void DamageHero(int target, int damage)
    {
        switch (target)
        {
            case 1:
                Hero1.CurrentHP -= damage;
                UpdateStats();
                break;
            case 2:
                Hero2.CurrentHP -= damage;
                UpdateStats();
                break;
            case 3:
                Hero3.CurrentHP -= damage;
                UpdateStats();
                break;
            case 4:
                Hero4.CurrentHP -= damage;
                UpdateStats();
                break;
        }
    }

    public void RemoveMana(int target, int mana)
    {
        switch (target)
        {
            case 1:
                Hero1.CurrentMana -= mana;
                break;
            case 2:
                Hero2.CurrentMana -= mana;
                break;
            case 3:
                Hero3.CurrentMana -= mana;
                break;
            case 4:
                Hero4.CurrentMana -= mana;
                break;
        }
    }

    public void DamageEnemy(int target, int damage)
    {
        switch (target)
        {
            case 1:
                Enemy1.CurrentHP -= damage;
                UpdateStats();
                break;
            case 2:
                Enemy2.CurrentHP -= damage;
                UpdateStats();
                break;
            case 3:
                Enemy3.CurrentHP -= damage;
                UpdateStats();
                break;
            case 4:
                Enemy4.CurrentHP -= damage;
                UpdateStats();
                break;
        }

        if(CheckIfEnemyDead(target)){
            Score.Instance.EnemiesDefeated += 1;
        }
    }

    public bool CheckIfHeroDead(int num)
    {
        switch (num)
        {
            case 1:
                return Hero1.CurrentHP <= 0;
            case 2:
                return Hero2.CurrentHP <= 0;
            case 3:
                return Hero3.CurrentHP <= 0;
            case 4:
                return Hero4.CurrentHP <= 0;
        }
        return false;
    }

    public bool CheckIfEnemyDead(int num)
{
    switch (num)
    {
        case 1:
            return Enemy1 != null && Enemy1.CurrentHP <= 0;
        case 2:
            return Enemy2 != null && Enemy2.CurrentHP <= 0;
        case 3:
            return Enemy3 != null && Enemy3.CurrentHP <= 0;
        case 4:
            return Enemy4 != null && Enemy4.CurrentHP <= 0;
        default:
            Debug.LogError("Invalid enemy number: " + num);
            return false;
    }
}

public void AddHealth(int heroIndex, int amount)
{
    switch (heroIndex)
        {
            case 1:
                Hero1.CurrentHP += amount;
                break;
            case 2:
                Hero2.CurrentHP += amount;
                break;
            case 3:
                Hero3.CurrentHP += amount;
                break;
            case 4:
                Hero4.CurrentHP += amount;
                break;
        }
    
}

public void AddMana(int heroIndex, int amount)
{
     switch (heroIndex)
        {
            case 1:
                Hero1.CurrentMana += amount;
                break;
            case 2:
                Hero2.CurrentMana += amount;
                break;
            case 3:
                Hero3.CurrentMana += amount;
                break;
            case 4:
                Hero4.CurrentMana += amount;
                break;
        }
}


    public bool AllHeroDead()
    {
        return CheckIfHeroDead(1) && CheckIfHeroDead(2) && CheckIfHeroDead(3) && CheckIfHeroDead(4);
    }

    public bool AllEnemyDead()
    {
        return CheckIfEnemyDead(1) && CheckIfEnemyDead(2) && CheckIfEnemyDead(3) && CheckIfEnemyDead(4);
    }

   


}

