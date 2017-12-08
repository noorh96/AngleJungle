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
    [AddComponentMenu( "UI/Picker/WheelEffect3D", 900 )]
    public class WheelEffect3D : EffectBase
    {
        public RectTransform.Axis layout
        {
            get
            {
                return m_Layout;
            }

            set
            {
                SetProperty( ref m_Layout, value );
            }
        }

        public float polygonSplitLength
        {
            get
            {
                return m_PolygonSplitLength;
            }

            set
            {
                SetProperty( ref m_PolygonSplitLength, value );
            }
        }

        public float radius
        {
            get
            {
                return m_Radius;
            }

            set
            {
                SetProperty( ref m_Radius, value );
            }
        }

        [SerializeField]
        protected RectTransform.Axis m_Layout = RectTransform.Axis.Vertical;
        
        [SerializeField]
        protected float m_PolygonSplitLength = 10f;

        [SerializeField]
		protected float m_Radius = 50f;
		
		static bool Intersect( float d0, float d1, float planeD, out float rate )
		{
			d0 = d0 - planeD;
			d1 = d1 - planeD;
			
			if( d0 * d1 >= 0 || d0 == d1 )
			{
				rate = -1;
				return false;
			}
			
			rate = Mathf.Clamp01(d0 / (d0 - d1));
			return true;
		}

#if !USE_BASE_VERTEX_EFFECT

        public override void ModifyMesh( VertexHelper vh )
        {
            SplitAndCurveMesh( vh );
        }
        
        void SplitAndCurveMesh( VertexHelper vh )
        {
            if( m_PolygonSplitLength <= 0 || !IsActive() )
            {
                return;
            }

            Vector3 normal = Vector3.up;

            if( RectTransform.Axis.Horizontal == m_Layout )
            {
                normal = Vector3.right;
            }
            else
            {
                normal = Vector3.up;
            }

            List<UIVertex> verts = ListPool<UIVertex>.Get();
            vh.GetUIVertexStream( verts );

            List<UIVertex> newVerts = ListPool<UIVertex>.Get();
            newVerts.Clear();

            List<UIVertex> tmpVerts = ListPool<UIVertex>.Get();
            tmpVerts.Clear();

            List<float> pointLocation = ListPool<float>.Get();
            pointLocation.Clear();

            try
            {
                float upper = float.MinValue, lower = float.MaxValue;

                for( int i = 0, count = verts.Count; i < count; ++i )
                {
                    float d = Vector3.Dot( verts[i].position, normal );
                    upper = Mathf.Max( d, upper );
                    lower = Mathf.Min( d, lower );
                }

                float splitInterval = m_PolygonSplitLength;

                for( int triangleIndex = 0, triangleCounts = verts.Count; triangleIndex < triangleCounts; triangleIndex += 3 )
                {
                    UIVertex a = verts[triangleIndex + 0];
                    UIVertex b = verts[triangleIndex + 1];
                    UIVertex c = verts[triangleIndex + 2];
                    
                    float aD = -Vector3.Dot( a.position, normal );
                    float bD = -Vector3.Dot( b.position, normal );
                    float cD = -Vector3.Dot( c.position, normal );
                    
                    pointLocation.Clear();
                    pointLocation.Add( 0f );
                    pointLocation.Add( 1f );
                    pointLocation.Add( 2f );

                    for( float d = lower + splitInterval; d < upper; d += splitInterval )
                    {
                        float rate;
                        if( Intersect( aD, bD, d, out rate ) && (rate % 1f) != 0f )
                            pointLocation.Add( rate );
                        if( Intersect( bD, cD, d, out rate ) && (rate % 1f) != 0f )
                            pointLocation.Add( rate + 1f );
                        if( Intersect( cD, aD, d, out rate ) && (rate % 1f) != 0f )
                            pointLocation.Add( rate + 2f );
                    }

                    if( pointLocation.Count == 3 )
                    {
                        // No points of intersection with a planes.
                        newVerts.Add( a );
                        newVerts.Add( b );
                        newVerts.Add( c );
                        continue;
                    }

                    pointLocation.Sort();
                    int points = pointLocation.Count;
                    
                    tmpVerts.Clear();

                    int target = 0;
                    float minD = float.MaxValue;

                    for( int i = 0; i < points; ++i )
                    {
                        float w = pointLocation[i];
                        int f = Mathf.FloorToInt( w );
                        UIVertex p = new UIVertex();

                        if( f == 0 )
                            p = Leap( a, b, w );
                        else if( f == 1 )
                            p = Leap( b, c, w - 1f );
                        else
                            p = Leap( c, a, w - 2f );

                        float d = Vector3.Dot( normal, p.position );
                        pointLocation[i] = d;
                        tmpVerts.Add( p );

                        if( minD > d )
                        {
                            target = i;
                            minD = d;
                        }
                    }

                    int next = (target + 1) % points;
                    int prev = (target - 1 + points) % points;
                    
                    while( target != prev && target != next )
                    {
                        newVerts.Add( tmpVerts[target] );
                        newVerts.Add( tmpVerts[next] );
                        newVerts.Add( tmpVerts[prev] );

                        float d = pointLocation[target];

                        if( Mathf.Abs( pointLocation[next] - d ) > Mathf.Abs( pointLocation[prev] - d ) * 1.0001f )
                        {
                            target = prev;
                            prev = (target - 1 + points) % points;
                        }
                        else
                        {
                            target = next;
                            next = (target + 1) % points;
                        }
                    }
                }

                //mesh curve
                {
                    float center = (upper + lower) * 0.5f;
                    float radius = m_Radius;
                    float distanceToRad = 1f / radius;
                    int layoutIndex = (m_Layout == RectTransform.Axis.Vertical ? 1 : 0);

                    for( int i = 0, count = newVerts.Count; i < count; ++i )
                    {
                        UIVertex v = newVerts[i];

                        Vector3 position = v.position;
                        float d = Vector3.Dot( position, normal );

                        float rad = (d - center) * distanceToRad;

                        position[layoutIndex] = Mathf.Sin( rad ) * radius;
                        position.z = radius - Mathf.Cos( rad ) * radius;

                        v.position = position;

                        newVerts[i] = v;
                    }
                }

                vh.Clear();
                vh.AddUIVertexTriangleStream( newVerts );
            }
            finally
            {
                ListPool<UIVertex>.Release( verts );
                ListPool<UIVertex>.Release( newVerts );
                ListPool<UIVertex>.Release( tmpVerts );
                ListPool<float>.Release( pointLocation );
            }
        }

#else

		public override void ModifyVertices( List<UIVertex> verts )
		{
			if( m_PolygonSplitLength <= 0 || !IsActive() )
			{
				return;
			}
			
			Vector3 normal = Vector3.up;
			RectTransform.Axis layout = m_Layout;
			
			if( RectTransform.Axis.Horizontal == layout )
			{
				normal = Vector3.right;
			}
			else
			{
				normal = Vector3.up;
			}
						
			List<UIVertex> newVerts = ListPool<UIVertex>.Get();
			newVerts.Clear();
			
			try
			{
				float upper = float.MinValue, lower = float.MaxValue;
				
				for( int i = 0, count = verts.Count; i < count; ++i )
				{
					float d = Vector3.Dot( verts[i].position, normal );
					upper = Mathf.Max( d, upper );
					lower = Mathf.Min( d, lower );
				}

				float splitInterval = m_PolygonSplitLength;
				
				for( int quadIndex = 0, quadCounts = verts.Count; quadIndex < quadCounts; quadIndex += 4 )
				{
					UIVertex a = verts[quadIndex + 0];
					UIVertex b = verts[quadIndex + 1];
					UIVertex c = verts[quadIndex + 2];
					UIVertex d = verts[quadIndex + 3];
					
					float aD = Vector3.Dot( a.position, normal );
					float bD = Vector3.Dot( b.position, normal );
					float cD = Vector3.Dot( c.position, normal );
					float dD = Vector3.Dot( d.position, normal );

					if( Mathf.Abs(aD - bD) < Mathf.Abs(aD) * 0.001f )
					{
						UIVertex tmp = a;
						a = b;
						b = c;
						c = d;
						d = tmp;

						aD = bD;
						bD = cD;
					}

					if( aD > bD )
					{
						UIVertex tmp1 = a;
						UIVertex tmp2 = b;
						a = c;
						b = d;
						c = tmp1;
						d = tmp2;

						aD = cD;
						bD = dD;
					}

					UIVertex a2 = a;
					UIVertex d2 = d;
					
					for( float p = lower + splitInterval; p < upper; p += splitInterval )
					{
						float rate;
						if( Intersect( aD, bD, p, out rate ) && (rate % 1f) != 0f )
						{
							UIVertex na2 = Leap( a, b, rate );
							UIVertex nd2 = Leap( d, c, rate );
							
							newVerts.Add( a2 );
							newVerts.Add( na2 );
							newVerts.Add( nd2 );
							newVerts.Add( d2 );
							
							a2 = na2;
							d2 = nd2;
						}
					}
					
					newVerts.Add( a2 );
					newVerts.Add( b );
					newVerts.Add( c );
					newVerts.Add( d2 );
				}
				
				//mesh curve
				{
					float center = (upper + lower) * 0.5f;
					float radius = m_Radius;
					float distanceToRad = 1f / radius;
					int layoutIndex = (m_Layout == RectTransform.Axis.Vertical ? 1 : 0);
					
					for( int i = 0, count = newVerts.Count; i < count; ++i )
					{
						UIVertex v = newVerts[i];
						
						Vector3 position = v.position;
						float d = Vector3.Dot( position, normal );
						
						float rad = (d - center) * distanceToRad;
						
						position[layoutIndex] = Mathf.Sin( rad ) * radius;
						position.z = radius - Mathf.Cos( rad ) * radius;
						
						v.position = position;
						
						newVerts[i] = v;
					}
				}

				verts.Clear();
				verts.AddRange( newVerts );
			}
			finally
			{
				ListPool<UIVertex>.Release( newVerts );
			}
		}
		
		#endif
		
	}
}

