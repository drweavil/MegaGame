using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerButtonsSystem : MonoBehaviour {
	public static HammerButtonsSystem hammerButtonsSystem;
	public List<HammerButton> buttons = new List<HammerButton>();

	void Awake(){
		hammerButtonsSystem = this;
	}

	public static void DeactivateAll(){
		foreach (HammerButton button in hammerButtonsSystem.buttons) {
			button.active.SetActive (false);	
		}
	}

	public void Hammer1(){
		DialogController.dialogController.currentDeconstructionEquipmentRune = EquipmentGenerator.GetEquipmentRune (
			DialogController.dialogController.currentEquipmentInDialog,
			EquipmentGenerator.smallRunePercent, 3
		);
		DialogController.dialogController.currenHammerType = 1;
		EquipDialog.equipDialogStatic.deconstructionPage.SetRuneStats (DialogController.dialogController.currentDeconstructionEquipmentRune);
		if (DeconstructionPage.hammer1ValueInt == 0) {
			DialogController.dialogController.deconstructionButton.SetActive (false);
		} else {
			DialogController.dialogController.deconstructionButton.SetActive (true);
		}
	}

	public void Hammer2(){
		DialogController.dialogController.currentDeconstructionEquipmentRune = EquipmentGenerator.GetEquipmentRune (
			DialogController.dialogController.currentEquipmentInDialog,
			EquipmentGenerator.middleRunePercent, 3
		);
		DialogController.dialogController.currenHammerType = 2;
		EquipDialog.equipDialogStatic.deconstructionPage.SetRuneStats (DialogController.dialogController.currentDeconstructionEquipmentRune);
		if (DeconstructionPage.hammer2ValueInt == 0) {
			DialogController.dialogController.deconstructionButton.SetActive (false);
		} else {
			DialogController.dialogController.deconstructionButton.SetActive (true);
		}
	}

	public void Hammer3(){
		DialogController.dialogController.currentDeconstructionEquipmentRune = EquipmentGenerator.GetEquipmentRune (
			DialogController.dialogController.currentEquipmentInDialog,
			EquipmentGenerator.largeRunePercent, 3
		);
		DialogController.dialogController.currenHammerType = 3;
		EquipDialog.equipDialogStatic.deconstructionPage.SetRuneStats (DialogController.dialogController.currentDeconstructionEquipmentRune);
		if (DeconstructionPage.hammer3ValueInt == 0) {
			DialogController.dialogController.deconstructionButton.SetActive (false);
		} else {
			DialogController.dialogController.deconstructionButton.SetActive (true);
		}
	}
}
