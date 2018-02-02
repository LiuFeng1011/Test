using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

using System.Reflection;

/// <summary>
/// 游戏常用方法
/// </summary>
public static class GameCommon  {
	public const float GAME_DATA_VERSION = 0.0f;//

	public static float LOAD_DATA_VERSION = 0.0f;

	//base64字符串解码
	public static string UnBase64String(string value){
		if(value == null || value == ""){
			return "";
		}
		byte[] bytes = Convert.FromBase64String(value); 
		return Encoding.UTF8.GetString(bytes);
	}

	//base64字符串编码
	public static string ToBase64String(string value){
		if(value == null || value == ""){
			return "";
		}
		byte[] bytes = Encoding.UTF8.GetBytes(value);
		return Convert.ToBase64String(bytes);
	}

	/// <summary>  
	/// 把对象序列化为字节数组  
	/// </summary>  
	public static byte[] SerializeObject(object obj)
	{
		if (obj == null)
			return null;
		//内存实例
		MemoryStream ms = new MemoryStream();
		//创建序列化的实例
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(ms, obj);//序列化对象，写入ms流中  
		byte[] bytes = ms.GetBuffer();
		return bytes;
	}
	
	/// <summary>  
	/// 把字节数组反序列化成对象  
	/// UserDataMode userdata = (UserDataMode)GameCommon.DeserializeObject(data);
	/// </summary>  
	public static object DeserializeObject(byte[] bytes)
	{
		object obj = null;
		if (bytes == null)
			return obj;
		//利用传来的byte[]创建一个内存流
		MemoryStream ms = new MemoryStream(bytes);
		ms.Position = 0;
		BinaryFormatter formatter = new BinaryFormatter();
		obj = formatter.Deserialize(ms);//把内存流反序列成对象  
		ms.Close();
		return obj;
	}
	/// <summary>
	/// 把字典序列化
	/// </summary>
	/// <param name="dic"></param>
	/// <returns></returns>
	public static byte[] SerializeDic<T>(Dictionary<string, T> dic)
	{
		if (dic.Count == 0)
			return null;
		MemoryStream ms = new MemoryStream();
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(ms, dic);//把字典序列化成流
		byte[] bytes = ms.GetBuffer();
		
		return bytes;
	}
	/// <summary>
	/// 反序列化返回字典
	/// </summary>
	/// <param name="bytes"></param>
	/// <returns></returns>
	public static Dictionary<string, T> DeserializeDic<T>(byte[] bytes)
	{
		Dictionary<string, T> dic = null;
		if (bytes == null)
			return dic;
		//利用传来的byte[]创建一个内存流
		MemoryStream ms = new MemoryStream(bytes);
		BinaryFormatter formatter = new BinaryFormatter();
		//把流中转换为Dictionary
		dic = (Dictionary<string, T>)formatter.Deserialize(ms);
		return dic;
	}

	/// <summary>
	/// 把字List序列化
	/// </summary>
	/// <param name="dic"></param>
	/// <returns></returns>
	public static byte[] SerializeList<T>(List<T> dic)
	{
		if (dic.Count == 0)
			return null;
		MemoryStream ms = new MemoryStream();
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(ms, dic);//把字典序列化成流
		byte[] bytes = ms.GetBuffer();
		
		return bytes;
	}
	/// <summary>
	/// 反序列化返回List
	/// </summary>
	/// <param name="bytes"></param>
	/// <returns></returns>
	public static List<T> DeserializeList<T>(byte[] bytes)
	{
		List<T> list = null;
		if (bytes == null)
			return list;
		//利用传来的byte[]创建一个内存流
		MemoryStream ms = new MemoryStream(bytes);
		BinaryFormatter formatter = new BinaryFormatter();
		//把流中转换为List
		list = (List<T>)formatter.Deserialize(ms);
		return list;
	}

	/// <summary>
	/// 二进制数据写入文件 Writes the byte to file.
	/// </summary>
	/// <param name="data">Data.</param>
	/// <param name="tablename">path.</param>
	public static void WriteByteToFile(byte[] data,string path){

		FileStream fs = new FileStream(path, FileMode.Create);
		fs.Write(data,0,data.Length);
		fs.Close();
	}

