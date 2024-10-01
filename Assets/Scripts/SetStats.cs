using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetStats : MonoBehaviour
{
    private Score sc;

    

    void Awake()
    {
        sc = GetComponent<Score>();
        
    }

    private Stats Hero1;
    private Stats Hero2;
    private Stats Hero3;
    private Stats Hero4;
    private Stats Enemy1;
    private Stats Enemy2;
    private Stats Enemy3;
    private Stats Enemy4;

    private GameObject[] enemies;

    public Slider HPH1;
    public Slider HPH2;
    public Slider HPH3;
    public Slider HPH4;
    public Slider HPE1;
    public Slider HPE2;
    public Slider HPE3;
    public Slider HPE4;

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
        Debug.Log("Started");
        InitializeSliders();
    }

    void Update(){
        UpdateStats();
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
        hero.MaxMana = 100;
        hero.CurrentMana = 100;
        hero.Type = Stats.FireType;
    }

    private void LoadEnemiesFromJson()
    {
        Debug.Log("Loading enemies from JSON."); // Debug log added
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
        int enemyCount = 0; // Counter to track enemy creation
        for (int i = 0; i < 4; i++)
        {
            // Create enemy and increment count
            if (i == 0) Enemy1 = CreateRandomEnemy();
            else if (i == 1) Enemy2 = CreateRandomEnemy();
            else if (i == 2) Enemy3 = CreateRandomEnemy();
            else if (i == 3) Enemy4 = CreateRandomEnemy();

            enemyCount++;
        }
        Debug.Log("Total enemies created: " + enemyCount); // Log the total number of enemies created
    }

    private Stats CreateRandomEnemy()
    {
        // Create a new GameObject for the enemy
        GameObject enemyObject = new GameObject("Enemy");
        
        // Add the Stats component to the new GameObject
        Stats enemyStats = enemyObject.AddComponent<Stats>();

        if (enemiesList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, enemiesList.Count);
            Enemy randomEnemy = enemiesList[randomIndex];

            // Set stats based on the selected enemy from JSON
            enemyStats.Type = randomEnemy.Element;
            enemyStats.MaxHP = randomEnemy.Health;
            enemyStats.CurrentHP = randomEnemy.Health;
            enemyStats.Damage = randomEnemy.Damage;
        }
        else
        {
            Debug.LogError("Enemy list is empty!");
        }

        return enemyStats;
    }


    private void InitializeSliders()
    {
        if (HPH1 == null || HPH2 == null || HPE1 == null){
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

    


    public int returnMaxHeroHP(int index)
    {
        switch (index)
        {
            case 1:
                return Hero1.MaxHP;
            case 2:
                return Hero2.MaxHP;
            case 3:
                return Hero3.MaxHP;
            case 4:
                return Hero4.MaxHP;
            default:
                Debug.LogError("Invalid hero index: " + index);
                return 0; // or another default value indicating an error
        }
    }
    public int returnCurrentHeroHP(int index)
    {
        switch (index)
        {
            case 1:
                return Hero1.CurrentHP;
            case 2:
                return Hero2.CurrentHP;
            case 3:
                return Hero3.CurrentHP;
            case 4:
                return Hero4.CurrentHP;
            default:
                Debug.LogError("Invalid hero index: " + index);
                return 0; // default value indicating an error
        }
    }

    public int returnMaxEnemyHP(int index)
    {
        switch (index)
        {
            case 1:
                return Enemy1.MaxHP;
            case 2:
                return Enemy2.MaxHP;
            case 3:
                return Enemy3.MaxHP;
            case 4:
                return Enemy4.MaxHP;
            default:
                Debug.LogError("Invalid enemy index: " + index);
                return 0; // default value indicating an error
        }
    }

    public int returnCurrentEnemyHP(int index)
    {
        switch (index)
        {
            case 1:
                return Enemy1.CurrentHP;
            case 2:
                return Enemy2.CurrentHP;
            case 3:
                return Enemy3.CurrentHP;
            case 4:
                return Enemy4.CurrentHP;
            default:
                Debug.LogError("Invalid enemy index: " + index);
                return 0; // or another default value indicating an error
        }
    }



    public void UpdateStats()
    {
        HPH1.value = returnCurrentHeroHP(1);
        HPH2.value = returnCurrentHeroHP(2);
        HPH3.value = returnCurrentHeroHP(3);
        HPH4.value = returnCurrentHeroHP(4);

        HPE1.value = returnCurrentEnemyHP(1);
        HPE2.value = returnCurrentEnemyHP(2);
        HPE3.value = returnCurrentEnemyHP(3);
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

    public string returnType(int Num)
    {
        switch (Num)
        {
            case 1:
                return Stats.FireType;
            case 2:
                return Stats.IceType;
            case 3:
                return Stats.WindType;
            case 4:
                return Stats.ThunderType;
            case 5:
                return Stats.EclipseType;
            case 6:
                return Stats.SolarType;
        }
        return null;
    }

    public int getEnemyDamage(int enemy) {
    switch (enemy)
    {
        case 1:
            return Enemy1.Damage;
        case 2:
            return Enemy2.Damage;
        case 3:
            return Enemy3.Damage;
        case 4:
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
            sc.EnemiesDefeated += 1;
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

public void RestoreHealth(int heroIndex, int amount)
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

public void RestoreMana(int heroIndex, int amount)
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

