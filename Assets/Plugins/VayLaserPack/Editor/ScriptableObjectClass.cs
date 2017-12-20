using UnityEngine;
using UnityEditor;

public class ScriptableObjectClass
{
    [MenuItem("Assets/Create/Laser Particle ")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<LaserScriptableObject>();
    }
}