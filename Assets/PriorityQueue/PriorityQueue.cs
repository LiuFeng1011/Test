using System.Collections.Generic;

public class PriorityQueue
{
    private List<Node> nodeList = new List<Node>();

    // 堆节点
    private class Node
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

    //获取队列数据数量
    public int GetCount()
    {
        return nodeList.Count - 1;
    }

    /// <summary>
    /// 添加数据
    /// </summary>
    /// <returns>The push.</returns>
    /// <param name="data">数据实体.</param>
    /// <param name="val">堆中根据此值来对数据进行比较.</param>
    public void Push(object data, int val)
    {
        nodeList.Add(new Node(data, val));

        //up
        Up(nodeList.Count - 1);
    }

    //取出数据
    public object Out()
    {
        if (nodeList.Count <= 1) return null;
        Node node = nodeList[1];
        nodeList[1] = nodeList[nodeList.Count - 1];
        nodeList.RemoveAt(nodeList.Count - 1);
        Down(1);
        return node.data;
    }

    //上浮
    private void Up(int addindex){
        //父节点是否存在，并进行判断是否需要上浮
        if (addindex > 1 && (nodeList[addindex / 2].val > nodeList[addindex].val))
        {
            Node node = nodeList[addindex / 2];
            nodeList[addindex / 2] = nodeList[addindex];
            nodeList[addindex] = node;
            //递归 继续上浮
            Up(addindex / 2);
        }
    }

    //下沉
    private void Down(int index)
    {
        int targetIndex = 0;
        //左孩子是否存在
        if (index * 2 < nodeList.Count)
        {
            targetIndex = index * 2;
        }
        else
        {
            return;
        }
        //右孩子是否存在，如果存在与左孩子进行比较，去较小的一个
        if (targetIndex + 1 < nodeList.Count &&
           nodeList[targetIndex].val > nodeList[targetIndex + 1].val)
        {
            targetIndex += 1;
        }

        //与孩子进行比较
        if (nodeList[index].val < nodeList[targetIndex].val)
        {
            return;
        }

        //下沉
        Node node = nodeList[index];
        nodeList[index] = nodeList[targetIndex];
        nodeList[targetIndex] = node;

        //递归 继续下沉
        Down(targetIndex);
    }

}