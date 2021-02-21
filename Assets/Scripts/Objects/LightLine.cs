using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLine : MonoBehaviour {

	void OnParticleCollision(GameObject other)
	{
		if (other.tag == Global.TAG_POWER_GEM) 
		{
			other.GetComponent<PowerGem> ().ActivateGem();
		}

		if (other.tag == Global.TAG_MIRROR_RECEIVER) 
		{
            //Mirror mirror = other.GetComponent<Mirror>();

            //if(mirror.isReceiver)
            //{
                other.GetComponentInParent<Mirror>().ActiveLight();
            //}
        }

        print("collision!");
        print(other.tag);
	}
}
