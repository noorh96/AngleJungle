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
    [ExecuteInEditMode]
    public abstract class EffectBase : UIBehaviour
    {
        /// <summary>
        /// EffectComponent is set in Graphics component of childrens.
        /// </summary>
        public void SetEffectComponentToAllChildGraphics()
        {
            SetEffectComponentToAllChildGraphics( transform );
        }

        public void SetEffectComponentToAllChildGraphics( Transform target )
        {
            List<Graphic> graphics = ListPool<Graphic>.Get();

            try
            {
                GetComponentsInChildren<Graphic>( true, graphics );

                for( int i = 0, count = graphics.Count; i < count; ++i )
                {
                    Graphic g = graphics[i];

                    if( g.GetComponent<EffectComponent>() == null )
                    {
                        EffectComponent effect;
#if !UNITY_EDITOR
                        effect = g.gameObject.AddComponent<EffectComponent>();
#else
                        effect = Undo.AddComponent<EffectComponent>( g.gameObject );
#endif
                        effect.SetParent( this );
                        effect.SetDirty( false );
                    }
                }
            }
            finally
            {
                ListPool<Graphic>.Release( graphics );
            }
        }

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

        protected override void OnDestroy()
        {
            SetDirty();
            base.OnDestroy();
        }

        public static UIVertex Leap( UIVertex a, UIVertex b, float t )
        {
            UIVertex v = new UIVertex();
            v.color = Color32.Lerp( a.color, b.color, t );
            v.normal = Vector3.Lerp( a.normal, b.normal, t );
            v.position = Vector3.Lerp( a.position, b.position, t );
            v.tangent = Vector4.Lerp( a.tangent, b.tangent, t );
            v.uv0 = Vector2.Lerp( a.uv0, b.uv0, t );
            v.uv1 = Vector2.Lerp( a.uv1, b.uv1, t );
            return v;
        }

#if USE_BASE_VERTEX_EFFECT
		public abstract void ModifyVertices( List<UIVertex> verts );
#else
        public abstract void ModifyMesh( VertexHelper vh );
#endif

        protected bool SetProperty<T>( ref T currentValue, T newValue )
        {
            if( (currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals( newValue )) )
                return false;

            currentValue = newValue;
            SetDirty();
            return true;
        }

        protected void SetDirty()
        {
            SetDirty( transform );
        }

        protected void SetDirty( Transform target )
        {
            List<EffectComponent> components = ListPool<EffectComponent>.Get();

            target.GetComponents<EffectComponent>( components );
            for( int i = 0, count = components.Count; i < count; ++i )
                components[i].SetDirty();

            ListPool<EffectComponent>.Release( components );

#if USE_BASE_VERTEX_EFFECT

			if( target.GetComponent<LayoutGroup>() != null || target.GetComponent<LayoutElement>() != null )
			{
				LayoutRebuilder.MarkLayoutForRebuild( (RectTransform)target );
            }
#else
			ILayoutElement layoutElement = target.GetComponent<ILayoutElement>();
			
			if( layoutElement != null )
			{
				LayoutRebuilder.MarkLayoutForRebuild( (RectTransform)target );
			}
#endif
                

            foreach( Transform child in target )
            {
                if( child.GetComponent<EffectBase>() != null )
                {
                    //child of a different effect, so skip.
                    continue;
                }

                SetDirty( child );
            }
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            SetDirty();
        }

#endif
    }
}

