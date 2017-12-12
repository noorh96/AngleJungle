using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGem : MonoBehaviour {

	public GameObject door;
	public GameObject PowerGemParticle;
	public GameObject mask;
	public int ActivateNum = 1;

	private Sprite greyPG;
	private Sprite redPG;
	public GameObject L1;
	public GameObject L2;
	public GameObject L3;
	public GameObject L4;

	public Sprite greyPG1;
	public Sprite greyPG2;
	public Sprite greyPG3;
	public Sprite greyPG4;

	public Sprite redPG1;
	public Sprite redPG2;
	public Sprite redPG3;
	public Sprite redPG4;

	private float countDown = -1f;
	private float countFactor = 3f;
	private SpriteRenderer sp;
	// Use this for initialization
	void Start () {
		sp = GetComponent<SpriteRenderer> ();
		L1.SetActive (false);
		L2.SetActive (false);
		L3.SetActive (false);
		L4.SetActive (false);
		switch (ActivateNum) {
		case 1:
			greyPG = greyPG1;
			redPG = redPG1;
			countFactor = 0.5f;
			L1.transform.localPosition = new Vector2 (-0.15f, -2.26f);
			break;
		case 2:
			greyPG = greyPG2;
			redPG = redPG2;
			countFactor = 1.4f;
			L1.transform.localPosition = new Vector2 (-0.924f, -2.188f);
			L2.transform.localPosition = new Vector2 (0.627f, -2.158f);
			break;
		case 3:
			greyPG = greyPG3;
			redPG = redPG3;
			countFactor = 2.3f;
			L1.transform.localPosition = new Vector2 (-1.429f, -2.164f);
			L2.transform.localPosition = new Vector2 (-0.06f, -2.298f);
			L3.transform.localPosition = new Vector2 (1.272f, -2.17f);
			break;
		case 4:
			greyPG = greyPG4;
			redPG = redPG4;
			countFactor = 3.2f;
			L1.transform.localPosition = new Vector2 (-1.903f, -1.981f);
			L2.transform.localPosition = new Vector2 (-0.869f, -2.231f);
			L3.transform.localPosition = new Vector2 (0.481f, -2.261f);
			L4.transform.localPosition = new Vector2 (1.77f, -1.994f);
			break;
		}
		sp.sprite = greyPG;
		PowerGemParticle.SetActive (false);
		StartCoroutine (CountDown());
	}
	
	// Update is called once per frame

	void Update () {
		if (ActivateNum == 1) {
			if (countDown >= 4)
				L1.SetActive (true);
			else
				L1.SetActive (false);
		}
		else if (ActivateNum == 2) {
			if (countDown >= 0)
				L1.SetActive (true);
			else
				L1.SetActive (false);
			if (countDown >= 4)
				L2.SetActive (true);
			else
				L2.SetActive (false);
		}
		else if (ActivateNum == 3) {
			if (countDown >= 0)
				L1.SetActive (true);
			else
				L1.SetActive (false);
			if (countDown >= 1)
				L2.SetActive (true);
			else
				L2.SetActive (false);
			if (countDown >= 4)
				L3.SetActive (true);
			else
				L3.SetActive (false);
		}else if (ActivateNum == 4) {
			if (countDown >= 0)
				L1.SetActive (true);
			else
				L1.SetActive (false);
			if (countDown >= 1)
				L2.SetActive (true);
			else
				L2.SetActive (false);
			if (countDown >= 2)
				L3.SetActive (true);
			else
				L3.SetActive (false);
			if (countDown >= 4)
				L4.SetActive (true);
			else
				L4.SetActive (false);
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		print (coll.gameObject.name);
	}

	public void ActivateGem(){
		countDown = Mathf.Min((countDown + 1f), 4);
		//countDown ++;
		if (countDown >= 4) {
			PowerGemParticle.SetActive (true);
			sp.sprite = redPG;
			door.GetComponent<Door> ().OpenDoor ();
		}
	}
		

	public void DeactivateGem(){
		PowerGemParticle.SetActive (false);
		sp.sprite = greyPG;
		door.GetComponent<Door> ().CloseDoor();
	}
		
	IEnumerator CountDown(){
		while (true) {
			yield return new WaitForSeconds (0.01f);
			countDown = Mathf.Max((countDown - countFactor), -1f);//1:-0.5; 2:-1; 3:-2.3; 4:3.0
			if (countDown <= 0) {
				DeactivateGem ();
			}
		}
	}

//	private GUIStyle guiStyle = new GUIStyle ();
//	void OnGUI ()
//	{
//		guiStyle.fontSize = 36;
//		guiStyle.normal.textColor = Color.blue;
//		GUI.Label (new Rect(10,300,100,60),"countDown: "+countDown, guiStyle);
//		
//
//	}
}
