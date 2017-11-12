using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipDialog : MonoBehaviour {
	public GameObject equipDialog;
	public static EquipDialog equipDialogStatic;
	public EquipDialogInfo equipDialogInfo;
	public EquipDialogBuffInfo equipDialogBuffInfo;
	public SlideDialog slideDialog;
	public EquipDialogRuneInfo runeInfo;
	public EquipDialogInventorySkillInfo inventorySkillInfo;
	public CommonDialogInfo commonDialogInfo;
	public SkillDialogInfo skillDialogInfo;

	public ModificationsPage modificationsPage;
	public DeconstructionPage deconstructionPage;

	public SkillTalentDialog skillTalentDialog;

	public GameObject dialogPagesPanel;

	void Awake(){
		equipDialogStatic = this;
	}

	public void OpenDialogInfo(Equipment equip, Equipment sweepingEquip, BackpackItem item = null){
		equipDialog.SetActive (true);
		DeactiveAllWindows ();
		DialogPages.ActivatePagesPanel ();
		DialogController.dialogController.currentDialog = equipDialog;
		equipDialogInfo.equipDialogInfo.SetActive (true);
		equipDialogInfo.SetStatsByEquip (equip, sweepingEquip, item /*true*/);
		dialogPagesPanel.SetActive (true);
	}

	public void OpenDialogBuffInfo(int buffID, BackpackItem item){
		equipDialog.SetActive (true);
		DeactiveAllWindows ();
		DialogPages.DeactivatePagesPanel ();
		DialogController.dialogController.currentDialog = equipDialog;
		equipDialogBuffInfo.equipDialogBuffInfo.SetActive (true);
		equipDialogBuffInfo.SetBuffInfo (buffID, item);
	}

	public void OpenDialogProcessBuffInfo(int buffID){
		equipDialog.SetActive (true);
		DeactiveAllWindows ();
		DialogPages.DeactivatePagesPanel ();
		DialogController.dialogController.currentDialog = equipDialog;
		equipDialogBuffInfo.equipDialogBuffInfo.SetActive (true);
		equipDialogBuffInfo.SetProcessBuffInfo (buffID);
		dialogPagesPanel.SetActive (false);
	}

	public void OpenSlideDialog(int minimum, int maximum, bool withPrice = false, float unitPrice = 0){
		equipDialog.SetActive (true);
		DeactiveAllWindows ();
		DialogPages.DeactivatePagesPanel ();
		DialogController.dialogController.currentDialog = equipDialog;
		slideDialog.gameObject.SetActive (true);
		slideDialog.SetParams (minimum, maximum, withPrice, unitPrice);
		DialogController.DeactivateButtons ();
		DialogController.dialogController.deleteBackpackItemFromSlideDialog.SetActive (true);
		dialogPagesPanel.SetActive (false);
	}

	public void OpenRuneDialog (BackpackItem item){
		equipDialog.SetActive (true);
		DeactiveAllWindows ();
		DialogPages.DeactivatePagesPanel ();
		runeInfo.gameObject.SetActive (true);
		runeInfo.SetInfo (item);
		DialogController.dialogController.currentDialog = equipDialog;
		dialogPagesPanel.SetActive (false);
	}

	public void OpenInventorySkillDialog (BackpackItem item){
		equipDialog.SetActive (true);
		DeactiveAllWindows ();
		DialogPages.DeactivatePagesPanel ();
		inventorySkillInfo.gameObject.SetActive (true);
		inventorySkillInfo.SetInfo (item);
		DialogController.dialogController.currentDialog = equipDialog;
		dialogPagesPanel.SetActive (false);
	}

	public void OpenCommonDialog(BackpackItem item, string title, string description, bool withCommonDescription = false, string commonDescription = ""){
		equipDialog.SetActive (true);
		DeactiveAllWindows ();
		DialogPages.DeactivatePagesPanel ();
		commonDialogInfo.gameObject.SetActive (true);
		commonDialogInfo.SetItemInfo (item, true, withCommonDescription);
		commonDialogInfo.title.text = title;
		commonDialogInfo.description.text = description;
		commonDialogInfo.commonDescriptionText.text = commonDescription;
		DialogController.dialogController.currentDialog = equipDialog;
		dialogPagesPanel.SetActive (false);
	}

	public void OpenSkillDialog(int skillID, BackpackItem item = default(BackpackItem)){
		equipDialog.SetActive (true);
		DeactiveAllWindows ();
		DialogPages.DeactivatePagesPanel ();
		skillDialogInfo.gameObject.SetActive (true);
		skillDialogInfo.SetInfo (skillID, item);
		DialogController.DeactivateButtons ();
		DialogController.dialogController.currentDialog = equipDialog;
		dialogPagesPanel.SetActive (false);
	}

	public void OpenSkillTalentDialog(int skillID){
		equipDialog.SetActive (true);
		DeactiveAllWindows ();
		DialogPages.DeactivatePagesPanel ();
		skillTalentDialog.gameObject.SetActive (true);
		skillTalentDialog.SetInfo (skillID);
		DialogController.DeactivateButtons ();
		DialogController.dialogController.currentDialog = equipDialog;
		dialogPagesPanel.SetActive (false);
	}

	public void DeactiveAllWindows(){
		equipDialogInfo.equipDialogInfo.SetActive (false);
		equipDialogBuffInfo.equipDialogBuffInfo.SetActive (false);
		slideDialog.gameObject.SetActive (false);
		runeInfo.gameObject.SetActive (false);
		inventorySkillInfo.gameObject.SetActive (false);
		commonDialogInfo.gameObject.SetActive (false);
		skillDialogInfo.gameObject.SetActive (false);

		modificationsPage.gameObject.SetActive (false);
		deconstructionPage.gameObject.SetActive (false);

		skillTalentDialog.gameObject.SetActive (false);


	}




}
