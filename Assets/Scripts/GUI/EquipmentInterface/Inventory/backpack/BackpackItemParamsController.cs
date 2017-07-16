using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackItemParamsController : MonoBehaviour {
	public const float buffWeight = 0.05f;
	public const float runeWeight = 0.05f;
	public const float buffPricePercent = 0.30f;

	public static BackpackItem GetNewBackpackItem(object itemContent, float complexity = 0){
		BackpackItem newItem = new BackpackItem ();
		if (itemContent.GetType () == typeof(Equipment)) {
			Equipment equip = (Equipment)itemContent;
			float weight = GetWeightByEquip (equip);
			equip.weight = weight;
			newItem.itemContent.Add (equip);
			newItem.weight = weight;
			float price = GetPriceByEquip (equip);
			equip.price = price;
			newItem.price = price;
		} else if (itemContent.GetType () == typeof(Buff)) {
			newItem.itemContent.Add (itemContent);
			newItem.price = EquipmentGenerator.GetPriceByComplexity (PlayerController.currentComplexity) * buffPricePercent;
			newItem.weight = buffWeight;
		} else if (itemContent.GetType () == typeof(EquipmentRune)) {
			EquipmentRune rune = (EquipmentRune)itemContent;
			newItem.itemContent.Add (itemContent);
			newItem.weight = runeWeight;
			newItem.price = GetRunePriceByComplexity (rune.complexity);
		} else if (itemContent.GetType () == typeof(InventorySkill)) {
			InventorySkillInfo info = InventorySkills.GetSkillInfoByID (((InventorySkill)itemContent).skillID);
			newItem.itemContent.Add (itemContent);
			newItem.weight = info.weight;
			newItem.price = EquipmentGenerator.GetPriceByComplexity (PlayerController.currentComplexity) * info.pricePercent;
		}

		newItem.itemID = PlayerController.GetBackpackItemID ();//PlayerController.maximumBackpackID;
		//PlayerController.maximumBackpackID += 1;
		return newItem;
	}


	public static float GetWeightByEquip(Equipment equip){
		float value = 0;
		if (equip.slotID == Equipment.head) {
			value = Random.Range (2f, 5f);
		} else if (equip.slotID == Equipment.chest) {
			value = Random.Range (10f, 15f);
		} else if (equip.slotID == Equipment.legs) {
			value = Random.Range (7f, 10f);
		} else if (equip.slotID == Equipment.trinket) {
			value = Random.Range (1f, 3f);
		} else if (equip.slotID == Equipment.finger) {
			value = Random.Range (0.5f, 1.5f);
		} else if (equip.slotID == Equipment.neck) {
			value = Random.Range (0.3f, 1.5f);
		} else if (equip.slotID == Equipment.meleeWeapon) {
			value = Random.Range (10f, 15f);
		} else if (equip.slotID == Equipment.fireWeapon) {
			value = Random.Range (10f, 15f);
		} else if (equip.slotID == Equipment.elementalWeapon) {
			value = Random.Range (10f, 15f);
		}
		return value;
	}

	public static float GetPriceByEquip(Equipment equip){
		float value = 0;
		if (equip.slotID == Equipment.head) {
			value = Random.Range (10f, 100f);
		} else if (equip.slotID == Equipment.chest) {
			value = Random.Range (10f, 100f);
		} else if (equip.slotID == Equipment.legs) {
			value = Random.Range (7f, 100f);
		} else if (equip.slotID == Equipment.trinket) {
			value = Random.Range (50f, 100f);
		} else if (equip.slotID == Equipment.finger) {
			value = Random.Range (50f, 100f);
		} else if (equip.slotID == Equipment.neck) {
			value = Random.Range (50f, 100f);
		} else if (equip.slotID == Equipment.meleeWeapon) {
			value = Random.Range (20f, 100f);
		} else if (equip.slotID == Equipment.fireWeapon) {
			value = Random.Range (20f, 100f);
		} else if (equip.slotID == Equipment.elementalWeapon) {
			value = Random.Range (20f, 100f);
		}

		value = EquipmentGenerator.GetPriceByComplexity (equip.complexity) * (value/100);
		return value;
	}

	public static float GetRunePriceByComplexity(float complexity){
		float value = Random.Range (50f, 100f);
		value = EquipmentGenerator.GetPriceByComplexity (complexity) * (value/100);
		return value;
	}


}
