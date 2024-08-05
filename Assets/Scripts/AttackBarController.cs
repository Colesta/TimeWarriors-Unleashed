using UnityEngine;
using UnityEngine.UI;

public class AttackBarController : MonoBehaviour
{
    public Slider attackBarSlider; // Reference to the slider UI element
    public float moveSpeed = 1.0f; // Speed of the moving line
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
        if (Input.GetKeyDown(KeyCode.E)) // Check for player input
        {
            float attackPower = CheckPlayerInput();
            Debug.Log("Attack power: " + attackPower);
        }
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

    public float CheckPlayerInput()
    {
        float hitPosition = attackBarSlider.value;
        Debug.Log("Attack at position: " + hitPosition);

        if (hitPosition >= 0.0f && hitPosition <= 0.15f || hitPosition >= 0.85f && hitPosition <= 1.0f)
        {
            return 0.5f;
        }
        else if (hitPosition > 0.15f && hitPosition <= 0.33f || hitPosition >= 0.67f && hitPosition < 0.85f)
        {
            return 1.0f;
        }
        else if (hitPosition > 0.33f && hitPosition <= 0.45f || hitPosition >= 0.55f && hitPosition < 0.67f)
        {
            return 1.5f;
        }
        else if (hitPosition > 0.45f && hitPosition < 0.55f)
        {
            return 2.0f;
        }
        else
        {
            return 0.0f; // Default return value in case of unexpected input
        }
    }
}
