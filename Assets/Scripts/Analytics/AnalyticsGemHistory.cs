using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsGemHistory {

	public Dictionary<string, List<AnalyticsGem>> mirrorData = new Dictionary<string, List<AnalyticsGem>>();

	/// <summary>
	/// Method for tracking gem placements in the level.
	/// </summary>
	/// <param name="mirror">Mirror that gem was placed in.</param>
	/// <param name="gemName">Name of gem that was placed - name described the gem value.</param>
	public void AddGem(string mirror, string action, string gemName)
	{
		if (!mirrorData.ContainsKey (mirror)) 
		{
			mirrorData.Add (mirror, new List<AnalyticsGem> ());
		} 

		mirrorData [mirror].Add (new AnalyticsGem(action,gemName));
	}

	/// <summary>
	/// Prints some debug strings.
	/// </summary>
	public void DebugPrint()
	{
        /*foreach (var mirror in mirrorData) 
		{
			Debug.Log (mirror.Key);

			foreach (var gem in mirror.Value) 
			{
				Debug.Log (gem.action + " " + gem.gemName);			
			}	
		}*/

        Debug.Log(ToJson());
	}

    /// <summary>
    /// Converts the gem history data into json form.
    /// </summary>
    /// <returns>The JSON data of gem history</returns>
    public string ToJson()
    {
        return JsonConvert.SerializeObject(mirrorData);
    }
}
