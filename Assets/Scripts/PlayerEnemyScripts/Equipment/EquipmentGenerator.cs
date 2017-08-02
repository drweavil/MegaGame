using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EquipmentGenerator : MonoBehaviour {
	public const float smallRunePercent = 10, middleRunePercent = 20, largeRunePercent = 35;
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

		newEquip.complexity = complexity;
		newEquip.healthPoints = (complexityPercent * (type.staminaPercent/100));
		newEquip.criticalPoints = (complexityPercent * (type.criticalPointsPercent/100));
		newEquip.physicalArmorPoints = (complexityPercent * (type.physicalArmorPointsPercent/100));
		newEquip.elementalArmorPoints = (complexityPercent * (type.elementalArmorPointsPercent/100));
		newEquip.physicalDamagePoints = (complexityPercent * (type.physicalDamagePointsPercent/100));
		newEquip.elementalDamagePoints = (complexityPercent * (type.elementalDamagePointsPercent/100));

		newEquip.title = "Трусы счастья со знаком офигенности";
		newEquip.description = "Большое описание. Большое описание. Большое описание." +
			" Большое описание. Большое описание. Большое описание. Большое описание." +
			" Большое описание. Большое описание. Большое описание. Большое описание." +
			" Большое описание. Большое описание. Большое описание. Большое описание." +
			" Большое описание. Большое описание. Большое описание. Большое описание." +
			" Большое описание. Большое описание. Большое описание. Большое описание." +
			" Большое описание. Большое описание. Большое описание. Большое описание.";
		newEquip.skinID = GetEquipmentSkinByComplexity (complexity, slotID);
		if (slotID == Equipment.meleeWeapon || slotID == Equipment.fireWeapon || slotID == Equipment.elementalWeapon) {
			newEquip.weaponDamage = GetWeaponDamageByComplexity (complexity);
		}

		return newEquip;
	}


	public static void SetRuneSlotsToEquipment(ref Equipment equip, float rune1Chance, float rune2Chance){
		float rune1Random = Random.Range (0, 100.0000000001f);
		if (rune1Random <= rune1Chance) {
			equip.rune1Enable = true;
			float rune2Random = Random.Range (0, 100.0000000001f);
			if (rune2Random <= rune2Chance) {
				equip.rune2Enable = true;
			}
		}
	}

	public static EquipmentRune GetEquipmentRune(Equipment equip, float runePercent, int statsNumber){
		EquipmentRune rune = new EquipmentRune ();
		Dictionary<string, float> equipStats = new Dictionary<string, float> ();
		equipStats.Add ("health", equip.healthPoints);
		equipStats.Add ("physDamage", equip.physicalDamagePoints);
		equipStats.Add ("elemDamage", equip.elementalDamagePoints);
		equipStats.Add ("critical", equip.criticalPoints);
		equipStats.Add ("physArmor", equip.physicalArmorPoints);
		equipStats.Add ("elemArmor", equip.elementalArmorPoints);

		List<KeyValuePair<string, float>> euipStatsList = new List<KeyValuePair<string, float>> ();
		euipStatsList = equipStats.OrderBy (x => x.Value).ToList ();//.ToDictionary(t => t.Key, t => t.Value);

		euipStatsList = euipStatsList.GetRange (euipStatsList.Count - statsNumber, statsNumber).ToList();
		equipStats = euipStatsList.ToDictionary(l=> l.Key, l=>l.Value);

		if (equipStats.ContainsKey ("health")) {
			rune.healthPoints = equip.healthPoints * (runePercent / 100);
		}
		if (equipStats.ContainsKey ("physDamage")) {
			rune.physicalDamagePoints = equip.physicalDamagePoints * (runePercent / 100);
		}
		if (equipStats.ContainsKey ("elemDamage")) {
			rune.elementalDamagePoints = equip.elementalDamagePoints * (runePercent / 100);
		}
		if (equipStats.ContainsKey ("critical")) {
			rune.criticalPoints = equip.criticalPoints * (runePercent / 100);
		}
		if (equipStats.ContainsKey ("physArmor")) {
			rune.physicalArmorPoints = equip.physicalArmorPoints * (runePercent / 100);
		}
		if (equipStats.ContainsKey ("elemArmor")) {
			rune.elementalArmorPoints = equip.elementalArmorPoints * (runePercent / 100);
		}

		rune.skinID = GetRuneSkinByComplexyty (equip.complexity);
		rune.complexity = equip.complexity;
		return rune;
	}


	public static int GetRuneSkinByComplexyty(float complexity){
		List<int> tier_1 = new List<int> (new int[]{1, 2, 3, 4, 5, 6});
		List<int> tier_2 = new List<int> (new int[]{7, 8, 9, 10, 11, 12});
		List<int> tier_3 = new List<int> (new int[]{13, 14, 15, 16, 17, 18});
		List<int> tier_4 = new List<int> (new int[]{19, 20, 21, 22, 23, 24});

		float tier1Complexity = (PlayerController.maximumComplexity / 2)/2;
		float tier2Complexity = PlayerController.maximumComplexity / 2;
		float tier3Complexity = tier2Complexity + tier1Complexity;
		float tier4Complexity = PlayerController.maximumComplexity;

		List<int> finalList = new List<int> ();
		if (complexity <= tier1Complexity) {
			finalList = tier_1;
		} else if (complexity > tier1Complexity || complexity <= tier2Complexity) {
			finalList = tier_2;
		} else if (complexity > tier2Complexity || complexity <= tier3Complexity) {
			finalList = tier_3;
		} else if (complexity > tier3Complexity || complexity <= tier4Complexity) {
			finalList = tier_4;
		}
		return finalList[Random.Range (0, 6)];
	}

	public static EquipmentRune GetEquipmentRuneByComplexity(float complexity, float runePercent){
		Equipment equip = GetEquipment (complexity, Equipment.fireWeapon);
		return GetEquipmentRune (equip, runePercent, 3);
	}

	public static Equipment GetTestRandomEquipment(float complexity, int slotID){
		Equipment newEquip = GetEquipment (complexity, slotID);
		SetRuneSlotsToEquipment (ref newEquip, 60f, 60f);
		if (newEquip.rune1Enable) {
			if (Random.Range (0, 100f) <= 60) {
				newEquip.rune1 = GetEquipmentRune (GetEquipment (complexity, slotID), smallRunePercent, 3);
			}
		}
		if (newEquip.rune2Enable) {
			if (Random.Range (0, 100f) <= 60) {
				newEquip.rune2 = GetEquipmentRune (GetEquipment (complexity, slotID), smallRunePercent, 3);
			}
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
		} else if(slotID == Equipment.trinket){
			value = Random.Range (1, 31);
		} else if(slotID == Equipment.finger){
			value = Random.Range (1, 28);
		} else if(slotID == Equipment.neck){
			value = Random.Range (1, 31);
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


	public static Buff GetRandomBuff(){
		int buffID = Random.Range (1, 7);
		return Buffs.GetBuff (buffID);
	}

	public static Consumable GetHammer(int type){
		Consumable hammer = new Consumable ();
		hammer.consumableType = Consumable.hammer;
		hammer.consumableSubType = type;
		return hammer;
	}

	public static float GetPriceByComplexity(float complexity){
		return (PlayerController.maximumPrice * complexity) / PlayerController.maximumComplexity;
	}
}
