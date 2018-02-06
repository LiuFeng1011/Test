using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System;

public class ConfigClassDic
{
    public Dictionary<string, List<ConfigClass>> dic;
}

[Serializable]
public class ConfigClass
{
    public virtual string GetId() { Debug.LogError("don't called this function!"); return ""; }
    public virtual void toString() { Debug.Log("the child can`t find function : <toString>"); }
}

[Serializable]
public class ExcelATest1 : ConfigClass
{
    public string id;
    public string eat1_1;
    public string eat1_2;
    public int eat1_3;

    public override string GetId()
    {
        return id;
    }

    public override void toString()
    {
        Debug.Log("[ExcelATest1] id:" + id);
    }
}
[Serializable]
public class ExcelATest2 : ConfigClass
{

    public string id;
    public string eat2_1;
    public string eat2_2;
    public int eat2_3;


    public override string GetId()
    {
        return id;
    }

    public override void toString()
    {
        Debug.Log("[ExcelATest2] id:" + id);
    }
}
[Serializable]
public class ExcelBTest1 : ConfigClass
{

    public string id;
    public string ebt1_1;
    public string ebt1_2;
    public int ebt1_3;


    public override string GetId()
    {
        return id;
    }

    public override void toString()
    {
        Debug.Log("[ExcelBTest1] id:" + id);
    }
}