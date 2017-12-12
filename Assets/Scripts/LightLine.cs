using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLine : MonoBehaviour {
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}
		

	void OnParticleCollision(GameObject other)
	{
		if (other.tag == "PowerGem") {
			other.GetComponent<PowerGem> ().ActivateGem();
		}
		if (other.tag == "MirrorReceiver") {
			other.GetComponentInParent<Mirror> ().ActiveLight();
		}
	}
}
