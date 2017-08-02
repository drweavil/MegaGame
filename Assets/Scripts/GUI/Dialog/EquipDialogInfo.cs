using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipDialogInfo : MonoBehaviour {
	public GameObject equipDialogInfo;

	public GameObject info;

	public GameObject title;
	public Text titleText;

	public GameObject runeLine;

	public GameObject rune1Cell;
	public GameObject rune1;
	public GameObject rune1Empty;
	public Image rune1Image;

	public GameObject rune2Cell;
	public GameObject rune2;
	public GameObject rune2Empty;
	public Image rune2Image;

	public GameObject healthRune1;
	public GameObject healthRune2;
	public Text healthText;
	public Text healthRune1Text;
	public Text healthRune2Text;
	public Text healthTotal;

	public GameObject physicalDamageRune1;
	public GameObject physicalDamageRune2;
	public Text physicalDamageText;
	public Text physicalDamageRune1Text;
	public Text physicalDamageRune2Text;
	public Text physicalDamageTotal;

	public GameObject elementalDamageRune1;
	public GameObject elementalDamageRune2;
	public Text elementalDamageText;
	public Text elementalDamageRune1Text;
	public Text elementalDamageRune2Text;
	public Text elementalDamageTotal;

	public GameObject criticalRune1;
	public GameObject criticalRune2;
	public Text criticalText;
	public Text criticalRune1Text;
	public Text criticalRune2Text;
	public Text criticalTotal;

	public GameObject physicalArmorRune1;
	public GameObject physicalArmorRune2;
	public Text physicalArmorText;
	public Text physicalArmorRune1Text;
	public Text physicalArmorRune2Text;
	public Text physicalArmorTotal;

	public GameObject elementalArmorRune1;
	public GameObject elementalArmorRune2;
	public Text elementalArmorText;
	public Text elementalArmorRune1Text;
	public Text elementalArmorRune2Text;
	public Text elementalArmorTotal;

	public GameObject weaponDamageLine;
	public GameObject weaponDamageRune1;
	public GameObject weaponDamageRune2;
	public Text weaponDamageText;
	public Text weaponDamageTotal;

	public GameObject weightPriceLine;
	public Text weightText;
	public GameObject price;
	public Text priceText;

	public GameObject description;
	public Text descriptionText;

	public ScrollRectUpdater scrollRectUpdater;



	public void SetStatsByEquip(Equipment equip, Equipment sweepingEquip, BackpackItem item = null/*, bool withPrice = false*/){
		DisactiveAllLinesAndValues ();

		titleText.text = equip.title;
		title.SetActive (true);

		if (equip.rune1Enable || equip.rune2Enable) {
			runeLine.SetActive (true);
			if (equip.rune1Enable) {
				rune1Cell.SetActive (true);
				if (equip.rune1 == null) {
					rune1Empty.SetActive (true);
				} else {
					rune1.SetActive (true);
					rune1Image.sprite = SkillPanelController.skillPanelController.GetSkillTexture ("rune_"+equip.rune1.skinID);
				}
			}

			if (equip.rune2Enable) {
				rune2Cell.SetActive (true);
				if (equip.rune2 == null) {
					rune2Empty.SetActive (true);
				} else {
					rune2.SetActive (true);
					rune2Image.sprite = SkillPanelController.skillPanelController.GetSkillTexture ("rune_"+equip.rune2.skinID);
				}
			}
		}

		SetHealthLine (equip, sweepingEquip);
		SetPhysicalDamageLine (equip, sweepingEquip);
		SetElementalDamageLine (equip, sweepingEquip);
		SetCriticalLine (equip, sweepingEquip);
		SetPhysicalArmorLine (equip, sweepingEquip);
		SetElementalArmorLine (equip, sweepingEquip);
		if (equip.slotID == Equipment.meleeWeapon ||
		   equip.slotID == Equipment.fireWeapon ||
		   equip.slotID == Equipment.elementalWeapon) {
			SetWeaponDamageLine (equip, sweepingEquip);
		}


		weightText.text = System.Math.Round (equip.weight, 2).ToString();
		priceText.text = System.Math.Round (equip.price, 2).ToString();
		/*if (item != null) {
			weightPriceLine.SetActive (true);
			weightText.text = System.Math.Round (item.weight, 2).ToString();
			if (withPrice) {
				price.SetActive (true);
				priceText.text = System.Math.Round (item.price, 2).ToString();
			}
		}*/

		descriptionText.text = equip.description;
		description.SetActive (true);
		scrollRectUpdater.UpdateRect ();
	}


	void SetHealthLine(Equipment equip, Equipment sweepingEquip){
		float equipPercent = Stats.GetPlusHealthByPoints (equip.healthPoints); 
		float rune1Percent = 0;
		float rune2Percent = 0;

		if (equip.rune1Enable) {
			healthRune1.SetActive (true);
			if (equip.rune1 != null) {
				rune1Percent = Stats.GetPlusHealthByPoints (equip.rune1.healthPoints);
			}
		}
		if (equip.rune2Enable) {
			healthRune2.SetActive (true);
			if (equip.rune2 != null) {
				rune2Percent = Stats.GetPlusHealthByPoints (equip.rune2.healthPoints);
			}
		}

		healthText.text = System.Math.Round(equipPercent, 0).ToString ();
		if (rune1Percent == 0) {
			healthRune1Text.text = "-";
		} else {
			healthRune1Text.text = System.Math.Round (rune1Percent, 0).ToString ();
		}


		if (rune2Percent == 0) {
			healthRune2Text.text = "-";
		} else {
			healthRune2Text.text = System.Math.Round (rune2Percent, 0).ToString ();
		}

		float totalChange = equipPercent + rune1Percent + rune2Percent;

		totalChange -= Stats.GetPlusHealthByPoints (sweepingEquip.GetOverallHealthPoints ());

		if (totalChange < 0) {
			healthTotal.color = DialogController.minusColor;
			healthTotal.text = System.Math.Round (totalChange, 0).ToString ();
		} else if (totalChange > 0) {
			healthTotal.color = DialogController.plusColor;
			healthTotal.text = "+" + System.Math.Round (totalChange, 0).ToString ();
		} else {
			healthTotal.color = DialogController.statColor;
			healthTotal.text = System.Math.Round (totalChange, 0).ToString ();
		}
	}

	void SetPhysicalDamageLine(Equipment equip, Equipment sweepingEquip){
		float equipPercent = Stats.GetPlusPhysicalDamageByPoints (equip.physicalDamagePoints); 
		float rune1Percent = 0;
		float rune2Percent = 0;

		if (equip.rune1Enable) {
			physicalDamageRune1.SetActive (true);
			if (equip.rune1 != null) {
				rune1Percent = Stats.GetPlusPhysicalDamageByPoints (equip.rune1.physicalDamagePoints);
			}
		}
		if (equip.rune2Enable) {
			physicalDamageRune2.SetActive (true);
			if (equip.rune2 != null) {
				rune2Percent = Stats.GetPlusPhysicalDamageByPoints (equip.rune2.physicalDamagePoints);
			}
		}

		physicalDamageText.text = System.Math.Round(equipPercent, 2).ToString ();
		if (rune1Percent == 0) {
			physicalDamageRune1Text.text = "-";
		} else {
			physicalDamageRune1Text.text = System.Math.Round (rune1Percent, 2).ToString ();
		}


		if (rune2Percent == 0) {
			physicalDamageRune2Text.text = "-";
		} else {
			physicalDamageRune2Text.text = System.Math.Round (rune2Percent, 2).ToString ();
		}

		float totalChange = equipPercent + rune1Percent + rune2Percent;

		totalChange -= Stats.GetPlusPhysicalDamageByPoints (sweepingEquip.GetOverallPhysicalDamagePoints ());

		if (totalChange < 0) {
			physicalDamageTotal.color = DialogController.minusColor;
			physicalDamageTotal.text = System.Math.Round (totalChange, 2).ToString ();
		} else if (totalChange > 0) {
			physicalDamageTotal.color = DialogController.plusColor;
			physicalDamageTotal.text = "+" + System.Math.Round (totalChange, 2).ToString ();
		} else {
			physicalDamageTotal.color = DialogController.statColor;
			physicalDamageTotal.text = System.Math.Round (totalChange, 2).ToString ();
		}
	} 

	void SetElementalDamageLine(Equipment equip, Equipment sweepingEquip){
		float equipPercent = Stats.GetPlusElementalDamageByPoints (equip.elementalDamagePoints); 
		float rune1Percent = 0;
		float rune2Percent = 0;

		if (equip.rune1Enable) {
			elementalDamageRune1.SetActive (true);
			if (equip.rune1 != null) {
				rune1Percent = Stats.GetPlusElementalDamageByPoints (equip.rune1.elementalDamagePoints);
			}
		}
		if (equip.rune2Enable) {
			elementalDamageRune2.SetActive (true);
			if (equip.rune2 != null) {
				rune2Percent = Stats.GetPlusElementalDamageByPoints (equip.rune2.elementalDamagePoints);
			}
		}

		elementalDamageText.text = System.Math.Round(equipPercent, 2).ToString ();
		if (rune1Percent == 0) {
			elementalDamageRune1Text.text = "-";
		} else {
			elementalDamageRune1Text.text = System.Math.Round (rune1Percent, 2).ToString ();
		}


		if (rune2Percent == 0) {
			elementalDamageRune2Text.text = "-";
		} else {
			elementalDamageRune2Text.text = System.Math.Round (rune2Percent, 2).ToString ();
		}

		float totalChange = equipPercent + rune1Percent + rune2Percent;

		totalChange -= Stats.GetPlusElementalDamageByPoints (sweepingEquip.GetOverallElementalDamagePoints ());

		if (totalChange < 0) {
			elementalDamageTotal.color = DialogController.minusColor;
			elementalDamageTotal.text = System.Math.Round (totalChange, 2).ToString ();
		} else if (totalChange > 0) {
			elementalDamageTotal.color = DialogController.plusColor;
			elementalDamageTotal.text = "+" + System.Math.Round (totalChange, 2).ToString ();
		} else {
			elementalDamageTotal.color = DialogController.statColor;
			elementalDamageTotal.text = System.Math.Round (totalChange, 2).ToString ();
		}
	} 

	void SetCriticalLine(Equipment equip, Equipment sweepingEquip){
		float equipPercent = Stats.GetPlusCriticalByPoints (equip.criticalPoints); 
		float rune1Percent = 0;
		float rune2Percent = 0;

		if (equip.rune1Enable) {
			criticalRune1.SetActive (true);
			if (equip.rune1 != null) {
				rune1Percent = Stats.GetPlusCriticalByPoints (equip.rune1.criticalPoints);
			}
		}
		if (equip.rune2Enable) {
			criticalRune2.SetActive (true);
			if (equip.rune2 != null) {
				rune2Percent = Stats.GetPlusCriticalByPoints (equip.rune2.criticalPoints);
			}
		}

		criticalText.text = System.Math.Round(equipPercent, 2).ToString ();
		if (rune1Percent == 0) {
			criticalRune1Text.text = "-";
		} else {
			criticalRune1Text.text = System.Math.Round (rune1Percent, 2).ToString ();
		}


		if (rune2Percent == 0) {
			criticalRune2Text.text = "-";
		} else {
			criticalRune2Text.text = System.Math.Round (rune2Percent, 2).ToString ();
		}

		float totalChange = equipPercent + rune1Percent + rune2Percent;

		totalChange -= Stats.GetPlusCriticalByPoints (sweepingEquip.GetOverallCriticalPoints ());

		if (totalChange < 0) {
			criticalTotal.color = DialogController.minusColor;
			criticalTotal.text = System.Math.Round (totalChange, 2).ToString ();
		} else if (totalChange > 0) {
			criticalTotal.color = DialogController.plusColor;
			criticalTotal.text = "+" + System.Math.Round (totalChange, 2).ToString ();
		} else {
			criticalTotal.color = DialogController.statColor;
			criticalTotal.text = System.Math.Round (totalChange, 2).ToString ();
		}
	} 

	void SetPhysicalArmorLine(Equipment equip, Equipment sweepingEquip){
		float equipPercent = Stats.GetPlusPhysicalArmorByPoints (equip.physicalArmorPoints); 
		float rune1Percent = 0;
		float rune2Percent = 0;

		if (equip.rune1Enable) {
			physicalArmorRune1.SetActive (true);
			if (equip.rune1 != null) {
				rune1Percent = Stats.GetPlusPhysicalArmorByPoints (equip.rune1.physicalArmorPoints);
			}
		}
		if (equip.rune2Enable) {
			physicalArmorRune2.SetActive (true);
			if (equip.rune2 != null) {
				rune2Percent = Stats.GetPlusPhysicalArmorByPoints (equip.rune2.physicalArmorPoints);
			}
		}

		physicalArmorText.text = System.Math.Round(equipPercent, 2).ToString ();
		if (rune1Percent == 0) {
			physicalArmorRune1Text.text = "-";
		} else {
			physicalArmorRune1Text.text = System.Math.Round (rune1Percent, 2).ToString ();
		}


		if (rune2Percent == 0) {
			physicalArmorRune2Text.text = "-";
		} else {
			physicalArmorRune2Text.text = System.Math.Round (rune2Percent, 2).ToString ();
		}

		float totalChange = equipPercent + rune1Percent + rune2Percent;

		totalChange -= Stats.GetPlusPhysicalArmorByPoints (sweepingEquip.GetOverallPhysicalArmorPoints ());

		if (totalChange < 0) {
			physicalArmorTotal.color = DialogController.minusColor;
			physicalArmorTotal.text = System.Math.Round (totalChange, 2).ToString ();
		} else if (totalChange > 0) {
			physicalArmorTotal.color = DialogController.plusColor;
			physicalArmorTotal.text = "+" + System.Math.Round (totalChange, 2).ToString ();
		} else {
			physicalArmorTotal.color = DialogController.statColor;
			physicalArmorTotal.text = System.Math.Round (totalChange, 2).ToString ();
		}
	} 

	void SetElementalArmorLine(Equipment equip, Equipment sweepingEquip){
		float equipPercent = Stats.GetPlusElementalArmorByPoints (equip.elementalArmorPoints); 
		float rune1Percent = 0;
		float rune2Percent = 0;

		if (equip.rune1Enable) {
			elementalArmorRune1.SetActive (true);
			if (equip.rune1 != null) {
				rune1Percent = Stats.GetPlusElementalArmorByPoints (equip.rune1.elementalArmorPoints);
			}
		}
		if (equip.rune2Enable) {
			elementalArmorRune2.SetActive (true);
			if (equip.rune2 != null) {
				rune2Percent = Stats.GetPlusElementalArmorByPoints (equip.rune2.elementalArmorPoints);
			}
		}

		elementalArmorText.text = System.Math.Round(equipPercent, 2).ToString ();
		if (rune1Percent == 0) {
			elementalArmorRune1Text.text = "-";
		} else {
			elementalArmorRune1Text.text = System.Math.Round (rune1Percent, 2).ToString ();
		}


		if (rune2Percent == 0) {
			elementalArmorRune2Text.text = "-";
		} else {
			elementalArmorRune2Text.text = System.Math.Round (rune2Percent, 2).ToString ();
		}

		float totalChange = equipPercent + rune1Percent + rune2Percent;

		totalChange -= Stats.GetPlusElementalArmorByPoints (sweepingEquip.GetOverallElementalArmorPoints ());

		if (totalChange < 0) {
			elementalArmorTotal.color = DialogController.minusColor;
			elementalArmorTotal.text = System.Math.Round (totalChange, 2).ToString ();
		} else if (totalChange > 0) {
			elementalArmorTotal.color = DialogController.plusColor;
			elementalArmorTotal.text = "+" + System.Math.Round (totalChange, 2).ToString ();
		} else {
			elementalArmorTotal.color = DialogController.statColor;
			elementalArmorTotal.text = System.Math.Round (totalChange, 2).ToString ();
		}
	} 

	void SetWeaponDamageLine(Equipment equip, Equipment sweepingEquip){
		weaponDamageLine.SetActive (true);
		float equipPercent = equip.weaponDamage; 
		weaponDamageText.text = System.Math.Round(equipPercent, 2).ToString ();
		float totalChange = equipPercent;

		totalChange -= sweepingEquip.weaponDamage;

		if (equip.rune1Enable) {
			weaponDamageRune1.SetActive (true);
		}
		if (equip.rune2Enable) {
			weaponDamageRune2.SetActive (true);
		}


		if (totalChange < 0) {
			weaponDamageTotal.color = DialogController.minusColor;
			weaponDamageTotal.text = System.Math.Round (totalChange, 2).ToString ();
		} else if (totalChange > 0) {
			weaponDamageTotal.color = DialogController.plusColor;
			weaponDamageTotal.text = "+" + System.Math.Round (totalChange, 2).ToString ();
		} else {
			weaponDamageTotal.color = DialogController.statColor;
			weaponDamageTotal.text = System.Math.Round (totalChange, 2).ToString ();
		}
	} 


	void DisactiveAllLinesAndValues(){
		title.SetActive (false);

		runeLine.SetActive (false);
		rune1Cell.SetActive (false);
		rune1.SetActive (false);
		rune1Empty.SetActive (false);
		rune2Cell.SetActive (false);
		rune2.SetActive (false);
		rune2Empty.SetActive (false);

		//healthLine.SetActive (false);
		healthRune1.SetActive (false);
		healthRune2.SetActive (false);

		//physicalDamageLine.SetActive (false);
		physicalDamageRune1.SetActive (false);
		physicalDamageRune2.SetActive (false);

		//elementalDamageLine.SetActive (false);
		elementalDamageRune1.SetActive (false);
		elementalDamageRune2.SetActive (false);

		//criticalLine.SetActive (false);
		criticalRune1.SetActive (false);
		criticalRune2.SetActive (false);

		//physicalArmorLine.SetActive (false);
		physicalArmorRune1.SetActive (false);
		physicalArmorRune2.SetActive (false);

		//elementalArmorLine.SetActive (false);
		elementalArmorRune1.SetActive (false);
		elementalArmorRune2.SetActive (false);

		weaponDamageLine.SetActive (false);
		weaponDamageRune1.SetActive (false);
		weaponDamageRune2.SetActive (false);

		/*weightPriceLine.SetActive (false);
		price.SetActive (false);*/

		description.SetActive (false);
	}
}
