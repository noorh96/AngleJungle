using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Trophys : MonoBehaviour {

	public List<GameObject> TrophyList;//get this
	public GameObject Tro1, Tro2, Tro3, Tro4, Tro5, Tro6;
	public List<GameObject> InitTrosList;

	// Use this for initialization
	void Awake () {
		TrophyList = new List<GameObject> ();
		InitTrosList = new List<GameObject> ();
		InitTrosList.Add (Tro1);
		InitTrosList.Add (Tro2);
		InitTrosList.Add (Tro3);
		InitTrosList.Add (Tro4);
		InitTrosList.Add (Tro5);
		InitTrosList.Add (Tro6);
		SaveLoad.Load ();
		for (int i = 0; i < 6; i++) {
			TrophyList.Add (InitTrosList[SaveLoad.data.TrophyOrder[i] - 1]);
		}
	}
}


