using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMount : MonoBehaviour {
    //每2个定点为一组，此值代表有多少组
    const int count = 50;
    //每两组顶点的间隔距离，此值越小曲线越平滑
    const float pointdis = 0.2f;

    Material mat;
    MeshCollider mc;

    //曲线
    public AnimationCurve anim;

	// Use this for initialization
	void Start () {

        mat = Resources.Load<Material>("Shader/MeshMount/Custom_meshshader");
        mc = gameObject.AddComponent<MeshCollider>();

        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();  
        mr.material = mat;  
  
        DrawSquare();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void DrawSquare()  
    {  
        //创建mesh
        Mesh mesh = gameObject.AddComponent<MeshFilter>().mesh;  
        mesh.Clear();

        //定义顶点列表
        List<Vector3> pointList = new List<Vector3>();
        //uv列表
        List<Vector2> uvList = new List<Vector2>();
        //第一列的前2个点直接初始化好
        pointList.Add(new Vector3(0, 0, 0));
        pointList.Add(new Vector3(0, 1, 0));

        //设置前2个点的uv
        uvList.Add(new Vector2(0, 0));
        uvList.Add(new Vector2(0,1));

        //三角形数组
        List<int> triangleList = new List<int>();

        for (int i = 0; i < count; i ++){
            //计算当前列位于什么位置
            float rate = (float)i / (float)count;
            //计算当前的顶点
            pointList.Add(new Vector3((i + 1)*pointdis, 0, 0));

            //这里从曲线函数中取出当点的高度
            pointList.Add(new Vector3((i + 1)*pointdis,anim.Evaluate(rate), 0));

            //uv直接使用rate即可
            uvList.Add(new Vector2(rate, 0));
            uvList.Add(new Vector2(rate, 1));

            //计算当前2个点与前面2个点组成的2个三角形
            int startindex = i * 2;
            triangleList.Add(startindex + 0);
            triangleList.Add(startindex + 1);
            triangleList.Add(startindex + 2);

            triangleList.Add(startindex + 3);
            triangleList.Add(startindex + 2);
            triangleList.Add(startindex + 1);
        }

        //把最终的顶点和三角形数组赋予mesh;
        mesh.vertices = pointList.ToArray();
        mesh.triangles = triangleList.ToArray();
        mesh.uv = uvList.ToArray();
        mesh.RecalculateNormals();

        //把mesh赋予MeshCollider
        mc.sharedMesh = mesh;
    }  
}
