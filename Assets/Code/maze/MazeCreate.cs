using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 迷宫生成类
/// 迷宫生成思路：
/// 规则:二维数组中奇数行列视为围墙，偶数为路径
/// 从起始点开始，随机从上下左右四个位置寻找周围没有被找到过的位置，找到后此点标记为1，并把此点与前一点之间的位置设置为1
/// 如果全部位置已经找到过，则退回到上一个点重复次逻辑，直到所有点都记录到或 退回到起始点且没有可用点
/// </summary>
public class MazeCreate : MonoBehaviour
{

    public enum PointType{
        wall = 0,//墙
        way = 1,//路
        startpoint = 2,//起始点
        endpoint = 3,//结束点
        nullpoint = 4,//空位置，不进行任何操作
    }

    //行数
    public int row{ get; private set; }
    //列数
    public int col{ get; private set; }
    //全部点数量
    int maxcount;

    private MazeCreate(int row, int col){
        this.row = row;
        this.col = col;
        Start();
    }

    public static MazeCreate GetMaze(int row, int col)
    {
        MazeCreate maze = new MazeCreate(row,col);
        return maze;
    }
    
    /// <summary>
    /// 迷宫生成过程中，记录已经找到的点
    /// </summary>
    public List<int> findList = new List<int>();

    //迷宫数据
    public List<List<int>> mapList = new List<List<int>>();

    void Start(){

        maxcount = row * col;

        //初始化数组
        for (int i = 0; i < row; i++)
        {
            mapList.Add(new List<int>());
            for (int j = 0; j < col; j++)
            {
                mapList[i].Add((int)PointType.wall);
            }
        }

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j ++){
                mapList[row / 2 - 2 + i][col / 2 - 2 + j] = (int)PointType.nullpoint;
            }
        }


        //起始点
        int _row = Random.Range(1, row - 1);
        int _col = Random.Range(1, col - 1);
        if (_row % 2 == 0) {  _row += 1; }
        if (_col % 2 == 0) { _col += 1; }
 
        mapList[_row][_col] = (int)PointType.startpoint;

        int nowindex = _row * col + _col;
        findList.Add(nowindex);

        //递归生成路径
        FindPoint(nowindex);
    }

    void FindPoint(int nowindex){
        if(findList.Count >= maxcount){
            return;
        }

        List<int> nearpoint = new List<int>();
        FindNearPoint(nearpoint,nowindex);
        while (nearpoint.Count > 0)
        {
            int rand = Random.Range(0, nearpoint.Count);

            //中间的格子
            int midindex = nowindex + (nearpoint[rand] - nowindex) / 2;
            SetPoint(midindex);

            //新的格子
            int newindex = nearpoint[rand];
            SetPoint(newindex);
            nearpoint.RemoveAt(rand);
            //递归
            FindPoint(newindex);

            FindNearPoint(nearpoint, nowindex);
        } 
    }

    //寻找附近可用的点
    void FindNearPoint(List<int> nearpoint,int index){
        nearpoint.Clear();
        int _row = index / col;
        int _col = index % col;
        //up
        if (_row >= 2)
        {
            AddNearPoint(nearpoint, (_row - 2) * col + _col);
        }
        //down
        if (_row < row - 2)
        {
            AddNearPoint(nearpoint, (_row + 2) * col + _col);
        }
        //left
        if (_col >= 2)
        {
            AddNearPoint(nearpoint, _row * col + _col - 2);
        }
        //up
        if (_col < col - 2)
        {
            AddNearPoint(nearpoint, _row * col + _col + 2);
        }

    }

    //设置路径
    void SetPoint(int index)
    {
        int _row = index / col;
        int _col = index % col;
        mapList[_row][_col] = (int)PointType.way;

        findList.Add(index);
    }

    //附近的点是否满足寻找条件
    void AddNearPoint(List<int> nearpoint, int p)
    {
        int _row = p / col;
        int _col = p % col;

        if (p >= 0 && p < maxcount && mapList[_row][_col] == (int)PointType.wall)
        {
            nearpoint.Add(p);
        }
    }


}
