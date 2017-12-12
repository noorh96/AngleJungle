using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCamera : MonoBehaviour {

	public float dragSpeed = 1f;
	// Internal variables for managing touches and drags
	private float scrollVelocity = 0f;
	private float timeTouchPhaseEnded = 0f;

	public Vector2 scrollPosition;

	public float inertiaDuration = 0.75f;
	
	// Update is called once per frame
	void Update () {

		if (Input.touchCount != 1)
		{

			if ( scrollVelocity != 0.0f )
			{
				// slow down over time
				float t = (Time.time - timeTouchPhaseEnded) / inertiaDuration;
				if (scrollPosition.x <= 0 || Camera.main.ScreenToViewportPoint(scrollPosition).x >= 12.66f)
				{
					// bounce back if top or bottom reached
					scrollVelocity = 0;
				}

				float frameVelocity = Mathf.Lerp(scrollVelocity * dragSpeed, 0, t);
				scrollPosition.x -= frameVelocity * Time.deltaTime;

				// after N seconds, we've stopped
				if (t >= 1.0f) scrollVelocity = 0.0f;
			}
			transform.position = Camera.main.ScreenToViewportPoint(scrollPosition);
			return;
		}

		Touch touch = Input.touches[0];
		bool fInsideList = true;//IsTouchInsideList(touch.position);

		if (touch.phase == TouchPhase.Began && fInsideList)
		{
			//selected = TouchToRowIndex(touch.position);
			scrollVelocity = 0.0f;
		}
		else if (touch.phase == TouchPhase.Moved && fInsideList)
		{
			// dragging
			if (!((scrollPosition.x <= 0 && touch.deltaPosition.x > 0) || (Camera.main.ScreenToViewportPoint (scrollPosition).x >= 12.66f && touch.deltaPosition.x < 0))) {
				scrollPosition.x -= touch.deltaPosition.x * dragSpeed;
			}
		}
		else if (touch.phase == TouchPhase.Ended)
		{
				// impart momentum, using last delta as the starting velocity
				// ignore delta < 10; precision issues can cause ultra-high velocity
				if (Mathf.Abs(touch.deltaPosition.x) >= 3) 
					scrollVelocity = (int)(touch.deltaPosition.x / touch.deltaTime);

				timeTouchPhaseEnded = Time.time;
		}
		transform.position = Camera.main.ScreenToViewportPoint(scrollPosition);

		//if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
			//Vector2 touchDeltaPosition = Camera.main.ScreenToViewportPoint(Input.GetTouch (0).deltaPosition);
			//transform.Translate (-touchDeltaPosition.x * dragSpeed, 0, 0);
			//transform.position = new Vector2 (Mathf.Clamp(transform.position.x, 0, 12.66f), 0);
		//}
	}
	void OnGUI(){
//		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
//			Touch touch = Input.GetTouch (0);
//			Vector2 touchDeltaPosition = Camera.main.ScreenToViewportPoint(touch.deltaPosition);
//			transform.position -= new Vector3(touchDeltaPosition.x, 0, 0);
//		}
//		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
//			Touch touch = Input.GetTouch (0);
//			if (Mathf.Abs (touch.deltaPosition.y) >= 10) {
//				
//			}
//		}
	}
}
