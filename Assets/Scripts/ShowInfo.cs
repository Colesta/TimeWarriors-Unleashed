using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Net.Security;

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
    public Stats s;
    public SetStats ss;

    
    //When the mouse hovers over a certain GameObject, The Stats of that object will be displayed on Screen. This was done to minimize UI elements on screen to be less overwhelming
    public void OnPointerEnter(PointerEventData eventData)
    {
        visibleObject.SetActive(true);
        ss = Hero1.GetComponent<SetStats>();

        //trigger Object is the object that when the mouse hovers over it, the "Visible Object" (Whihc is the same object for every case) will be visible
        //while the same object is being made visible everytime, each if statement chnages the values of the object, making it unique to every game object whilst cuttong down on UI elements (Meaning less startup time and loding)

        
        if (triggerObject == Hero1)
        {

            

            HealthText.text = ss.returnCurrentHeroHP(1) + "/" + ss.returnMaxHeroHP(1);
            HPSlider.maxValue = ss.returnMaxHeroHP(1);
            HPSlider.value = ss.returnCurrentHeroHP(1);

            ManaText.text = ss.returnCurrentMana(1) + "/" + ss.returnMaxMana(1);
            ManaSlider.maxValue = ss.returnMaxMana(1);
            ManaSlider.value = ss.returnCurrentMana(1);

            typeText.text = ss.returnType(1);

            Mana.SetActive(true);

            

        }


        else if (triggerObject == Hero2)
        {
            HealthText.text = ss.returnCurrentHeroHP(2) + "/" + ss.returnMaxHeroHP(2);
            HPSlider.maxValue = ss.returnMaxHeroHP(2);
            HPSlider.value = ss.returnCurrentHeroHP(2);

            ManaText.text = ss.returnCurrentMana(2) + "/" + ss.returnMaxMana(2);
            ManaSlider.maxValue = ss.returnMaxMana(2);
            ManaSlider.value = ss.returnCurrentMana(2);

            typeText.text = ss.returnType(2);

            Mana.SetActive(true);

           
        }
        else if (triggerObject == Hero3)
        {
            HealthText.text = ss.returnCurrentHeroHP(3) + "/" + ss.returnMaxHeroHP(3);
            HPSlider.maxValue = ss.returnMaxHeroHP(3);
            HPSlider.value = ss.returnCurrentHeroHP(3);

            ManaText.text = ss.returnCurrentMana(3) + "/" + ss.returnMaxMana(3);
            ManaSlider.maxValue = ss.returnMaxMana(3);
            ManaSlider.value = ss.returnCurrentMana(3);

            typeText.text = ss.returnType(3);

            Mana.SetActive(true);

           

        }
        else if (triggerObject == Hero4)
        {
            HealthText.text = ss.returnCurrentHeroHP(4) + "/" + ss.returnMaxHeroHP(4);
            HPSlider.maxValue = ss.returnMaxHeroHP(4);
            HPSlider.value = ss.returnCurrentHeroHP(4);


            ManaText.text = ss.returnCurrentMana(4) + "/" + ss.returnMaxMana(4);
            ManaSlider.maxValue = ss.returnMaxMana(4);
            ManaSlider.value = ss.returnCurrentMana(4);

            typeText.text = ss.returnType(4);

            Mana.SetActive(true);

        }
        else if (triggerObject == Enemy1)
        {
            HealthText.text = ss.returnCurrentEnemyHP(1) + "/" + ss.returnMaxEnemyHP(1);
            HPSlider.maxValue = ss.returnMaxEnemyHP(1);
            HPSlider.value = ss.returnCurrentEnemyHP(1);


            typeText.text = ss.returnType(1);

            Mana.SetActive(false);

           

        }
        else if (triggerObject == Enemy2)
        {
            HealthText.text = ss.returnCurrentEnemyHP(2) + "/" + ss.returnMaxEnemyHP(2);
            HPSlider.maxValue = ss.returnMaxEnemyHP(2);
            HPSlider.value = ss.returnCurrentEnemyHP(2);

            typeText.text = ss.returnType(1);

            Mana.SetActive(false);

          

        }
        else if (triggerObject == Enemy3)
        {
            HealthText.text = ss.returnCurrentEnemyHP(3) + "/" + ss.returnMaxEnemyHP(3);
            HPSlider.maxValue = ss.returnMaxEnemyHP(3);
            HPSlider.value = ss.returnCurrentEnemyHP(3);

            typeText.text = ss.returnType(1);

            Mana.SetActive(false);

          
        }
        else if (triggerObject == Enemy4)
        {
            HealthText.text = ss.returnCurrentEnemyHP(4) + "/" + ss.returnMaxEnemyHP(4);
            HPSlider.maxValue = ss.returnMaxEnemyHP(4);
            HPSlider.value = ss.returnCurrentEnemyHP(4);

            typeText.text = ss.returnType(1);

            Mana.SetActive(false);

           

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // When the mouse exits the trigger object, hide the visible object
        visibleObject.SetActive(false);


    }
}
