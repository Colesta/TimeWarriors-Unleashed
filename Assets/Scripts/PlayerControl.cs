using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float movSpeed;
    float speedX, speedY;
    Rigidbody2D rb;

    // Track the starting position
    private Vector2 startPosition;
    // Distance traveled
    private float totalDistance;
    // Reference to Score script
    private Score sc;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position; // Store the starting position
        sc = FindObjectOfType<Score>(); // Find the Score component in the scene
        totalDistance = 0f; // Initialize distance
    }

    // Update is called once per frame
    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal") * movSpeed;
        speedY = Input.GetAxisRaw("Vertical") * movSpeed;
        rb.velocity = new Vector2(speedX, speedY);

        // Calculate distance traveled
        float distanceTravelled = Vector2.Distance(startPosition, transform.position);
        
        // Check if distance has changed
        if (distanceTravelled > totalDistance)
        {
            totalDistance = distanceTravelled; // Update total distance
            UpdateScore(totalDistance); // Update the score
        }
    }

    // Method to update the score
    private void UpdateScore(float distance)
    {
        if (sc != null)
        {
            sc.distanceRan += distance; // Update the distance in Score script
            sc.UpdateCurrentScore(); // Call the method to save/update the score
        }
    }
}
