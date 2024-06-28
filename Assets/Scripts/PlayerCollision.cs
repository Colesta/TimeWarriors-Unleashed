using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    // Reference to the triangle GameObject
    public GameObject triangle;
    // Reference to the square GameObject
    public GameObject square;

    // Boolean to track if the collision happened
    private bool collisionOccurred = false;

    void Update()
    {
        if (collisionOccurred && Input.GetKeyDown(KeyCode.E))
        {
            // Make sure to replace SwitchGameScene.LoadScene with SceneManager.LoadScene
            // since LoadScene should be a static method or belong to a specific instance.
            SceneManager.LoadScene("Battle");
        }
    }

    // Method called when another collider enters the trigger collider attached to the object where this script is attached (circle)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object collided with is the square
        if (collision.gameObject == square)
        {
            // Make the triangle visible
            triangle.SetActive(true);
            // Set the collisionOccurred flag to true
            collisionOccurred = true;
        }
    }
}


