using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSingleton : MonoBehaviour {

	private static SoundSingleton instance = null;
	
    public static SoundSingleton Instance
    {
		get { return instance; }
	}
	// Use this for initialization
	void Awake () 
    {
		if (instance != null && instance != this) 
        {
			Destroy (this.gameObject);
			return;
		} 
        else 
        {
			instance = this;
		}

		DontDestroyOnLoad (this.gameObject);
	}
}
