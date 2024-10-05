using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using UnityEngine.EventSystems; 

public class Shop : MonoBehaviour, IPointerEnterHandler
{
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI HealthPotionsText;
    public TextMeshProUGUI ManaPotionsText;
    public TextMeshProUGUI DialougeText;
    public GameObject HealthPotion;
    public GameObject ManaPotion;
    public GameObject UltimatePotion;

    public Button ReturnButton;
    public GameObject BuyChoice;

    public GameObject HeroSelection;

    public Button Hero1;
    public Button Hero2;
    public Button Hero3;
    public Button Hero4;
    private int buttonPressedValue;

    public string currentItem = "null";

    public int PriceHealth = 50;
    public int PriceMana = 75;
    public int PriceUltimate = 200;

    public string DefaultDialouge = "Hello there Traveller, what would you like?";

    

    void Start()
    {
        // Initializes screen to show the next level you will fight, and takes Variables from permanent storage that relate to the current user and also puts that on screen to be used
        DialougeText.text = DefaultDialouge;

        Score.Instance.UpdateCurrentScore();
        UpdateInventory();

        Hero1.onClick.AddListener(() => OnButtonPressed(1));
        Hero2.onClick.AddListener(() => OnButtonPressed(2));
        Hero3.onClick.AddListener(() => OnButtonPressed(3));
        Hero4.onClick.AddListener(() => OnButtonPressed(4));
    }

    // Method to handle potion hover
    public void OnPotionHover(GameObject potion)
    {
        BuyChoice.SetActive(true);
        ReturnButton.gameObject.SetActive(false);

        // Handle if Health potion Selected
        if (potion == HealthPotion)
        {
            DialougeText.text = "That is a Health Potion, they restore 100 HP and cost: " + PriceHealth + " Gold. Would you like it?";
            currentItem = "Health";
        }
        // Handle if Mana potion Selected
        else if (potion == ManaPotion)
        {
            DialougeText.text = "That is a Mana Potion, they restore 100 MP and cost: " + PriceMana + " Gold. Would you like it?";
            currentItem = "Mana";
        }
        // Handle if Ultimate potion Selected
        else if (potion == UltimatePotion)
        {
            DialougeText.text = "That is your Team's Ultimate Moves. They cost " + PriceUltimate + " each. Would you like it?";
            currentItem = "Ultimate";
        }
    }


   

    // IPointerEnterHandler implementation
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Ensure to get the correct trigger object (the collider)
        GameObject triggerObject = eventData.pointerEnter;

        if (triggerObject == HealthPotion || triggerObject == ManaPotion || triggerObject == UltimatePotion)
        {
            OnPotionHover(triggerObject); // Call hover method for the relevant potion
        }
    }

    
   
    //If player doesnt want to buy an item
    public void onNoButtonClick()
    {
        DialougeText.text = "Alright then, what else would you like?";
        BuyChoice.SetActive(false);
        ReturnButton.gameObject.SetActive(true);
    }

//If player does want to buy an item
    public void onYesButtonClick()
    {
       BuyChoice.SetActive(false);
    //Check if player can buy the Item Selected
        switch (currentItem)
        {
            case "Health":
                if (CanAffordItem(PriceHealth))
                {
                    BuyHealthPotions();
                    DialougeText.text = "Thank you for your Purchase!";
                    ReturnButton.gameObject.SetActive(true);
                }
                else
                {
                    DialougeText.text = "You do not have enough money for that...";
                    ReturnButton.gameObject.SetActive(true);
                }
                break;
            case "Mana":
                if (CanAffordItem(PriceMana))
                {
                    BuyManaPotions();
                    DialougeText.text = "Thank you for your Purchase!";
                    ReturnButton.gameObject.SetActive(true);
                }
                else
                {
                    DialougeText.text = "You do not have enough money for that...";
                    ReturnButton.gameObject.SetActive(true);
                }
                break;
            case "Ultimate":
                if (CanAffordItem(PriceUltimate))
                {
                    ChooseUltimate();
                }
                else
                {
                    DialougeText.text = "You do not have enough money for that...";
                    ReturnButton.gameObject.SetActive(true);
                }
                break;
        }
    }

    // Add Health potions to inventory and decrease your total money by cost of item
    public void BuyHealthPotions()
    {
        Score.Instance.Money -= PriceHealth; 
        Score.Instance.UpdateCurrentScore();
        Inventory.Instance.HealthPotions += 1;
        UpdateInventory();
    }

