using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class MapEditor : MonoBehaviour {

    public Color sideColor = Color.white;
    public Color insideColor = Color.black;

	public float width = 10.0f;
	public float height = 1.0f;

	public int mapHeight = 10;
	public int mapWidth = 100;

	//public float autoDPos = 0.1f;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnDrawGizmos()
	{
        DrawBGLine();
	}

    public void DrawBGLine(){
#if UNITY_EDITOR
        Gizmos.color = insideColor;
		
        for (float y = 0; y < mapHeight; y += height)
		{
			Gizmos.DrawLine(new Vector3(0, y , -1f),
			                new Vector3(mapWidth, y , -1f));
			
            Handles.Label(new Vector3(-1f,y+0.5f , 0f), "" + y);
		}
		
		for (float x = 0; x < mapWidth; x += width)
		{
            Gizmos.DrawLine(new Vector3(x,0, -1f),
                            new Vector3(x,mapHeight, -1f));
			
            Handles.Label(new Vector3(x,-0.2f, 0f), "" + x);
        }

        Gizmos.color = sideColor;
        
        Gizmos.DrawLine(new Vector3(0, 0 , -1f),new Vector3(mapWidth, 0 , -1f));
        Gizmos.DrawLine(new Vector3(mapWidth, 0 , -1f),new Vector3(mapWidth, mapHeight, -1f));
        Gizmos.DrawLine(new Vector3(mapWidth, mapHeight , -1f),new Vector3(0, mapHeight , -1f));
        Gizmos.DrawLine(new Vector3(0, mapHeight  , -1f),new Vector3(0, 0 , -1f));

#endif
	}

}
