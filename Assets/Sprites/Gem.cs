using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{

	public GameObject MirrorGO;
	public int gemAngle = 45;
	public GameObject OnSelectPar;
	public AudioSource as_PickGem;
	public GameObject initialMirror;
	private Vector3 originalPos;
	private bool dragging = false;
	private Transform toDrag;
	private float dist;
	private Vector3 offset;
	private bool collMirror = false;
	public bool onSlot = false;
	private Vector2 slotPosition;
	public Vector3 initialScale;
	public Vector3 originalScale;
	public GameObject gemToBeSwapped;
	private int layerMask;
	// Use this for initialization
	void Start ()
	{
		layerMask = ~ (1 << LayerMask.NameToLayer("PowerGem"));
		initialScale = transform.localScale;
		originalPos = transform.position;
		originalScale = transform.localScale;
		OnSelectPar.SetActive (false);
		PutGemInMirror ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Global.isPaused)
			return;
		Vector3 v3;
		Touch touch;
		Vector3 pos;
		if (Input.touchCount <= 0) {
			dragging = false; 
			Global.isDragging = false;
			OnSelectPar.SetActive (false);
		} else {
			touch = Input.touches [0];
			pos = touch.position;
			if (touch.phase == TouchPhase.Began) {

				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (touch.position), Vector2.zero, Mathf.Infinity, layerMask);
				if (hit && (hit.collider.gameObject == gameObject)) {
//					if (Global.isDragging == false)
//						as_PickGem.Play ();
					
					if (onSlot && MirrorGO != null) {
						onSlot = false;
						MirrorGO.GetComponent<Mirror> ().slots--;
						MirrorGO.GetComponent<Mirror> ().pickerNumber -= gemAngle;
						MirrorGO.GetComponent<Mirror> ().ReleasePosition (gameObject);
						Global.antiCheater = 2;
					}
					toDrag = hit.transform;
					dist = hit.transform.position.z - Camera.main.transform.position.z;
					v3 = new Vector3 (pos.x, pos.y, dist);
					v3 = Camera.main.ScreenToWorldPoint (v3);
					offset = toDrag.position - v3;
					dragging = true;
					Global.isDragging = true;
					OnSelectPar.SetActive (true);
				}
			}
			if (dragging && touch.phase == TouchPhase.Moved) {
				v3 = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, dist);
				v3 = Camera.main.ScreenToWorldPoint (v3);
				toDrag.position = v3 + offset;
			}
			if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)) {
				dragging = false;
				Global.isDragging = false;
				OnSelectPar.SetActive (false);
				if (gemToBeSwapped != null) {
					gemToBeSwapped.GetComponent<Gem>().ReleaseThisGem ();
				}
				if (collMirror && MirrorGO != null) {
					if (MirrorGO.GetComponent<Mirror> ().slots + 1 > MirrorGO.GetComponent<Mirror> ().maximumSlots) {
						onSlot = false;
					} else {
						MirrorGO.GetComponent<Mirror> ().slots++;
						onSlot = true;
						//fetch the position of gem
						slotPosition = MirrorGO.GetComponent<Mirror> ().ArrangePosition (gameObject);
						MirrorGO.GetComponent<Mirror> ().pickerNumber += gemAngle;
						//set anti-cheater conuter to 2
						Global.antiCheater = 2;
					}
				}
			}
		}

		if (onSlot) {
			transform.position = new Vector3 (slotPosition.x, slotPosition.y, transform.position.z);
			transform.localScale = originalScale / 1.6f;
		} else {
			transform.localScale = originalScale;
			if (!dragging) {
				//gem limitation
				//transform.position = Vector3.Lerp (transform.position, originalPos, Time.deltaTime);
				//transform.position = new Vector3(0,0,-1f);
				transform.position = new Vector3(Mathf.Clamp(transform.position.x, -6.24f, 6.24f), Mathf.Clamp(transform.position.y, -3.4f, 5.4f), transform.position.z);
			}
			originalScale = initialScale;
		}

		if (dragging) {
			GetComponent<SpriteRenderer> ().sortingOrder = 11;
		} else {
			GetComponent<SpriteRenderer> ().sortingOrder = 10;
		}
	}

	public void PutGemInMirror(){
		if (initialMirror != null) {
			if (initialMirror.GetComponent<Mirror> ().slots + 1 > initialMirror.GetComponent<Mirror> ().maximumSlots) {
				onSlot = false;
			} else {
				initialMirror.GetComponent<Mirror> ().slots++;
				onSlot = true;
				//fetch the position of gem
				slotPosition = initialMirror.GetComponent<Mirror> ().ArrangePosition (gameObject);
				initialMirror.GetComponent<Mirror> ().pickerNumber += gemAngle;

			}
		}
	}

	public void ReleaseThisGem(){
		if (onSlot && MirrorGO != null) {
			onSlot = false;
			MirrorGO.GetComponent<Mirror> ().slots--;
			MirrorGO.GetComponent<Mirror> ().pickerNumber -= gemAngle;
			MirrorGO.GetComponent<Mirror> ().ReleasePosition (gameObject);
			if (!dragging) {
				transform.position += new Vector3 (0f, -1f, 0f);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "MirrorCollider") {
			collMirror = true;
			MirrorGO = coll.gameObject;
		}
	}

	void OnTriggerStay2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "MirrorCollider") {
			collMirror = true;
			MirrorGO = coll.gameObject;
		}
		if (coll.gameObject.tag == "Draggable" && coll.gameObject.GetComponent<Gem>().onSlot) {
			if (Vector2.Distance (coll.gameObject.transform.position, gameObject.transform.position) < 0.3f) {
				gemToBeSwapped = coll.gameObject;
				gemToBeSwapped.GetComponent<Gem>().originalScale = gemToBeSwapped.GetComponent<Gem>().initialScale * 1.6f;
			}
			if (gemToBeSwapped == coll.gameObject && Vector2.Distance (coll.gameObject.transform.position, gameObject.transform.position) >= 0.3f) {
				gemToBeSwapped.GetComponent<Gem> ().originalScale = gemToBeSwapped.GetComponent<Gem> ().initialScale;
				gemToBeSwapped = null;
			}
		}
	}

	void OnTriggerExit2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "MirrorCollider") {
			collMirror = false;
			MirrorGO = null;
		}
		if (coll.gameObject.tag == "Draggable" && coll.gameObject.GetComponent<Gem>().onSlot) {
			if (gemToBeSwapped = coll.gameObject) {
				gemToBeSwapped.GetComponent<Gem> ().originalScale = gemToBeSwapped.GetComponent<Gem> ().initialScale;
				gemToBeSwapped = null;
			}
		}
	}

}
