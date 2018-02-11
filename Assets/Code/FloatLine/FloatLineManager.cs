using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatLineManager : MonoBehaviour {
    LineRenderer lr;

    List<GameObject> objList = new List<GameObject>();

    //顶点数量
    [SerializeField,Range(1,100)] public int pointcount = 40;
    //顶点距离
    [SerializeField, Range(0.01f, 1f)] public static float pointdis = 0.2f;

    float time = 30;
	// Use this for initialization
	void Start () {
        lr = gameObject.GetComponent<LineRenderer>();

        lr.positionCount = pointcount;
        //生成顶点
        for (int i = 0; i < pointcount; i++)
        {
            GameObject obj = new GameObject(i+"");
            obj.transform.position = new Vector3(transform.position.x + (float)i * pointdis,transform.position.y,0);
            objList.Add(obj);

            FloatLinePoint point = obj.AddComponent<FloatLinePoint>();

            if(i == 0){
                point.parentObj = gameObject;
            }else{
                point.parentObj = objList[i - 1];
            }

        }
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime*10;
        Vector3[] vs = new Vector3[objList.Count];
        for (int i = 0; i < objList.Count; i ++){
            float x = i * Mathf.Sin(time + i) * 0.01f;
            Vector3 newpos = new Vector3(objList[i].transform.position.x,
                                         objList[i].transform.position.y - x,
                                         objList[i].transform.position.z);
            
            vs[i] = newpos;
        }
        lr.SetPositions(vs);
	}
}
