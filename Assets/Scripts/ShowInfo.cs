using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShowInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI HealthText;
    public Slider HPSlider;
    public TextMeshProUGUI ManaText;
    public Slider ManaSlider;
    public TextMeshProUGUI typeText;

    public GameObject Mana;

    public GameObject visibleObject;
    public GameObject triggerObject;

    public Color fillColor;

    public GameObject Hero1;
    public GameObject Hero2;
    public GameObject Hero3;
    public GameObject Hero4;
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public GameObject Enemy4;
    
    private Stats s;
    private SetStats ss;

    void Awake()
    {
        s = GetComponent<Stats>();
        ss = FindObjectOfType<SetStats>(); // Find the SetStats manager

        if (ss == null)
        {
            Debug.LogError("SetStats manager is missing from the scene!");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (visibleObject == null)
        {
            Debug.LogError("visibleObject is not assigned!");
            return;
        }

        visibleObject.SetActive(true);

        // Null checks for all heroes/enemies
        if (Hero1 == null || Hero2 == null || Hero3 == null || Hero4 == null ||
            Enemy1 == null || Enemy2 == null || Enemy3 == null || Enemy4 == null)
        {
            Debug.LogError("One or more hero/enemy GameObjects are not assigned!");
            return;
        }

        if (triggerObject == null)
        {
            Debug.LogError("triggerObject is not assigned!");
            return;
        }

        // Handle Hero1
        if (triggerObject == Hero1)
        {
            UpdateUIWithHeroStats(1);
        }
        // Handle Hero2
        else if (triggerObject == Hero2)
        {
            UpdateUIWithHeroStats(2);
        }
        // Handle Hero3
        else if (triggerObject == Hero3)
        {
            UpdateUIWithHeroStats(3);
        }
        // Handle Hero4
        else if (triggerObject == Hero4)
        {
            UpdateUIWithHeroStats(4);
        }
        // Handle Enemy1
        else if (triggerObject == Enemy1)
        {
            UpdateUIWithEnemyStats(1);
        }
        // Handle Enemy2
        else if (triggerObject == Enemy2)
        {
            UpdateUIWithEnemyStats(2);
        }
        // Handle Enemy3
        else if (triggerObject == Enemy3)
        {
            UpdateUIWithEnemyStats(3);
        }
        // Handle Enemy4
        else if (triggerObject == Enemy4)
        {
            UpdateUIWithEnemyStats(4);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (visibleObject != null)
        {
            visibleObject.SetActive(false);
        }
        else
        {
            Debug.LogError("visibleObject is not assigned!");
        }
    }

    private void UpdateUIWithHeroStats(int heroIndex)
    {
        if (HealthText == null || HPSlider == null || ManaText == null || ManaSlider == null || typeText == null || Mana == null)
        {
            Debug.LogError("One or more UI elements are not assigned!");
            return;
        }

        HealthText.text = ss.returnCurrentHeroHP(heroIndex) + "/" + ss.returnMaxHeroHP(heroIndex);
        HPSlider.maxValue = ss.returnMaxHeroHP(heroIndex);
        HPSlider.value = ss.returnCurrentHeroHP(heroIndex);

        ManaText.text = ss.returnCurrentMana(heroIndex) + "/" + ss.returnMaxMana(heroIndex);
        ManaSlider.maxValue = ss.returnMaxMana(heroIndex);
        ManaSlider.value = ss.returnCurrentMana(heroIndex);

        typeText.text = ss.returnType(heroIndex);

        Mana.SetActive(true); // Ensure Mana UI is visible for heroes
    }

    private void UpdateUIWithEnemyStats(int enemyIndex)
    {
        if (HealthText == null || HPSlider == null || typeText == null || Mana == null)
        {
            Debug.LogError("One or more UI elements are not assigned!");
            return;
        }

        HealthText.text = ss.returnCurrentEnemyHP(enemyIndex) + "/" + ss.returnMaxEnemyHP(enemyIndex);
        HPSlider.maxValue = ss.returnMaxEnemyHP(enemyIndex);
        HPSlider.value = ss.returnCurrentEnemyHP(enemyIndex);

        typeText.text = ss.returnType(enemyIndex);

        Mana.SetActive(false); // Hide Mana UI for enemies
    }
}
