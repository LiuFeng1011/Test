using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointFloat : MonoBehaviour {

    private float startY;
    public float speed = 1;
    public float distance = 0;
	// Use this for initialization
	void Start () {
        startY = transform.position.y;
        //distance = Random.Range(0, 3.1415926f / 2f);
        //speed = Random.Range(1, 3);
	}
	
	// Update is called once per frame
	void Update () {
        distance += Time.deltaTime * speed;

        transform.position = new Vector3(transform.position.x,
                                         startY + Mathf.Sin(distance)*2,
                                         transform.position.z);
	}
}
