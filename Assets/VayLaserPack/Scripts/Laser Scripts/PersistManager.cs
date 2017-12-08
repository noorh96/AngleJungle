using UnityEngine;
using System.IO;

/// <summary>
/// This class is responsible for comparing gameobject and hash code to determine if they are the same object
/// </summary>
public class PersistManager : MonoBehaviour {

    /// <summary>
    /// Called on start up to look for the saved files
    /// </summary>
    void OnEnable()
    {

        foreach (string file in System.IO.Directory.GetFiles(Application.dataPath + "/VayLaserPack/Resources/ScriptableObjects/"))
        {
            if (!file.Contains(".meta"))
            {
                string[] data = file.Split(',');
                string ObjectName = Parse(data[0]);
                string ObjectHash = ParseExtension(data[1]);
                GameObject targetObject = GameObject.Find(ObjectName);
                if (targetObject == null)
                {
                    File.Delete(file);
                    File.Delete(file + ".meta");
                }
                else
                {
                    if (targetObject.GetHashCode() != int.Parse(ObjectHash))
                    {
                        File.Delete(file);
                        File.Delete(file + ".meta");

                    }
                }

            }
        }

    }

    /// <summary>
    /// Used for parsing acutal file names
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    string Parse(string data)
    {
        string result = "";
        for (int i = data.Length - 1; i >= 0; i--)
        {
            if (data[i] == '/')
            {
                for (int ii = i + 1; ii < data.Length; ii++)
                {

                    result += data[ii];
                }
                return result;

            }
        }

        return "";
    }


    /// <summary>
    /// Used for parsing the extension of file names
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    string ParseExtension(string data)
    {
        string result = "";
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] != '.')
            {
                result += data[i];
            }
            else
            {
                break;
            }
        }
        return result;
    }
}
