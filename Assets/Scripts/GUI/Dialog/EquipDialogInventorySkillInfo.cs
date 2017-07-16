using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipDialogInventorySkillInfo : MonoBehaviour {
	public Text title;
	public Text description;
	public Text weight;
	public Text price;
	public ScrollRectUpdater scrollRectUpdater;


	public void SetInfo(BackpackItem item){
		InventorySkill skill = (InventorySkill)item.itemContent [0];
		title.text = skill.GetTitle ();
		description.text = skill.GetDescription ();
		weight.text = System.Math.Round (item.weight, 2).ToString();
		price.text = System.Math.Round (item.price, 2).ToString();
		scrollRectUpdater.UpdateRect ();
	}
}
