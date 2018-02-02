using UnityEngine;
using System.Collections;

public class EventTest : MonoBehaviour ,EventObserver{
	public string uname = "aaa";
	public string password = "bbb";
	// Use this for initialization
	void Start () {
        EventManager.Register(this,EventID.EVENT_1,EventID.EVENT_2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI () 
	{
		if (GUI.Button (new Rect (10,10,150,50), "send eve1")) 
		{
            Event1 eve = new Event1(1,2,"123");
			eve.Send();
        }

        if (GUI.Button(new Rect(10, 70, 150, 50), "send eve2"))
        {
            EventData.CreateEvent(EventID.EVENT_2).Send();
        }
	}

	public void HandleEvent(EventData data){
        switch(data.eid){
            case EventID.EVENT_1:
                Event1 eve = data as Event1;
                Debug.Log("receive event_1 msg!");
                Debug.Log(string.Format("t1:{0},t2:{1},s1:{2}",eve.t1,eve.t2,eve.s1) );
                break;
            case EventID.EVENT_2:
                Debug.Log("receive event_2 msg!");
                break;
        }

	}

	void OnDestroy(){
		EventManager.Remove(this);
	}
}
