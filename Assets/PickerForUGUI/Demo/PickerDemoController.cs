using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PickerDemoController : MonoBehaviour
{
	void Awake()
	{
		GameObject.DontDestroyOnLoad(gameObject);
        LoadScene( "CustomSkinDemo" );
    }

    static void LoadScene( string name )
    {
	#if UNITY_5_2 || UNITY_5_1 || UNITY_5_0 || UNITY_4_7 || UNITY_4_6
		Application.LoadLevel( name );
	#else
		UnityEngine.SceneManagement.SceneManager.LoadScene( name );
	#endif
	}

    void OnGUI()
	{
		float scale = Camera.main.pixelHeight / 480f;

		GUI.matrix = Matrix4x4.Scale(new Vector3(scale,scale,1));

		GUILayout.BeginHorizontal();

        if( GUILayout.Button( "Wheel Effect 3D Demo" ) )
        {
            LoadScene( "WheelEffect3DDemo" );
        }

        if( GUILayout.Button("Parameter Demo") )
		{
            LoadScene( "ParameterDemo");
		}

		if( GUILayout.Button("Custom Skin Demo") )
		{
            LoadScene( "CustomSkinDemo");
		}

		if( GUILayout.Button("Util Demo") )
		{
            LoadScene( "UtilDemo");
		}

		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();

		if( GUILayout.Button("Object Item Demo") )
		{
            LoadScene( "ObjectItemDemo");
		}

		if( GUILayout.Button("Animated Item Demo") )
		{
            LoadScene( "AnimatedItemDemo");
		}

		if( GUILayout.Button("Massive Picker Demo") )
		{
            LoadScene( "MassiveDemo");
		}

		GUILayout.EndHorizontal();
	}
}

