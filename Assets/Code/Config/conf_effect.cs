using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conf_effect {
	public 	int	id	;	/*	id(int)	*/
	public 	string	description	;	/*	描述(string)	*/
	public 	int	loop	;	/*	"是否循环(int)
0:否
1:是"	*/
	public 	int	res_type	;	/*	"资源类型(int)
1:预知体
2:png"	*/
	public 	string	file_path	;	/*	路径	*/
	public 	int	repeat_count	;/*同时存在数量(int)*/
	public 	int	out_show	;/*是否在屏幕外显示(int)0:否1:是*/
}


public class ConfEffectManager{
	public List<conf_effect> datas {get;private set;}
	Dictionary<int, conf_effect> dic = new Dictionary<int, conf_effect>();

	public void Load(){

		if(datas != null) datas.Clear();
		dic.Clear();

		datas = ConfigManager.Load<conf_effect>();
		for(int i = 0 ; i < datas.Count ; i ++){
			dic.Add(datas[i].id,datas[i]);
		}

	}

	public conf_effect GetData(int id){
		if(!dic.ContainsKey(id)){
			return null;
		}
		return dic[id];
	}
}
