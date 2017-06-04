using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackItemParamsController : MonoBehaviour {
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

		value = EquipmentGenerator.GetPriceByComplexity (equip.complexity);
		return value;
	}


}
