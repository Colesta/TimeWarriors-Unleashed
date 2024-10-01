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

    public DropdownHandler dh;

    private int CurrentPlayer = 1;
    private int currentMove = 1;
    private float storedAttackPower; // Variable to hold the stored attack power

    public TextMeshProUGUI NumHealthP;
    public TextMeshProUGUI NumManaP;

    private SetStats ss;
    private Score sc; 
    private AttackBarController ab;

    void Awake()
    {
        ss = GetComponent<SetStats>();
        sc = GetComponent<Score>();
        ab = GetComponent<AttackBarController>();
    }

    void Start()
    {
        sc.UpdateCurrentScore();
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
    }

    public int returnCurrentPlayer(){
        return this.CurrentPlayer;
    }

    public int returnCurrentMove(){
        return this.currentMove;
    }

    public void StoreAttackPower(float power)
{
    storedAttackPower = power; // Store the attack power as a float
    Debug.Log("Stored Attack Power: " + storedAttackPower);
}


    // Coroutine for handling the attack bar input and applying damage
    private IEnumerator ExecuteHeroMove(int heroIndex, int moveDamage, int manaCost, bool isMultiTarget = false){
   

   Debug.Log("Executing hero move for hero index: " + heroIndex);

    // Attack bar logic
    Debug.Log("Starting attack bar logic...");
    yield return StartCoroutine(ab.StartAttackBar());
    Debug.Log("Attack bar logic completed.");
    
    // Additional debug for mana cost application
    Debug.Log("Removing mana for hero index: " + heroIndex);
    ss.RemoveMana(heroIndex, manaCost);

    // Calculate damage
    int totalDamage = (int)(moveDamage * (float)storedAttackPower);
    Debug.Log("Total damage calculated: " + totalDamage);

    if (isMultiTarget)
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
        int target = dh.SelectedTarget();
        ss.DamageEnemy(target, totalDamage); // Make sure totalDamage is an int
    }

   // Update stats and change enemy appearance if dead
ss.UpdateStats();
ChangeEnemyOnDeath();

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
    switch(CurrentPlayer) 
    {
        case 1:
            Hero1Move1();
            break;
        case 2:
            Hero2Move1();
            break;
        case 3:
            Hero3Move1();
            break;
        case 4:
            Hero4Move1();
            break;
        default:
            Debug.LogError("Invalid player number: " + CurrentPlayer);
            break; // Optional: Handle invalid player case
    }
}


public void Move2(){
    switch(CurrentPlayer){
       case 1:
                Hero1Move2();
                break;
        case 2:
                Hero2Move2();
                break;
        case 3:
                Hero3Move2();
                break;
        case 4:
                Hero4Move2();
                break;
    }
}

public void Ultimate(){
    switch(CurrentPlayer){
        case 1:
                Hero1Ultimate();
                break;
        case 2:
                Hero2Ultimate();
                break;
        case 3:
                Hero3Ultimate();
                break;
        case 4:
                Hero4Ultimate();
                break;
    }
}


    // Move 1 for Hero 1 (Single-target attack)
   public void Hero1Move1() 
{
    Debug.LogError("Hero 1 Move 1 method is being accessed");
    StartCoroutine(ExecuteHeroMove(1, 75, 15));
}


    // Move 2 for Hero 1 (Multi-target attack)
    private void Hero1Move2()
    {
        StartCoroutine(ExecuteHeroMove(1, 50, 30, isMultiTarget: true));
    }

     private void Hero1Ultimate()
    {
        StartCoroutine(ExecuteHeroMove(1, 50, 30, isMultiTarget: true));
    }

    // Move 1 for Hero 2 (Single-target attack)
    private void Hero2Move1()
    {
        StartCoroutine(ExecuteHeroMove(2, 75, 15));
    }

    // Move 2 for Hero 2 (Multi-target attack)
    private void Hero2Move2()
    {
        StartCoroutine(ExecuteHeroMove(2, 50, 20, isMultiTarget: true));
    }

      private void Hero2Ultimate()
    {
        StartCoroutine(ExecuteHeroMove(1, 50, 30, isMultiTarget: true));
    }

    // Move 1 for Hero 3 (Single-target attack)
    private void Hero3Move1()
    {
        StartCoroutine(ExecuteHeroMove(3, 75, 15));
    }

    // Move 2 for Hero 3 (Multi-target attack)
    private void Hero3Move2()
    {
        StartCoroutine(ExecuteHeroMove(3, 50, 30, isMultiTarget: true));
    }

      private void Hero3Ultimate()
    {
        StartCoroutine(ExecuteHeroMove(1, 50, 30, isMultiTarget: true));
    }

    // Move 1 for Hero 4 (Single-target attack)
    private void Hero4Move1()
    {
        StartCoroutine(ExecuteHeroMove(4, 75, 15));
    }

    // Move 2 for Hero 4 (Multi-target attack)
    private void Hero4Move2()
    {
        StartCoroutine(ExecuteHeroMove(4, 50, 30, isMultiTarget: true));
    }

      private void Hero4Ultimate()
    {
        StartCoroutine(ExecuteHeroMove(1, 50, 30, isMultiTarget: true));
    }

    // Potions will increase either your health or mana for all players and decrease the amount of that item you have in your inventory by 1
    public void HealthPotion()
    {
        int health = 50;
        ss.RestoreHealth(CurrentPlayer, health); // Restore health using SetStats
        sc.UpdateCurrentScore();
        Items.SetActive(false);
        Buttons.SetActive(true);
        NextPlayersTurn();
    }

    public void ManaPotion()
    {
        int mana = 50;
        ss.RestoreMana(CurrentPlayer, mana); // Restore mana using SetStats
        sc.UpdateCurrentScore();
        Items.SetActive(false);
        Buttons.SetActive(true);
        NextPlayersTurn();
    }

    // Turn the enemies into little skulls if they die
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
