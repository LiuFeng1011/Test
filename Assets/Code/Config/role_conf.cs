using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class role_conf
{
    public int id;   
    public string name;  
    public string description;  
    public string file_path;     
    public int unlock_type; 
    public int unlock_price;  
}



public class RoleConfManager
{ 
    public List<role_conf> datas = new List<role_conf>();
    Dictionary<int, role_conf> dic = new Dictionary<int, role_conf>();

    public int defaultid = -1;

    public void Load()
    {
        if (datas != null) datas.Clear();

        List<role_conf> _datas = ConfigManager.Load<role_conf>();

        for (int i = 0; i < _datas.Count; i++)
        {
            role_conf conf = _datas[i];
            dic.Add(conf.id, conf);
            datas.Add(conf);

            if(defaultid == -1 && conf.unlock_type == -1){
                defaultid = conf.id;
            }

        }
    }

    public role_conf GetConf(int roleid){
        if(dic.ContainsKey(roleid)){
            return dic[roleid];
        }
        return null;
    }
}