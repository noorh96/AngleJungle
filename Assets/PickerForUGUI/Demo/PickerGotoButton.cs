using UnityEngine;
using System.Collections;

public class PickerGotoButton : MonoBehaviour 
{
	[SerializeField] int m_Index = 0;
	[SerializeField] Picker.MassivePickerScrollRect		m_ScrollRect = null;

	public void OnClick()
	{
		m_ScrollRect.ScrollAt( m_Index );
	}
}
