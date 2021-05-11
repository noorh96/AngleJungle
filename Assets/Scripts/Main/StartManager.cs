using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour {

    public GameObject InfoCanvas;
    public GameObject ResetCanvas;
    public GameObject ResetConfirmCanvas;
	bool showLoading = false;
	GameObject musicManager;
    public Log logObject;

    // Use this for initialization
    void Awake() 
    {
        InfoCanvas.SetActive(false);
        ResetCanvas.SetActive(false);
        ResetConfirmCanvas.SetActive(false);
        SaveLoad.Load();
        
        logObject = new Log();
        Debug.Log(Application.persistentDataPath); // move this to init message location
		Debug.Log("TESTING MESSAGE IS LOGGING EVEN WORKING??");
		logObject.WriteToFile("\n \n \n====================================================================");
		logObject.WriteToFile("Initializing Log file");

    }

	void Start () 
    {
		showLoading = false;
		musicManager = GameObject.FindGameObjectWithTag (Global.TAG_MUSIC_MANAGER);
		musicManager.GetComponent<MusicManager> ().PlayBGM ();
	}
	
    void OnGUI()
    {
//        if (GUI.Button(new Rect(10, 10, 160, 100), "Reset(1)"))
//        {
//            Reset(0);
//        }
//        if (GUI.Button(new Rect(210, 10, 160, 100), "Reset(7)"))
//        {
//            Reset(7);
//        }
//        if (GUI.Button(new Rect(410, 10, 160, 100), "Reset(13)"))
//        {
//            Reset(13);
//        }
//        if (GUI.Button(new Rect(610, 10, 160, 100), "Reset(18)"))
//        {
//            Reset(18);
//        }
//        if (GUI.Button(new Rect(810, 10, 160, 100), "Reset(30)"))
//        {
//            Reset(30);
//        }

//		if (GUI.Button(new Rect(10, 200, 160, 100), "Level(31)"))
//		{
//			SceneManager.LoadScene("Level31");
//		}
//		if (GUI.Button(new Rect(210, 200, 160, 100), "Level(32)"))
//		{
//			SceneManager.LoadScene("Level32");
//		}
//		if (GUI.Button(new Rect(410, 200, 160, 100), "Level(33)"))
//		{
//			SceneManager.LoadScene("Level33");
//		}
//		if (GUI.Button(new Rect(610, 200, 160, 100), "Level(34)"))
//		{
//			SceneManager.LoadScene("Level34");
//		}
//		if (GUI.Button(new Rect(810, 200, 160, 100), "Level(35)"))
//		{
//			SceneManager.LoadScene("Level35");
//		}
//		if (GUI.Button(new Rect(810, 200, 160, 100), "Level(36)"))
//		{
//			SceneManager.LoadScene("Level36");
//		}

		if (showLoading) 
        {
			GUIStyle loadingStyle = new GUIStyle ();
			Font myfont = (Font)Resources.Load (Global.FONT_ASAP_MEDIUM, typeof(Font));
			loadingStyle.font = myfont;
			loadingStyle.fontSize = 60;
			loadingStyle.alignment = TextAnchor.MiddleCenter;
			loadingStyle.normal.textColor = Color.white;
			loadingStyle.hover.textColor = Color.white;
			GUI.depth = -1001;																// make the black texture render on top (drawn last)
			GUI.Label (new Rect (Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 100), "Loading....", loadingStyle);
		}

    }

    public void Reset(int level)
    {
        SaveLoad.data = new PlayerData (level, 0, false);
        SaveLoad.Save ();
        ShowResetConfirm();
    }

    public void LoadStage()
    {
		musicManager.GetComponent<MusicManager> ().PlayMenuButton ();
		
        //reset everything
		if (SaveLoad.data.LevelProgress < 1) {
			SaveLoad.data.LevelProgress = 1;
			SaveLoad.data.TrophyOrder.Shuffle ();
			SaveLoad.Save ();
			//StartCoroutine (LoadStageCo("Level1"));
			SceneManager.LoadScene (Global.SCENE_MAP);
        }
        else 
        { 
            SceneManager.LoadScene (Global.SCENE_MAP);
        }
    }
	
    IEnumerator LoadStageCo(string stageName)
    {
		showLoading = true;
		float fadeTime = GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene (stageName);
	}

    public void HideInfo()
    {
        InfoCanvas.SetActive(false);
		musicManager.GetComponent<MusicManager> ().PlayClickButton ();
    }

    public void ShowInfo()
    {
        InfoCanvas.SetActive(true);
		musicManager.GetComponent<MusicManager> ().PlayMenuButton ();
    }

    public void ShowReset()
    {
        ResetCanvas.SetActive(true);
		musicManager.GetComponent<MusicManager> ().PlayMenuButton ();
    }

    public void HideReset()
    {
        ResetCanvas.SetActive(false);
		musicManager.GetComponent<MusicManager> ().PlayClickButton ();
    }

    private void ShowResetConfirm()
    {
        ResetCanvas.SetActive(false);
        ResetConfirmCanvas.SetActive(true);
		musicManager.GetComponent<MusicManager> ().PlayClickButton ();
    }

    public void HideResetConfirm()
    {
        ResetConfirmCanvas.SetActive(false);
		musicManager.GetComponent<MusicManager> ().PlayClickButton ();
    }
}
