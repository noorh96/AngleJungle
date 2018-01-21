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
	public int protractorOpenedNum;
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

		Analytics.CustomEvent (levelName, dataDict);

		// Flush mirrorData for next level
		if (gemHistory.mirrorData != null || gemEndState.mirrorData != null) 
		{
			gemHistory.mirrorData.Clear ();
			gemEndState.mirrorData.Clear ();
		}
	}

	public void DebugPrint()
	{
		Debug.Log (Global.ANALYTICS_LEVEL_TIME + " " + levelTime);
		Debug.Log (Global.ANALYTICS_PROTRACTOR_OPENED + " " + protractorOpenedNum);
		Debug.Log ("GEM HISTORY");
		gemHistory.DebugPrint ();
		Debug.Log ("GEM END STATE");
		gemEndState.DebugPrint ();
	}
}
