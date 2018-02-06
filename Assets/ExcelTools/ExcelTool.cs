using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using Excel;
using System.Data;

using System.Runtime.Serialization.Formatters.Binary;

public class ExcelTool {


	//生成配置表文件
	void HandleXLSX(){
		LoadData("test");
	}


	/// <summary>
	/// 载入一个excel文件 Loads the data.
	/// </summary>
	/// <param name="filename">Filename.</param>
	public static string LoadData(string filename)
	{
        FileStream stream = File.Open(Application.dataPath + "/ExcelTools/xlsx/" + filename, FileMode.Open, FileAccess.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		
		DataSet result = excelReader.AsDataSet();

		string ret = "";
		//处理所有的子表
		for(int i = 0 ; i < result.Tables.Count; i++){
            Debug.Log(result.Tables[i].TableName);
			bool issuccess = HandleATable(result.Tables[i]);
			if(issuccess )
				ret += result.Tables[i].TableName + "\n";
		}
		return ret;
	}

	public static string GetClassNameByName(string tablename){

		if(tablename.Substring(0,6) == "string"){
			return "GameStringConf";
		}
		return tablename;
	}

	/// <summary>
	/// 处理一张表 Handle A table.
	/// </summary>
	/// <param name="result">Result.</param>
	public static bool HandleATable(DataTable result){
		Debug.Log(result.TableName);

		//创建这个类
		Type t = Type.GetType(GetClassNameByName(result.TableName));
		if(t == null){
			Debug.Log("the type is null  : " + result.TableName);
			return false;
		}

		int columns = result.Columns.Count;
		int rows = result.Rows.Count;

		//行数从0开始  第0行为注释
		int fieldsRow = 1;//字段名所在行数
		int contentStarRow = 2;//内容起始行数
		
		//获取所有字段
		string[] tableFields = new string[columns];
		
		for(int j =0; j < columns; j++)
		{
			tableFields[j] = result.Rows[fieldsRow][j].ToString();
			//Debuger.Log(tableFields[j]);
		}

		//存储表内容的字典
        List<ConfigClass> datalist = new List<ConfigClass>();

		//遍历所有内容
		for(int i = contentStarRow;  i< rows; i++)
		{
			ConfigClass o = Activator.CreateInstance(t) as ConfigClass;

			for(int j =0; j < columns; j++)
			{
				System.Reflection.FieldInfo info = o.GetType().GetField(tableFields[j]);

				if(info == null){
					continue;
				}

				string val = result.Rows[i][j].ToString();

				if(info.FieldType ==  typeof(int)){
					info.SetValue(o,int.Parse(val));
				}else if(info.FieldType ==  typeof(float)){
					info.SetValue(o,float.Parse(val));
				}else{
					info.SetValue(o,val);
				}
				//Debuger.Log(val);
			}
			o.toString();

            datalist.Add(o);

		}	

        SaveTableData(datalist,result.TableName + ".msconfig");
		return true;
	}

	/// <summary>
	/// 把Dictionary序列化为byte数据
	/// Saves the table data.
	/// </summary>
	/// <param name="dic">Dic.</param>
	/// <param name="tablename">Tablename.</param>
    public static void SaveTableData(List<ConfigClass> datalist ,string tablename){

        byte[] dicdata = SerializeObj(datalist);
		//WriteByteToFile(gzipData,tablename);
        WriteByteToFile(dicdata,SaveConfigFilePath(tablename));
	}

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="dic"></param>
    /// <returns></returns>
    public static byte[] SerializeObj(object obj)
    {
        MemoryStream ms = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(ms, obj);//把字典序列化成流
        byte[] bytes = ms.GetBuffer();

        return bytes;
    }
    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static object DeserializeObj(byte[] bytes)
    {
        object dic = null;
        if (bytes == null)
            return dic;
        //利用传来的byte[]创建一个内存流
        MemoryStream ms = new MemoryStream(bytes);
        BinaryFormatter formatter = new BinaryFormatter();
        //把流中转换为Dictionary
        dic = (List<ConfigClass>)formatter.Deserialize(ms);
        return dic;
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

    /// <summary>
    /// 读取文件二进制数据 Reads the byte to file.
    /// </summary>
    /// <returns>The byte to file.</returns>
    /// <param name="path">Path.</param>
    public static byte[] ReadByteToFile(string path)
    {
        //Debug.Log(path);
        //如果文件不存在，就提示错误
        if (!File.Exists(path))
        {
            Debug.Log("读取失败！不存在此文件");
            Debug.Log(path);
            return null;
        }
        FileStream fs = new FileStream(path, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);
        byte[] data = br.ReadBytes((int)br.BaseStream.Length);

        fs.Close();
        return data;
    }

    public static string GetConfigFilePath(string tablename)
    {
        string src = "";

        if (Application.platform == RuntimePlatform.Android)
        {
            src = "jar:file://" + Application.dataPath + "!/assets/Config/" + tablename;
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            src = "file://" + Application.dataPath + "/Raw/Config/" + tablename;
        }
        else
        {
            src = "file://" + Application.streamingAssetsPath + "/Config/" + tablename;
        }
        return src;
    }

    public static string SaveConfigFilePath(string tablename)
    {
        return Application.streamingAssetsPath + "/Config/" + tablename;
    }

}
