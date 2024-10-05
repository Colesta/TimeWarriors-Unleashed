using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public int target = 1; //default value

    private void Start()
    {
        // Ensure the dropdown component is assigned in the Inspector.
        if (dropdown != null)
        {
            // Add an event listener to detect changes in the Dropdown's value.
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
    }

    // Method to get the value selected from a drop-down menu
    private void OnDropdownValueChanged(int value)
    {
        // Get the selected option from the Dropdown's options list.
        string selectedOption = dropdown.options[value].text;

        // Try parsing the selected option as an integer
        if (int.TryParse(selectedOption, out int parsedTarget))
        {
            target = parsedTarget; // If parsing successful, set target
        }
        else
        {
            Debug.LogError("Selected option is not a valid number: " + selectedOption);
            
            target = value; // Defaulting to using the dropdown index if parsing fails
        }
    }

    // Method to return the currently selected target
    public int SelectedTarget()
    {
        return target; // Return the target value (either parsed from text or dropdown index)
    }
}
