using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class SetStats : MonoBehaviour
{
    public Stats s;
    public Score sc;

    public Stats Hero1;
    public Stats Hero2;
    public Stats Hero3;
    public Stats Hero4;
    public Stats Enemy1;
    public Stats Enemy2;
    public Stats Enemy3;
    public Stats Enemy4;

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
    public class JsonData
    {
        public List<Enemy> Enemies;
    }

    private Enemy randomEnemy;

    void Start()
    {
        InitializeHeroes();
        InitializeEnemies();
        InitializeSliders();
    }

    private void InitializeHeroes()
    {
        Hero1.MaxHP = 500;
        Hero1.CurrentHP = 500;
        Hero1.MaxMana = 100;
        Hero1.CurrentMana = 100;
        Hero1.Type = s.FireType;

        Hero2.MaxHP = 500;
        Hero2.CurrentHP = 500;
        Hero2.MaxMana = 100;
        Hero2.CurrentMana = 100;
        Hero2.Type = s.FireType;

        Hero3.MaxHP = 500;
        Hero3.CurrentHP = 500;
        Hero3.MaxMana = 100;
        Hero3.CurrentMana = 100;
        Hero3.Type = s.FireType;

        Hero4.MaxHP = 500;
        Hero4.CurrentHP = 500;
        Hero4.MaxMana = 100;
        Hero4.CurrentMana = 100;
        Hero4.Type = s.FireType;
    }

    private void InitializeEnemies()
    {
        Stats[] enemies = { Enemy1, Enemy2, Enemy3, Enemy4 };

        for (int i = 0; i < enemies.Length; i++)
        {
            GenerateRandomEnemy();
            enemies[i].MaxHP = randomEnemy.Health;
            enemies[i].CurrentHP = randomEnemy.Health;
            enemies[i].Type = randomEnemy.Element;
        }
    }

    private void InitializeSliders()
    {
        HPH1.maxValue = returnMaxHeroHP(1);
        HPH1.value = returnCurrentHeroHP(1);

        HPH2.maxValue = returnMaxHeroHP(2);
        HPH2.value = returnCurrentHeroHP(2);

        HPH3.maxValue = returnMaxHeroHP(3);
        HPH3.value = returnCurrentHeroHP(3);

        HPH4.maxValue = returnMaxHeroHP(0);
        HPH4.value = returnCurrentHeroHP(0);

        HPE1.maxValue = returnMaxEnemyHP(1);
        HPE1.value = returnCurrentEnemyHP(1);

        HPE2.maxValue = returnMaxEnemyHP(2);
        HPE2.value = returnCurrentEnemyHP(2);

        HPE3.maxValue = returnMaxEnemyHP(3);
        HPE3.value = returnCurrentEnemyHP(3);

        HPE4.maxValue = returnMaxEnemyHP(4);
        HPE4.value = returnCurrentEnemyHP(4);
    }

    private void GenerateRandomEnemy()
    {
        // Load JSON data from Resources folder
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Enemies");
        if (jsonTextAsset == null)
        {
            Debug.LogError("Failed to load JSON file.");
            return;
        }

        // Deserialize JSON data into JsonData object
        JsonData data = JsonUtility.FromJson<JsonData>(jsonTextAsset.text);

        // Check if there are enemies in the data
        if (data.Enemies == null || data.Enemies.Count == 0)
        {
            Debug.LogError("No enemies found in the JSON data.");
            return;
        }

        // Randomly pick one enemy from the array using Unity's Random class
        int randomIndex = UnityEngine.Random.Range(0, data.Enemies.Count);
        randomEnemy = data.Enemies[randomIndex];
    }

    public int returnMaxHeroHP(int index) { /* Implementation */ }
    public int returnCurrentHeroHP(int index) { /* Implementation */ }
    public int returnMaxEnemyHP(int index) { /* Implementation */ }
    public int returnCurrentEnemyHP(int index) { /* Implementation */ }

    public void InitializeScreen()
    {
        Hero1.CurrentHP = 500;
        Hero1.CurrentMana = 100;

        Hero2.CurrentHP = 500;
        Hero2.CurrentMana = 100;

        Hero3.CurrentHP = 500;
        Hero3.CurrentMana = 100;

        Hero4.CurrentHP = 500;
        Hero4.CurrentMana = 100;

        Enemy1.CurrentHP = 500;
        Enemy2.CurrentHP = 500;
        Enemy3.CurrentHP = 500;
        Enemy4.CurrentHP = 500;
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
                return s.FireType;
            case 2:
                return s.IceType;
            case 3:
                return s.WindType;
            case 4:
                return s.ThunderType;
            case 5:
                return s.EclipseType;
            case 6:
                return s.SolarType;
        }
        return null;
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
                return Enemy1.CurrentHP <= 0;
            case 2:
                return Enemy2.CurrentHP <= 0;
            case 3:
                return Enemy3.CurrentHP <= 0;
            case 4:
                return Enemy4.CurrentHP <= 0;
        }
        return false;
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
