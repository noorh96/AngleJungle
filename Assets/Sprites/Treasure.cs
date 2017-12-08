using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Treasure : MonoBehaviour {

	public Sprite sp_closedChest;
	public Sprite sp_openChest;
	public GameObject par_Chest;
	public GameObject GM;
	public GameObject TreasureCanvas;
	private GameObject musicManager;
	Scene scene;
	int levelIndex;
	int TrophyIndex;
	GameObject treasure;
	Vector3 originTreasureScale;
	bool isTreasureCanvasActivated = false;
	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer> ().sprite = sp_closedChest;
		par_Chest.SetActive (false);
		scene = SceneManager.GetActiveScene ();
		levelIndex = scene.buildIndex - 1;
		//Play Ambient Sound
		musicManager = GameObject.FindGameObjectWithTag ("MusicManager");
		if (levelIndex <= 7) {
			musicManager.GetComponent<MM> ().PlayS1Sound ();
		} else if (levelIndex > 7 && levelIndex <= 13) {
			musicManager.GetComponent<MM> ().PlayS2Sound ();
		} else if (levelIndex > 13 && levelIndex <= 18) {
			musicManager.GetComponent<MM> ().PlayS3Sound ();
		} else if (levelIndex > 18 && levelIndex <= 25) {
			musicManager.GetComponent<MM> ().PlayS4Sound ();
		}else {
			musicManager.GetComponent<MM> ().PlayS5Sound ();
		}
		if(TreasureCanvas != null)
			TreasureCanvas.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (treasure != null) {
			treasure.transform.position = Vector2.Lerp (treasure.transform.position, Camera.main.gameObject.transform.position, Time.deltaTime);
			treasure.transform.localScale = Vector2.Lerp (treasure.transform.localScale, originTreasureScale * 2.4f, Time.deltaTime);
		}
		if (TreasureCanvas != null && isTreasureCanvasActivated) {
			TreasureCanvas.GetComponentInChildren<Image> ().color = Color.Lerp (TreasureCanvas.GetComponentInChildren<Image> ().color, new Color(1,1,1,0.8f), Time.deltaTime);
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			bool isFirstTimePassThisLevel = false;
			SaveLoad.Load ();
			if (SaveLoad.data.LevelProgress == levelIndex) {
				SaveLoad.data.LevelProgress++;
				SaveLoad.Save ();
				isFirstTimePassThisLevel = true;
			}
			coll.gameObject.GetComponent<Player> ().Get ();
			GetComponent<SpriteRenderer> ().sprite = sp_openChest;
			par_Chest.SetActive (true);
			if (isFirstTimePassThisLevel && (levelIndex == 1 || levelIndex == 7 || levelIndex == 13 || levelIndex == 18 || levelIndex == 25 || levelIndex == 30)) {
				StartCoroutine (CountToShowTreasure ());
				switch (levelIndex) {
				case 1:
					TrophyIndex = 0;
					break;				
				case 7:
					TrophyIndex = 1;
					break;
				case 13:
					TrophyIndex = 2;
					break;
				case 18:
					TrophyIndex = 3;
					break;
				case 25:
					TrophyIndex = 4;
					break;
				case 30:
					TrophyIndex = 5;
					break;
				}
			} else {
				StartCoroutine (CountToClearStage ());
			}
		}
	}

	void TreasureOnScreen(){
		//Instantiate Treasure
		treasure = (GameObject)Instantiate (GM.GetComponent<Trophys> ().TrophyList [TrophyIndex], gameObject.transform.position, Quaternion.identity);
		originTreasureScale = treasure.transform.localScale;
		//TreasureCanvas
		if (TreasureCanvas != null) {
			TreasureCanvas.SetActive (true);
			isTreasureCanvasActivated = true;
		}
	}

	IEnumerator CountToClearStage(){
		yield return new WaitForSeconds (1);
		GM.GetComponent<GM> ().StageClear ();
	}

	IEnumerator CountToShowTreasure(){
		yield return new WaitForSeconds (0.2f);
		TreasureOnScreen ();
		GM.GetComponent<GM> ().ShowTreasure ();
	}
}
