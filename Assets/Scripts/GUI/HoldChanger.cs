using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoldChanger : MonoBehaviour {
	public float holdTime = 1.5f;
	public float changeTime = 0.4f;
	public bool isHolding = false;
	public UnityEvent holdEvent;
	public UnityEvent inChangeProcessEvent;
	public UnityEvent changeProcessOver;

	bool timerActive = false;
	Timer holdTimer = new Timer();

	public void IsHolding(bool value){
		isHolding = value;
	}

	public void Update(){
		if (isHolding) {
			if (timerActive) {
				if (holdTimer.TimeIsOver ()) {
					inChangeProcessEvent.Invoke ();
					holdEvent.Invoke ();
					holdTimer.SetTimer (changeTime);
				}
			} else {
				timerActive = true;
				holdTimer.SetTimer (holdTime);
			}
		} else {
			timerActive = false;
			changeProcessOver.Invoke ();
		}
	}
}
