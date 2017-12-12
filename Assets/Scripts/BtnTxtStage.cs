using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BtnTxtStage : MonoBehaviour {
	Vector3 originPos;
	Vector3 downPos;
	// Use this for initialization
	void Start () {
		originPos = gameObject.transform.position;
		downPos = originPos - new Vector3 (0, 0.03f, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponentInParent<Button> ().IsInteractable () == false)
			gameObject.SetActive (false);
		else
			gameObject.SetActive (true);
	}
	public void BtnDown(){
		gameObject.transform.position = downPos;
	}

	public void BtnUp(){
		gameObject.transform.position = originPos;
	}

}
