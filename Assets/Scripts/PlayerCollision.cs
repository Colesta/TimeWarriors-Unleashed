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

    public SceneController sc;

    // Dictionary to map trigger objects to their corresponding portal objects
    private Dictionary<GameObject, GameObject> triggerPortalMap;

    // Boolean to track if the collision happened
    private bool collisionOccurred = false;

    // Store the last portal that caused the collision
    private GameObject lastPortal;

    private LevelSystem ls;

    void Awake()
    {
        ls = GetComponent<LevelSystem>();
    }

    void Start()
    {
        
        // Initialize the dictionary
        triggerPortalMap = new Dictionary<GameObject, GameObject>
        {
            { trigOne, portalOne },
            { trigTwo, portalTwo },
            { trigThree, portalThree },
            { shopTrig, ShopKeeper },
        };
    }

    void Update()
    {
        if (collisionOccurred && Input.GetKeyDown(KeyCode.E))
        {
            if (lastPortal != null)
            {
                Debug.Log("Interacting with portal: " + lastPortal.name);
                
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
                
                Debug.Log("Collided with: " + kvp.Value.name);
                Debug.Log("collisionOccurred set to: " + collisionOccurred);

                // Activate the corresponding trigger after setting the flag
                kvp.Key.SetActive(true);

                // Update the dialogue text immediately upon collision
                if (lastPortal == ShopKeeper)
                {
                    DialougeText.text = "It's the Shopkeeper, I should visit sometime between battles.";
                }
                else
                {
                    DialougeText.text = "You've reached " + lastPortal.name;
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
                Debug.Log("Exited collision with: " + kvp.Value.name);
                Debug.Log("collisionOccurred set to: " + collisionOccurred);
            }
        }
    }

    // Method to check if the player can access a level
    private bool canAccessLevel(string level)
    {
        switch (lastPortal.name)
        {
            case "Level 1":
                return ls.getCurrentLevel() >= 1; // Return true if level access is allowed
            case "Level 2":
                return ls.getCurrentLevel() >= 2;
            case "Level 3":
                return ls.getCurrentLevel() >= 3;
            default:
                Debug.LogWarning("Unknown level: " + level); // Log if level doesn't match
                return false; // Return false for unrecognized levels
        }
    }
}
