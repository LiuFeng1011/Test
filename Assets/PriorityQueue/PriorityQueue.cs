using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{
    private List<Node> nodeList = new List<Node>();

    // 堆节点
    public class Node
    {
        //数据
        public object data { get; set; }

        public int val { get; set; }

        public Node(object data, int val)
        {
            this.data = data;
            this.val = val;
        }
    }

    public PriorityQueue()
    {
        nodeList.Add(null);
    }

    public void Add(object data, int val)
    {
        nodeList.Add(new Node(data, val));

        //up
        int addindex = nodeList.Count - 1;
        while (addindex > 1 && (nodeList[addindex / 2].val > nodeList[addindex].val))
        {
            Node node = nodeList[addindex / 2];
            nodeList[addindex / 2] = nodeList[addindex];
            nodeList[addindex] = node;
            addindex = addindex / 2;
        }
    }

    public object Del()
    {
        if (nodeList.Count <= 1) return null;
        nodeList[1] = nodeList[nodeList.Count - 1];
        nodeList.RemoveAt(nodeList.Count - 1);

    }

    //下沉
    public void Down(int index)
    {
        
    }
}