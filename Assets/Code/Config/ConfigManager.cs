using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.IO;

using System.Reflection;


/**
 *Y游戏配置表更新流程
 * 配置表url
 * const stirng CONFIGURL = "http://hideseek.dreamgear82.com/hideseek/config/excel/";
 * 1.获取配置表信息,地址:
 * string infopath = CONFIGURL+"configinfo.txt";
 * 数据样例:
 * [{"name":"constant.xml","size":2351,"md5":"1c739d23906a8e9a30767d9459a83839"},{"name":"dailytask.xml","size":3699,"md5":"92f093f57a5f5a7397d870195ac15d5f"}]
 * 2.根据配置表信息的name和md5 与本地文件进行比较，筛选出需要更新的配置表文件列表，进行更新
 * 3.下载配置表
 * 下载指定配置表文件:CONFIGURL+name;
 */

/**
 * 游戏配置表管理类
 * 
 * 添加配置表流程
 * 1.新建配置表管理脚本 
 * 2.创建数据类 xxxData ，属性字段与配置表一一对应
 * 3.创建数据管理类 xxxDataManager，
 * 4.实现管理类的load方法
 * 5.在ConfigManager中定义静态变量，在LoadData方法中调用管理类的Load方法
 */
public class ConfigManager {
    
	public static bool loadDown = false;

    public static ConfEffectManager confEffectManager = new ConfEffectManager();

    public static void LoadData(){
        Debug.Log("===========启动配置表管理器===========");
        confEffectManager.Load();
        Debug.Log("----------配置表管理器启动成功-----------");
    }

	public static List<T> Load<T>() where T : new(){
		//这里要注意配置对象要与配置文件名称相同
		string[] names = (typeof(T)).ToString().Split('.');

        //获取真实名称
		string filename = names[names.Length - 1];
		XmlDocument doc = new XmlDocument();
        //加载xml文件
        string data = Resources.Load("Config/"+filename).ToString(); 

        doc.LoadXml(data); 
        XmlNode xmlNode = doc.DocumentElement;

		XmlNodeList xnl = xmlNode.ChildNodes;

		List<T> ret = new List<T>();

        //遍历所有内容
		foreach (XmlNode xn in xnl)
		{
            //找到符合条件的数据
			if (xn.Name.ToLower() == filename)
			{
                //实例化数据对象
				T obj = new T();

				Type t = obj.GetType();

                //获取对象的全部属性
				FieldInfo[] fields = t.GetFields();

				string msg = "";
				try{
                    //遍历全部属性，并从配置表中找出对应字段的数据并赋值
                    //根据属性的类型对数据进行转换
					foreach (FieldInfo field in fields){
                        if(xn.Attributes[field.Name] == null){
                            Debug.Log("the field [" + field.Name + "] is null !!!");
                            continue;
                        }
						string val = xn.Attributes[field.Name].Value;
						if(val == null){
							Debug.Log("the field [" + field.Name + "] is null !!!" );
							continue;
						}

						msg = field.Name + " : "+  val + "   type : " + field.FieldType;
                        if (field.FieldType == typeof(int))
                        {
							field.SetValue(obj, int.Parse(val));
                        }
                        else if (field.FieldType == typeof(float))
                        {
                            field.SetValue(obj, float.Parse(val));
                        }
                        else if (field.FieldType == typeof(string))
                        {
                            field.SetValue(obj, val);
                        }

					}	
					ret.Add(obj);
				}catch(Exception e){
					Debug.LogError("=====================" + filename + "==================");
					Debug.LogError(e.Message);
					Debug.LogError(msg);
				}

			}
		}

		return ret;
	}



}
