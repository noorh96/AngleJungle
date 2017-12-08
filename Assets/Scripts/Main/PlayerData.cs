using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class PlayerData
{
	public int LevelProgress;
	public int TrophyGot;
	public bool ProTuGot;
	public List<int> TrophyOrder = new List<int> ();
	public PlayerData(){
		LevelProgress = 0;
		TrophyGot = 0;
		ProTuGot = false;
		TrophyOrder.Add (1);
		TrophyOrder.Add (2);
		TrophyOrder.Add (3);
		TrophyOrder.Add (4);
		TrophyOrder.Add (5);
		TrophyOrder.Add (6);
	}

	public PlayerData(int _LevelProgress, int _TrophyGot, bool _ProTuGot){
		LevelProgress = _LevelProgress;
		TrophyGot = _TrophyGot;
		ProTuGot = false;
		TrophyOrder.Add (1);
		TrophyOrder.Add (2);
		TrophyOrder.Add (3);
		TrophyOrder.Add (4);
		TrophyOrder.Add (5);
		TrophyOrder.Add (6);
	}
}
