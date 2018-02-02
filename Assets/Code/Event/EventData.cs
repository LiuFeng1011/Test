using UnityEngine;
using System.Collections;

public class EventData {
    public EventID eid;

    public EventData(EventID eid){
        this.eid = eid;
    }

	public void Send(){
        if(EventManager.instance() != null)EventManager.instance().SendEvent(this);
	}

    public static EventData CreateEvent(EventID eventid){
        EventData data = new EventData(eventid);
        return data;
    }
}

public class Event1 : EventData
{
    public int t1 { get; private set; }
    public int t2 { get; private set; }
    public string s1 { get; private set; }

    public Event1(int t1,int t2,string s1) : base(EventID.EVENT_1)
    {
        this.t1 = t1;
        this.t2 = t2;
        this.s1 = s1;
    }
}