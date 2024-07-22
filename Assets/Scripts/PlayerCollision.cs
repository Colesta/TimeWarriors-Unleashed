using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public GameObject trigShop;
    public GameObject Shopkeeper;

    public GameObject trigOne;
    public GameObject portalOne;

    public SceneController sc;

    // Boolean to track if the collision happened
    private bool collisionOccurred = false;

    void Update()
    {
        if (collisionOccurred && Input.GetKeyDown(KeyCode.E))
        {
            // Make sure to replace SwitchGameScene.LoadScene with SceneManager.LoadScene
            // since LoadScene should be a static method or belong to a specific instance.
           sc.gotoBattle();
        }
    }

    // Method called when another collider enters the trigger collider attached to the object where this script is attached (circle)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object collided with is the square
        if (collision.gameObject == portal)
        {
            trig.SetActive(true);
            collisionOccurred = true;
        }
        else{
            trig.SetActive(false);
            collisionOccurred = true;
        }
    }


}


