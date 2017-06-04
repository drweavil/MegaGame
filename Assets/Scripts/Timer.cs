using UnityEngine;
using System.Collections;

[System.Serializable]
public class Timer {
	private float endTime = 0;
	public delegate void TimerAction ();

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

	public float ResidualTime(){
		float value = 0;
		if (endTime > Time.time) {
			value = endTime - Time.time;
		}
		return value;
	}

	public IEnumerator ActionAfterTimer(TimerAction action){
		while (!TimeIsOver ()) {
			yield return null;
		}
		action();
		yield break;
	}

	public static string GetTimeStringBySeconds(float seconds){
		int timeH = (int)System.Math.Floor (seconds / 3600);
		int timeM = (int)System.Math.Floor (seconds / 60);
		float timeS = seconds;

		int finalTimeH = timeH;
		int finalTimeM = timeM - (timeH * 60);
		float finalTimeS = timeS - (timeM * 60);

		string timeString = "";

		if (finalTimeH != 0) {
			timeString += finalTimeH.ToString() + LanguageController.jsonFile["menu"]["timeAbbreviations"]["h"] + ". ";
		}

		if (finalTimeM != 0) {
			timeString += finalTimeM.ToString() + LanguageController.jsonFile["menu"]["timeAbbreviations"]["m"] + ". ";
		}

		if (finalTimeS != 0) {
			if ((finalTimeS % 1) == 0) {
				finalTimeS = (float)System.Math.Round (finalTimeS, 0);
			} else {
				finalTimeS = (float)System.Math.Round (finalTimeS, 2);
			}
			timeString += finalTimeS.ToString() + LanguageController.jsonFile["menu"]["timeAbbreviations"]["s"] + ". ";
		}

		return timeString;
	}
}
