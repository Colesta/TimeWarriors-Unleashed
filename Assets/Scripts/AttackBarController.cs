using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttackBarController : MonoBehaviour
{
    public Slider attackBarSlider;
    public GameObject attackBar; // Attack bar UI element
    public GameObject battleOptionMenu; // Menu UI element
    public GameObject battleMenu;

    private bool movingRight = true;

    private HeroManager hm;

    private bool isAttackBarActive = false; // Flag to track if attack bar is active

    void Awake()
    {
        hm = GetComponent<HeroManager>();
        
        // Check if HeroManager is properly assigned
        if (hm == null)
        {
            Debug.LogError("HeroManager component is missing from the GameObject.");
        }
    }

    void Start()
    {
        
        // Check if the Slider is assigned
        if (attackBarSlider != null)
        {
            attackBarSlider.value = 0.5f; // Start the line in the middle
            
        }
        else
        {
            Debug.LogError("AttackBarSlider is not assigned in the Inspector.");
        }

        // Check if attackBar and battleOptionMenu are assigned
        if (attackBar == null)
        {
            Debug.LogError("AttackBar GameObject is not assigned in the Inspector.");
        }
        if (battleOptionMenu == null)
        {
            Debug.LogError("BattleOptionMenu GameObject is not assigned in the Inspector.");
        }

   
    }

    void Update()
    {
        if (isAttackBarActive)
        {
            MoveLine();

            if (Input.GetKeyDown(KeyCode.E)) // Player confirms attack with key press
            {
                float attackPower = CheckPlayerInput();
                

                // Store calculated attack power in the Hero Manager
                if (hm != null)
                {
                    hm.StoreAttackPower(attackPower);
                }
                else
                {
                    Debug.LogError("HeroManager reference is missing.");
                }

                StopAttackBar(); // Stop the attack bar after confirming the attack
            }
        }
    }

    // Method to start the attack bar when an attack begins
    public IEnumerator StartAttackBar()
    {
        
        isAttackBarActive = true;
        attackBar.SetActive(true);

        

        // Hide the battle menu if available
        if (battleOptionMenu != null)
        {
            battleOptionMenu.SetActive(false);
           
        }
        else
        {
            Debug.LogError("BattleOptionMenu GameObject is not assigned.");
        }

        while (isAttackBarActive)
        {
            MoveLine(); // Update the UI

            if (Input.GetKeyDown(KeyCode.E))
            {
                float attackPower = CheckPlayerInput();
               

                if (hm != null)
                {
                    hm.StoreAttackPower(attackPower);
                }
                else
                {
                    Debug.LogError("HeroManager reference is missing.");
                }

                StopAttackBar();
            }

            yield return null; // Wait for the next frame
        }
    }

    // Method to stop the attack bar and reset its state
    public void StopAttackBar()
    {
        isAttackBarActive = false;
        if (attackBar != null)
        {
            attackBar.SetActive(false); // Hide attack bar
           
        }

        if (battleMenu != null)
        {
            battleMenu.SetActive(true); // Show battle menu 
          
        }


        

    }

    // Control the Movement of the line on the Attack Bar
    void MoveLine()
    {
        if (attackBarSlider != null)
        {
            if (movingRight)
            {
                attackBarSlider.value += Time.deltaTime * getLineSpeed(Difficulty.Instance.GetCurrentDifficulty());
                if (attackBarSlider.value >= 1.0f)
                {
                    movingRight = false;
                }
            }
            else
            {
                attackBarSlider.value -= Time.deltaTime * getLineSpeed(Difficulty.Instance.GetCurrentDifficulty());
                if (attackBarSlider.value <= 0.0f)
                {
                    movingRight = true;
                }
            }
        }
        else
        {
            Debug.LogError("AttackBarSlider is not assigned.");
        }
    }

    public float CheckPlayerInput()
    {
        if (attackBarSlider == null)
        {
            return 0.0f;
        }

        float hitPosition = attackBarSlider.value;

        if (hitPosition >= 0.0f && hitPosition <= 0.15f || hitPosition >= 0.85f && hitPosition <= 1.0f)
        {
            return 0.5f; // Weak attack
        }
        else if (hitPosition > 0.15f && hitPosition <= 0.33f || hitPosition >= 0.67f && hitPosition < 0.85f)
        {
            return 1.0f; // Standard attack
        }
        else if (hitPosition > 0.33f && hitPosition <= 0.45f || hitPosition >= 0.55f && hitPosition < 0.67f)
        {
            return 1.5f; // Strong attack
        }
        else if (hitPosition > 0.45f && hitPosition < 0.55f)
        {
            return 2.0f; // Critical attack
        }
        else
        {
            return 0.0f; // Missed attack
        }
    }
    
    // Determine speed of the line depending on the Difficulty
    public float getLineSpeed(string difficulty){
        switch(difficulty){
            case "Easy":
                return 0.5F;
            case "Medium":
                return 1.0F;
            case "Hard":
                return 1.5F;
            default:
                return 0.0F;

        }
    }
}
