﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{

	public GameObject mainCamera;
	public GameObject canvasInGame;
	public GameObject canvasPause;
	public GameObject canvasStageClear;
	public GameObject Player;
	public GameObject GoButton;
	public AudioSource asButton_Dado;
	public AudioSource asButton_Dalala;
	public AudioSource asButton_Du;
	public AudioSource goButton_as;
	public AudioSource music_win;
	public AudioSource complete_level;
	public GameObject door;
	private Coroutine countToWin;
	private bool isStartToWinCalled = false;
	private GameObject[] protractors;
	private bool isProtractorOn = false;
	private GameObject musicManager;
	private bool isPauseMenuOn = false;
	bool isLevelFinished = false;
	// Use this for initialization
	void Awake ()
	{
		//TODO: [time to complete the level] start counting
		isPauseMenuOn = false;
		isLevelFinished = false;
		StartCoroutine (AntiCheater());
		//SaveLoad.data = new PlayerData (7, 0, false);
		//SaveLoad.Save ();
		mainCamera.GetComponent<RapidBlurEffect> ().enabled = false;
		canvasInGame.SetActive (true);
		canvasPause.SetActive (false);
		canvasStageClear.SetActive (false);
		GoButton.SetActive (false);
		Global.isDragging = false;
		Global.isPaused = false;
		protractors = GameObject.FindGameObjectsWithTag ("Protractor");
		foreach (GameObject pro in protractors) {
			//pro.SetActive (false);
		}
		musicManager = GameObject.FindGameObjectWithTag ("MusicManager");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.touchCount > 0 && (Input.GetTouch (0).phase == TouchPhase.Began || Input.GetTouch (0).phase == TouchPhase.Moved)) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position), Vector2.zero);
			if (hit && hit.collider != null && hit.collider.tag == "GoButton" && !isPauseMenuOn) {
				goButton_as.Play ();
				Win ();
			}
		
		}
	}

