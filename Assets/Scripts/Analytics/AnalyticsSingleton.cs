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
	public Dictionary<string, List<string>> mirrorData = new Dictionary<string, List<string>>(); 

	/// <summary>
	/// Method for tracking gem placements in the level.
	/// </summary>
	/// <param name="mirror">Mirror that gem was placed in.</param>
	/// <param name="gemName">Name of gem that was placed - name described the gem value.</param>
	public void AddGem(string mirror, string gemName)
	{
		if (!mirrorData.ContainsKey (mirror)) 
		{
			mirrorData.Add (mirror, new List<string> ());
		} 

		mirrorData [mirror].Add (gemName);
	}

	/// <summary>
	/// Dispatchs the analytics level data about gems.
	/// </summary>
	public void DispatchData()
	{
		levelTime = levelEnd - levelStart;

		Dictionary<string, object> dataDict = new Dictionary<string, object> ();

		dataDict.Add ("levelTime", levelTime);

		foreach (KeyValuePair<string, List<string>> mirror in mirrorData)
		{
			dataDict.Add (mirror.Key, mirror.Value.Count);
		}

		Analytics.CustomEvent (levelName, dataDict);

		// Flush mirrorData for next level
		if (mirrorData != null) 
		{
			mirrorData.Clear ();
		}
	}
}
