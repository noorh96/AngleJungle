using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandClickTutorial : MonoBehaviour {
	public float showUpSeconds = 3f;
	public GameObject hand;
	// Use this for initialization
	void Start () {
		StartCoroutine (ShowHand());
		hand.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator ShowHand(){
		yield return new WaitForSeconds (showUpSeconds);
		if(!Global.isPaused)
			hand.SetActive (true);
	}
}
