using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment{
	public const int head = 0, chest = 1, legs = 2, 
	trinket = 3, finger = 4, neck = 5,
	meleeWeapon = 6, fireWeapon = 7, elementalWeapon = 8;




	public int slotID;
	public int skinID = 0;
	public float weight = 0;
	public float price = 0;
	public float complexity = 0;
	public string title = "";
	public string description = "";
	public bool isNullEquip = false;

	public float healthPoints = 0;
	public float criticalPoints = 0;
	public float physicalArmorPoints = 0;
	public float elementalArmorPoints = 0;
	public float physicalDamagePoints = 0;
	public float elementalDamagePoints = 0;


	public float weaponDamage = 0;

	public EquipmentRune rune1;
	public EquipmentRune rune2;
	public bool rune1Enable = false;
	public bool rune2Enable = false;


	public string GetTitle (){
		return title;
	}
	public string GetDescription(){
		return description;
	}

	public float GetOverallHealthPoints(){
		float value = healthPoints;
		if (rune1 != null) {
			value += rune1.healthPoints;
		}

		if (rune2 != null) {
			value += rune2.healthPoints;
		}
		return value;
	}

	public float GetOverallCriticalPoints(){
		float value = criticalPoints;
		if (rune1 != null) {
			value += rune1.criticalPoints;
		}

		if (rune2 != null) {
			value += rune2.criticalPoints;
		}
		return value;
	}

	public float GetOverallPhysicalArmorPoints(){
		float value = physicalArmorPoints;
		if (rune1 != null) {
			value += rune1.physicalArmorPoints;
		}

		if (rune2 != null) {
			value += rune2.physicalArmorPoints;
		}
		return value;
	}

	public float GetOverallElementalArmorPoints(){
		float value = elementalArmorPoints;
		if (rune1 != null) {
			value += rune1.elementalArmorPoints;
		}

		if (rune2 != null) {
			value += rune2.elementalArmorPoints;
		}
		return value;
	}

	public float GetOverallPhysicalDamagePoints(){
		float value = physicalDamagePoints;
		if (rune1 != null) {
			value += rune1.physicalDamagePoints;
		}

		if (rune2 != null) {
			value += rune2.physicalDamagePoints;
		}
		return value;
	}


	public float GetOverallElementalDamagePoints(){
		float value = elementalDamagePoints;
		if (rune1 != null) {
			value += rune1.elementalDamagePoints;
		}

		if (rune2 != null) {
			value += rune2.elementalDamagePoints;
		}
		return value;
	}

}
