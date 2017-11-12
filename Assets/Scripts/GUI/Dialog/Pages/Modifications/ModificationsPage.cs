using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ModificationsPage : MonoBehaviour {
	public GameObject rune1;
	public GameObject rune2;
	public static ModificationsPage modificationsPage;

	public Image rune1Image;
	public GameObject rune1EmptySlot;
	public GameObject rune1FilledSlot;

	public Text rune1HealthText;
	public Text rune1PhysicalDamageText;
	public Text rune1ElementalDamageText;
	public Text rune1CriticalText;
	public Text rune1PhysicalArmorText;
	public Text rune1ElementalArmorText;
	public LumenArea rune1Area;

	public Image rune2Image;
	public GameObject rune2EmptySlot;
	public GameObject rune2FilledSlot;

	public Text rune2HealthText;
	public Text rune2PhysicalDamageText;
	public Text rune2ElementalDamageText;
	public Text rune2CriticalText;
	public Text rune2PhysicalArmorText;
	public Text rune2ElementalArmorText;
	public LumenArea rune2Area;


	public List<BackpackItem> runeItems = new List<BackpackItem> ();

	public List<RuneButton> runeButtons = new List<RuneButton> ();

	public List<ModificationsFilterButton> filterButtons = new List<ModificationsFilterButton>();

	int pageItemsCount = 6;
	public int currenRuneItemsPage = 1;
	public int maximumRuneItemsPage = 0;


	public RuneButton draggedRune;
	public int draggedBackpackItemID;
	public static EquipmentRune currentDraggedRune;
	public List<LumenArea> areas = new List<LumenArea>();
	public EquipmentRune equipmentRune1Slot;
	public int equipmentRune1BackpackItemID = -1;
	public EquipmentRune equipmentRune2Slot;
	public int equipmentRune2BackpackItemID = -1;
	public List<int> removingBackpackItems = new List<int>();

	public void Awake(){
		modificationsPage = this;
	}


	public void DeactivateAreas(){
		foreach(LumenArea area in areas){
			area.lumen.SetActive (false);
		}
	}

	public void NullBackpackItemsIDs(){
		equipmentRune1BackpackItemID = -1;
		equipmentRune2BackpackItemID = -1;
	}

	public void ReDeactivateRuneButtons(){
		//removingBackpackItems.Clear ();
		foreach (RuneButton button in runeButtons) {
			if (ModificationsPage.modificationsPage.equipmentRune1Slot != button.buttonRune &&
			    ModificationsPage.modificationsPage.equipmentRune2Slot != button.buttonRune) {
				button.deactivated.SetActive (false);
				button.runeDeactivated = false;
				/*if (removingBackpackItems.FindIndex (b => b == button.backpackItemID) != -1) {
					removingBackpackItems.Remove (button.backpackItemID);
				}*/
			} else {
				button.deactivated.SetActive (true);
				button.runeDeactivated = true;
				/*if (removingBackpackItems.FindIndex (b => b == button.backpackItemID) == -1) {
					removingBackpackItems.Add (button.backpackItemID);
				}*/
			}
		}
	}

	public void SetRunesByEquip(Equipment equip){
		if (equip.rune1Enable) {
			rune1.SetActive (true);
			if (equip.rune1 != null) {
				SetRune1 (equip.rune1);
			} else {
				SetRune1Empty ();
			}
			rune1Area.deactive = false;
		} else {
			rune1.SetActive (false);
			equipmentRune1Slot = null;
			rune1Area.deactive = true;
		}

		if (equip.rune2Enable) {
			rune2.SetActive (true);
			if (equip.rune2 != null) {
				SetRune2 (equip.rune2);
			} else {
				SetRune2Empty ();
			}
			rune2Area.deactive = false;
		} else {
			rune2.SetActive (false);
			equipmentRune2Slot = null;
			rune2Area.deactive = true;
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
		equipmentRune1Slot = rune;
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
		equipmentRune1Slot = null;
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
		equipmentRune2Slot = rune;
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
		equipmentRune2Slot = null;
	}


	public void SetNewRuneToSlot1(){
		equipmentRune1Slot = currentDraggedRune;
		equipmentRune1BackpackItemID = draggedBackpackItemID;
		SetRune1 (currentDraggedRune);
		DialogController.DeactivateButtons ();
		DialogController.dialogController.modificationButton.SetActive (true);

	}

	public void SetNewRuneToSlot2(){
		equipmentRune2Slot = currentDraggedRune;
		equipmentRune2BackpackItemID = draggedBackpackItemID;
		SetRune2 (currentDraggedRune);
		DialogController.DeactivateButtons ();
		DialogController.dialogController.modificationButton.SetActive (true);
	}

	public void FindRunes(){
		runeItems = PlayerController.backPackItems.FindAll (b => b.itemContent[0].GetType() == typeof(EquipmentRune));
	}

	public void SortRunesByStatID(int statID){
		if (statID == Stats.healthStatID) {
			runeItems = runeItems.OrderBy (r => ((EquipmentRune)r.itemContent [0]).healthPoints).ToList ();
		} else if(statID == Stats.critStatID) {
			runeItems = runeItems.OrderBy (r => ((EquipmentRune)r.itemContent [0]).criticalPoints).ToList ();
		} else if(statID == Stats.physicalDamageStatID) {
			runeItems = runeItems.OrderBy (r => ((EquipmentRune)r.itemContent [0]).physicalDamagePoints).ToList ();
		} else if(statID == Stats.elementalDamageStatID) {
			runeItems = runeItems.OrderBy (r => ((EquipmentRune)r.itemContent [0]).elementalDamagePoints).ToList ();
		} else if(statID == Stats.physicalArmorStatID) {
			runeItems = runeItems.OrderBy (r => ((EquipmentRune)r.itemContent [0]).physicalArmorPoints).ToList ();
		} else if(statID == Stats.elementalDamageStatID) {
			runeItems = runeItems.OrderBy (r => ((EquipmentRune)r.itemContent [0]).elementalDamagePoints).ToList ();
		}

		runeItems.Reverse ();

	}

	public void DrawRunes(){
		DeactivateRuneButtons ();
		List<BackpackItem> items = GetRuneItemsByPage (currenRuneItemsPage);

		for (int i = 0; i < items.Count; i++) {
			EquipmentRune rune = items [i].itemContent [0] as EquipmentRune;
			runeButtons [i].gameObject.SetActive (true);
			runeButtons [i].backpackItemID = items [i].itemID;
			runeButtons [i].SetRune (rune);
		}
		ReDeactivateRuneButtons ();
	}

	public void DeactivateRuneButtons(){
		foreach (RuneButton runeButton in runeButtons) {
			runeButton.gameObject.SetActive (false);
		}
	}

	public void DeactivateRuneFilterButtons(){
		foreach (ModificationsFilterButton button in filterButtons) {
			button.activated.SetActive (false);
		}
	}

	public  List<BackpackItem> GetRuneItemsByPage(int page){
		int size = 0;

		size = runeItems.Count;
		int index = (page * pageItemsCount) - pageItemsCount;
		int count = 0;
		int deltaPageSize = size - page * pageItemsCount;
		if (deltaPageSize >= 0) {
			count = pageItemsCount;
		}
		if (deltaPageSize < 0) {
			count = pageItemsCount + deltaPageSize;
		}

		return runeItems.GetRange (index, count);

	}


	public void RecomputePages(){
		maximumRuneItemsPage = GetPagesCount ();
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


	public void RuneItemsPageDown(){
		if (currenRuneItemsPage == 1) {
			currenRuneItemsPage = maximumRuneItemsPage;
		} else {
			currenRuneItemsPage -= 1;
		}

		DrawRunes ();
	}

	public void RuneItemsPageUp(){
		if (currenRuneItemsPage == maximumRuneItemsPage) {
			currenRuneItemsPage = 1;
		} else {
			currenRuneItemsPage += 1;
		}

		DrawRunes ();
	}
}
