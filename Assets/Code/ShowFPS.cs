using UnityEngine;
using System.Collections;

public class ShowFPS : MonoBehaviour
{

    GUIStyle fontStyle = new GUIStyle();

    public float f_UpdateInterval = 0.5F;

    private float f_LastInterval;

    private int i_Frames = 0;

    private float f_Fps;

    Rect _pos = new Rect(0, 600, 200, 200);
    float _outlineWidth = 2;

    void Start()
    {
        f_LastInterval = Time.realtimeSinceStartup;

        i_Frames = 0;
        
        fontStyle.normal.textColor = Color.black;   //设置字体颜色  
        fontStyle.fontSize = 25;       //字体大小  

        _pos = new Rect(10, Screen.height - 50, 200, 200);
    }

    void OnGUI()
    {
        _pos.y -= _outlineWidth;
        _pos.x -= _outlineWidth;
        fontStyle.normal.textColor = Color.cyan;   //设置字体颜色  
        fontStyle.fontSize++;
        GUI.Label(_pos, "FPS:" + f_Fps.ToString("f2") , fontStyle);
        _pos.y += _outlineWidth;
        _pos.x += _outlineWidth;
        fontStyle.normal.textColor = Color.black;   //设置字体颜色  
        fontStyle.fontSize--;
        GUI.Label(_pos, "FPS:" + f_Fps.ToString("f2") , fontStyle);
    }

    void Update()
    {
        ++i_Frames;

        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

            i_Frames = 0;

            f_LastInterval = Time.realtimeSinceStartup;
        }
    }
}
