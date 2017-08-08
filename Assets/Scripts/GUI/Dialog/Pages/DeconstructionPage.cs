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
		equipHealth.text = System.Math.Round(Stats.GetPlusHealthByPoints(equip.healthPoints), 2).ToString();
		equipPhysicalDamage.text = System.Math.Round(Stats.GetPlusPhysicalDamageByPoints(equip.physicalDamagePoints), 2).ToString();
		equipElementalDamage.text = System.Math.Round(Stats.GetPlusElementalDamageByPoints(equip.elementalDamagePoints), 2).ToString();
		equipCritical.text = System.Math.Round(Stats.GetPlusCriticalByPoints(equip.criticalPoints), 2).ToString();
		equipPhysicalArmor.text = System.Math.Round(Stats.GetPlusPhysicalArmorByPoints(equip.physicalArmorPoints), 2).ToString();
		equipElementalArmor.text = System.Math.Round(Stats.GetPlusElementalArmorByPoints(equip.elementalArmorPoints), 2).ToString();
	}

	public void SetRuneStats(EquipmentRune rune){
		runeIconObject.SetActive (true);
		runeIcon.sprite = rune.GetIcon ();
		runeHealth.text = System.Math.Round(Stats.GetPlusHealthByPoints(rune.healthPoints), 2).ToString();
		runePhysicalDamage.text = System.Math.Round(Stats.GetPlusPhysicalDamageByPoints(rune.physicalDamagePoints), 2).ToString();
		runeElementalDamage.text = System.Math.Round(Stats.GetPlusElementalDamageByPoints(rune.elementalDamagePoints), 2).ToString();
		runeCritical.text = System.Math.Round(Stats.GetPlusCriticalByPoints(rune.criticalPoints), 2).ToString();
		runePhysicalArmor.text = System.Math.Round(Stats.GetPlusPhysicalArmorByPoints(rune.physicalArmorPoints), 2).ToString();
		runeElementalArmor.text = System.Math.Round(Stats.GetPlusElementalArmorByPoints(rune.elementalArmorPoints), 2).ToString();
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
