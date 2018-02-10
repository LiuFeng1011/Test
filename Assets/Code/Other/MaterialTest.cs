using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Material material = new Material(Shader.Find("shadername"));
        material.color = Color.green;
        //material.SetVector("_Color",new Vector4(1,1,1,1));
        gameObject.GetComponent<Renderer>().material = material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
