using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// author : liufeng
/// 基本思想就是当列表向上移动时，我们就把超出屏幕上部的item移动到列表末尾位置并重置数据。当列表向下移动时，就把超出屏幕下部的item移动到列表开头。
/// </summary>
public class ScrollViewManager : MonoBehaviour {
	List<ScrollViewItem> itemList = new List<ScrollViewItem>();
	List<ScrollViewItemData> itemDataList = new List<ScrollViewItemData> ();

	UIScrollView sv;
	UIGrid grid;

    //记录scrollviet上一次的位置，用于判断scrollview的移动方向
	float svLastPos = 0;

	//最大y坐标
	float maxHeight = 0;

	//最小y坐标
	float minHeight = 0;

	// Use this for initialization
	void Start () {
		//初始化测试数据

		for (int i = 0; i < 20; i++) {
			itemDataList.Add (new ScrollViewItemData (i,"第" + i + "个元素"));
		}


		sv = transform.GetComponent<UIScrollView> ();
		grid = transform.Find ("Grid").GetComponent<UIGrid>();

		Vector2 viewsize = transform.GetComponent<UIPanel> ().GetViewSize ();
		int count = (int)(viewsize.y / grid.cellHeight + 2);
		Debug.Log (count);
		for (int i = 0; i < count ; i++) {
			if (itemDataList.Count <= i)
				break;
			
			GameObject o = Resources.Load("Prefabs/ScrollViewItem") as GameObject;
			GameObject obj = NGUITools.AddChild(grid.gameObject, o);

			ScrollViewItem item = obj.GetComponent<ScrollViewItem>();
			itemList.Add (item);

			item.SetData (itemDataList[i]);
		}
		grid.repositionNow = true;
		grid.Reposition();

		svLastPos = grid.transform.localPosition.y;

		maxHeight = viewsize.y / 2 + grid.cellHeight / 2;

		minHeight = -maxHeight;

	}

	// Update is called once per frame
	void Update () {
        float moveDis = sv.transform.localPosition.y - svLastPos;
        if (Mathf.Abs(moveDis) > 0.05) {
            bool isup = moveDis > 0;
			if (isup) {
				while (itemList [0].transform.localPosition.y + sv.transform.localPosition.y > maxHeight &&
				       itemList [itemList.Count - 1].data.index < itemDataList.Count - 1) {
					ScrollViewItem item = itemList [0];
					item.SetData (itemDataList [itemList [itemList.Count - 1].data.index + 1]);

					itemList.Add (item);
					itemList.RemoveAt (0);

					item.transform.localPosition = itemList[itemList.Count - 2].transform.localPosition - 
						new Vector3 (0,grid.cellHeight,0);
				}
			} else {
				while (itemList [itemList.Count - 1].transform.localPosition.y + sv.transform.localPosition.y   < minHeight &&
					itemList [0].data.index > 0) {
					ScrollViewItem item = itemList [itemList.Count - 1];
					item.SetData (itemDataList [itemList [0].data.index - 1]);

					itemList.Insert (0, item);
					itemList.RemoveAt (itemList.Count - 1);

					item.transform.localPosition = itemList[1].transform.localPosition + 
						new Vector3 (0,grid.cellHeight,0);
				}
			}

			svLastPos = sv.transform.localPosition.y;
		}

	}
}
