using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    public float movSpeed;
    float speedX, speedY;
    Rigidbody2D rb;
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI HealthPotionsText;
    public TextMeshProUGUI ManaPotionsText;



    // Track the starting position
    private Vector2 startPosition;
    // Last known position
    private Vector2 lastPosition;
    // Distance traveled
    private float totalDistance;
    // Reference to Score script
    private Score sc;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position; // Store the starting position
        lastPosition = startPosition; // Initialize last position
        sc = GetComponent<Score>() ?? FindObjectOfType<Score>(); // Find the Score component in the scene
        totalDistance = 0f; // Initialize distance

        MoneyText.text = "Gold: " + Score.Instance.Money;
        HealthPotionsText.text = "Health Potions: " + Inventory.Instance.HealthPotions;
        ManaPotionsText.text = "Mana Potions: " + Inventory.Instance.ManaPotions;



       
    }

    // Update is called once per frame
    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal") * movSpeed;
        speedY = Input.GetAxisRaw("Vertical") * movSpeed;
        rb.velocity = new Vector2(speedX, speedY);

        // Calculate distance traveled
        float distanceTravelled = Vector2.Distance(lastPosition, transform.position);
        
        // Update score only if there is a distance change
        if (distanceTravelled > 0)
        {
            totalDistance += distanceTravelled; // Update total distance
            UpdateScore(totalDistance); // Update the score
             // Clear dialogue text if player moves
            
        }

        lastPosition = transform.position; // Update last position
    }

    // Method to update the score
    private void UpdateScore(float distance)
    {
      
            Score.Instance.CurrentDistance += distance; // Update the distance in Score script 
            Score.Instance.UpdateCurrentScore(); // Call the method to save/update the score
            
        
    }
}

