using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour {

	private static Singleton instance = null;
	
    public static Singleton Instance
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
