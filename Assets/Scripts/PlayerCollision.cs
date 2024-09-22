using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerCollision : MonoBehaviour
{
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

    void Start()
    {
        // Initialize the dictionary
        triggerPortalMap = new Dictionary<GameObject, GameObject>
        {
            { trigOne, portalOne },
            { trigTwo, portalTwo },
            { trigThree, portalThree },
            { shopTrig, ShopKeeper },
            // Add more portals if necessary
        };
    }

    void Update()
    {
        if (collisionOccurred && Input.GetKeyDown(KeyCode.E))
        {
            // Log the name of the portal being interacted with
            if (lastPortal != null)
            {
                Debug.Log("Interacting with portal: " + lastPortal.name);
                sc.gotoBattle();
            }

            // Example: Transition to another scene (battle/shop)
            if (lastPortal == ShopKeeper)
            {
                sc.gotoShop(); // Or gotoShop(), depending on your logic
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
                kvp.Key.SetActive(true); // Activate the corresponding trigger
                collisionOccurred = true;
                lastPortal = kvp.Value;  // Store the last portal that caused the collision
                Debug.Log("Collided with: " + kvp.Value.name);
                return;
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
                Debug.Log("Exited collision with: " + kvp.Value.name);
            }
        }
    }
}
