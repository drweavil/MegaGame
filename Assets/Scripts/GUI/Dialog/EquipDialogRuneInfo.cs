using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EquipDialogRuneInfo : MonoBehaviour {
	public Text title;

	public GameObject physicalDamage;
	public Text physicalDamageText;
	public GameObject elementalDamage;
	public Text elementalDamageText;
	public GameObject critical;
	public Text criticalText;
	public GameObject health;
	public Text healthText;
	public GameObject physicalArmor;
	public Text physicalArmorText;
	public GameObject elementalArmor;
	public Text elementalArmorText;

	public Text description;

	public Text weight;
	public Text price;

	public ScrollRectUpdater scrollRectUpdater;



	public void SetInfo(BackpackItem item){
		DeactivateAllStats ();
		EquipmentRune rune = item.itemContent[0] as EquipmentRune;
		title.text = rune.GetTitle ();
		if (rune.healthPoints != 0) {
			health.SetActive (true);
			healthText.text = System.Math.Round(Stats.GetPlusHealthByPoints (rune.healthPoints), 0).ToString();
		}

		if (rune.physicalDamagePoints != 0) {
			physicalDamage.SetActive (true);
			physicalDamageText.text = System.Math.Round(Stats.GetPlusPhysicalDamageByPoints (rune.physicalDamagePoints), 2).ToString();
		}
			

		if (rune.elementalDamagePoints != 0) {
			elementalDamage.SetActive (true);
			elementalDamageText.text = System.Math.Round(Stats.GetPlusElementalDamageByPoints (rune.elementalDamagePoints), 2).ToString();
		}

		if (rune.criticalPoints != 0) {
			critical.SetActive (true);
			criticalText.text = System.Math.Round(Stats.GetPlusCriticalByPoints (rune.criticalPoints), 2).ToString();
		}

		if (rune.physicalArmorPoints != 0) {
			physicalArmor.SetActive (true);
			physicalArmorText.text = System.Math.Round(Stats.GetPlusPhysicalArmorByPoints (rune.physicalArmorPoints), 2).ToString();
		}

		if (rune.elementalArmorPoints != 0) {
			elementalArmor.SetActive (true);
			elementalArmorText.text = System.Math.Round(Stats.GetPlusElementalArmorByPoints (rune.elementalArmorPoints), 2).ToString();
		}

		description.text = rune.GetDesctiption ();

		weight.text = System.Math.Round(item.weight, 2).ToString ();
		price.text = System.Math.Round(item.price, 2).ToString ();
		scrollRectUpdater.UpdateRect ();
	}

	public void DeactivateAllStats(){
		physicalDamage.gameObject.SetActive (false);
		elementalDamage.gameObject.SetActive (false);
		health.gameObject.SetActive (false);
		critical.gameObject.SetActive (false);
		physicalArmor.gameObject.SetActive (false);
		elementalArmor.gameObject.SetActive (false);
	}
}
