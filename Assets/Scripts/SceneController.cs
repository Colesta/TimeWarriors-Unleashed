using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
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

   





