using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	private float endTime = 0;

	public void SetTimer(float seconds){
		endTime = Time.time + seconds;
	}

	public bool TimeIsOver(){
		bool itIs = false;
		if(endTime <= Time.time){
			itIs = true;
		}
		return itIs;
	}
}
