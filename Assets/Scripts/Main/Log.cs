using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Log : MonoBehaviour {
	private string filename;
	private string filePath;
	private string timeStamp;
	private string logText;

	public Log()
	{
		filename = "";
		filePath =  Application.persistentDataPath + "/log.txt";
		timeStamp = "";
		logText = "";
	}
	public void WriteToFile(string text)
	{
		timeStamp = "[" +System.DateTime.Now.ToString("hh:mm:ss")+"] ";
		logText = timeStamp + text + "\n";
		try
		{
			if (!File.Exists(filePath))
			{
				Debug.Log("New file created");
				Debug.Log("Preparing to write to file");
				File.WriteAllText(filePath, logText);
				// File.WriteAllLines(filePath, textList);
			}
			else
			{
				Debug.Log("Existing log file found, appending to file");
				File.AppendAllText(filePath, logText);
			}
		}
		catch (System.Exception e)
			{
				Debug.Log(e);
			}
	}
	public void LoadFile()
    {
		Debug.Log("Reading");
		string[] levelsInfo = File.ReadAllLines(filePath);
    }
}
