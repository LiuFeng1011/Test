using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson; 
using System.IO;  
//using System;  

public class MEMapWayPoint : Editor {
	public static string mapname = "mapway";
	[MenuItem("关卡编辑器/保存路径")]
	public static void SaveLevel(){
		
		// 场景路径
		/*
		string scenePath = AssetDatabase.GetAssetPath(selectObject);
		
		Debug.Log("=====================================");
		Debug.Log(sceneName + "   path : " + scenePath );
		// 打开这个关卡
		EditorApplication.OpenScene(scenePath);
		*/

		//获取场景中全部道具
		Object[] objects = Object.FindObjectsOfType(typeof(GameObject));

		Dictionary<string, List<string>> post = new Dictionary<string, List<string>>();

		foreach (GameObject sceneObject in objects){
			
			if(sceneObject.name == "WayPoint"){
				foreach (Transform child in sceneObject.transform)
				{
					MapWayPoint editor = child.GetComponent<MapWayPoint>();
					if(editor != null){
						if(editor.pointList.Count <= 0) Debug.LogError("The point child is null : " + child.transform.position );
						List<string > childlist = new List<string>();

						for(int i = 0 ; i < editor.pointList.Count ; i ++){
							if(editor.pointList[i] == null){
								continue;
							}
							
							childlist.Add(GetPosString(editor.pointList[i].transform.position));
						}
						
						post.Add(GetPosString(editor.transform.position),childlist);
					}
				}
			}
		}

		//保存文件
        string filePath = GetDataFilePath(mapname + ".text");
		byte[] byteArray = System.Text.Encoding.Default.GetBytes ( JsonMapper.ToJson(post) );
		WriteByteToFile(byteArray,filePath );

		Debug.Log(JsonMapper.ToJson(post));
	}

	public static void Save(string name){
		mapname = name;
		SaveLevel();
	}

	//================================读取================================

	[MenuItem("关卡编辑器/读取路径")]
	public static void LoadLevel(){

		List<GameObject> delarr = new List<GameObject>();
		GameObject WayPoint = null;
		foreach (GameObject sceneObject in Object.FindObjectsOfType(typeof(GameObject))){
			if( sceneObject.name == "WayPoint"){
				WayPoint = sceneObject;
				break;
			}
		}

		if(WayPoint == null){
			GameObject tGO = new GameObject("WayPoint");  
			tGO.AddComponent<MapDraw>();  
			WayPoint = tGO;
		}

		//读取文件
        byte[] pointData = ReadByteToFile(GetDataFilePath(mapname+".text"));

		if(pointData == null){
			return;
		}


		foreach (Transform child in WayPoint.transform){
			delarr.Add(child.gameObject);
		}
		//删除旧物体
		foreach(GameObject obj in delarr){
			DestroyImmediate(obj);
		}

		string str = System.Text.Encoding.Default.GetString ( pointData );
		Debug.Log(str);
		Dictionary<string, List<string>> post = JsonMapper.ToObject<Dictionary<string, List<string>>>(str);

		Dictionary<string,MapWayPoint> temp = new Dictionary<string,MapWayPoint>();
		foreach (KeyValuePair<string, List<string>> pair in post)  
		{  
			List<string> list = pair.Value;

			MapWayPoint obj = GetObj(WayPoint,temp,pair.Key);

			for(int i = 0 ; i < list.Count ; i ++){
				Debug.Log("add");
				MapWayPoint child = GetObj(WayPoint,temp,list[i]);
				obj.pointList.Add(child.gameObject);
			}
		}  

//		Debug.Log(JsonMapper.ToJson(post["0"][0]));
	}

	public static void Load(string name){
		mapname = name;
		LoadLevel();
	}


	public static string GetPosString(Vector3 pos){
		return pos.x + "," + pos.y + "," + pos.z;
	}

	public static Vector3 GetPosByString(string pos){
		Vector3 ret = Vector3.zero;
		try{
			string[] s = pos.Split(',');

			ret.x = float.Parse(s[0]);
			ret.y = float.Parse(s[1]);
			ret.z = float.Parse(s[2]);

		}catch(System.Exception e){
			Debug.Log(e.Message);
		}
		return ret;
	}

	//加载路径点时，获取存档中的路径点，没有则创建
	public static MapWayPoint GetObj(GameObject parent, Dictionary<string,MapWayPoint> temp,string name){
		MapWayPoint obj;
		if(temp.ContainsKey(name)){
			obj = temp[name];
		}else{

			GameObject tempObj = Resources.Load("Prefabs/WayPoingUnit") as GameObject;
			tempObj = (GameObject)Instantiate(tempObj);
			tempObj.transform.parent = parent.transform;

			tempObj.transform.position = GetPosByString(name);

			obj = tempObj.GetComponent<MapWayPoint>();
			temp.Add(name,obj);
		}
		return obj;
	}

    /// <summary>
    /// 读取文件二进制数据 Reads the byte to file.
    /// </summary>
    /// <returns>The byte to file.</returns>
    /// <param name="path">Path.</param>
    public static byte[] ReadByteToFile(string path)
    {
        //如果文件不存在，就提示错误
        if (!File.Exists(path))
        {
            Debug.Log("读取失败！不存在此文件");
            return null;
        }
        FileStream fs = new FileStream(path, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);
        byte[] data = br.ReadBytes((int)br.BaseStream.Length);

        fs.Close();
        fs.Dispose();
        br.Close();

        return data;
    }

    /// <summary>
    /// 二进制数据写入文件 Writes the byte to file.
    /// </summary>
    /// <param name="data">Data.</param>
    /// <param name="tablename">path.</param>
    public static void WriteByteToFile(byte[] data, string path)
    {

        FileStream fs = new FileStream(path, FileMode.Create);
        fs.Write(data, 0, data.Length);
        fs.Close();
    }


    public static string GetDataFilePath(string filename)
    {
        return Application.dataPath + "/Resources/MapWayData/" + filename;
    }
}
