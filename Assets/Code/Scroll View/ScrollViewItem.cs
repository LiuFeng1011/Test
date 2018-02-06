using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewItem : MonoBehaviour {
	UILabel label;
	public ScrollViewItemData data;
	// Use this for initialization
	void Awake () {
		label = transform.Find("Label").GetComponent<UILabel> ();

	}

	public void SetData(ScrollViewItemData data){
		this.data = data;
		label.text = data.name ;
	}



	// Update is called once per frame
	void Update () {
		
	}
}
