using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class InventorySkills : MonoBehaviour {
	//weight reduce

	public static InventorySkillInfo GetSkillInfoByID(int id){
		MethodInfo method = typeof(InventorySkills).GetMethod ("skillInfo_" + id.ToString (), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		InventorySkillInfo type = (InventorySkillInfo)method.Invoke (null, null);
		return type;
	}

	public static void UseSkill(InventorySkillInfo info){
		MethodInfo method = typeof(InventorySkills).GetMethod ("skill_" + info.skillID.ToString (), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		method.Invoke (null, null);
	}

	static void skill_1(){
		foreach (BackpackItem item in PlayerController.backPackItems) {
			item.weight = item.weight * 0.5f;
		}
		BackpackView.backpackView.RecomputeWeight ();
		if (InterfaceSystemController.interfaceSystemController.equipmentController.equipmentInterface.activeInHierarchy) {
			BackpackView.backpackView.RedrawBackpack ();
		}
	}
	static InventorySkillInfo skillInfo_1(){
		InventorySkillInfo info = new InventorySkillInfo ();
		info.skillID = 1;
		info.pricePercent = 0.7f;
		info.weight = 0.05f;
		return info;
	}


	static void skill_2(){
		PlayerController.maximumWeight += 50;
		BackpackView.backpackView.RedrawBackpack ();
	}
	static InventorySkillInfo skillInfo_2(){
		InventorySkillInfo info = new InventorySkillInfo ();
		info.skillID = 2;
		info.pricePercent = 2f;
		info.weight = 2f;
		return info;
	}
}
