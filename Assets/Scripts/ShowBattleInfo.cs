using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ShowBattleInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI HealthText;
    public Slider HPSlider;
    public TextMeshProUGUI ManaText;
    public Slider ManaSlider;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI DialougeText;
    public GameObject Mana;
    public GameObject CharacterInfoDisplay;
    public GameObject MoveInfoDisplay;

    public GameObject Hero1;
    public GameObject Hero2;
    public GameObject Hero3;
    public GameObject Hero4;
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public GameObject Enemy4;

    public Button Move1;
    public Button Move2;
    public Button MoveUltimate;

    public TextMeshProUGUI MoveName;
    public TextMeshProUGUI MoveDescription;
    public TextMeshProUGUI Damage;
    public TextMeshProUGUI ManaCost;
    public TextMeshProUGUI Type;
    public TextMeshProUGUI Target;

    private GameManager ss;
    private HeroManager hm;

    void Awake()
    {
        ss = FindObjectOfType<GameManager>();
        hm = FindObjectOfType<HeroManager>();
        Debug.Log("ShowBattleInfo script initialized. GameManager and HeroManager found.");
    }

    public void OnPointerEnter(PointerEventData eventData)
{
    GameObject triggerObject = eventData.pointerEnter;
    Debug.Log("Pointer entered: " + (triggerObject != null ? triggerObject.name : "null"));

    if (triggerObject == null)
    {
        Debug.LogError("No trigger object detected!");
        return;
    }

    // Hide the info display initially
    MoveInfoDisplay.SetActive(false);
    CharacterInfoDisplay.SetActive(false);

    // List of valid objects to check against
    List<GameObject> validTriggerObjects = new List<GameObject> { Move1.gameObject, Move2.gameObject, MoveUltimate.gameObject, Hero1, Hero2, Hero3, Hero4, Enemy1, Enemy2, Enemy3, Enemy4 };

    // Check if the triggerObject is in the valid list
    if (validTriggerObjects.Contains(triggerObject))
    {
        if (triggerObject == Move1.gameObject)
        {
            Debug.Log("Move 1 selected.");
            MoveInfoDisplay.SetActive(true);
            UpdateUIWithMoveInfo(hm.returnCurrentPlayer(), 1); 
        }
        else if (triggerObject == Move2.gameObject)
        {
            Debug.Log("Move 2 selected.");
            MoveInfoDisplay.SetActive(true);
            UpdateUIWithMoveInfo(hm.returnCurrentPlayer(), 2); 
        }
        else if (triggerObject == MoveUltimate.gameObject)
        {
            Debug.Log("Ultimate move selected.");
            MoveInfoDisplay.SetActive(true);
            UpdateUIWithMoveInfo(hm.returnCurrentPlayer(), 3); 
        }
        else if (triggerObject == Hero1)
        {
            Debug.Log("Hero 1 hovered.");
            CharacterInfoDisplay.SetActive(true);
            UpdateUIWithHeroStats(1);
        }
        else if (triggerObject == Hero2)
        {
            Debug.Log("Hero 2 hovered.");
            CharacterInfoDisplay.SetActive(true);
            UpdateUIWithHeroStats(2);
        }
        else if (triggerObject == Hero3)
        {
            Debug.Log("Hero 3 hovered.");
            CharacterInfoDisplay.SetActive(true);
            UpdateUIWithHeroStats(3);
        }
        else if (triggerObject == Hero4)
        {
            Debug.Log("Hero 4 hovered.");
            CharacterInfoDisplay.SetActive(true);
            UpdateUIWithHeroStats(4);
        }
        else if (triggerObject == Enemy1)
        {
            Debug.Log("Enemy 1 hovered.");
            CharacterInfoDisplay.SetActive(true);
            UpdateUIWithEnemyStats(1);
        }
        else if (triggerObject == Enemy2)
        {
            Debug.Log("Enemy 2 hovered.");
            CharacterInfoDisplay.SetActive(true);
            UpdateUIWithEnemyStats(2);
        }
        else if (triggerObject == Enemy3)
        {
            Debug.Log("Enemy 3 hovered.");
            CharacterInfoDisplay.SetActive(true);
            UpdateUIWithEnemyStats(3);
        }
        else if (triggerObject == Enemy4)
        {
            Debug.Log("Enemy 4 hovered.");
            CharacterInfoDisplay.SetActive(true);
            UpdateUIWithEnemyStats(4);
        }
    }
    else
    {
        // Hide the info display if hovering over a non-interactive object
        MoveInfoDisplay.SetActive(false);
        CharacterInfoDisplay.SetActive(false);
        Debug.Log("Hovering over a non-interactive object.");
    }
}



    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exited.");
        DialougeText.gameObject.SetActive(false); // Hide dialogue text on exit
        CharacterInfoDisplay.SetActive(false);
        MoveInfoDisplay.SetActive(false); // Hide move info on exit
    }

    private void UpdateUIWithHeroStats(int heroIndex)
    {
        Debug.Log("Updating UI with hero stats for Hero " + heroIndex);

        if (HealthText == null || HPSlider == null || ManaText == null || ManaSlider == null || typeText == null || Mana == null)
        {
            Debug.LogError("One or more UI elements are not assigned!");
            return;
        }

        string[] heroNames = { "Asterius", "Yoshitsune", "Wangchuk", "Freya" };
        if (heroIndex >= 1 && heroIndex <= 4)
        {
            DialougeText.text = $"Hero {heroIndex}: {heroNames[heroIndex - 1]}";
        }
        DialougeText.gameObject.SetActive(true);

        Debug.Log("Hero " + heroIndex + " stats: HP = " + ss.returnCurrentHeroHP(heroIndex) + "/" + ss.returnMaxHeroHP(heroIndex) +
                  ", Mana = " + ss.returnCurrentMana(heroIndex) + "/" + ss.returnMaxMana(heroIndex));

        HealthText.text = ss.returnCurrentHeroHP(heroIndex) + "/" + ss.returnMaxHeroHP(heroIndex);
        HPSlider.maxValue = ss.returnMaxHeroHP(heroIndex);
        HPSlider.value = ss.returnCurrentHeroHP(heroIndex);

        ManaText.text = ss.returnCurrentMana(heroIndex) + "/" + ss.returnMaxMana(heroIndex);
        ManaSlider.maxValue = ss.returnMaxMana(heroIndex);
        ManaSlider.value = ss.returnCurrentMana(heroIndex);

        Mana.SetActive(true); // Ensure Mana UI is visible for heroes
        typeText.gameObject.SetActive(false);
    }

    private void UpdateUIWithEnemyStats(int enemyIndex)
    {
        Debug.Log("Updating UI with enemy stats for Enemy " + enemyIndex);

        if (HealthText == null || HPSlider == null || typeText == null || Mana == null)
        {
            Debug.LogError("One or more UI elements are not assigned!");
            return;
        }

        Debug.Log("Enemy " + enemyIndex + " stats: HP = " + ss.returnCurrentEnemyHP(enemyIndex) + "/" + ss.returnMaxEnemyHP(enemyIndex) +
                  ", Type = " + ss.returnEnemyType(enemyIndex));

        DialougeText.text = "Enemy " + enemyIndex + ": " + ss.GetEnemySpecies(enemyIndex);
        DialougeText.gameObject.SetActive(true);

        HealthText.text = ss.returnCurrentEnemyHP(enemyIndex) + "/" + ss.returnMaxEnemyHP(enemyIndex);
        HPSlider.maxValue = ss.returnMaxEnemyHP(enemyIndex);
        HPSlider.value = ss.returnCurrentEnemyHP(enemyIndex);

        typeText.gameObject.SetActive(true);
        typeText.text = "Element Type: Eclipsed " + ss.returnEnemyType(enemyIndex);

        Mana.SetActive(false); // Hide Mana UI for enemies
    }

    private void UpdateUIWithMoveInfo(int heroIndex, int moveIndex)
    {
        Debug.Log("Updating UI with move info for Hero " + heroIndex + ", Move " + moveIndex);

        if (Damage == null || ManaCost == null || Type == null || Target == null || MoveName == null || MoveDescription == null)
        {
            Debug.LogError("One or more UI elements for move info are not assigned!");
            return;
        }

        HeroManager.MoveInfo moveInfo = hm.GetMoveInfo(heroIndex, moveIndex);
        if (moveInfo == null)
        {
            Debug.LogError($"Move info not found for Hero {heroIndex}, Move {moveIndex}");
            return;
        }

        Debug.Log($"Move Info: Name = {moveInfo.Name}, Description = {moveInfo.Description}, Damage = {moveInfo.Damage}, Mana Cost = {moveInfo.ManaCost}");

        MoveName.text = moveInfo.Name;
        MoveDescription.text = moveInfo.Description;
        Damage.text = "Damage: " + moveInfo.Damage.ToString();
        ManaCost.text = "Mana Cost: " + moveInfo.ManaCost.ToString();
        Type.text = "Element Type: " + moveInfo.Type;
        Target.text = moveInfo.IsMultiTarget ? "Target Type: Multi-Target" : "Target Type: Single-Target";
    }
}

