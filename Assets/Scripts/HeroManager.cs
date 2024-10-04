using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroManager : MonoBehaviour
{
    public Sprite Tombstone;

    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public GameObject Enemy4;

    public GameObject BackgroundImage;
    public GameObject Buttons;
    public GameObject OptionButton;
    public GameObject Items;
    public TextMeshProUGUI currentHeroText;

    public GameObject sharedUltimateButton; // Reference to the shared button for the ultimate move

    public DropdownHandler dh;

    private int CurrentPlayer = 1;
    private int CurrentMove = 1;
    private float storedAttackPower; // Variable to hold the stored attack power

    public TextMeshProUGUI NumHealthP;
    public TextMeshProUGUI NumManaP;

    private GameManager ss;
    private AttackBarController ab;


    private Hero[] heroes; // Array of heroes

    void Awake()
    {
        ss = GetComponent<GameManager>();
        ab = GetComponent<AttackBarController>();


        // Initialize heroes
        heroes = new Hero[]
        {
        new Hero("Hero1", new MoveInfo[]
        {
            new MoveInfo(GetMoveDamage(1,1), GetMoveManaCost(1,1), Stats.FireType, false, GetMoveName(1, 1), GetMoveDescription(1, 1)),   // Move1
            new MoveInfo(GetMoveDamage(1,2), GetMoveManaCost(1,2), Stats.FireType, false, GetMoveName(1, 2), GetMoveDescription(1, 2)),    // Move2
            new MoveInfo(GetMoveDamage(1,3), GetMoveManaCost(1,3), Stats.FireType, true, GetMoveName(1, 3), GetMoveDescription(1, 3))   // Ultimate Move
        }),
        new Hero("Hero2", new MoveInfo[]
        {
            new MoveInfo(GetMoveDamage(2,1), GetMoveManaCost(2,1), Stats.IceType, true, GetMoveName(2, 1), GetMoveDescription(2, 1)),    // Move1
            new MoveInfo(GetMoveDamage(2,2), GetMoveManaCost(2,2), Stats.IceType, false, GetMoveName(2, 2), GetMoveDescription(2, 2)),     // Move2
            new MoveInfo(GetMoveDamage(2,3), GetMoveManaCost(2,3), Stats.IceType, true, GetMoveName(2, 3), GetMoveDescription(2, 3))     // Ultimate Move
        }),
        new Hero("Hero3", new MoveInfo[]
        {
            new MoveInfo(GetMoveDamage(3,1), GetMoveManaCost(3,1), Stats.ThunderType, true, GetMoveName(3, 1), GetMoveDescription(3, 1)), // Move1
            new MoveInfo(GetMoveDamage(3,2), GetMoveManaCost(3,2), Stats.ThunderType, true, GetMoveName(3, 2), GetMoveDescription(3, 2)),  // Move2
            new MoveInfo(GetMoveDamage(3,3), GetMoveManaCost(3,3), Stats.ThunderType, false, GetMoveName(3, 3), GetMoveDescription(3, 3)) // Ultimate Move
        }),
        new Hero("Hero4", new MoveInfo[]
        {
            new MoveInfo(GetMoveDamage(4,1), GetMoveManaCost(4,1), Stats.WindType, false, GetMoveName(4, 1), GetMoveDescription(4, 1)),     // Move1
            new MoveInfo(GetMoveDamage(4,2), GetMoveManaCost(4,2), Stats.WindType, true, GetMoveName(4, 2), GetMoveDescription(4, 2)),     // Move2
            new MoveInfo(GetMoveDamage(4,3), GetMoveManaCost(4,3), Stats.WindType, true, GetMoveName(4, 3), GetMoveDescription(4, 3))     // Ultimate Move
        })
        };
    }


    void Start()
    {
        currentHeroText.text = "Current Hero: " + CurrentPlayer;
        Debug.Log(returnCurrentPlayer() + "");
    }

    public void NextPlayersTurn()
    {
        CurrentPlayer += 1;
        if (CurrentPlayer > 4)
        {
            CurrentPlayer = 1;
        }
        currentHeroText.text = "Current Hero: " + CurrentPlayer;
        UpdateUltimateButtonVisibility(); // Update button visibility when changing players
    }

    private void UpdateUltimateButtonVisibility()
    {
        // Check if the current hero's ultimate move is unlocked
        bool isUltimateUnlocked = Inventory.Instance.IsUltimateUnlocked(CurrentPlayer); // Make sure you have this method in Inventory
        sharedUltimateButton.SetActive(isUltimateUnlocked); // Set the button active or inactive
    }

    public int returnCurrentPlayer()
    {
        return this.CurrentPlayer;
    }

    public int returnCurrentMove()
    {
        return this.CurrentMove;
    }

    public void StoreAttackPower(float power)
    {
        storedAttackPower = power; // Store the attack power as a float
        Debug.Log("Stored Attack Power: " + storedAttackPower);
    }

    // Coroutine for handling the attack bar input and applying damage
    private IEnumerator ExecuteHeroMove(int heroIndex)
    {
        
        MoveInfo moveInfo = GetMoveInfo(heroIndex, CurrentMove);
        string playerMoveType = moveInfo.Type; // Get the player's move type
        int target = dh.SelectedTarget(); //get the enemy targetted

      

        Debug.Log("Executing hero move for hero index: " + heroIndex + " Move: " + moveInfo.Name);

        // Attack bar logic
        
        yield return StartCoroutine(ab.StartAttackBar());
        

        // Additional debug for mana cost application
        Debug.Log("Removing mana for hero index: " + heroIndex);
        ss.RemoveMana(heroIndex, moveInfo.ManaCost);
         // Calculate damage
        float effectivenessMultiplier = ElementalEffect(playerMoveType, target);
        int totalDamage = (int)(moveInfo.Damage * storedAttackPower * effectivenessMultiplier);

        Debug.Log("Total damage calculated: " + totalDamage);

        if (moveInfo.IsMultiTarget)
        {
            // Apply damage to all enemies for multi-target moves
            for (int i = 1; i <= 4; i++)
            {
                ss.DamageEnemy(i, totalDamage); // Make sure totalDamage is an int
            }
        }
        else
        {
            // Get selected target from dropdown
            
            ss.DamageEnemy(target, totalDamage); // Make sure totalDamage is an int
        }

        // Update stats and change enemy appearance if dead
        ss.UpdateStats();

        ss.ChangeEnemyOnDeath();

        // End turn
        NextPlayersTurn(); // Move to the next player's turn

        // Check if the next players are dead and skip them if necessary
        if (ss.CheckIfHeroDead(CurrentPlayer))
        {
            yield break; // If the current player is dead, stop execution
        }

        // Check the next players
        if (ss.CheckIfHeroDead(CurrentPlayer + 1))
        {
            NextPlayersTurn();
            if (ss.CheckIfHeroDead(CurrentPlayer + 2))
            {
                NextPlayersTurn();
                if (ss.CheckIfHeroDead(CurrentPlayer + 3))
                {
                    NextPlayersTurn();
                }
            }
        }
    }

    public void Move1()
    {
        Debug.Log("Move 1 method is being accessed for player: " + CurrentPlayer);
        CurrentMove = 1; // Set current move to Move1
        StartCoroutine(ExecuteHeroMove(CurrentPlayer));
    }

    public void Move2()
    {
        CurrentMove = 2; // Set current move to Move2
        StartCoroutine(ExecuteHeroMove(CurrentPlayer));
    }

    public void Ultimate()
    {
        CurrentMove = 3; // Set current move to Ultimate Move
        StartCoroutine(ExecuteHeroMove(CurrentPlayer));
    }

    public MoveInfo GetMoveInfo(int heroIndex, int moveIndex)
    {
        if (heroIndex < 1 || heroIndex > heroes.Length)
        {
            Debug.LogError("Hero index is out of range!");
            return new MoveInfo(0, 0, "", false, "Unknown Move", "No description available."); // Default case
        }

        Hero hero = heroes[heroIndex - 1]; // Subtract 1 for zero-based index
        if (moveIndex < 1 || moveIndex > hero.Moves.Length)
        {
            Debug.LogError("Move index is out of range!");
            return new MoveInfo(0, 0, "", false, "Unknown Move", "No description available."); // Default case
        }

        // Get the move info and set its description
        MoveInfo moveInfo = hero.Moves[moveIndex - 1]; // Subtract 1 for zero-based index
        moveInfo.Description = GetMoveDescription(heroIndex, moveIndex); // Set the description
        return moveInfo;
    }

  private int GetMoveDamage(int heroIndex, int moveIndex)
    {
        switch (heroIndex)
        {
            case 1:
                switch (moveIndex)
                {
                    case 1: return 50;
                    case 2: return 75;
                    case 3: return 350;
                    default: return 0;
                }
            case 2:
                switch (moveIndex)
                {
                    case 1: return 45;
                    case 2: return 85;
                    case 3: return 280;
                    default: return 0;
                }
            case 3:
                switch (moveIndex)
                {
                  case 1: return 75;
                    case 2: return 90;
                    case 3: return 400;
                    default: return 0;
                }
            case 4:
                switch (moveIndex)
                {
                    case 1: return 120;
                    case 2: return 60;
                    case 3: return 240;
                    default: return 0;
                }
            default:
                return 0;
        }
    }
    private int GetMoveManaCost(int heroIndex, int moveIndex)
    {
        switch (heroIndex)
        {
            case 1:
                switch (moveIndex)
                {
                    case 1: return 10;
                    case 2: return 25;
                    case 3: return 50;
                    default: return 0;
                }
            case 2:
                switch (moveIndex)
                {
                    case 1: return 8;
                    case 2: return 30;
                    case 3: return 55;
                    default: return 0;
                }
            case 3:
                switch (moveIndex)
                {
                  case 1: return 20;
                    case 2: return 30;
                    case 3: return 60;
                    default: return 0;
                }
            case 4:
                switch (moveIndex)
                {
                    case 1: return 30;
                    case 2: return 20;
                    case 3: return 45;
                    default: return 0;
                }
            default:
                return 0;
        }
    }

    private string GetMoveName(int heroIndex, int moveIndex)
    {
        switch (heroIndex)
        {
            case 1:
                switch (moveIndex)
                {
                    case 1: return "Cross Slash";
                    case 2: return "Inferno Blade";
                    case 3: return "Myriad Truths";
                    default: return "Unknown move.";
                }
            case 2:
                switch (moveIndex)
                {
                    case 1: return "Cyclone Slash";
                    case 2: return "Killer Wind";
                    case 3: return "Flying Fujin";
                    default: return "Unknown move.";
                }
            case 3:
                switch (moveIndex)
                {
                    case 1: return "FrostBite";
                    case 2: return "Ice Age";
                    case 3: return "Divine Judgement";
                    default: return "Unknown move.";
                }
            case 4:
                switch (moveIndex)
                {
                    case 1: return "SkullCracker";
                    case 2: return "Wild Thunder";
                    case 3: return "Thor's Fury";
                    default: return "Unknown move.";
                }
            default:
                return "Unknown move.";
        }
    }

    // Method to retrieve the move description based on hero and move index
    private string GetMoveDescription(int heroIndex, int moveIndex)
    {
        switch (heroIndex)
        {
            case 1:
                switch (moveIndex)
                {
                    case 1: return "Two Quick slashes of your blade";
                    case 2: return "A powerful strike imbuded with your flames";
                    case 3: return "ALl will be revealed!";
                    default: return "Unknown move.";
                }
            case 2:
                switch (moveIndex)
                {
                    case 1: return "Your blade will cut as fast as a powerful Cyclone";
                    case 2: return "A flurry of cuts and slashes, imbuded with the spirit of the wind";
                    case 3: return "Emobdy the power of the Wind God Fujin";
                    default: return "Unknown move.";
                }
            case 3:
                switch (moveIndex)
                {
                    case 1: return "Engluf your enemies in an Icy Prison";
                    case 2: return "Unleash the power of Winter on your foes";
                    case 3: return "Let the heavens Judge your foe";
                    default: return "Unknown move.";
                }
            case 4:
                switch (moveIndex)
                {
                    case 1: return "A powerful strike to the head";
                    case 2: return "Use your thunder to overcome your foes";
                    case 3: return "Envoke the power of The Mighty Thor";
                    default: return "Unknown move.";
                }
            default:
                return "Unknown move.";
        }
    }

    // Potions will increase either your health or mana for all players and decrease the amount of that item you have in your inventory by 1
    public void HealthPotion()
    {
        int healthToAdd = 100;
        if (Inventory.Instance.HealthPotions > 0)
        {
            Inventory.Instance.HealthPotions--;
            ss.AddHealth(dh.SelectedTarget(), healthToAdd);
            NumHealthP.text = "x " + Inventory.Instance.HealthPotions;
        }
    }

    public void ManaPotion()
    {
        int manaToAdd = 100;
        if (Inventory.Instance.ManaPotions > 0)
        {
            Inventory.Instance.ManaPotions--;
            ss.AddMana(dh.SelectedTarget(), manaToAdd);
            NumManaP.text = "x " + Inventory.Instance.ManaPotions;
        }
    }

    public float ElementalEffect(string playerMoveType, int enemyIndex)
{
    string enemyType = ss.returnEnemyType(enemyIndex); // Assuming a method to get enemy's type
    float effectivenessMultiplier = 1.0f;

    // Determine effectiveness
    if (playerMoveType == Stats.FireType && enemyType == Stats.IceType)
    {
        effectivenessMultiplier = 1.5f; // Fire > Ice
    }
    else if (playerMoveType == Stats.IceType && enemyType == Stats.ThunderType)
    {
        effectivenessMultiplier = 1.5f; // Ice > Thunder
    }
    else if (playerMoveType == Stats.ThunderType && enemyType == Stats.WindType)
    {
        effectivenessMultiplier = 1.5f; // Thunder > Wind
    }
    else if (playerMoveType == Stats.WindType && enemyType == Stats.FireType)
    {
        effectivenessMultiplier = 1.5f; // Wind > Fire
    }
    else if (playerMoveType == Stats.FireType && enemyType == Stats.WindType)
    {
        effectivenessMultiplier = 0.5f; // Wind < Ice
    }
    else if (playerMoveType == Stats.IceType && enemyType == Stats.FireType)
    {
        effectivenessMultiplier = 0.5f; // Fire < Wind
    }
    else if (playerMoveType == Stats.ThunderType && enemyType == Stats.IceType)
    {
        effectivenessMultiplier = 0.5f; // Ice < Fire
    }
    else if (playerMoveType == Stats.WindType && enemyType == Stats.ThunderType)
    {
        effectivenessMultiplier = 0.5f; // Thunder < Ice
    }
    else if (playerMoveType == Stats.SolarType){
         effectivenessMultiplier = 3.0f; // Solar effective agaisnt everything
    }
    

    Debug.Log($"Elemental effectiveness for {playerMoveType} against {enemyType}: {effectivenessMultiplier}x");
    // Apply the multiplier when calculating damage in the ExecuteHeroMove coroutine
    return effectivenessMultiplier;
}



    // Hero class to hold hero data
    // Hero class to hold hero data
    [System.Serializable]
    public class Hero
    {
        public string Name;
        public MoveInfo[] Moves;

        public Hero(string name, MoveInfo[] moves)
        {
            Name = name;
            Moves = moves;
        }
    }

    // MoveInfo class to hold information about each move
    [System.Serializable]
    public class MoveInfo
    {
        public int Damage;
        public int ManaCost;
        public string Type; // You can change this to an enum if you prefer
        public bool IsMultiTarget;
        public string Name; // Field for the move name
        public string Description; // New field for the move description

        public MoveInfo(int damage, int manaCost, string type, bool isMultiTarget, string name, string description)
        {
            Damage = damage;
            ManaCost = manaCost;
            Type = type;
            IsMultiTarget = isMultiTarget;
            Name = name;
            Description = description;
        }
    }
}