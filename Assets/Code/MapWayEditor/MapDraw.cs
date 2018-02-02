using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapDraw : MonoBehaviour {

	public void OnDrawGizmos()
	{

		#if UNITY_EDITOR
		Gizmos.color = Color.blue;
		Handles.color = Color.blue;


		//获取场景中全部道具
		Object[] objects = Object.FindObjectsOfType(typeof(GameObject));

		Dictionary<string, List<string>> post = new Dictionary<string, List<string>>();

		foreach (GameObject sceneObject in objects){

			if(sceneObject.name == "WayPoint"){
				foreach (Transform child in sceneObject.transform)
				{
					MapWayPoint editor = child.GetComponent<MapWayPoint>();
					if(editor != null){
						for(int i = 0 ; i < editor.pointList.Count ; i ++){
							if(editor.pointList[i] == null){
								continue;
							}
							editor.transform.LookAt(editor.pointList[i].transform);
							DrawLine(editor.transform,editor.pointList[i].transform,Vector3.Distance(editor.transform.position,editor.pointList[i].transform.position));
						}
					}
				}
			}
		}
		#endif
	}
	public void DrawLine(Transform t,Transform t2,float dis){
		#if UNITY_EDITOR
		Handles.ArrowCap(0, t.position + (dis-Mathf.Min(1,dis)) * t.forward, t.rotation, Mathf.Min(1,dis));
		Gizmos.DrawLine(t.position,t2.position);

		//		Gizmos.DrawLine(v1,v2);
		#endif
	}
//	public void DrawLine(Vector3 v1,Vector3 v2){
//		#if UNITY_EDITOR
//		Handles.ArrowCap(0, v1, transform.rotation, transform.localScale.z);
////		Gizmos.DrawLine(v1,v2);
//		#endif
//	}
}
