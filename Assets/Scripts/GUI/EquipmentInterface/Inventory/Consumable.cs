using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class Consumable : MonoBehaviour {
	public const int hammer = 1;
	public static Dictionary<int, string> typeIconNames = new Dictionary<int, string> {
		{1, "hammer"}
	};
	public static Dictionary<int, Dictionary<int, float>> consumablePricePercents = new Dictionary<int, Dictionary<int, float>> {
		{1, new Dictionary<int, float> {
				{1, 0.10f}, 
				{2, 0.20f},
				{3, 0.35f}
			}
		}
	};
	public static Dictionary<int, float> typeWeight = new Dictionary<int, float> {
		{1, 0.05f}
	};
	public int consumableType = 0;
	public int consumableSubType = 0;
	public bool withCommonDescription = false;


	public Consumable Clone(){
		Consumable consumable = new Consumable ();
		consumable.SetData (this);
		return consumable;
	}


	void SetData(Consumable data){
		/*consumableType = data.consumableType;
		consumableSubType = data.consumableSubType;*/
		foreach (FieldInfo field in data.GetType().GetFields()) {
			if (!field.IsStatic) {
				FieldInfo thisField = this.GetType ().GetField (field.Name);
				if (thisField != null) {
					thisField.SetValue (this, field.GetValue (data));
				}
			}
		}
	}

	public string GetConsumableIconName(){
		return typeIconNames [consumableType] + "_";
	}

	public string GetTitle(){
		return LanguageController.jsonFile ["consumables"] [typeIconNames [consumableType]] [consumableSubType.ToString ()]["title"];
	}

	public string GetDescription(){
		return LanguageController.jsonFile ["consumables"] [typeIconNames [consumableType]] [consumableSubType.ToString ()]["description"];
	}

	public string GetCommonDescription(){
		return LanguageController.jsonFile ["consumables"] [typeIconNames [consumableType]] ["commonDescription"];
	}

	public float GetConsumablePrice(){
		return consumablePricePercents [consumableType] [consumableSubType] * EquipmentGenerator.GetPriceByComplexity(PlayerController.currentComplexity);
	}

	public float GetConsumableWeight(){
		if (typeWeight.ContainsKey (consumableType)) {
			return typeWeight [consumableType];
		} else {
			return 0.05f;
		}
	}
}
