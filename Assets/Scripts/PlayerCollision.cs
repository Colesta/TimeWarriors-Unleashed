using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class PlayerCollision : MonoBehaviour
{
    public TextMeshProUGUI DialougeText;
    public GameObject trigOne;
    public GameObject portalOne;

    public GameObject trigTwo;
    public GameObject portalTwo;

    public GameObject trigThree;
    public GameObject portalThree;

    public GameObject ShopKeeper;
    public GameObject shopTrig;

    public GameObject StoryTeller;
    public GameObject storyTrig;

    public GameObject Overworld;
    public GameObject Story;

    public SceneController sc;

    // Dictionary to map trigger objects to their corresponding portal objects
    private Dictionary<GameObject, GameObject> triggerPortalMap;

    // Boolean to track if the collision happened
    private bool collisionOccurred = false;

    // Store the last portal that caused the collision
    private GameObject lastPortal;

    void Start()
    {
        // Initialize the dictionary
        triggerPortalMap = new Dictionary<GameObject, GameObject>
        {
            { trigOne, portalOne },
            { trigTwo, portalTwo },
            { trigThree, portalThree },
            { shopTrig, ShopKeeper },
            { storyTrig, StoryTeller },
        };
    }

    void Update()
    {
        if (collisionOccurred && Input.GetKeyDown(KeyCode.E))
        {
            if (lastPortal != null)
            {
      

                if (canAccessLevel(lastPortal.name))
                {
                    sc.gotoBattle();
                }
                else
                {
                    DialougeText.text = "I'm not strong enough to fight here yet.";
                }
            }

            if (lastPortal == ShopKeeper)
            {
                sc.gotoShop(); // Transition to the shop
            }
            else if (lastPortal == StoryTeller)  
            {
                Overworld.SetActive(false);
                Story.SetActive(true);
            }
           
        }
    }

    // Method called when another collider enters the trigger collider attached to the object where this script is attached (circle)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object collided with is in the triggerPortalMap
        foreach (var kvp in triggerPortalMap)
        {
            if (collision.gameObject == kvp.Value)
            {
                // First check if the collision occurred
                collisionOccurred = true;
                lastPortal = kvp.Value;  // Store the last portal that caused the collision

             

                // Activate the corresponding trigger after setting the flag
                kvp.Key.SetActive(true);

                // Update the dialogue text immediately upon collision
                if (lastPortal == ShopKeeper)
                {
                    DialougeText.text = "It's the Shopkeeper, I should visit sometime between battles.";
                }
                else if (lastPortal == StoryTeller)  
                {
                    DialougeText.text = "What's going on here?";
                }
                else
                {
                    DialougeText.text = "This is " + lastPortal.name;
                }

                return; // Exit after processing the first match
            }
        }
    }

    // Method called when another collider exits the trigger collider attached to the object where this script is attached (circle)
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the object exited collision is in the triggerPortalMap
        foreach (var kvp in triggerPortalMap)
        {
            if (collision.gameObject == kvp.Value)
            {
                kvp.Key.SetActive(false);  // Deactivate the trigger
                collisionOccurred = false;
                lastPortal = null;  // Clear the last portal reference
                DialougeText.text = ""; 
               
            }
        }
    }

    // Method to check if the player can access a level
    private bool canAccessLevel(string level)
    {
    // Get the player's current level from the LevelSystem
    int currentLevel = LevelSystem.Instance.getCurrentLevel();

    // Check if the player can access the desired level based on their current level
    switch (lastPortal.name)
    {
        case "Level 1":
            return currentLevel >= 1; // Can access Level 1 if current level is 1 or higher
        case "Level 2":
            return currentLevel >= 2; // Can access Level 2 if current level is 2 or higher
        case "Level 3":
            return currentLevel >= 3; // Can access Level 3 if current level is 3 or higher
        default:
            return false; // If no valid level is found, deny access
    }
    }
}
