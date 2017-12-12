using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


	//Moving Target Position
	public Transform targetPosition;
	private bool isWalking=false;
	private AudioSource walking_as;
	public Transform SwitchPoint;
	private float WalkingSpeed = 0.02f;
	Animator animator;
	bool foundTreasure = false;
	ChaStatus status = ChaStatus.Idle;
	float perc = 0f;
	// Use this for initialization
	void Start () {
		walking_as = GetComponent<AudioSource> ();
		walking_as.Stop ();
		animator = GetComponent<Animator> ();
	}
	enum ChaStatus{
		Idle, Cel, Walk, Get
	};
	// Update is called once per frame
	void Update () {
		if (status == ChaStatus.Walk) {
			animator.SetBool ("Walk", true);
			animator.SetBool ("Idle", false);
			animator.SetBool ("Cel", false);
			perc += Time.deltaTime;
			transform.position = Vector3.Lerp (transform.position, targetPosition.position, perc * WalkingSpeed);
		} else if (status == ChaStatus.Cel) {
			animator.SetBool ("Idle", false);
			animator.SetBool ("Walk", false);
			animator.SetBool ("Cel", true);
		} else if (status == ChaStatus.Get) {
			animator.SetBool ("Idle", false);
			animator.SetBool ("Walk", false);
			animator.SetBool ("Cel", true);
		} else {
			animator.SetBool ("Idle", true);
			animator.SetBool ("Walk", false);
			animator.SetBool ("Cel", false);
		}

		if (transform.position.x < SwitchPoint.position.x) {
			Camera.main.gameObject.GetComponent<CameraMovement> ().isMovingLeft = true;
		} else {
			Camera.main.gameObject.GetComponent<CameraMovement> ().isMovingLeft = false;
		}


//		if (false) {
//			
//			if (Input.touchCount > 0 && (Input.GetTouch (0).phase == TouchPhase.Began || Input.GetTouch (0).phase == TouchPhase.Moved)) {
//				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position), Vector2.zero);
//				//Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
//
//				if (hit)
//				if (hit.collider != null && !Global.isDragging && hit.collider.tag == "Floor") {
//					targetPosition = hit.point;
//					isWalking = true;
//				}
//
//			}
//
//			if (isWalking) {
//				transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime);
//				if (((Vector2)transform.position - targetPosition).magnitude < 0.1f) {
//					isWalking = false;
//				}
//			}
//
//			if (transform.position.x < SwitchPoint.position.x) {
//				Camera.main.gameObject.GetComponent<CameraMovement> ().isMovingLeft = true;
//			} else {
//				Camera.main.gameObject.GetComponent<CameraMovement> ().isMovingLeft = false;
//			}
//		}
	}

	public void Win(){
		status = ChaStatus.Walk;
		walking_as.Stop ();
		walking_as.Play ();
	}
		

	public void Get(){
		status = ChaStatus.Get;
		walking_as.Stop ();
	}

	public void FeelHappy(){
		status = ChaStatus.Cel;
		walking_as.Stop ();
	}

}