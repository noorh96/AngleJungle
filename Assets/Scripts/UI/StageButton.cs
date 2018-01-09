using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour {

	public GameObject star;

	// Use this for initialization
	void Awake () 
	{
		star.SetActive (false);
	}

	public void ShowStar()
	{
		star.SetActive (true);
	}
}
