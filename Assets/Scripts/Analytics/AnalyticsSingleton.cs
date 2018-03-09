using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
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
		Dictionary<string, object> dataDict = new Dictionary<string, object> ();

		dataDict.Add (Global.ANALYTICS_LEVEL_TIME, levelTime);
        dataDict.Add(Global.ANALYTICS_GEM_END_STATE, gemEndState.ToJson());
        dataDict.Add(Global.ANALYTICS_GEM_HISTORY, gemHistory.ToJson());

		Analytics.CustomEvent (levelName, dataDict);
        Debug.Log("DISPATCHED ANALYTICS DATA!");

		// Flush mirrorData for next level
		if (gemHistory.actionData != null || gemEndState.mirrorData != null) 
		{
			gemHistory.actionData.Clear ();
			gemEndState.mirrorData.Clear ();
		}
        Debug.Log("FLUSHING GEM DICTIONARIES!");
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
