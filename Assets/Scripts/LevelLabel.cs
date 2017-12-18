using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLabel : MonoBehaviour {
	
    public bool Uppercase = false;
	
    // Use this for initialization
	void Start () 
    {
		Scene scene = SceneManager.GetActiveScene ();
		int levelIndex = scene.buildIndex - 1;
		string levelIndexStr = levelIndex < 10 ? (" " + levelIndex.ToString ()) : levelIndex.ToString ();
		if(Uppercase)
			GetComponent<Text> ().text = "LEVEL " + levelIndexStr;
		else
			GetComponent<Text> ().text = "Level " + levelIndexStr;
	}
}
