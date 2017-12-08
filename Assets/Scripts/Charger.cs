using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Charger : MonoBehaviour {

	private bool isChargerActivated = false;
	private Slider slider;
	public float ChargingTime = 1f;
	// Use this for initialization
	void Start () {
		slider = GetComponent<Slider> ();
		isChargerActivated = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isChargerActivated) {
			slider.value = Mathf.Lerp (0f, 1f, Time.deltaTime * ChargingTime);
		} else {
			slider.value = Mathf.Lerp (1f, 0f, Time.deltaTime * ChargingTime);
		}
			
	}

	public void ActivateCharger()
	{
		isChargerActivated = true;
	}

	public void DeactivateCharger(){
		isChargerActivated = false;
	}
}
