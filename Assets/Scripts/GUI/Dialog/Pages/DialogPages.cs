using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPages : MonoBehaviour {
	public List<PageButton> pageButtons = new List<PageButton>();
	public static DialogPages dialogPages;
	public PageButton statsButton;
	public GameObject pagesLine;


	void Awake(){
		dialogPages = this;
	}

	public static void ActivatePagesPanel(){
		dialogPages.pagesLine.SetActive (true);
		dialogPages.statsButton.Activate ();
	}

	public static void DeactivatePagesPanel(){
		dialogPages.pagesLine.SetActive (false);
	}

	public void ActivateEquipParamsDialog(){
		//DialogController.dialogController.currentEquipmentInDialog;
		EquipDialog.equipDialogStatic.DeactiveAllWindows();
		EquipDialog.equipDialogStatic.equipDialogInfo.gameObject.SetActive (true);
		Equipment equip = DialogController.dialogController.currentEquipmentInDialog;
		DialogController.DeactivateButtons ();
		if (DialogController.dialogController.currentEquipmentInDialogEquipped) {
			EquipDialog.equipDialogStatic.OpenDialogInfo (equip, PlayerController.GetNullEquipment (equip.slotID));
			DialogController.dialogController.equipmentSlotToBackpackButton.SetActive (true);
		} else {
			EquipDialog.equipDialogStatic.OpenDialogInfo (equip, PlayerController.GetEquipmentBySlotID (equip.slotID));
			DialogController.dialogController.backpackAreaDeleteItemButton.SetActive (true);
			DialogController.dialogController.backpackAreaEquipButton.SetActive (true);
		}

	}

	public void ActivateModificationsPage(){
		EquipDialog.equipDialogStatic.DeactiveAllWindows();
		EquipDialog.equipDialogStatic.modificationsPage.gameObject.SetActive (true);
	}

	public void ActivateDeconstructionPage(){
		EquipDialog.equipDialogStatic.DeactiveAllWindows();
		EquipDialog.equipDialogStatic.deconstructionPage.gameObject.SetActive (true);
	}

	public static void DeactivateAllPages(){
		foreach (PageButton button in dialogPages.pageButtons) {
			button.activePanel.SetActive (false);
			button.deactivePanel.SetActive (true);
		}
	}
}
