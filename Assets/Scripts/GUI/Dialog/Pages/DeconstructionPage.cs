using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeconstructionPage : MonoBehaviour {
	public Image equipIcon;
	public Text equipHealth;
	public Text equipPhysicalDamage;
	public Text equipElementalDamage;
	public Text equipCritical;
	public Text equipPhysicalArmor;
	public Text equipElementalArmor;

	public Image runeIcon;
	public GameObject runeIconObject;
	public Text runeHealth;
	public Text runePhysicalDamage;
	public Text runeElementalDamage;
	public Text runeCritical;
	public Text runePhysicalArmor;
	public Text runeElementalArmor;


	public Text hammer1Value;
	public Text hammer2Value;
	public Text hammer3Value;


	public static int hammer1ValueInt = 0;
	public static int hammer2ValueInt = 0;
	public static int hammer3ValueInt = 0;



	public void SetEquipStats(Equipment equip){
		equipIcon.sprite = equip.GetIcon ();
		equipHealth.text = Stats.GetPlusHealthByPoints(equip.healthPoints).ToString();
		equipPhysicalDamage.text = Stats.GetPlusPhysicalDamageByPoints(equip.physicalDamagePoints).ToString();
		equipElementalDamage.text = Stats.GetPlusElementalDamageByPoints(equip.elementalDamagePoints).ToString();
		equipCritical.text = Stats.GetPlusCriticalByPoints(equip.criticalPoints).ToString();
		equipPhysicalArmor.text = Stats.GetPlusPhysicalArmorByPoints(equip.physicalArmorPoints).ToString();
		equipElementalArmor.text = Stats.GetPlusElementalArmorByPoints(equip.elementalArmorPoints).ToString();
	}

	public void SetRuneStats(EquipmentRune rune){
		runeIconObject.SetActive (true);
		runeIcon.sprite = rune.GetIcon ();
		runeHealth.text = Stats.GetPlusHealthByPoints(rune.healthPoints).ToString();
		runePhysicalDamage.text = Stats.GetPlusPhysicalDamageByPoints(rune.physicalDamagePoints).ToString();
		runeElementalDamage.text = Stats.GetPlusElementalDamageByPoints(rune.elementalDamagePoints).ToString();
		runeCritical.text = Stats.GetPlusCriticalByPoints(rune.criticalPoints).ToString();
		runePhysicalArmor.text = Stats.GetPlusPhysicalArmorByPoints(rune.physicalArmorPoints).ToString();
		runeElementalArmor.text = Stats.GetPlusElementalArmorByPoints(rune.elementalArmorPoints).ToString();
	}


	public void SetNullRune(){
		runeIconObject.SetActive (false);
		runeHealth.text = "-";
		runePhysicalDamage.text = "-";
		runeElementalDamage.text = "-";
		runeCritical.text = "-";
		runePhysicalArmor.text = "-";
		runeElementalArmor.text = "-";
	}

	public void RecomputeHammerValues(){
		List<BackpackItem> hammers = PlayerController.backPackItems.FindAll (bi => {
			if (bi.itemContent[0].GetType () == typeof(Consumable)) {
				Consumable consumable = bi.itemContent[0] as Consumable;
				if (consumable.consumableType == Consumable.hammer) {
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}
		});

		//Debug.Log (hammers.Count);
		hammer1ValueInt = 0;
		hammer2ValueInt = 0;
		hammer3ValueInt = 0;

		foreach (BackpackItem item in hammers) {
			Consumable cons = item.itemContent[0] as Consumable;

			int itemCount = item.itemCount;
			/*if (item.itemCount == 0) {
				itemCount = 1;
			}*/
			if (cons.consumableSubType == 1) {
				
				hammer1ValueInt += itemCount;
			}
			if (cons.consumableSubType == 2) {
				hammer2ValueInt += itemCount;
			}
			if (cons.consumableSubType == 3) {
				hammer3ValueInt += itemCount;
			}
		}

		hammer1Value.text = hammer1ValueInt.ToString ();
		hammer2Value.text = hammer2ValueInt.ToString ();
		hammer3Value.text = hammer3ValueInt.ToString ();
	}


	public void SetDeactivateHammers(){
		HammerButtonsSystem.DeactivateAll ();
		DialogController.DeactivateButtons ();
	}



}
