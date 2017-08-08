using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModificationsPage : MonoBehaviour {
	public GameObject rune1;
	public GameObject rune2;

	public Image rune1Image;
	public GameObject rune1EmptySlot;
	public GameObject rune1FilledSlot;

	public Text rune1HealthText;
	public Text rune1PhysicalDamageText;
	public Text rune1ElementalDamageText;
	public Text rune1CriticalText;
	public Text rune1PhysicalArmorText;
	public Text rune1ElementalArmorText;

	public Image rune2Image;
	public GameObject rune2EmptySlot;
	public GameObject rune2FilledSlot;

	public Text rune2HealthText;
	public Text rune2PhysicalDamageText;
	public Text rune2ElementalDamageText;
	public Text rune2CriticalText;
	public Text rune2PhysicalArmorText;
	public Text rune2ElementalArmorText;


	public List<BackpackItem> runeItems = new List<BackpackItem> ();

	public List<RuneButton> runeButtons = new List<RuneButton> ();

	int pageItemsCount = 6;
	public int currenRuneItemsPage = 1;


	public void SetRunesByEquip(Equipment equip){
		if (equip.rune1Enable) {
			rune1.SetActive (true);
			if (equip.rune1 != null) {
				SetRune1 (equip.rune1);
			} else {
				SetRune1Empty ();
			}
		} else {
			rune1.SetActive (false);
		}

		if (equip.rune2Enable) {
			rune2.SetActive (true);
			if (equip.rune2 != null) {
				SetRune2 (equip.rune1);
			} else {
				SetRune2Empty ();
			}
		} else {
			rune2.SetActive (false);
		}
	}

	public void SetRune1(EquipmentRune rune){
		rune1EmptySlot.SetActive (false);
		rune1FilledSlot.SetActive (true);
		rune1Image.sprite = rune.GetIcon ();

		rune1HealthText.text = System.Math.Round (Stats.GetPlusHealthByPoints(rune.healthPoints), 0).ToString();
		rune1CriticalText.text = System.Math.Round (Stats.GetPlusCriticalByPoints(rune.criticalPoints), 2).ToString();
		rune1PhysicalDamageText.text = System.Math.Round (Stats.GetPlusPhysicalDamageByPoints(rune.physicalDamagePoints), 2).ToString();
		rune1ElementalDamageText.text = System.Math.Round (Stats.GetPlusElementalDamageByPoints(rune.elementalDamagePoints), 2).ToString();
		rune1PhysicalArmorText.text = System.Math.Round (Stats.GetPlusPhysicalArmorByPoints(rune.physicalArmorPoints), 2).ToString();
		rune1ElementalArmorText.text = System.Math.Round (Stats.GetPlusElementalArmorByPoints(rune.elementalArmorPoints), 2).ToString();
	}

	public void SetRune1Empty(){
		rune1EmptySlot.SetActive (true);
		rune1FilledSlot.SetActive (false);

		rune1HealthText.text = "-";
		rune1PhysicalDamageText.text = "-";
		rune1ElementalDamageText.text = "-";
		rune1CriticalText.text = "-";
		rune1PhysicalArmorText.text = "-";
		rune1ElementalArmorText.text = "-";
	}


	public void SetRune2(EquipmentRune rune){
		rune2EmptySlot.SetActive (false);
		rune2FilledSlot.SetActive (true);
		rune2Image.sprite = rune.GetIcon ();

		rune2HealthText.text = System.Math.Round (Stats.GetPlusHealthByPoints(rune.healthPoints), 0).ToString();
		rune2CriticalText.text = System.Math.Round (Stats.GetPlusCriticalByPoints(rune.criticalPoints), 2).ToString();
		rune2PhysicalDamageText.text = System.Math.Round (Stats.GetPlusPhysicalDamageByPoints(rune.physicalDamagePoints), 2).ToString();
		rune2ElementalDamageText.text = System.Math.Round (Stats.GetPlusElementalDamageByPoints(rune.elementalDamagePoints), 2).ToString();
		rune2PhysicalArmorText.text = System.Math.Round (Stats.GetPlusPhysicalArmorByPoints(rune.physicalArmorPoints), 2).ToString();
		rune2ElementalArmorText.text = System.Math.Round (Stats.GetPlusElementalArmorByPoints(rune.elementalArmorPoints), 2).ToString();
	}

	public void SetRune2Empty(){
		rune2EmptySlot.SetActive (true);
		rune2FilledSlot.SetActive (false);

		rune2HealthText.text = "-";
		rune2PhysicalDamageText.text = "-";
		rune2ElementalDamageText.text = "-";
		rune2CriticalText.text = "-";
		rune2PhysicalArmorText.text = "-";
		rune2ElementalArmorText.text = "-";
	}

	public void FindRunes(){
		runeItems = PlayerController.backPackItems.FindAll (b => b.itemContent[0].GetType() == typeof(EquipmentRune));
	}

	public void DrawRunes(){
		DeactivateRuneButtons ();
		List<BackpackItem> items = GetBackpackItemsByPage (currenRuneItemsPage);

		for (int i = 0; i < items.Count; i++) {
			EquipmentRune rune = items [i].itemContent [0] as EquipmentRune;
			runeButtons [i].gameObject.SetActive (true);
			runeButtons [i].SetRune (rune);
		}
	}

	public void DeactivateRuneButtons(){
		foreach (RuneButton runeButton in runeButtons) {
			runeButton.gameObject.SetActive (false);
		}
	}

	public  List<BackpackItem> GetBackpackItemsByPage(int page){
		int size = 0;

		size = runeItems.Count;
		int index = (page * pageItemsCount) - pageItemsCount;
		int count = 0;
		int deltaPageSize = size - page * pageItemsCount;
		if (deltaPageSize >= 0) {
			count = 4;
		}
		if (deltaPageSize < 0) {
			count = pageItemsCount + deltaPageSize;
		}

		return runeItems.GetRange (index, count);

	}

	public  int GetPagesCount(){
		int value = 0;
		int size = 0;
		size = runeItems.Count;
		if (size <= pageItemsCount) {
			value = 1;
		}

		if (size > pageItemsCount) {
			float floatPage = size / pageItemsCount;
			float roundPage = (float)System.Math.Floor (floatPage);

			if (size % pageItemsCount == 0) {
				value = (int)floatPage;
			} else {
				value = (int)(roundPage + 1);
			}
		}
		return value;
	}
}
