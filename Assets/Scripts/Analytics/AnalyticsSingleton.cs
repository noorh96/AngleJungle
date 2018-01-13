using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class AnalyticsSingleton : Singleton<AnalyticsSingleton> {

	protected AnalyticsSingleton () {}

	public float levelStart, levelEnd, levelTime;
	public string levelName;
	//public static gemPlacements;

	/// <summary>
	/// Dispatchs the analytics level data about gems.
	/// </summary>
	public void DispatchData()
	{
		levelTime = levelEnd - levelStart;

		Analytics.CustomEvent("levelEnded", new Dictionary<string, object>
			{	
				{ levelName, levelTime }
			});
	}
}
