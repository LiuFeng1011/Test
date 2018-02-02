using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode] 
public class MainCamera : MonoBehaviour {

    public Material m;

    Camera mainCamera;

    public Color newColor;
    public float distance;


	// Use this for initialization
	void Start () {
        m.SetColor("_Color", newColor);
        m.SetFloat("_ChangeDis", distance);

        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, m);
    }
}
