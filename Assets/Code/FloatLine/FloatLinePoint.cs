using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatLinePoint : MonoBehaviour {

    public GameObject parentObj;

    const float speed = 0.7f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.position + 
            ((parentObj.transform.position + new Vector3(FloatLineManager.pointdis,0,0)) -  transform.position) * speed;
	}
}
