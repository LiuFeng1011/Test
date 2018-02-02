using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// author:liufeng
/// date:170812
/// 游戏内特效管理器
/// </summary>
public class GameEffectManager {
	//特效池
	Dictionary<int ,List<GameEffect>> effectPool = new Dictionary<int ,List<GameEffect>>();

	public GameEffectManager(){

		//创建特效,加入到池里
		List<conf_effect> _conf = ConfigManager.confEffectManager.datas;

		for(int i = 0 ; i < _conf.Count ; i ++){
			conf_effect conf = _conf[i];
			for(int j = 0 ; j < conf.repeat_count ; j++){
				CreateEffect(conf);
			}
		}
	}


	/// <summary>
	/// 添加特效到世界
	/// </summary>
	/// <returns>The world effect.</returns>
	/// <param name="effectid">配置表中的id.</param>
    /// <param name="pos">出生位置.</param>
    /// <param name="scale">特效的尺寸.</param>
    public GameEffect AddWorldEffect(int effectid,Vector3 pos, float scale){
		GameEffect ge = GetEffect(effectid,pos);
		if(ge == null) return null;
		ge.transform.position = pos;
        ge.Play(scale);
		return ge;
	}

	/// <summary>
    ///  添加特效到指定物体
    /// </summary>
    /// <returns>The effect.</returns>
    /// <param name="effectid">配置表中的id.</param>
    /// <param name="obj">跟随物体，如果此物体不为空，特效会跟随此体移动，直到父物体变为null.</param>
    /// <param name="pos">特效在obj中的相对坐标(偏移).</param>
    /// <param name="scale">特效的尺寸.</param>
    public GameEffect AddEffect(int effectid, GameObject obj, Vector3 pos, float scale){
        if(obj == null){
            return AddWorldEffect(effectid,pos,scale);
        }
		GameEffect ge = GetEffect(effectid,obj.transform.position + pos);
		if(ge == null) return null;
        ge.SetParent(obj);
        ge.Play(scale);
		return ge;
	}

	GameEffect GetEffect(int effectid ,Vector3 worldPos){
		//对应池是否存在
		if(!effectPool.ContainsKey(effectid)){
			return null;
		}

		//寻找空闲特效
		List<GameEffect> pool = effectPool[effectid];
		GameEffect ret = null;
		for(int i = 0 ; i < pool.Count ; i ++){
			GameEffect eff = pool[i];

			if(eff.gameObject.activeSelf){
				continue;
			}

			ret = eff;
			break;
		}

        //如果没有可用特效，则创建一个新的
        if(ret == null){
            conf_effect conf = ConfigManager.confEffectManager.GetData(effectid);
            if(conf != null) ret = CreateEffect(conf);
        }

		return ret;
	}

    //添加一个特效到缓存池
    GameEffect CreateEffect(conf_effect effect){
        Object obj = Resources.Load(effect.file_path);
		if(obj == null){
            Debug.LogError("cant find file ! : " + effect.file_path);
		}
        if(effect.res_type == 1){
			GameObject go = (GameObject)MonoBehaviour.Instantiate(obj);  

			GameEffect ge = go.AddComponent<GameEffect>();
            ge.Init(effect);

            if (!effectPool.ContainsKey(effect.id))
            {
                effectPool.Add(effect.id, new List<GameEffect>());
            }
            effectPool[effect.id].Add(ge);

			return ge;
		}
		return null;
	}

}
