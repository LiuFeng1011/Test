using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int data = 0;

        data = Set1(data, 2);
        data = Set1(data, 4);

        Debug.Log("data : " + data);

        Debug.Log(data + "第3位为" + (Chect(data, 3) ? 1 : 0));
        Debug.Log(data + "第4位为" + (Chect(data, 4) ? 1 : 0));

        data = Set0(data, 4);

        Debug.Log(data + "第4位为" + (Chect(data, 4) ? 1 : 0));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //检测num的第index个二进制位是否为1
    public static bool Chect(int num,int index){
        int temp = (int)Mathf.Pow(2, index);
        return (num & temp) == temp;
    }

    //将num的第index个二进制设置为0
    public static int Set0(int num, int index)
    {
        return num - ((int)Mathf.Pow(2, index));
    }
    //将num的第index个二进制设置为1
    public static int Set1(int num,int index){
        return num | ((int)Mathf.Pow(2, index));
    }
}
