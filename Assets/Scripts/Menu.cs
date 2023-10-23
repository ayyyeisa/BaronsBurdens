using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayDragon()
    {
        EditorSceneManager.LoadScene("DragonRidingScene");
    }
    public void PlayCatapult()
    {
        EditorSceneManager.LoadScene("CatapultScene");
    }
    public void PlayDuel()
    {

        EditorSceneManager.LoadScene("DuelScene");


    }
    public void QuitGame()
    {
         Debug.Log("Quit");
        //On Selction of the Quit Button, Game will close
        Application.Quit();
        
       
    }



}
