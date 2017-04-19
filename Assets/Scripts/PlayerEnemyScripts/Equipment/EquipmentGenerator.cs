using UnityEngine;
using System.Collections;


public class EquipmentGenerator : MonoBehaviour {
	public static float GetWeaponDamageByComplexity(float complexity){
		float percent = Random.Range(0.2f, 0.5f); 
		float weaponDamage = (float)(System.Math.Round((complexity * 0.2f)* percent));
		if (weaponDamage == 0) {
			weaponDamage = 1;
		}
		return weaponDamage;
	}


	public static Equipment GetEquipment(float complexity, int slotID = -1, bool isRandomType = true, int enemyType = 0){
		int equipType;
		if (isRandomType) {
			equipType = Random.Range (1, 10);
		} else {
			equipType = enemyType;
		}

		EnemyType type = EnemyType.GetType (equipType);
		Equipment newEquip = new Equipment ();
		if (slotID == -1) {
			newEquip.slotID = Random.Range (0, 10);
		} else {
			newEquip.slotID = slotID;
		}

		float complexityPercent = (complexity * SlotPercent (slotID));

		newEquip.healthPoints = (complexityPercent * (type.staminaPercent/100));
		newEquip.criticalPoints = (complexityPercent * (type.criticalPointsPercent/100));
		newEquip.physicalArmorPoints = (complexityPercent * (type.physicalArmorPointsPercent/100));
		newEquip.elementalArmorPoints = (complexityPercent * (type.elementalArmorPointsPercent/100));
		newEquip.physicalDamagePoints = (complexityPercent * (type.physicalDamagePointsPercent/100));
		newEquip.elementalDamagePoints = (complexityPercent * (type.elementalDamagePointsPercent/100));
		newEquip.skinID = GetEquipmentSkinByComplexity (complexity, slotID);
		if (slotID == Equipment.meleeWeapon || slotID == Equipment.fireWeapon || slotID == Equipment.elementalWeapon) {
			newEquip.weaponDamage = GetWeaponDamageByComplexity (complexity);
		}

		return newEquip;
	}


	public static int GetEquipmentSkinByComplexity(float complexity, int slotID){
		int value = 0;
		if (slotID == Equipment.meleeWeapon) {
			value = Random.Range (1, 51);
		} else if (slotID == Equipment.fireWeapon) {
			value = Random.Range (1, 52);
		} else if (slotID == Equipment.elementalWeapon) {
			value = Random.Range (1, 62);
		} else {
			value = Random.Range (1, 64);
		}
		return value;
	}

	public static float SlotPercent(int slotID){
		float value = 0;
		if (slotID == Equipment.head) {
			value = 0.21f;
		}
		if (slotID == Equipment.chest) {
			value = 0.27f;
		}
		if (slotID == Equipment.legs) {
			value = 0.25f;
		}
		if (slotID == Equipment.trinket) {
			value = 0.09f;
		}
		if (slotID == Equipment.finger) {
			value = 0.09f;
		}
		if (slotID == Equipment.neck) {
			value = 0.09f;
		}

		if (slotID == Equipment.meleeWeapon || slotID == Equipment.fireWeapon || slotID == Equipment.elementalWeapon) {
			value = 0.1f;
		}
		return value;
	}
}
