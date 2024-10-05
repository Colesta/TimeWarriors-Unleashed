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
    public TextMeshProUGUI numHPot;
    public TextMeshProUGUI numMPot;


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
        //When the Battle Starts, everything needed for the game to run will be initialized
        InitializeHeroes();
        LoadEnemiesFromJson();
        InitializeEnemies();
        InitializeSliders();
        InitializePotions();
        InitializeScreen();
        StartCoroutine(es.BattleSequence());
        
    }

    void Update()
    {
        //Always calling this so the player always knows the health and mana of all the heroes and health of enemies
        UpdateStats();
    }

    // Change the background of the Game depending on the Current level
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

    //Initilaize all the Hero Objects 
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

    //The Stats that all the heroes share
    private void SetHeroStats(Stats hero)
    {
        hero.MaxHP = 650;
        hero.CurrentHP = 650;
        hero.MaxMana = 200;
        hero.CurrentMana = 200;
    }

    //Set in the UI the amount of each potion that you have 
    private void InitializePotions(){
        numHPot.text = "x" + Inventory.Instance.HealthPotions;
        numMPot.text = "x" + Inventory.Instance.ManaPotions;
    }

    //Get the Enemy data from Json
    private void LoadEnemiesFromJson()
    {
        
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
    //Assign the enemyObject created in CreateRandomEnemy using Json Data to the Enemy Objects in the Battle Screen
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

    //Create an enemy object with the Json Data, randomly choosing an enemy in the file and using its corresponding stats
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

    //Get the Animator for a specific Enemy Species
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



    //Initilaize the Health and Mana Sliders on the Battle Scree, displaying to the Player the State of all the Enemies and Heroes
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

//If an Enemy is dead, their Sprite and UI will dissapear indicating they are dead
    public void ChangeEnemyOnDeath(int enemyIndex)
{
    switch (enemyIndex)
    {
        case 1:
            if (CheckIfEnemyDead(1))
            {
                Enemy1UI.SetActive(false);
                
            }
            break;

        case 2:
            if (CheckIfEnemyDead(2))
            {
                Enemy2UI.SetActive(false);
            }
            break;

        case 3:
            if (CheckIfEnemyDead(3))
            {
                Enemy3UI.SetActive(false);
              
            }
            break;

        case 4:
            if (CheckIfEnemyDead(4))
            {
                Enemy4UI.SetActive(false);
         
            }
            break;

        default:
            Debug.LogError("Invalid enemy index: " + enemyIndex);
            break;
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
    //Return the Max Heroes HP to initalize the Slider
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

    //Return the Current Heroes HP to initalize the Slider, and keep it updated with the current state of the Player
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

//Return the Max Enemies HP to initalize the Slider
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

 //Return the Current Enemy's HP to initalize the Slider, and keep it updated with the current state of the Player
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

//Return the Max Heroes Mana to initalize the Slider
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

 //Return the Current Heroes Mana to initalize the Slider, and keep it updated with the current state of the Player
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

//return the Elemental Type of the Enemy
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

//Get the a specific Enemies Damage. Also Checks that they have been initilaized
    public int getEnemyDamage(int enemy) 
{
    switch (enemy) 
    {
        case 1:
            if (Enemy1 == null)
            {
                Debug.LogError("Enemy1 is not assigned.");
                return 0; 
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
            return 0; 
    }
}


 //Remove health from a certain hero
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

//Remove mana from a certain hero
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

//Remove health from a certain enemy
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

//Check if a certain hero is dead
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

//Check if a certain enemy is dead
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

//add health to a ceratin hero, used in Health Potions
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
//add mana to a ceratin hero, used in Mana Potions
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

//Checks if all the heroes are dead
    public bool AllHeroDead()
    {
        return CheckIfHeroDead(1) && CheckIfHeroDead(2) && CheckIfHeroDead(3) && CheckIfHeroDead(4);
    }
//Checks if all the enemies are dead
    public bool AllEnemyDead()
    {
        return CheckIfEnemyDead(1) && CheckIfEnemyDead(2) && CheckIfEnemyDead(3) && CheckIfEnemyDead(4);
    }

   


}

