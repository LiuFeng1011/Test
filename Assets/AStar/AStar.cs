using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    //节点
    private class MapNode{
        //节点状态 
        public enum enNodeState{
            normal,//待机
            open,//开启
            close //关闭
        }

        public MapNode parent = null;//父节点
        public float g, h, f;
        public Vector2 p;//位置
        public enNodeState state = enNodeState.normal;
    }

    ////附近的格子 8方向
    //int[,] nearArray= new int[,]{
    //    {0, 1},   
    //    {1, 1},   
    //    {1, 0},   
    //    {1, -1},  
    //    {0, -1},  
    //    {-1, -1}, 
    //    {-1, 0},  
    //    {-1, 1}   
    //};
    //附近的格子 4方向
    int[,] nearArray = new int[,]{
        {0, 1},
        {1, 0},
        {0, -1},
        {-1, 0},
    };
    Vector2 startPosition, endPosition;//起始点和结束点

    //开放列表，在插入时根据MapNode的f值进行排序，即优先队列
    List<MapNode> openList = new List<MapNode>();

    //所有点
    MapNode[,] mapList;

    //向开放列表中加入节点，这里需要进行排序
    void PushNode(MapNode node)
    {
        node.state = MapNode.enNodeState.open;
        for (int i = 0; i < openList.Count; i++){
            if (openList[i].f > node.f){
                openList.Insert(i,node);
                return;
            }
        }
        openList.Add(node);
    }

    //创建节点时即对节点进行估价
    MapNode CreateNode(Vector2 p,MapNode parent){
        MapNode node = new MapNode();
        node.parent = parent;
        node.p = p;

        //f = g+h
        //g和h直接使用曼哈顿距离
        //--------g------
        if(parent != null){
            node.g = GetNodeG(parent,node);
        }else {
            node.g = 0;
        }

        //--------h------
        node.h =GetNodeH(node);

        //--------f------
        node.f = node.g + node.h;

        //创建的节点加入到节点列表中
        mapList[(int)node.p.x,(int)node.p.y] = node;

        return node;
    }

    float GetNodeG(MapNode parent,MapNode node){
        //曼哈顿距离
        float dis = Mathf.Abs(parent.p.x - node.p.x) + Mathf.Abs(parent.p.y - node.p.y);
        //欧式距离
        //float dis = Vector2.Distance(parent.p, node.p);
        return parent.g + dis;
    }

    float GetNodeH( MapNode node)
    {
        //曼哈顿距离
        return Mathf.Abs(endPosition.x - node.p.x) + Mathf.Abs(endPosition.y - node.p.y);
        //欧式距离
        //return Vector2.Distance(endPosition,node.p);
    }

    //开始
    public List<Vector2> StratAStar(int[,] map,Vector2 startPosition,Vector2 endPosition){
        
        mapList = new MapNode[map.GetLength(0),map.GetLength(1)];

        //附近可移动点的数量
        int nearcount = nearArray.GetLength(0);

        this.startPosition = startPosition;
        this.endPosition = endPosition;

        //起始点加入开启列表
        MapNode startNode = CreateNode(startPosition,null);
        PushNode(startNode);

        MapNode endNode = null;//目标节点

        //开始寻找路径
        while(openList.Count > 0){
            //取出开启列表中f值最低的节点，由于我们在向开启列表中添加节点时已经进行了排序，所以这里直接取第0个值即可
            MapNode node = openList[0];

            //如果node为目标点则结束寻找
            if(Vector2.Distance(node.p,endPosition) <= 0){
                endNode = node;
                break;
            }

            //设置为关闭状态并从开启列表中移除
            node.state = MapNode.enNodeState.close;
            openList.RemoveAt(0);

            //当前点坐标
            //相邻格子加入到开启列表
            for (int i = 0; i < nearcount; i ++){
                Vector2 nearPosition = node.p - new Vector2(nearArray[i,0], nearArray[i,1]);

                //位置是否超出范围
                if((nearPosition.x < 0 || nearPosition.x >= map.GetLength(0)) ||
                   (nearPosition.y < 0 || nearPosition.y >= map.GetLength(1))){
                    continue;
                }
                //该位置是否可以移动
                if(map[(int)nearPosition.x,(int)nearPosition.y] != 0){
                    continue;
                }

                //是否已经创建过这个点
                MapNode nearNode = mapList[(int)nearPosition.x, (int)nearPosition.y];
                if(nearNode != null){
                    //该节点已经创建过

                    //节点是否关闭
                    if(nearNode.state == MapNode.enNodeState.close){
                        continue;
                    }

                    //重新计算g
                    float newg = GetNodeG(node, nearNode);
                    if(newg < nearNode.g){
                        nearNode.parent = node;
                        nearNode.g = newg;
                        nearNode.f = nearNode.g + nearNode.h;

                        //重新对开放列表排序
                        openList.Remove(nearNode);
                    }
                }else{
                    //创建节点
                    nearNode = CreateNode(nearPosition, node);
                }

                PushNode(nearNode);
            }
        }

        //路径数据
        List<Vector2> ret = new List<Vector2>();

        if (endNode == null)
        {
            Debug.Log("no path!");
            return ret;
        }

        //将路径保存到数组中
        while(endNode.parent != null){
            ret.Insert(0,endNode.p);
            endNode = endNode.parent;
        }

        return ret;
    }
}
