using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsGemHistory {

	public Dictionary<string, List<AnalyticsGem>> actionData = new Dictionary<string, List<AnalyticsGem>>();
    
    /// <summary>
    /// Adds an action to the action history data structure. MADE FOR ONLY ACTIONS CURRENTLY!
    /// </summary>
    /// <param name="selection">What object was selected.</param>
    /// <param name="action">What action was performed.</param>
    /// <param name="input">What object was input.</param>
    /// <param name="actionTime">When the action occur.</param>
    public void AddAction(string selection, string action, string input, float actionTime)
    {
        if (!actionData.ContainsKey(Global.ANALYTICS_ACTIONS))
        {
            actionData.Add(Global.ANALYTICS_ACTIONS, new List<AnalyticsGem>());
        }
        
        actionData[Global.ANALYTICS_ACTIONS].Add(new AnalyticsGem(selection, action, input, actionTime));
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
        return JsonConvert.SerializeObject(actionData);
    }
}
