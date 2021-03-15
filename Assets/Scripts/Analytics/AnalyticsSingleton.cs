using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnalyticsSingleton : Singleton<AnalyticsSingleton> {

	protected AnalyticsSingleton () {}

	// Parameters for analytics
	public float levelStart, levelEnd, levelTime;
	public string levelName;
	public AnalyticsGemEndState gemEndState = new AnalyticsGemEndState();
	public AnalyticsGemHistory gemHistory = new AnalyticsGemHistory();

	public void CalculateLevelTime()
	{
		levelTime = levelEnd - levelStart;
	}

	/// <summary>
	/// Dispatchs the analytics level data about gems.
	/// </summary>	
	public void DispatchData()
	{
        Dictionary<string, object> dataDict = new Dictionary<string, object>
        {
            { Global.ANALYTICS_LEVEL_TIME, levelTime },
            { Global.ANALYTICS_GEM_END_STATE, gemEndState.ToJson() },
            { Global.ANALYTICS_GEM_HISTORY, gemHistory.ToJson() }
        };

        // Enable when UnityEngine analytics is available
        //Analytics.CustomEvent (levelName, dataDict);
        Debug.Log("DISPATCHED ANALYTICS DATA!");

        Debug.Log("FLUSHING GEM DICTIONARIES!");
        gemHistory.actionData.Clear (); 
		gemEndState.mirrorData.Clear ();
	}

	public void DebugPrint()
	{
		Debug.Log (Global.ANALYTICS_LEVEL_TIME + " " + levelTime);
		Debug.Log ("gem_history");
		gemHistory.DebugPrint ();
		Debug.Log ("gem_end_state");
		gemEndState.DebugPrint ();
	}
}
