using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour {
	public GameObject star;
	// Use this for initialization
	void Awake () {
		star.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowStar(){
		star.SetActive (true);
	}
}
