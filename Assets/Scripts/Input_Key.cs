using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Key : MonoBehaviour {

    public ParticleSystem my_PS;
	// Use this for initialization
	void Awake () {
        if (GetComponent<ParticleSystem>() != null)
        {
            my_PS = GetComponent<ParticleSystem>();
        }
        else
        {
            Debug.LogError("这个脚本是挂在粒子物体身上的~");
        }
        
        my_PS.Stop();//初始化停止粒子
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            my_PS.Play();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            my_PS.Stop();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("发生了粒子碰撞，碰撞到的物体为："+other.name);
    }
}
