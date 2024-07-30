using UnityEngine;
using UnityEngine.UI;

public class AttackBarController : MonoBehaviour
{
    public Slider attackBarSlider; // Reference to the slider UI element
    public float moveSpeed = 2.0f; // Speed of the moving line
    private bool movingRight = true; // Direction of the moving line

    void Start()
    {
        if (attackBarSlider != null)
        {
            attackBarSlider.value = 0.5f; // Start the line in the middle
        }
    }

    void Update()
    {
        MoveLine();
        CheckPlayerInput();
    }

    void MoveLine()
    {
        if (attackBarSlider != null)
        {
            if (movingRight)
            {
                attackBarSlider.value += Time.deltaTime * moveSpeed;
                if (attackBarSlider.value >= 1.0f)
                {
                    movingRight = false;
                }
            }
            else
            {
                attackBarSlider.value -= Time.deltaTime * moveSpeed;
                if (attackBarSlider.value <= 0.0f)
                {
                    movingRight = true;
                }
            }
        }
    }

    void CheckPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Check for player input (space bar)
        {
            float hitPosition = attackBarSlider.value;
            Debug.Log("Attack at position: " + hitPosition);

            // Implement your attack logic here, using the hitPosition value
            // For example, you could determine the damage based on how close the hitPosition is to the center (0.5)
        }
    }
}
