using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class AnalyticsGemEndState {

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
	/// Prints some debug strings.
	/// </summary>
	public void DebugPrint()
	{
        //foreach (var mirror in mirrorData) 
        //{
        //	Debug.Log (mirror.Key);

        //	foreach (var gem in mirror.Value) 
        //	{
        //		Debug.Log (gem);			
        //	}	
        //}

        Debug.Log(ToJson());
	}

    /// <summary>
    /// Converts the gem end state data to JSON.
    /// </summary>
    /// <returns>JSON data of gem end state</returns>
    public string ToJson()
    {
        return JsonConvert.SerializeObject(mirrorData);
    }

	/// <summary>
	/// Builds the state of the gems at the end of the level.
	/// </summary>
	/// <param name="levelMirrors">Level mirrors.</param>
	public void BuildGemEndState(List<Mirror> levelMirrors)
	{
		foreach (var mirror in levelMirrors) 
		{
			foreach (var slot in mirror.slotList) 
			{
				AddGem (mirror.gameObject.name, slot.targetGO.gameObject.name);
			}	
		}
	}
}