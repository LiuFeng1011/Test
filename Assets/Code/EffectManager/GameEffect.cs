using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game effect.
/// </summary>
public class GameEffect : MonoBehaviour {
    //配置文件
	public conf_effect conf{get;private set;}
    //粒子系统
	ParticleSystem ps ;
    //播放时间
    float playTime;
    //跟随物体，如果此物体不为空，特效会跟随父物体移动，直到父物体变为null
    public GameObject parent;

	public void Init(conf_effect conf){
		this.conf = conf;
		ps = transform.GetComponent<ParticleSystem>();
		gameObject.SetActive(false);
	}

	public void Play(float scale){
		playTime = 0;

		gameObject.SetActive(true);

		ps.Play();

        SetScale(transform, scale);
	}

    public void SetScale(Transform t, float scale){
        for (int i = 0; i < t.childCount; i ++){
            SetScale(t.GetChild(i),scale);
        }
        t.localScale = new Vector3(scale, scale, scale);
    }

	void Update(){
        playTime += Time.deltaTime;
        if (parent != null)
        {
            transform.position = parent.transform.position;
            return;
        }

        if(playTime > ps.main.duration){
			Die();
		}
	}

    public void SetParent(GameObject obj){
        this.parent = obj;
    } 

	public void Die(){
		gameObject.SetActive(false);

	}

	void OnDestroy(){
	}
}
