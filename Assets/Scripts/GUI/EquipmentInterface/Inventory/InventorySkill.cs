using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[System.Serializable]
public class InventorySkill {
	public int skillID = 0;



	public InventorySkill Clone(){
		InventorySkill newSkill = new InventorySkill ();
		newSkill.SetData (this);
		return newSkill;
	}


	void SetData(InventorySkill data){
		foreach (FieldInfo field in data.GetType().GetFields()) {
			FieldInfo thisField = this.GetType ().GetField (field.Name);
			if(thisField != null){
				thisField.SetValue(this, field.GetValue(data));
			}
		}
	}

	public string GetTitle(){
		return LanguageController.jsonFile ["inventorySkills"] ["skill_" + skillID.ToString ()] ["title"];
	}

	public string GetDescription(){
		return LanguageController.jsonFile ["inventorySkills"] ["skill_" + skillID.ToString ()] ["description"];
	}
}
