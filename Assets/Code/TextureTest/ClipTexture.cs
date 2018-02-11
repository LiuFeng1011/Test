using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipTexture : MonoBehaviour
{
    [SerializeField,Range(0,1)]
    public float camber = 0.5f;
    private float lastCamber = 0.5f;


    [SerializeField, Range(0, 10)]
    public float radius = 5f;
    private float lastradius = 5f;

    SpriteRenderer sr;
    // Use this for initialization
    void Start()
    {
        GameObject obj = new GameObject("luna");

        sr = obj.AddComponent<SpriteRenderer>();

        Clip("luna");
    }

    // Update is called once per frame
    void Update()
    {
        if(lastCamber != camber){
            lastCamber = camber;
            Clip("luna");
        }
        if (lastradius != radius)
        {
            lastradius = radius;
            Clip("luna");
        }
    }

    public void Clip(string filepath){
        Texture2D tex = (Texture2D)Resources.Load(filepath) as Texture2D;  

        for (int w = 0; w < tex.width; w++)
        {
            for (int h = 0; h < tex.height; h++)
            {
                float wrate = (float)w / (float)tex.width - 0.5f;
                float hrate = (float)h / (float)tex.height - 0.5f;


                Color source = tex.GetPixel(w, h);

                float val = Mathf.Pow(Mathf.Abs(wrate / camber), radius) +
                                 Mathf.Pow(Mathf.Abs(hrate / camber), radius);
                
                source.a = 1- Mathf.Floor(Mathf.Clamp(val, 0, 1)) ;

                tex.SetPixel(w,h,source);
            }
        }

        tex.Apply();

        Sprite pic = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        sr.sprite = pic;
    }
}
