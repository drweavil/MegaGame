using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[System.Serializable]
public class Buff{
	public int buffID = -1;
	public bool isUnremovable = false;
	public bool isInfinityBuff = false;
	public float buffTime = 0;
	public float originBuffTime = 0;
	public float buffPercent = 0;
	public bool canDeactivate = true;
	public float currentBuffNumber = 0;
	public float maximumTime = 3600;



	public Buff Clone(){
		Buff newBuff = new Buff ();
		newBuff.SetData (this);
		return newBuff;
	}


	void SetData(Buff zoneChunk){
		foreach (FieldInfo field in zoneChunk.GetType().GetFields()) {
			if (!field.IsStatic) {
				FieldInfo thisField = this.GetType ().GetField (field.Name);
				if (thisField != null) {
					thisField.SetValue (this, field.GetValue (zoneChunk));
				}
			}
		}
	}


	public string GetTitle(){
		return LanguageController.jsonFile ["buffs"] ["buff-" + this.buffID.ToString()]["title"];
	}

	public string GetDescription(){
		string description = LanguageController.jsonFile ["buffs"] ["buff-" + this.buffID.ToString ()] ["description"];

		foreach (SimpleJSON.JSONString pattern in LanguageController.jsonFile["buffs"] ["buff-" + this.buffID.ToString()]["patterns"].AsArray) {
			string patternString = pattern;
			int index = description.IndexOf (patternString);
			description = description.Remove (index, patternString.Length);
			description = description.Insert (index, GetParamStringByPattern(patternString));
		}
		return description;
	}

	public string GetProcessDescription(){
		string description = LanguageController.jsonFile ["buffs"] ["buff-" + this.buffID.ToString ()] ["descriptionInProcess"];
		foreach (SimpleJSON.JSONString pattern in LanguageController.jsonFile["buffs"]  ["buff-" + this.buffID.ToString()]["patternsProcess"].AsArray) {
			string patternString = pattern;
			int index = description.IndexOf (patternString);
			description = description.Remove (index, patternString.Length);
			description = description.Insert (index, GetParamStringByPattern(patternString));
		}
		return description;
	}


	public string GetParamStringByPattern(string pattern){
		string value = "";
		if (pattern == "[p!000!p]") {
			if ((buffPercent % 1) == 0) {
				value = System.Math.Round (buffPercent, 0).ToString ();
			} else {
				value = System.Math.Round (buffPercent, 2).ToString ();
			}
		} else if (pattern == "[p!001!p]") {
			value = Timer.GetTimeStringBySeconds (buffTime);
		}
		return value;
	}
}
