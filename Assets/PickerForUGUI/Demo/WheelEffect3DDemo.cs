using UnityEngine;
using System.Collections;

public class WheelEffect3DDemo : MonoBehaviour
{
    public Transform m_PickerTransform = null;

    public Picker.WheelEffect3D m_WheelEffect = null;

    public void SetAngle( float value )
    {
        m_PickerTransform.localRotation = Quaternion.Euler( 0, value, 0 );
    }

    public void SetRadius( float value )
    {
        m_WheelEffect.radius = value;
    }

    public void SetVirtical( bool on )
    {
        if( on )
            m_WheelEffect.layout = RectTransform.Axis.Vertical;
    }

    public void SetHorizontal( bool on )
    {
        if( on )
            m_WheelEffect.layout = RectTransform.Axis.Horizontal;
    }

    public void SetEnable( bool enable )
    {
        m_WheelEffect.enabled = enable;
    }
}
