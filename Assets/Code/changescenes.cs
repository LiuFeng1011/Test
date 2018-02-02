using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changescenes : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("ChangeScene", 0.1f);
	}

    void ChangeScene(){
        Application.LoadLevel("Test");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
