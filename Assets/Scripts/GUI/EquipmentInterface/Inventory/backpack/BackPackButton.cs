using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackPackButton : MonoBehaviour {
	public int inventoryObjectId;
	public Image buttonIcon;
	public GameObject button;
	public GameObject stackFrame;
	public GameObject withoutStackFrame;
	public Text stackText;
	public GameObject backLight;
	public GameObject active;
	public BackPackButtonEquipContent equipContent;
	public BackpackButtonBuffContent buffContent;
	public BackpackButtonRuneContent runeContent;
	public BackpackButtonInventorySkillContent inventorySkillContent;
	public BackpackButtonCommonContent commonContent;


	public void ButtonDown(){
		active.SetActive (true);
	}

	public void ButtonUp(){
		active.SetActive (false);

		BackpackItem item = BackpackController.GetBackpackItemByID (inventoryObjectId);
		DialogController.dialogController.currenBackpackItemInDialog = item;
		if (item.itemContent [0].GetType () == typeof(Equipment)) {
			Equipment equip = (Equipment)item.itemContent [0];
			/*if (equip.slotID == Equipment.head) {
				EquipDialog.equipDialogStatic.OpenDialogInfo (equip, PlayerController.head, item);
			} else if (equip.slotID == Equipment.chest) {
				EquipDialog.equipDialogStatic.OpenDialogInfo (equip, PlayerController.chest, item);
			} else if (equip.slotID == Equipment.legs) {
				EquipDialog.equipDialogStatic.OpenDialogInfo (equip, PlayerController.legs, item);
			} else if (equip.slotID == Equipment.neck) {
				EquipDialog.equipDialogStatic.OpenDialogInfo (equip, PlayerController.neck, item);
			} else if (equip.slotID == Equipment.finger) {
				EquipDialog.equipDialogStatic.OpenDialogInfo (equip, PlayerController.finger, item);
			} else if (equip.slotID == Equipment.trinket) {
				EquipDialog.equipDialogStatic.OpenDialogInfo (equip, PlayerController.trinket, item);
			} else if (equip.slotID == Equipment.meleeWeapon) {
				EquipDialog.equipDialogStatic.OpenDialogInfo (equip, PlayerController.meleeWeapon, item);
			} else if (equip.slotID == Equipment.fireWeapon) {
				EquipDialog.equipDialogStatic.OpenDialogInfo (equip, PlayerController.fireWeapon, item);
			} else if (equip.slotID == Equipment.elementalWeapon) {
				EquipDialog.equipDialogStatic.OpenDialogInfo (equip, PlayerController.elementalWeapon, item);
			}*/
			EquipDialog.equipDialogStatic.OpenDialogInfo (equip, PlayerController.GetEquipmentBySlotID(equip.slotID), item);
			DialogController.dialogController.currentEquipmentInDialog = equip;
			DialogController.dialogController.currentEquipmentInDialogEquipped = false;


			DialogController.DeactivateButtons ();
			DialogController.dialogController.backpackAreaDeleteItemButton.SetActive (true);
			DialogController.dialogController.backpackAreaEquipButton.SetActive (true);


		} else if (item.itemContent [0].GetType () == typeof(Buff)) {
			Buff buff = item.itemContent [0] as Buff;
			EquipDialog.equipDialogStatic.OpenDialogBuffInfo (buff.buffID, item);

			DialogController.DeactivateButtons ();
			DialogController.dialogController.backpackAreaDeleteItemButton.SetActive (true);
			DialogController.dialogController.activateBuffButton.SetActive (true);
		} else if (item.itemContent [0].GetType () == typeof(EquipmentRune)) {
			EquipDialog.equipDialogStatic.OpenRuneDialog (item);
			DialogController.DeactivateButtons ();
			DialogController.dialogController.backpackAreaDeleteItemButton.SetActive (true);
		} else if (item.itemContent [0].GetType () == typeof(InventorySkill)) {
			EquipDialog.equipDialogStatic.OpenInventorySkillDialog (item);
			DialogController.DeactivateButtons ();
			DialogController.dialogController.useInventorySkillButton.SetActive (true);
			DialogController.dialogController.backpackAreaDeleteItemButton.SetActive (true);
		} else if (item.itemContent [0].GetType () == typeof(Consumable)) {
			Consumable consumable = item.itemContent [0] as Consumable;
			EquipDialog.equipDialogStatic.OpenCommonDialog (item, consumable.GetTitle(), consumable.GetDescription(), true, consumable.GetCommonDescription());
			DialogController.DeactivateButtons ();
			DialogController.dialogController.backpackAreaDeleteItemButton.SetActive (true);
		}
	}





	public void SetButton(BackpackItem item){
		button.SetActive (true);
		DeactivateContents ();
		stackFrame.SetActive (false);
		if (item.itemCount > 1) {
			stackFrame.SetActive (true);
			stackText.text = item.itemCount.ToString ();
		}

		inventoryObjectId = item.itemID;

		if (item.itemContent [0].GetType () == typeof(Equipment)) {
			equipContent.gameObject.SetActive (true);
			Equipment equip = (Equipment)item.itemContent [0];
			if (equip.slotID == Equipment.head) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetArmorIcon ("head_" + equip.skinID);
			} else if (equip.slotID == Equipment.chest) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetArmorIcon ("chest_" + equip.skinID);
			} else if (equip.slotID == Equipment.legs) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetArmorIcon ("legs_" + equip.skinID);
			} else if (equip.slotID == Equipment.trinket) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetAccessoryIcon ("trinket_" + equip.skinID);
			} else if (equip.slotID == Equipment.finger) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetAccessoryIcon ("finger_" + equip.skinID);
			} else if (equip.slotID == Equipment.neck) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetAccessoryIcon ("neck_" + equip.skinID);
			} else if (equip.slotID == Equipment.meleeWeapon) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetWeaponIcon ("m_w_" + equip.skinID);
			} else if (equip.slotID == Equipment.fireWeapon) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetWeaponIcon ("r_w_" + equip.skinID);
			} else if (equip.slotID == Equipment.elementalWeapon) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetWeaponIcon ("e_w_" + equip.skinID);
			}
			equipContent.SetEquipmentStats (item, true);
		} else if (item.itemContent [0].GetType () == typeof(Buff)) {
			Buff buff = (Buff)item.itemContent [0];
			buttonIcon.sprite = SkillPanelController.skillPanelController.GetSkillTexture ("burst_" + buff.buffID);
			buffContent.gameObject.SetActive (true);
			buffContent.SetContent (item);
		} else if (item.itemContent [0].GetType () == typeof(EquipmentRune)) {
			EquipmentRune rune = item.itemContent [0] as EquipmentRune;
			runeContent.gameObject.SetActive (true);
			buttonIcon.sprite = SkillPanelController.skillPanelController.GetSkillTexture ("rune_" + rune.skinID.ToString ());
			;
			runeContent.SetInfo (item);
		} else if (item.itemContent [0].GetType () == typeof(InventorySkill)) {
			InventorySkill skill = item.itemContent [0] as InventorySkill;
			inventorySkillContent.gameObject.SetActive (true);
			buttonIcon.sprite = SkillPanelController.skillPanelController.GetSkillTexture ("inventorySkill_" + skill.skillID.ToString ());
			;
			inventorySkillContent.SetInfo (item);
		} else if (item.itemContent [0].GetType () == typeof(Consumable)) {
			Consumable consumable = (Consumable)item.itemContent [0];
			commonContent.gameObject.SetActive (true);
			buttonIcon.sprite = SkillPanelController.skillPanelController.GetSkillTexture (consumable.GetConsumableIconName() + consumable.consumableSubType.ToString());
			commonContent.SetInfo (item, consumable.GetTitle ());
		}
	}

	void DeactivateContents(){
		equipContent.gameObject.SetActive (false);
		buffContent.gameObject.SetActive (false);
		runeContent.gameObject.SetActive (false);
		inventorySkillContent.gameObject.SetActive (false);
		commonContent.gameObject.SetActive (false);
	}


}
