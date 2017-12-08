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

public class SetText : MonoBehaviour
{
    public bool m_FlaotToIntText = false;

	void Set( string text )
	{
		Text textComponent = GetComponent<Text>();

		if( textComponent != null )
		{
			textComponent.text = text;
		}
	}

	public void SetGameObjectName( GameObject obj )
	{
		Set(obj.name);
	}

	public void SetFloat( float f )
	{
        if( !m_FlaotToIntText )
        {
            Set( f.ToString( "F2" ) );
        }
        else
        {
            Set( Mathf.RoundToInt(f).ToString() );
        }
		
	}
}

