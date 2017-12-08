#if UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1
#define USE_BASE_VERTEX_EFFECT
#endif

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

namespace Picker
{
#if USE_BASE_VERTEX_EFFECT
	[RequireComponent(typeof(RectTransform))]
	public class EffectComponent : BaseVertexEffect
#else
	[RequireComponent(typeof(RectTransform))]
	public class EffectComponent : BaseMeshEffect
#endif
    {
        public void SetParent( EffectBase parent )
        {
            m_EffectParent = parent;
        }

        EffectBase m_EffectParent = null;

        protected override void OnTransformParentChanged()
        {
            base.OnTransformParentChanged();
            SetParent( GetComponentInParent<EffectBase>() );
        }

#if USE_BASE_VERTEX_EFFECT

		public override void ModifyVertices(List<UIVertex> verts)
		{
			if( !IsActive() )
				return;
			
			if( m_EffectParent != null )
			{
				m_EffectParent.ModifyVertices( verts );
			}
		}

#else

        public override void ModifyMesh( VertexHelper vh )
        {
            if( !IsActive() )
                return;
            
            if( m_EffectParent != null )
            {
                m_EffectParent.ModifyMesh( vh );
            }
        }

#endif

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }

        protected override void OnDisable()
        {
            SetDirty();
            base.OnDisable();
        }

        public void SetDirty( bool updateParent = true )
        {
            if( updateParent )
            {
                SetParent( GetComponentInParent<EffectBase>() );
            }

            this.graphic.SetVerticesDirty();
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            SetDirty();
        }

#endif
    }
}


