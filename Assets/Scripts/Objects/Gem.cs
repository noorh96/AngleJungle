using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
	// Ints
	private int layerMask;
	private int interfaceType;

	public int gemAngle = 45;

	// Audiosources
	public AudioSource as_PickGem;

	// Game objects
	public GameObject initialMirror;
	public GameObject gemToBeSwapped;
	public GameObject OnSelectPar;
	public GameObject MirrorGO;

	// Vectors
	private Vector2 slotPosition;

	private Vector3 originalPos;
	private Vector3 offset;

	public Vector3 initialScale, originalScale;

	// Bools
	private bool dragging = false;
	private bool collMirror = false;
	private bool isMouseDown = false;

	public bool onSlot = false;

	// Tranforrms
	private Transform toDrag;

	// Floats
	private float dist;

	// Use this for initialization
	void Start ()
	{
		// Checks for touch if no touch is available uses mouse
		if (Input.touchSupported) 
		{
			interfaceType = (int) Global.Interface.Touch;
		} 
		else 
		{
			interfaceType = (int) Global.Interface.Mouse;
		}

		layerMask = ~ (1 << LayerMask.NameToLayer(Global.LAYER_POWER_GEM));
		initialScale = transform.localScale;
		originalPos = transform.position;
		originalScale = transform.localScale;
		OnSelectPar.SetActive (false);
		PutGemInMirror ();
	}

	/// <summary>
	/// Update() is where the dragging system installed and managed.
	/// </summary>
	void Update ()
	{
		//TODO:[what gems they pick up]
		//TODO:[how many taps the player spend]
		if (Global.isPaused)
			return;

		Vector3 v3, pos;

		switch (interfaceType) 
		{
		// Handle touch
		case (int) Global.Interface.Touch:

			Touch touch;

			// No touch happening
			if (Input.touchCount <= 0)
			{
				SetNoInput();
			} 
			// Something is being touched
			else
			{
				touch = Input.touches[0];
				pos = touch.position;

				// Handling case of input touch begins
				if (touch.phase == TouchPhase.Began)
				{
					RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero, Mathf.Infinity, layerMask);

					// Handle collisions between raycast and gameobjects
					if (hit && (hit.collider.gameObject == gameObject))
					{
						//if (Global.isDragging == false)
						//as_PickGem.Play ();

						if (onSlot && MirrorGO != null)
						{
							AnalyticsSingleton.Instance.gemHistory.AddGem (MirrorGO.GetComponent<Mirror> ().name, Global.ANALYTICS_ACTION_REMOVED, gameObject.name);
							onSlot = false;
							MirrorGO.GetComponent<Mirror>().slots--;
							MirrorGO.GetComponent<Mirror>().pickerNumber -= gemAngle;
							MirrorGO.GetComponent<Mirror>().ReleasePosition(gameObject);
							Global.antiCheater = 2;
						}

						toDrag = hit.transform;
						dist = hit.transform.position.z - Camera.main.transform.position.z;
						v3 = new Vector3(pos.x, pos.y, dist);
						v3 = Camera.main.ScreenToWorldPoint(v3);
						offset = toDrag.position - v3;
						dragging = true;
						Global.isDragging = true;
						OnSelectPar.SetActive(true);
					}
				}

				// Handling phase of input touch is moved
				if (dragging && touch.phase == TouchPhase.Moved)
				{
					v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
					v3 = Camera.main.ScreenToWorldPoint(v3);
					toDrag.position = v3 + offset;
				}

				// Handling phase of input touch ended or cancelled
				if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
				{
					SetNoInput();

					// Gem swap function
					if (gemToBeSwapped != null)
					{
						AnalyticsSingleton.Instance.gemHistory.AddGem (MirrorGO.GetComponent<Mirror> ().name, Global.ANALYTICS_ACTION_REMOVED, gemToBeSwapped.GetComponent<Gem> ().name);
						gemToBeSwapped.GetComponent<Gem>().ReleaseThisGem();
					}

					// Handling collisions with mirror
					if (collMirror && MirrorGO != null)
					{
						if (MirrorGO.GetComponent<Mirror>().slots + 1 > MirrorGO.GetComponent<Mirror>().maximumSlots)
						{
							onSlot = false;
						}
						else
						{
							AnalyticsSingleton.Instance.gemHistory.AddGem (MirrorGO.GetComponent<Mirror> ().name, Global.ANALYTICS_ACTION_PLACED, gameObject.name);
							MirrorGO.GetComponent<Mirror>().slots++;
							onSlot = true;
							//fetch the position of gem
							slotPosition = MirrorGO.GetComponent<Mirror>().ArrangePosition(gameObject);
							MirrorGO.GetComponent<Mirror>().pickerNumber += gemAngle;
							//set anti-cheater conuter to 2
							Global.antiCheater = 2;
						}
					}
				}
			}

			break;

			// Handle Mouse
		default:

			// Handling mouse lifted
			if (Input.GetMouseButtonUp(0))
			{
				isMouseDown = false;

				if (dragging)
				{
					// Gem swap function
					if (gemToBeSwapped != null)
					{
						AnalyticsSingleton.Instance.gemHistory.AddGem (MirrorGO.GetComponent<Mirror> ().name, Global.ANALYTICS_ACTION_REMOVED, gemToBeSwapped.GetComponent<Gem> ().name);
						gemToBeSwapped.GetComponent<Gem>().ReleaseThisGem();
					}

					// Handling collisions with mirror
					if (collMirror && MirrorGO != null)
					{
						if (MirrorGO.GetComponent<Mirror>().slots + 1 > MirrorGO.GetComponent<Mirror>().maximumSlots)
						{
							onSlot = false;
						}
						else
						{
							AnalyticsSingleton.Instance.gemHistory.AddGem (MirrorGO.GetComponent<Mirror> ().name, Global.ANALYTICS_ACTION_PLACED, gameObject.name);
							MirrorGO.GetComponent<Mirror>().slots++;
							onSlot = true;
							//fetch the position of gem
							slotPosition = MirrorGO.GetComponent<Mirror>().ArrangePosition(gameObject);
							MirrorGO.GetComponent<Mirror>().pickerNumber += gemAngle;
							//set anti-cheater conuter to 2
							Global.antiCheater = 2;
						}
					}
				}

				SetNoInput();
			} 

			// Handling left click press but not lifted
			if (Input.GetMouseButtonDown(0))
			{
				isMouseDown = true;
				pos = Input.mousePosition; 

				if (isMouseDown)
				{
					RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

					// Handle collisions between raycast and gameobjects
					if (hit && (hit.collider.gameObject == gameObject))
					{
						//if (Global.isDragging == false)
						//as_PickGem.Play ();

						if (onSlot && MirrorGO != null)
						{
							AnalyticsSingleton.Instance.gemHistory.AddGem (MirrorGO.GetComponent<Mirror> ().name, Global.ANALYTICS_ACTION_REMOVED, gameObject.name);
							onSlot = false;
							MirrorGO.GetComponent<Mirror>().slots--;
							MirrorGO.GetComponent<Mirror>().pickerNumber -= gemAngle;
							MirrorGO.GetComponent<Mirror>().ReleasePosition(gameObject);
							Global.antiCheater = 2;
						}

						toDrag = hit.transform;
						dist = hit.transform.position.z - Camera.main.transform.position.z;
						v3 = new Vector3(pos.x, pos.y, dist);
						v3 = Camera.main.ScreenToWorldPoint(v3);
						offset = toDrag.position - v3;
						dragging = true;
						Global.isDragging = true;
						OnSelectPar.SetActive(true);
					}
				}
			}

			// Handling dragging of gem that is pressed
			if (isMouseDown && dragging)
			{
				v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
				v3 = Camera.main.ScreenToWorldPoint(v3);
				toDrag.position = v3 + offset;
			}

			break;
		}

		if (onSlot) 
		{
			transform.position = new Vector3 (slotPosition.x, slotPosition.y, transform.position.z);
			transform.localScale = originalScale / 1.6f;
		} 
		else 
		{
			transform.localScale = originalScale;

			if (!dragging) 
			{
				//gem limitation
				//transform.position = Vector3.Lerp (transform.position, originalPos, Time.deltaTime);
				//transform.position = new Vector3(0,0,-1f);
				transform.position = new Vector3(Mathf.Clamp(transform.position.x, -6.24f, 6.24f), Mathf.Clamp(transform.position.y, -3.4f, 5.4f), transform.position.z);
			}

			originalScale = initialScale;
		}

		// Make the gem viewable above others when it is being dragged
		if (dragging) 
		{
			GetComponent<SpriteRenderer> ().sortingOrder = 11;
		} 
		else 
		{
			GetComponent<SpriteRenderer> ().sortingOrder = 10;
		}
	}

	/// <summary>
	/// Sets the world state to no input.
	/// </summary>
	private void SetNoInput()
	{
		dragging = false; 
		Global.isDragging = false;
		OnSelectPar.SetActive (false);
	}

	/// <summary>
	/// Puts the gem in slot and ask mirror to auto allocate the gem
	/// </summary>
	public void PutGemInMirror()
	{
		if (initialMirror != null) 
		{
			if (initialMirror.GetComponent<Mirror> ().slots + 1 > initialMirror.GetComponent<Mirror> ().maximumSlots) 
			{
				onSlot = false;
			} 
			else 
			{
				initialMirror.GetComponent<Mirror> ().slots++;
				onSlot = true;
				//fetch the position of gem
				slotPosition = initialMirror.GetComponent<Mirror> ().ArrangePosition (gameObject);
				initialMirror.GetComponent<Mirror> ().pickerNumber += gemAngle;
			}
		}
	}

	/// <summary>
	/// gem got released from its current slot
	/// </summary>
	public void ReleaseThisGem()
	{
		if (onSlot && MirrorGO != null) 
		{
			onSlot = false;
			MirrorGO.GetComponent<Mirror> ().slots--;
			MirrorGO.GetComponent<Mirror> ().pickerNumber -= gemAngle;
			MirrorGO.GetComponent<Mirror> ().ReleasePosition (gameObject);

			if (!dragging) 
			{
				transform.position += new Vector3 (0f, -1f, 0f);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		// behavior of entering mirror
		if (coll.gameObject.tag == Global.TAG_MIRROR_COLLIDER) 
		{
			collMirror = true;
			MirrorGO = coll.gameObject;
		}
	}

	void OnTriggerStay2D (Collider2D coll)
	{
		// behavior of hovering on a mirror
		if (coll.gameObject.tag == Global.TAG_MIRROR_COLLIDER) 
		{
			collMirror = true;
			MirrorGO = coll.gameObject;
		}
		// behavior of hovering on other gems
		if (coll.gameObject.tag == Global.TAG_DRAGGABLE && coll.gameObject.GetComponent<Gem>().onSlot) 
		{
			if (Vector2.Distance (coll.gameObject.transform.position, gameObject.transform.position) < 0.3f) 
			{
				gemToBeSwapped = coll.gameObject;
				gemToBeSwapped.GetComponent<Gem>().originalScale = gemToBeSwapped.GetComponent<Gem>().initialScale * 1.6f;
			}

			if (gemToBeSwapped == coll.gameObject && Vector2.Distance (coll.gameObject.transform.position, gameObject.transform.position) >= 0.3f) 
			{
				gemToBeSwapped.GetComponent<Gem> ().originalScale = gemToBeSwapped.GetComponent<Gem> ().initialScale;
				gemToBeSwapped = null;
			}
		}
	}

	void OnTriggerExit2D (Collider2D coll)
	{
		// behavior of exiting from mirror
		if (coll.gameObject.tag == Global.TAG_MIRROR_COLLIDER)
		{
			collMirror = false;
			MirrorGO = null;
		}
		// behavior of exiting from other gems
		if (coll.gameObject.tag == Global.TAG_DRAGGABLE && coll.gameObject.GetComponent<Gem>().onSlot) 
		{
			if (gemToBeSwapped = coll.gameObject) 
			{
				gemToBeSwapped.GetComponent<Gem> ().originalScale = gemToBeSwapped.GetComponent<Gem> ().initialScale;
				gemToBeSwapped = null;
			}
		}
	}
}