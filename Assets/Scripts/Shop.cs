using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Added this line for Button
using UnityEngine.EventSystems; // Added this line for IPointer interfaces

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

    private Stats s;
    private Score sc;

    void Awake()
    {
        s = GetComponent<Stats>(); // Get the Stats component attached to the same GameObject
        sc = GetComponent<Score>(); // Get the Score component attached to the same GameObject
    }

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
            DialougeText.text = "That is yours and your Team's Ultimate Moves. They cost " + PriceUltimate + " each. Would you like it?";
            currentItem = "Ultimate";
        }
    }

    // Method to handle exit from potion hover
   

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

    
   

    public void onNoButtonClick()
    {
        DialougeText.text = "Alright then, what else would you like?";
        BuyChoice.SetActive(false);
        ReturnButton.gameObject.SetActive(true);
    }

    public void onYesButtonClick()
    {
       BuyChoice.SetActive(false);

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

    // Add potions to inventory and decrease your total money by cost of item
    public void BuyHealthPotions()
    {
        Score.Instance.Money -= PriceHealth; // Make sure 'Money' is public property in Score
        Score.Instance.UpdateCurrentScore();
        s.HealthPotions += 1;
        UpdateInventory();
    }

    public void BuyManaPotions()
    {
        Score.Instance.Money -= PriceMana; // Make sure 'Money' is public property in Score
        Score.Instance.UpdateCurrentScore();
        s.ManaPotions += 1;
        UpdateInventory();
    }

    public void ChooseUltimate()
    {
        DialougeText.text = "Who's Ultimate move do you want to buy?";

        ActivateHeroButtons();

        switch (buttonPressedValue)
        {
            case 1:
                s.Ultimate1 = true;
                BuyUltimate();
                break;
            case 2:
                s.Ultimate2 = true;
                BuyUltimate();
                break;
            case 3:
                s.Ultimate3 = true;
                BuyUltimate();
                break;
            case 4:
                s.Ultimate4 = true;
                BuyUltimate();
                break;
        }
    }

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

    public void ActivateHeroButtons()
    {
        HeroSelection.SetActive(true);
        
    }

    public void DeactivateHeroButtons()
    {
        HeroSelection.SetActive(false);
    }

    public void UpdateInventory()
    {
        MoneyText.text = "Money: " + Score.Instance.Money;
        HealthPotionsText.text = "Health Potions: " + s.HealthPotions;
        ManaPotionsText.text = "Mana Potions: " + s.ManaPotions;
    }

    private int OnButtonPressed(int buttonValue)
    {
        buttonPressedValue = buttonValue; // Store the button value
        return buttonPressedValue; // Return the pressed button value
    }
}
