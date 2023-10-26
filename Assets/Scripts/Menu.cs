using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class Menu : MonoBehaviour
{

    public void PlayDragon()
    {
        //Opens Dragon Riding Scene from Menu
        EditorSceneManager.LoadScene(1);
    }
    public void PlayCatapult()
    {
        //Opens Catapult Scene from Menu
        EditorSceneManager.LoadScene(2);
    }
    public void PlayDuel()
    {
        //Opens Duel Scene from Menu
        EditorSceneManager.LoadScene(3);

    }
    public void QuitGame()
    {
         Debug.Log("Quit");
        //On Selction of the Quit Button, Game will close
        Application.Quit();
        
       
    }



}
