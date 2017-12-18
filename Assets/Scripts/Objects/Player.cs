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
	void Start () 
    {
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
	void Update () 
    {
		if (status == ChaStatus.Walk) 
        {
			animator.SetBool (Global.ANIMATION_WALK, true);
            animator.SetBool (Global.ANIMATION_IDLE, false);
			animator.SetBool (Global.ANIMATION_CELEBRATE, false);
			perc += Time.deltaTime;
			transform.position = Vector3.Lerp (transform.position, targetPosition.position, perc * WalkingSpeed);
		} 
        else if (status == ChaStatus.Cel) 
        {
            animator.SetBool (Global.ANIMATION_IDLE, false);
            animator.SetBool (Global.ANIMATION_WALK, false);
            animator.SetBool (Global.ANIMATION_CELEBRATE, true);
		} 
        else if (status == ChaStatus.Get) 
        {
            animator.SetBool (Global.ANIMATION_IDLE, false);
            animator.SetBool (Global.ANIMATION_WALK, false);
            animator.SetBool (Global.ANIMATION_CELEBRATE, true);
		} 
        else 
        {
            animator.SetBool (Global.ANIMATION_IDLE, true);
            animator.SetBool (Global.ANIMATION_WALK, false);
            animator.SetBool (Global.ANIMATION_CELEBRATE, false);
		}

		if (transform.position.x < SwitchPoint.position.x) 
        {
			Camera.main.gameObject.GetComponent<CameraMovement> ().isMovingLeft = true;
		}
        else 
        {
			Camera.main.gameObject.GetComponent<CameraMovement> ().isMovingLeft = false;
		}
	}

	/// <summary>
	/// Change animation status when the go button tapped
	/// </summary>
	public void Win()
    {
		status = ChaStatus.Walk;
		walking_as.Stop ();
		walking_as.Play ();
	}
		
	/// <summary>
	/// Change animation status when the character touch the chest box
	/// </summary>
	public void Get()
    {
		status = ChaStatus.Get;
		walking_as.Stop ();
	}

	/// <summary>
	/// Change animation status when  the go button apeears
	/// </summary>
	public void FeelHappy()
    {
		status = ChaStatus.Cel;
		walking_as.Stop ();
	}

}