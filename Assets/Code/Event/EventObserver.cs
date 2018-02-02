using UnityEngine;
using System.Collections;

public interface EventObserver {
	void HandleEvent(EventData resp);
}
