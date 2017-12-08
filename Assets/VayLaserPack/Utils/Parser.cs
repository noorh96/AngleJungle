using UnityEngine;
using System.Collections;

public class Parser {

/// <summary>
/// Returns a file name from file path
/// </summary>
/// <param name="data"></param>
/// <returns></returns>
   public static string GetFileName(string data)
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
}