// Add Mana potions to inventory and decrease your total money by cost of item
    public void BuyManaPotions()
    {
        Score.Instance.Money -= PriceMana; 
        Score.Instance.UpdateCurrentScore();
        Inventory.Instance.ManaPotions += 1;
        UpdateInventory();
    }

//Choose which hero's Ultimate you will buy, and check fi you have it already.
   public void ChooseUltimate()
{
    DialougeText.text = "Who's Ultimate move do you want to buy?";
    ActivateHeroButtons();

    switch (buttonPressedValue)
    {
        case 1:
            if (!checkIfUltimateBought(1)) {
                Inventory.Instance.Ultimate1 = true;
                BuyUltimate();
            } else {
                itemAlreadyBought();
            }
            break;
        case 2:
            if (!checkIfUltimateBought(2)) {
                Inventory.Instance.Ultimate2 = true;
                BuyUltimate();
            } else {
                itemAlreadyBought();
            }
            break;
        case 3:
            if (!checkIfUltimateBought(3)) {
                Inventory.Instance.Ultimate3 = true;
                BuyUltimate();
            } else {
                itemAlreadyBought();
            }
            break;
        case 4:
            if (!checkIfUltimateBought(4)) {
                Inventory.Instance.Ultimate4 = true;
                BuyUltimate();
            } else {
                itemAlreadyBought();
            }
            break;
    }
}

//Turn the Ultimate for the corresponding hero to True in the Inventory Class
    public void BuyUltimate()
    {
        Score.Instance.Money -= PriceUltimate;
        Score.Instance.UpdateCurrentScore();
        UpdateInventory();
        DeactivateHeroButtons();
        DialougeText.text = "Thank you for your Purchase!";
        ReturnButton.gameObject.SetActive(true);
    }

    public bool CanAffordItem(int itemPrice)
    {
        return itemPrice <= Score.Instance.Money; // Check if you can afford the item
    }

//Methods to manage the hero buttons 
    public void ActivateHeroButtons()
    {
        HeroSelection.SetActive(true);
        
    }

    public void DeactivateHeroButtons()
    {
        HeroSelection.SetActive(false);
    }

//Set the Inventory Ui on the Shop screen to match your current Inventory
    public void UpdateInventory()
    {
        MoneyText.text = "Gold: " + Score.Instance.Money;
        HealthPotionsText.text = "Health Potions: " + Inventory.Instance.HealthPotions;
        ManaPotionsText.text = "Mana Potions: " + Inventory.Instance.ManaPotions;
    }

    private int OnButtonPressed(int buttonValue)
    {
        buttonPressedValue = buttonValue; // Store the button value
        return buttonPressedValue; // Return the pressed button value
    }

//check if Ultimate is bought already
    private bool checkIfUltimateBought(int hero)
{
    switch (hero)
    {
        case 1:
            return Inventory.Instance.Ultimate1;
        case 2:
            return Inventory.Instance.Ultimate2;
        case 3:
            return Inventory.Instance.Ultimate3;
        case 4:
            return Inventory.Instance.Ultimate4;
        default:
            return false;
    }
}

//If Ultimate is already bought
    public void itemAlreadyBought(){
         DialougeText.text = "Seems you already bought that Item";
         DeactivateHeroButtons();
         ReturnButton.gameObject.SetActive(true);
    }
}