	/// <summary>
	/// 读取文件二进制数据 Reads the byte to file.
	/// </summary>
	/// <returns>The byte to file.</returns>
	/// <param name="path">Path.</param>
	public static byte[] ReadByteToFile(string path){
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
		return data;
	}

	public static UnityEngine.Object GetResource(string path){
		return Resources.Load(path);
	}

	//解析配置表中配置的坐标数组
	public static List<List<float>> ParseConfigPositionList(String val){
		List<List<float>> retlist = new List<List<float>>();
		if(val != null && !("" == val)){
			try{
				String[] ps = val.Split('|');
				for(int i = 0 ; i < ps.Length ; i ++){
					if(""==(ps[i])) continue;
					String[] points = ps[i].Split(',');
					if(points.Length < 2) continue;
					List<float> pointlist = new List<float>();
					pointlist.Add(float.Parse(points[0]));
					pointlist.Add(float.Parse(points[1]));
					retlist.Add(pointlist);
				}
			}catch(Exception e){
				
				Debug.Log(e.ToString());
			}
		}
		return retlist;
	}

	public static float GetVectorAngle(Vector3 from_, Vector3 to_){  
		Vector3 v3 = Vector3.Cross(from_,to_);  
		if(v3.z > 0)  
			return Vector3.Angle(from_,to_);  
		else  
			return 360-Vector3.Angle(from_,to_);  
	}  


	/// <summary>
	/// 获取当前时间戳
	/// </summary>
	/// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>
	/// <returns></returns>
	public static long GetTimeStamp(bool bflag = true)
	{
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
		long ret;
		if (bflag)
			ret = Convert.ToInt64(ts.TotalSeconds);
		else
			ret = Convert.ToInt64(ts.TotalMilliseconds);
		return ret;
	}

	/// <summary>
    /// 坐标点是否在屏幕内
    /// </summary>
    /// <returns><c>true</c>, if position in screen was ised, <c>false</c> otherwise.</returns>
    /// <param name="pos">Position.</param>
    /// <param name="cutDistance">Cut distance.</param>
    public static bool IsPositionInScreen(Vector3 pos,int cutDistance = 0){
		Vector3 screenPos = Camera.main.WorldToScreenPoint(pos); 
        if(screenPos.x < -cutDistance || 
           screenPos.y < -cutDistance || 
           screenPos.x > Screen.width + cutDistance || 
           screenPos.y > Screen.height + cutDistance){
			return false;
		}
		return true;
	}

	/// <summary>
	/// 世界坐标转NGUI坐标
	/// </summary>
	/// <returns>The position to NGUI position.</returns>
	/// <param name="camera">Camera.</param>
	public static Vector3 WorldPosToNGUIPos(Camera worldCamera,Camera uiCamera ,Vector3 pos){
		Vector3 screenpos = worldCamera.WorldToScreenPoint(pos); 
		screenpos.z = 0f;   //把z设置为0
		Vector3 uipos = uiCamera.ScreenToWorldPoint(screenpos);
		return uipos;
	}
}


/// <summary>
/// 测试类
/// </summary>
public class GameCommonTest{
	
	public static void Base64Test(){
		string base64string = GameCommon.ToBase64String("aaaa11233Base64编码和解码");
		
		string unbase64string = GameCommon.UnBase64String(base64string);
		
		Debug.Log("base64string : " + base64string);
		Debug.Log("unbase64string : " + unbase64string);
	}
	
	public static void SerializeDicTest(){
		
		Dictionary<string, int> test = new Dictionary<string, int>();
		
		test.Add("1",1);
		test.Add("2",2);
		test.Add("3",4);
		
		byte[] testbyte = GameCommon.SerializeDic<int>(test);
		
		Dictionary<string, int> testdic = GameCommon.DeserializeDic<int>(testbyte);
		
		Debug.Log("[SerializeDicTest]  : " + testdic["3"]);
	}
	
}