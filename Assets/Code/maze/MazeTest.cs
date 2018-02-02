using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeTest : MonoBehaviour {

    const int row = 21, col = 21;
    MazeCreate mazeCreate;
	// Use this for initialization
    void Start () {
        mazeCreate = MazeCreate.GetMaze(row, col);

        //for (int i = 0; i < row; i++)
        //{
        //    for (int j = 0; j < col; j++)
        //    {

        //        if (mazeCreate.mapList[i][j] == (int)MazeCreate.PointType.startpoint ||
        //            mazeCreate.mapList[i][j] == (int)MazeCreate.PointType.way)
        //        {
                    //GameObject column = (GameObject)Resources.Load("Prefabs/maze");
                    //column = MonoBehaviour.Instantiate(column);
                    //column.transform.position = new Vector3(i, 0, j); 
        //        }

        //    }
        //}
	}

    float addTime = 0;
    int addindex = 0;
	// Update is called once per frame
	void Update () {
        if (addindex >= mazeCreate.findList.Count)
        {
            return;
        }

        addTime += Time.deltaTime;

        if (addTime > 0.01)
        {
            addTime = 0;
            int index = mazeCreate.findList[addindex];

            int _row = index / col;
            int _col = index % col;

            GameObject column = (GameObject)Resources.Load("Prefabs/maze");
                    column = MonoBehaviour.Instantiate(column);
            column.transform.position = new Vector3(_row, 0, _col); 

            addindex++;
        }
	}
}
