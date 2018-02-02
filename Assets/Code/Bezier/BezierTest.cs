using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BezierTest : MonoBehaviour {

    float step = 20;
    float warp = 0.5f;

    public List<GameObject> obj;

    public List<Vector3> poslist = new List<Vector3>();

	// Use this for initialization
	void Start () {

        for (int i = 0; i < obj.Count; i ++){
            Vector3 v1 = obj[i].transform.position;

            int lastindex = Mathf.Min(i + 1, obj.Count - 1);
            Vector3 v4 = obj[lastindex].transform.position;

            Vector3 v2 = new Vector3(v1.x + (v4.x - v1.x) * 0.5f,v1.y,0);

            Vector3 v3 = new Vector3(v4.x - (v4.x - v1.x) * 0.5f, v4.y, 0);

            List<Vector3> vlist = new List<Vector3>();
            vlist.Add(v1);
            vlist.Add(v2);
            vlist.Add(v3);
            vlist.Add(v4);
            CreateLinePoint(vlist);
        }

	}

    void CreateLinePoint(List<Vector3> vlist){
        float rate = 0;

        while (rate < 1)
        {
            rate += 1f / step;

            float x = GetBezierat(
                vlist[0].x,
                vlist[1].x,
                vlist[2].x,
                vlist[3].x, rate);
            float y = GetBezierat(
                vlist[0].y,
                vlist[1].y,
                vlist[2].y,
                vlist[3].y, rate);

            poslist.Add(new Vector3(x, y, 0));
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static float GetBezierat(float a, float b, float c, float d, float t)
    {
        return (Mathf.Pow(1 - t, 3) * a +
                3 * t * (Mathf.Pow(1 - t, 2)) * b +
                3 * Mathf.Pow(t, 2) * (1 - t) * c +
                Mathf.Pow(t, 3) * d);
    }

    public void OnDrawGizmos()
    {

        #if UNITY_EDITOR
        Gizmos.color = Color.blue;
        Handles.color = Color.blue;

        for (int i = 0; i < poslist.Count; i ++){
            if (i == poslist.Count -1) continue;
            Gizmos.DrawLine(poslist[i], poslist[i+1]);
        }
        #endif
    }



}
