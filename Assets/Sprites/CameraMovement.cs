using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public bool isMovingLeft=false;
	public float timeToUnlockCamera=1.4f;
	private Vector3 leftPosition;
	private Vector3 rightPosition;
	private bool isCameraLocked=true;


	// Use this for initialization
	void Awake(){
		rightPosition = transform.position;
		leftPosition = rightPosition + new Vector3 (-3f,0,0);
		MoveToLeftImdtl ();
	}
	void Start () {
		StartCoroutine (CountToUnlockCamera());
	}
	
	// Update is called once per frame
	void Update () {
		if (!isCameraLocked) {
			if (isMovingLeft) {
				transform.position = Vector3.Lerp (transform.position, leftPosition, Time.deltaTime);
			} else {
				transform.position = Vector3.Lerp (transform.position, rightPosition, Time.deltaTime * 0.6f);
			}
		}
	}

	public void MoveToLeftImdtl(){
		transform.position = leftPosition;
	}

	IEnumerator CountToUnlockCamera(){
		yield return new WaitForSeconds (timeToUnlockCamera);
		isCameraLocked = false;
	}
}
