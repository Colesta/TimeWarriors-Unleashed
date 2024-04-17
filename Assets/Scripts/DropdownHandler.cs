using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public int target;


    

    private void Start()
    {


        // Ensure the dropdown component is assigned in the Inspector.
        if (dropdown != null)
        {
            // Add an event listener to detect changes in the Dropdown's value.
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
    }

    //Method to get the value selected from a drop down menu
    private void OnDropdownValueChanged(int value)
    {
        // Get the selected option from the Dropdown's options list.
        string selectedOption = dropdown.options[value].text;

       
        // Use the selected option as needed.
        target = int.Parse(selectedOption); 
            
       
    }
}
