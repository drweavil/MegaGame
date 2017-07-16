using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackpackButtonInventorySkillContent : MonoBehaviour {
	public Text title;
	public Text weight;
	public Text price;


	public void SetInfo(BackpackItem item){
		InventorySkill skill = item.itemContent [0] as InventorySkill;
		title.text = skill.GetTitle ();
		weight.text = System.Math.Round(item.weight, 2).ToString();
		price.text = System.Math.Round(item.price, 2).ToString();
	}
}