//	public void ButtonDadoSoundPlay(){
//		asButton_Dado.Play ();
//	}
//
//	public void ButtonDalalaSoundPlay(){
//		asButton_Dalala.Play ();
//	}
//
//	public void ButtonDuSoundPlay(){
//		asButton_Du.Play ();
//	}

	/// <summary>
	/// Shows the pause menu.
	/// </summary>
	public void ShowPauseMenu ()
	{
		mainCamera.GetComponent<RapidBlurEffect> ().enabled = true;
		canvasPause.SetActive (true);
		canvasInGame.SetActive (false);
		//Global.isPaused = true;
		isPauseMenuOn = true;
	}

	/// <summary>
	/// All buttons clicks are using this sound
	/// </summary>
	public void PlayButtonClickSound(){
		musicManager.GetComponent<MusicManager> ().PlayClickButton ();
	}

	/// <summary>
	/// Show congrat UI when you complete this level
	/// </summary>
	public void ShowStageClear ()
	{
		music_win.Play ();
		mainCamera.GetComponent<RapidBlurEffect> ().enabled = true;
		canvasStageClear.SetActive (true);
		canvasInGame.SetActive (false);
		canvasPause.SetActive (false);
		Global.isPaused = true;

	}

	/// <summary>
	/// Shows the treasure if this level is trophy level
	/// </summary>
	public void ShowTreasure(){
		music_win.Play ();
		StartCoroutine (GoToTreasureRoom());
	}

	/// <summary>
	/// Gos to treasure room if this level is trophy level.
	/// </summary>
	/// <returns>The to treasure room.</returns>
	IEnumerator GoToTreasureRoom(){
		yield return new WaitForSeconds (3.6f);
		SceneManager.LoadScene ("Treasure");
	}

	/// <summary>
	/// We use this to prevent making angles without placement
	/// </summary>
	IEnumerator AntiCheater(){
		while (true) {
			yield return new WaitForSeconds (1);
			if (Global.antiCheater > 0) {
				Global.antiCheater--;
			}
		}
	}

	public void SwitchBgmToWin(){
		//music_win.Play ();
	}

	/// <summary>
	/// Hides the pause menu.
	/// </summary>
	public void HidePauseMenu ()
	{
		mainCamera.GetComponent<RapidBlurEffect> ().enabled = false;
		canvasPause.SetActive (false);
		canvasInGame.SetActive (true);
		//if(!isLevelFinished)
			//Global.isPaused = false;
		isPauseMenuOn = false;
	}

	/// <summary>
	/// Puzzle solved
	/// </summary>
	public void DoorOpened ()
	{
		if (!isStartToWinCalled) {
			// when the puzzle solved, we call the coroutine to win, but it's not really the winning.
			// it can still be interupted by DoorClosed() method.
			countToWin = StartCoroutine (CountToWin ());
			isStartToWinCalled = true;
			//disable hand click tutorial
			GameObject handClickTutorial = GameObject.FindGameObjectWithTag ("HandClickTutorial");
			if(handClickTutorial != null)
				handClickTutorial.SetActive (false);
		}
	}
		
	/// <summary>
	/// Puzzle solved status reverted, interuppt DoorOpen()
	/// </summary>
	public void DoorClosed ()
	{
		
		if (countToWin != null)
			StopCoroutine (countToWin);
		isStartToWinCalled = false;
		GoButton.SetActive (false);
	}

	/// <summary>
	/// Restart the level
	/// </summary>
	public void ReloadCurrentScene ()
	{
		Scene scene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (scene.name);
	}

	/// <summary>
	/// Loads the next level.
	/// </summary>
	public void LoadNextScene ()
	{
		Scene scene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (scene.buildIndex + 1);
	}

	/// <summary>
	/// Loads the map scene.
	/// </summary>
	public void LoadHomeScene ()
	{
		SceneManager.LoadScene ("StageNew");
	}

	/// <summary>
	/// Loads the trophy room scene.
	/// </summary>
	public void LoadTreasureScene ()
	{
		SceneManager.LoadScene ("Treasure");
	}

	/// <summary>
	/// Show congrat UI when you complete this level
	/// </summary>
	public void StageClear ()
	{
		ShowStageClear ();
	}

	/// <summary>
	/// Real winning
	/// The little character started walking towards the chest box
	/// </summary>
	void Win ()
	{
		GoButton.SetActive (false);
		canvasInGame.SetActive (false);
		canvasPause.SetActive (false);
		mainCamera.GetComponent<RapidBlurEffect> ().enabled = false;
		Global.isPaused = true;
		Player.GetComponent<Player> ().Win ();
	}

	/// <summary>
	/// Counts to Win().
	/// </summary>
	/// <returns>The to window.</returns>
	IEnumerator CountToWin ()
	{
		yield return new WaitForSeconds (2.4f);
		GoButton.SetActive (true);
		complete_level.Play ();
		Global.isPaused = true;
		isLevelFinished = true;
		door.GetComponent<Door> ().DoorOpened ();
		Player.GetComponent<Player> ().FeelHappy ();
		//TODO: [time to complete the level] end counting
	}

	/// <summary>
	/// Toggle on all protractors
	/// </summary>
	public void ToggleAllProtractors(){
		if (isProtractorOn) {
			foreach (GameObject pro in protractors) {
				pro.SetActive (false);
			}
		} else {
			foreach (GameObject pro in protractors) {
				pro.SetActive (true);
			}
		}
		isProtractorOn = !isProtractorOn;
	}

	/// <summary>
	/// Used for debugging, cheat buttons
	/// </summary>
	void OnGUI ()
	{
//		if (GUI.Button (new Rect (10, 10, 70, 70), "Reload")) {
//			ReloadCurrentScene ();
//		}

//		if (GUI.Button (new Rect (10, 100, 70, 70), "Save")) {
//			Save ();
//		}
//
//		if (GUI.Button (new Rect (10, 190, 70, 70), "Load")) {
//			Load ();
//		}
//
//		GUI.Label (new Rect(10,300,100,60),"Coins: "+coinsAmount);
//
//		if (GUI.Button (new Rect (10, 400, 70, 70), "AddCoin")) {
//			coinsAmount++;
//		}
//
//		if (GUI.Button (new Rect (10, 500, 70, 70), "LoseCoin")) {
//			coinsAmount--;
//		}
//		if (GUI.Button (new Rect (10, 500, 130, 80), "Hint")) {
//			ToggleProtractor ();
//		}
	}



}
