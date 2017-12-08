using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Picker
{
    [CustomEditor( typeof( WheelEffect3D ), true ), CanEditMultipleObjects]
    public class WheelEffect3DEditor : Editor
    {
        protected SerializedProperty m_Layout;
        protected SerializedProperty m_PolygonSplitLength;
        protected SerializedProperty m_Radius;

        protected virtual void OnEnable()
        {
            m_Layout = serializedObject.FindProperty( "m_Layout" );
            m_PolygonSplitLength = serializedObject.FindProperty( "m_PolygonSplitLength" );
            m_Radius = serializedObject.FindProperty( "m_Radius" );
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField( m_Layout );
            EditorGUILayout.PropertyField( m_PolygonSplitLength );
            EditorGUILayout.PropertyField( m_Radius );
            EditorGUI.EndChangeCheck();
            serializedObject.ApplyModifiedProperties();


            EditorGUILayout.Separator();
            EditorGUILayout.HelpBox( "Children's Graphic Component needs attachment of EffectComponent.\nWhen EffectComponent isn't attached, it doesn't curve.", MessageType.Info );

            if( GUILayout.Button( "EffectComponent is added to all children's Graphic Component." ) )
            {
                foreach( Object target in targets )
                {
                    WheelEffect3D effect = target as WheelEffect3D;

                    if( effect != null )
                    {
                        effect.SetEffectComponentToAllChildGraphics();
                    }
                }
            }   
        }
    }
}
