using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public bool isMovingLeft=false;
	public float timeToUnlockCamera=1.4f;
	private Vector3 leftPosition, rightPosition;
	private bool isCameraLocked=true;
    private Vector3 LEFT_EDGE = new Vector3(-9.7f, 1f, -10f);
    private Vector3 RGHT_EDGE = new Vector3(6.8f, 1f, -10f);
    // Use this for initialization
    void Awake()
	{
        Vector3 left = Camera.main.ViewportToWorldPoint(new Vector3(0.0F, 0.0F, Camera.main.nearClipPlane));
        Vector3 right = Camera.main.ViewportToWorldPoint(new Vector3(1.0F, 0.0F, Camera.main.nearClipPlane));
        Vector3 offset = new Vector3((right.x - left.x) / 2, 0, 0);
        rightPosition = RGHT_EDGE - offset;
        leftPosition = LEFT_EDGE + offset;
        MoveToLeftImdtl ();
	}

	void Start () 
	{
		StartCoroutine (CountToUnlockCamera());
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isCameraLocked) 
		{
			if (isMovingLeft) 
			{
				transform.position = Vector3.Lerp (transform.position, leftPosition, Time.deltaTime);
			} 
			else 
			{
				transform.position = Vector3.Lerp (transform.position, rightPosition, Time.deltaTime * 0.6f);
			}
		}
	}

	public void MoveToLeftImdtl()
	{
		transform.position = leftPosition;
	}

	IEnumerator CountToUnlockCamera()
	{
		yield return new WaitForSeconds (timeToUnlockCamera);
		isCameraLocked = false;
	}
}
