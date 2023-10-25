//Menu.cs
//Carl Crumer
//Creation Date: October 22 2023
//
// The script handles the main menu and loading different minigame

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class Menu : MonoBehaviour
{

    public void PlayDragon()
    {
        //Opens Dragon Riding Scene from Menu
        EditorSceneManager.LoadScene("DragonRidingScene");
    }
    
    public void PlayCatapult()
    {
        //Opens Catapult Scene from Menu
        EditorSceneManager.LoadScene("CatapultScene");
    }
    public void PlayDuel()
    {
        //Opens Duel Scene from Menu
        EditorSceneManager.LoadScene("DuelScene");

    }
    public void QuitGame()
    {
         Debug.Log("Quit");
        //On Selction of the Quit Button, Game will close
        Application.Quit();
        
       
    }



}
