using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour {
	public static DialogController dialogController;
	public static Color32 plusColor = new Color32(118, 196, 118, 255);
	public static Color32 minusColor = new Color32(255, 12, 7, 255);
	public static Color32 statColor = new Color32(204, 224, 255, 255);
	public GameObject currentDialog;
	public int equipButtonIdInDialog;

	public BackpackItem currenBackpackItemInDialog;
	public int currentBuffID;
	public Equipment currentEquipmentInDialog;
	public bool currentEquipmentInDialogEquipped = false;
	public float currentRunePercent;
	public int currenHammerType;
	public EquipmentRune currentDeconstructionEquipmentRune;
	public int skillTalentID;

	public GameObject addTalentPointButton;
	public GameObject skillActivationButton;
	public GameObject deleteBackpackItemFromSlideDialog;
	public GameObject backpackAreaEquipButton;
	public GameObject backpackAreaDeleteItemButton;
	public GameObject equipmentSlotToBackpackButton;
	public GameObject buffRemoveButton;
	public GameObject activateBuffButton;
	public GameObject useInventorySkillButton;
	public GameObject deconstructionButton;
	public GameObject modificationButton;



	void Awake(){
		dialogController = this;
	}


	public void CloseCurrentDialog(){
		currentDialog.SetActive (false);
	}

	public static void DeactivateButtons(){
		dialogController.addTalentPointButton.SetActive (false);
		dialogController.skillActivationButton.SetActive (false);
		dialogController.deleteBackpackItemFromSlideDialog.SetActive (false);
		dialogController.backpackAreaEquipButton.SetActive (false);
		dialogController.backpackAreaDeleteItemButton.SetActive (false);
		dialogController.equipmentSlotToBackpackButton.SetActive (false);
		dialogController.buffRemoveButton.SetActive (false);
		dialogController.activateBuffButton.SetActive (false);
		dialogController.useInventorySkillButton.SetActive (false);
		dialogController.deconstructionButton.SetActive (false);
		dialogController.modificationButton.SetActive (false);
	}


	public void SkillActivationButton(){
		
		SkillActivator activator = DialogController.dialogController.currenBackpackItemInDialog.itemContent[0] as SkillActivator;
		PlayerController.skillStates [activator.skillID].skillActive = true;
		BackpackController.RemoveBackpackItemByID (DialogController.dialogController.currenBackpackItemInDialog.itemID);
		BackpackView.backpackView.RedrawBackpack ();
		CloseCurrentDialog ();
	}

	public void BackpackAreaEquipButton(){
		Equipment equip = (Equipment)currenBackpackItemInDialog.itemContent [0];
		Equipment playerEquip = PlayerController.GetEquipmentBySlotID(equip.slotID);
		if (playerEquip.isNullEquip) {
			PlayerController.SetEquip (equip);
			BackpackController.RemoveBackpackItemByID (currenBackpackItemInDialog.itemID, true);
		} else {
			PlayerController.SetEquip (equip);
			BackpackController.ChangeEquipBackpackItem (playerEquip, currenBackpackItemInDialog.itemID, true);
		}
		EquipmentController.equipmentController.RedrawEquipAndStats ();
		if (BackpackFilter.currentFilterID != 0) {
			BackpackFilter.RedrawFilters ();
		} else {
			BackpackController.backpackController.backpackView.RedrawBackpack ();
		}
		CloseCurrentDialog ();
	}

	public void BackpackAreaDeleteItemButton(){
		if (currenBackpackItemInDialog.itemCount <= 1) {
			BackpackController.RemoveBackpackItemByID (currenBackpackItemInDialog.itemID, true);
			CloseCurrentDialog ();
		} else {
			EquipDialog.equipDialogStatic.OpenSlideDialog (1, currenBackpackItemInDialog.itemCount);
		}
		if (BackpackFilter.currentFilterID != 0) {
			BackpackFilter.RedrawFilters ();
		}
		BackpackController.backpackController.backpackView.RedrawBackpack ();
	}

	public void DeleteBackpackItemFromSlideDialog(){
		BackpackItem item = PlayerController.backPackItems.Find (i => i == DialogController.dialogController.currenBackpackItemInDialog);
		if (EquipDialog.equipDialogStatic.slideDialog.currentValue == item.itemCount) {
			BackpackController.RemoveBackpackItemByID (item.itemID);
		} else {
			item.ChangeItemCount(item.itemCount - EquipDialog.equipDialogStatic.slideDialog.currentValue);
			BackpackView.backpackView.RecomputeWeight ();
			if (BackpackFilter.currentFilterID != 0) {
				BackpackFilter.RedrawFilters ();
			} else {
				BackpackController.backpackController.backpackView.RedrawBackpack ();
			}
		}

		CloseCurrentDialog ();
	}

	public void EquipmentSlotToBackpackButton(){
		Equipment tmp = new Equipment ();
		if (equipButtonIdInDialog == Equipment.head) {
			tmp = PlayerController.head;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.head));
		} else if (equipButtonIdInDialog == Equipment.chest) {
			tmp = PlayerController.chest;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.chest));
		} else if (equipButtonIdInDialog == Equipment.legs) {
			tmp = PlayerController.legs;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.legs));
		} else if (equipButtonIdInDialog == Equipment.neck) {
			tmp = PlayerController.neck;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.neck));
		} else if (equipButtonIdInDialog == Equipment.finger) {
			tmp = PlayerController.finger;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.finger));
		} else if (equipButtonIdInDialog == Equipment.trinket) {
			tmp = PlayerController.trinket;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.trinket));
		} else if (equipButtonIdInDialog == Equipment.meleeWeapon) {
			tmp = PlayerController.meleeWeapon;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.meleeWeapon));
		} else if (equipButtonIdInDialog == Equipment.fireWeapon) {
			tmp = PlayerController.fireWeapon;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.fireWeapon));
		} else if (equipButtonIdInDialog == Equipment.elementalWeapon) {
			tmp = PlayerController.elementalWeapon;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.elementalWeapon));
		}

		BackpackItem item = new BackpackItem ();
		item.weight = tmp.weight;
		item.price = tmp.price;
		item.itemContent.Add (tmp);
		item.itemID = PlayerController.GetBackpackItemID ();
		BackpackController.AddBackpackItem (item);

		EquipmentController.equipmentController.RedrawEquipAndStats ();

		CloseCurrentDialog ();
	}


	public void ActivateBuffButton(){
		Buff buff = ((Buff)dialogController.currenBackpackItemInDialog.itemContent [0]).Clone ();
		BuffsController.StartBuff (buff);
		if (dialogController.currenBackpackItemInDialog.itemCount == 1) {
			PlayerController.backPackItems.Remove (dialogController.currenBackpackItemInDialog);

		} else {
			dialogController.currenBackpackItemInDialog.itemCount -= 1;
		}
		if (BackpackFilter.currentFilterID != 0) {
			BackpackFilter.RedrawFilters ();
		} else {
			BackpackController.backpackController.backpackView.RedrawBackpack ();
		}
		CloseCurrentDialog ();
	}


	public void UseInventorySkillButton(){
		InventorySkill skill = ((InventorySkill)dialogController.currenBackpackItemInDialog.itemContent [0]).Clone ();
		InventorySkills.UseSkill (InventorySkills.GetSkillInfoByID(skill.skillID));

		if (dialogController.currenBackpackItemInDialog.itemCount == 1) {
			PlayerController.backPackItems.Remove (dialogController.currenBackpackItemInDialog);
		} else {
			dialogController.currenBackpackItemInDialog.itemCount -= 1;
		}
		if (BackpackFilter.currentFilterID != 0) {
			BackpackFilter.RedrawFilters ();
		} else {
			BackpackController.backpackController.backpackView.RedrawBackpack ();
		}
		CloseCurrentDialog ();
	}


	public void BuffRemoveButton(){
		BuffsController.StopBuff (currentBuffID);
		CloseCurrentDialog ();
	}


	public void DeconstructionButton(){
		//Equipment equip = currentEquipmentInDialog;
		if (currentEquipmentInDialogEquipped) {
			PlayerController.SetEquip (PlayerController.GetNullEquipment (currentEquipmentInDialog.slotID));
		} else {
			BackpackItem item = PlayerController.backPackItems.Find (b => b.itemContent[0] == currentEquipmentInDialog);
			BackpackController.RemoveBackpackItemByID (item.itemID);
		}

		BackpackItem hammerItem = PlayerController.backPackItems.Find (b => {
			if(b.itemContent[0].GetType() == typeof(Consumable)){
				Consumable cons = b.itemContent[0] as Consumable;
				if(cons.consumableType == Consumable.hammer && cons.consumableSubType == currenHammerType){
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}
		});


		if (hammerItem.itemCount == 1) {
			BackpackController.RemoveBackpackItemByID (hammerItem.itemID);
		} else {
			hammerItem.ChangeItemCount (hammerItem.itemCount - 1);
		}

		CloseCurrentDialog ();
		BackpackController.AddBackpackItem(BackpackItemParamsController.GetNewBackpackItem (currentDeconstructionEquipmentRune), true);
		if (BackpackFilter.currentFilterID != 0) {
			BackpackFilter.RedrawFilters ();
		} else {
			BackpackController.backpackController.backpackView.RedrawBackpack ();
		}
		EquipmentController.equipmentController.RedrawEquipAndStats ();
	}

	public void ModificationButton(){
		PlayerController.SetEquip(PlayerController.GetNullEquipment(DialogController.dialogController.currentEquipmentInDialog.slotID));
		DialogController.dialogController.currentEquipmentInDialog.rune1 = ModificationsPage.modificationsPage.equipmentRune1Slot;
		DialogController.dialogController.currentEquipmentInDialog.rune2 = ModificationsPage.modificationsPage.equipmentRune2Slot;
		PlayerController.SetEquip (DialogController.dialogController.currentEquipmentInDialog);

		if (ModificationsPage.modificationsPage.equipmentRune1BackpackItemID != -1) {
			BackpackController.RemoveBackpackItemByID (ModificationsPage.modificationsPage.equipmentRune1BackpackItemID);
		}
		if (ModificationsPage.modificationsPage.equipmentRune2BackpackItemID != -1) {
			BackpackController.RemoveBackpackItemByID (ModificationsPage.modificationsPage.equipmentRune2BackpackItemID);
		}

		if (BackpackFilter.currentFilterID != 0) {
			BackpackFilter.RedrawFilters ();
		} else {
			BackpackController.backpackController.backpackView.RedrawBackpack ();
		}

		EquipmentController.equipmentController.SetStats ();
		CloseCurrentDialog ();
	}


	public void AddTalentPointButton(){
		CurrentSkillState state = PlayerController.skillStates [DialogController.dialogController.skillTalentID];
		state.talentStatus += 1;
		PlayerController.talentsAvailableNumber -= 1;
		TalentsInterface.talentsInterface.DrawTalentBarInfo ();
		TalentsInterface.talentsInterface.ReDrawTalentButton (DialogController.dialogController.skillTalentID);
		CloseCurrentDialog ();
	}
}
