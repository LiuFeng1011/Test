using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseCamera : MonoBehaviour {

    Camera mainCamera;  
    public Material m;
    float progress = 0;
    public float distance; 
	// Use this for initialization
	void Start () {
        mainCamera = gameObject.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (progress  >=  1 ) return;
        progress += (Time.deltaTime * 0.5f);
        m.SetFloat("_Progress", progress); 
	}

    void OnRenderImage(RenderTexture src, RenderTexture dest)  
    {  
        Graphics.Blit(src, dest, m);  
    }  
}
