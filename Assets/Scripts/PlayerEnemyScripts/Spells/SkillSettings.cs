using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillSettings{
	public int skillID = -1;
	public int specID = -1;
	public Vector3 skillRectPosition;
	public float skillRectX = 0;
	public float skillRectY = 0;
	public float cooldown = 0;
	public float globalCooldown = 0;
	public int resourceRemove = 0;
	public int resourceAdd = 0;
	public float distance = 0;
	public float botUseDistance = 0;
	public float minimumDistanceToUse = 0;
	public float damagePercent = 0;
	public float slowTime = 0;
	public float slowSpeed = 0;
	public float plusSpeed = 0;
	public float stunTime = 0;
	public bool ignoreSilence = false;
	public float restorePercent = 0;
	public float existingTime = 0;
	public float chance = 0;
	public float stunChance = 0;
	public bool withSlideAIM = false;
	public bool withAimLine = false;
	public bool autoTarget = false;
	public string skillMethod = "";
	public bool canUseWithoutResource = false;
	public bool isAddResourceSkill = false;

	public float damageTalentPercentStep = 0;
	public int damageTalentStatusSoftCap = 0;

	public float cdTalentStep = 0;
	public int cdMaximumTalentStatus = 0;

	public float percentTalentStep = 0;
	public int percentMaximumTalentStatus = 0;

	public int descriptionType = 0;

	public int maximumTalentStatus = 0;


	public string GetTitle(){
		return LanguageController.jsonFile["skills"]["skill-" + /*skillID*/1.ToString()]["title"];
	}

	public string GetDescription(){
		string description = LanguageController.jsonFile["skills"]["skill-" + /*skillID*/1.ToString()]["description"];


		foreach (SimpleJSON.JSONString pattern in LanguageController.jsonFile["skills"]["skill-" + 1]["patterns"].AsArray) {
			string patternString = pattern;
			int index = description.IndexOf (patternString);
			description = description.Remove (index, patternString.Length);
			description = description.Insert (index, GetParamStringByPattern(patternString));
		}

		return description;	
	}

	public float GetSpeed(bool isPlayer = false){
		if (isPlayer) {
			return slowSpeed - (GetPlusPercent ()/100f);
		} else {
			return slowSpeed;
		}

	}

	public float GetCd(bool isPlayer = false){
		if (isPlayer) {
			return cooldown - GetMinusCd ();
		} else {
			return cooldown;
		}
	}

	public float GetPlusDamagePercent(bool isNext = false){
		
		int status;

		if (isNext) {
			status = PlayerController.skillStates [skillID].talentStatus + 1;
		} else {
			status = PlayerController.skillStates [skillID].talentStatus;
		}
			
		float finalPercent = 0;
		if (damageTalentStatusSoftCap >= status) {
			finalPercent = damageTalentPercentStep * status;
		} else {
			int delta = status - damageTalentStatusSoftCap;

			finalPercent = damageTalentPercentStep * (status - delta);

			float overCapStep = damageTalentPercentStep / 1.75f;
			float overcapPercent = 0; 

			for (int i = 0; i < delta; i++) {
				overcapPercent += overCapStep;
				overCapStep = overCapStep / 1.75f;
			}

			finalPercent += overcapPercent;
		}
		return finalPercent;
	}

	public float GetMinusCd(bool isNext = false){
		int status;

		if (isNext) {
			status = PlayerController.skillStates [skillID].talentStatus + 1;
		} else {
			status = PlayerController.skillStates [skillID].talentStatus;
		}

		float finalPercent = 0;
		if (cdTalentStep >= status) {
			finalPercent = cdTalentStep * status;
		}
		return finalPercent;
	}

	public float GetPlusPercent(bool isNext = false){
		int status;

		if (isNext) {
			status = PlayerController.skillStates [skillID].talentStatus + 1;
		} else {
			status = PlayerController.skillStates [skillID].talentStatus;
		}

		float finalPercent = 0;
		if (percentMaximumTalentStatus >= status) {
			finalPercent = percentMaximumTalentStatus * status;
		}
		return finalPercent;
	}


	public string GetTalentDescription(bool isNext = false){
		string description = LanguageController.jsonFile["talents"]["descriptionType-" + descriptionType.ToString()]["description"];
		 

		foreach (SimpleJSON.JSONString pattern in LanguageController.jsonFile["talents"]["descriptionType-" + descriptionType.ToString()]["patterns"].AsArray) {
			string patternString = pattern;
			int index = description.IndexOf (patternString);
			description = description.Remove (index, patternString.Length);
			description = description.Insert (index, GetParamStringByPattern(patternString, isNext));
		}
		return description;
	}



	public string GetParamStringByPattern(string pattern, bool isNext = false){
		string value = "";
		//TalentDamagePercent
		if (pattern == "[p!000!p]") {
			float percent = GetPlusDamagePercent (isNext);
			if ((percent % 1) == 0) {
				value = System.Math.Round (percent, 0).ToString ();
			} else {
				value = System.Math.Round (percent, 2).ToString ();
			}
			//TalentMinusCd
		} else if (pattern == "[p!001!p]") {
			value = Timer.GetTimeStringBySeconds (GetMinusCd (isNext));
			//finalDamage
		} else if (pattern == "[p!002!p]") {
			value = System.Math.Round (PlayerController.playerCharacterAPI.skills.SkillDamageBySkillId (skillID, specID, false).number, 2).ToString ();
			//finalSpeed
		} else if (pattern == "[p!003!p]") {
			float speed = 100f - (GetSpeed () * 100f);
			if (speed < 0) {
				speed = speed * -1;
			}
			value = System.Math.Round (GetSpeed(), 2).ToString ();
			//talentPlusPercent
		} else if (pattern == "[p!004!p]") {
			value = System.Math.Round (GetPlusPercent (), 2).ToString ();
			//finalCd
		} else if (pattern == "[p!005!p]") {
			value = System.Math.Round (GetCd (), 2).ToString ();
		}
		return value;
	}

}
