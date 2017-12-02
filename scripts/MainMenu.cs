//MainMenu.cs
//Cody O'Connor
//group 15 project, maggie the magbot


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	public void PlayGame() {
        SceneManager.LoadScene("Level1");
	}

    public void ExitGame(){
        Application.Quit();
    }


	
	
}
