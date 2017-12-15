using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;
using Picker;

class SlotUnit{
	public bool isSlotEmpty;
	public Vector2 position;
	public GameObject targetGO;
	public SlotUnit(bool _isSlotEmpty, Vector2 _position){
		isSlotEmpty = _isSlotEmpty;
		position = _position;
	}
}


public class Mirror : MonoBehaviour
{

	public GameObject sectorImage;
	public int pickerNumber = 0;
	public int maximumSlots=1;
	public int slots=0;
	public AudioSource as_putIn;
	public GameObject line;
	public Text Angle_Text;
	public bool isReceiver = false;

	private Sprite greyMirrorSp;
	private Sprite redMirrorSp;

	public Sprite greyMirror1;
	public Sprite greyMirror2;
	public Sprite greyMirror3;

	public Sprite redMirror1;
	public Sprite redMirror2;
	public Sprite redMirror3;


	public GameObject protractor;
	public AudioSource as_toggle;
	public GameObject toggleCollider;

	public GameObject TutorialHand;
	Vector2 shootingDirection = Vector2.zero;
	Vector2 CentralPoint = Vector2.zero;
	float angle = 0f;
	bool isActivated = false;
	List<SlotUnit> slotList;
	private int countDown=3;
	float showNumber = 0f;
	float degreeNumber = 0f;
	SpriteRenderer sr;
	private bool isProActivated = false;

	private float lerpTime = 16f;
	private float curLerpTime = 0;

	private bool isProtractorOn = false;
	private Vector3 proOriginalScale;
	// Use this for initialization
	void Awake ()
	{
		sr = GetComponent<SpriteRenderer> ();
		if (!isReceiver) {
			isActivated = true;
		} else {
			isActivated = false;
			StartCoroutine (CountDown());
		}


		Vector2 slotBasePosition = (Vector2)transform.position + new Vector2 (0,-0.48f);

		CentralPoint = transform.position;
		slotList = new List<SlotUnit> (maximumSlots);
		switch (maximumSlots) {
		case 1:
			slotList.Clear ();
			slotList.Add (new SlotUnit (true, slotBasePosition));
			greyMirrorSp = greyMirror1;
			redMirrorSp = redMirror1;
			break;
		case 2:
			slotList.Clear ();
			slotList.Add (new SlotUnit(true,slotBasePosition + new Vector2 (-0.30f,0)));
			slotList.Add (new SlotUnit(true,slotBasePosition + new Vector2 (0.30f,0)));
			greyMirrorSp = greyMirror2;
			redMirrorSp = redMirror2;
			break;
		case 3:
			slotList.Clear ();
			slotList.Add (new SlotUnit(true,slotBasePosition + new Vector2 (-0.58f,0)));
			slotList.Add (new SlotUnit(true,slotBasePosition));
			slotList.Add (new SlotUnit(true,slotBasePosition + new Vector2 (0.60f,0)));
			greyMirrorSp = greyMirror3;
			redMirrorSp = redMirror3;
			break;
		}
		if (!isActivated) {
			line.SetActive (false);
			sectorImage.SetActive (false);
			Angle_Text.gameObject.SetActive (false);
			sr.sprite = greyMirrorSp;
		} else {
			sr.sprite = redMirrorSp;
		}
		proOriginalScale = protractor.transform.localScale;
		protractor.transform.localScale = 0 * proOriginalScale;
	}

	// Update is called once per frame
	void Update ()
	{
		//shootingDirection = (Vector2)GyroRotation.transform.up;
		if (Global.isPaused)
			return;

		if (!isActivated) {
			line.SetActive (false);
			sectorImage.SetActive (false);
			Angle_Text.gameObject.SetActive (false);
			sr.sprite = greyMirrorSp;
		} else {
			line.SetActive (true);
			sectorImage.SetActive (true);
			Angle_Text.gameObject.SetActive (true);
			sr.sprite = redMirrorSp;
		}
		Vector2 vect2_tmp = new Vector2 (1, 0); // for text
		Vector2 vect2_angle = new Vector2 (1, 0); // for angle
		curLerpTime += Time.deltaTime;
		float perc = curLerpTime / lerpTime;
		//perc = 1f - Mathf.Cos (perc * Mathf.PI * 0.5f);
		if (perc > 0.14f) {
			perc = 1f;
		}
		degreeNumber = Mathf.Lerp (degreeNumber, pickerNumber, perc);//only time interpolates angles

		vect2_tmp = Quaternion.AngleAxis (showNumber / 2, Vector3.forward) * vect2_tmp;
		sectorImage.GetComponent<TestMesh> ().angleDegree = showNumber;
		angle = angle_360 (vect2_angle, new Vector2 (-1, 0));
		vect2_angle = Quaternion.AngleAxis (degreeNumber, Vector3.forward) * vect2_angle;

		line.transform.rotation = Quaternion.LookRotation (vect2_angle);

		if (Angle_Text) {
			showNumber = normalizeAngle(degreeNumber);
			Angle_Text.text = Mathf.RoundToInt(showNumber).ToString ()+"°";
			Angle_Text.gameObject.transform.localPosition = vect2_tmp * 100f;
		}
		Touch touch;
		if (Input.touchCount > 0) {
			touch = Input.touches [0];
			if (touch.phase == TouchPhase.Began) {
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (touch.position), Vector2.zero);
				if (hit && (hit.collider.gameObject == toggleCollider)) {
					ToggleProtractor ();
				}
			}
		}

		if (isProtractorOn) {
			protractor.transform.localScale = Vector3.Lerp (protractor.transform.localScale, proOriginalScale, Time.deltaTime * 10);
		} else {
			protractor.transform.localScale = Vector3.Lerp (protractor.transform.localScale, proOriginalScale * 0, Time.deltaTime * 10);
		}
	}

	private float normalizeAngle(float angle){
		return (angle %= 360) >= 0 ? angle : (angle + 360);
	}

	float angle_360 (Vector2 from_, Vector2 to_)
	{
		float angle = Vector2.Angle (from_, to_);
		if (angle > 0) {

		} else {

		}
		return angle;
	}

	public void ToggleProtractor(){
		GameObject handClickTutorial = GameObject.FindGameObjectWithTag ("HandClickTutorial");
		if(handClickTutorial != null)
			handClickTutorial.SetActive (false);
		if (isProActivated)
			isProtractorOn = false;
		else
			isProtractorOn = true;
		isProActivated = !isProActivated;
		as_toggle.Play ();
	}

	public Vector2 ArrangePosition(GameObject go){
		if(TutorialHand!=null)
			TutorialHand.SetActive(false);
		foreach (SlotUnit su in slotList) {
			if (su.isSlotEmpty) {
				curLerpTime = 0.0f;
				as_putIn.Play ();
				su.targetGO = go;
				su.isSlotEmpty = false;
				return su.position;
			}
		}
		return Vector2.zero;

	}

	public void ReleasePosition(GameObject go){
		foreach (SlotUnit su in slotList) {
			if (su.targetGO==go) {
				curLerpTime = 0.0f;
				su.targetGO = null;
				su.isSlotEmpty = true;
			}
		}
	}

	public void ActiveLight(){
		isActivated = true;
		countDown = 3;
	}

	public void DeactivateLight(){
		isActivated = false;
	}

	IEnumerator CountDown(){
		while (true) {
			yield return new WaitForSeconds (0.1f);
			countDown--;
			if (countDown <= 0) {
				DeactivateLight ();
			}
		}
	}


}
