using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartProcess : MonoBehaviour {
	public static GameObject startProcessController;
	public delegate void ActionAfterFewFrames ();

	void Awake () {
		startProcessController = this.gameObject;
	}

	public static IEnumerator StartActionAfterFewFrames(int frames, ActionAfterFewFrames action){
		for(int i= 0; i < frames; i++){
			yield return null;
		}

		action ();
		yield break;
	}
}
