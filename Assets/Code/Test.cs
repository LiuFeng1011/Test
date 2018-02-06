using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Dictionary<int, int> dic = new Dictionary<int, int>();
        int b = 0;
        bool a = dic.TryGetValue(1,out b);

        Debug.Log(a);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
