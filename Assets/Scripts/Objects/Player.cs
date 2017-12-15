/// <summary>
/// Manage animations and sounds of the little character
/// </summary>
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

	/// <summary>
	/// Defined animations for the character
	/// </summary>
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
	}

	/// <summary>
	/// Change animation status when the go button tapped
	/// </summary>
	public void Win(){
		status = ChaStatus.Walk;
		walking_as.Stop ();
		walking_as.Play ();
	}
		
	/// <summary>
	/// Change animation status when the character touch the chest box
	/// </summary>
	public void Get(){
		status = ChaStatus.Get;
		walking_as.Stop ();
	}

	/// <summary>
	/// Change animation status when  the go button apeears
	/// </summary>
	public void FeelHappy(){
		status = ChaStatus.Cel;
		walking_as.Stop ();
	}

}