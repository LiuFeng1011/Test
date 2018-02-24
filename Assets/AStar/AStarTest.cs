using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTest : MonoBehaviour {


    int[,] map = {
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,1,1,1,1,1,1,1,1,1,1},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,1,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,1,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,1,0,0,0,0,0,0},
        {1,1,1,1,1,1,1,1,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    };

	// Use this for initialization
	void Start () {

        Vector2 startPosition = new Vector2(4, 2);
        Vector2 endPosition = new Vector2(7, 13);


        //生成地图
        for (int i = 0; i < map.GetLength(0); i ++){
            for (int j = 0; j < map.GetLength(1); j ++){
                if(map[i,j] == 0){
                    CreateObj("AstarGround", new Vector2(i, j));
                }else{
                    CreateObj("AstarWall", new Vector2(i, j));
                } 
            }
        }
        CreateObj("AStarStartPoint", startPosition);
        CreateObj("AStarEndPoint", endPosition);

        AStar astar = new AStar();
        List<Vector2> path = astar.StratAStar(map,startPosition,endPosition);

        Debug.Log(path);

        for (int i = 0; i < path.Count; i ++){
            CreateObj("AstarPath",new Vector2(path[i].x, path[i].y));
        }
	}

    void CreateObj(string name ,Vector2 position){
        GameObject obj = (GameObject)Resources.Load(name);
        obj = MonoBehaviour.Instantiate(obj);
        obj.transform.position = new Vector3(position.x, 0, position.y); 
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
