using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode
{
    //节点状态 
    public enum enNodeState
    {
        normal,//待机
        open,//开启
        close //关闭
    }

    public MapNode parent = null;//父节点
    public float g, h, f;
    public Vector2 p;//位置
    public enNodeState state = enNodeState.normal;
}

public class PriorityQueueTest : MonoBehaviour {

	// Use this for initialization
	void Start () {

        PriorityQueue queue = new PriorityQueue();
        string logString = "";
        for (int i = 0; i < 10; i ++){
            MapNode node = new MapNode();
            node.f = Random.Range(0, 100);
            queue.Push(node, (int)(node.f));
            logString += "/" + node.f;
        }
        Debug.Log("push : " + logString);


        logString = "";
        object data = queue.Out();
        while( data != null){
            MapNode node = (MapNode)data;
            logString += "/" + node.f;
            data = queue.Out();
        }
        Debug.Log("out : " + logString);

	}
	
}
