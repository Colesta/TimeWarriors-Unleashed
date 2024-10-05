using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
   //Each Method will dismiss the scene currenlty Active, and call the correspnding Scene
   //Uses the Scene Manager class's methods to do this
   //For exmaple, if we were in the Main Menu Scene, this Mehtod would close that Scene and Open the Overworld Scene
   public void gotoOverworld(){

        SceneManager.LoadScene("Overworld");
  }

  public void gotoBattle(){

     SceneManager.LoadScene("Battle");
  }

  public void gotoShop(){

   SceneManager.LoadScene("Shop");

  }

   public void gotoMainMenu(){

   SceneManager.LoadScene("MainMenu");


  }


   }

   





