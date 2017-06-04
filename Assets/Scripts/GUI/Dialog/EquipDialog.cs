using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipDialog : MonoBehaviour {
	public GameObject equipDialog;
	public static EquipDialog equipDialogStatic;
	public EquipDialogInfo equipDialogInfo;
	public EquipDialogBuffInfo equipDialogBuffInfo;

	void Awake(){
		equipDialogStatic = this;
	}

	public void OpenDialogInfo(Equipment equip, Equipment sweepingEquip, BackpackItem item = null){
		equipDialog.SetActive (true);
		DeactiveAllWindows ();
		DialogController.dialogController.currentDialog = equipDialog;
		equipDialogInfo.equipDialogInfo.SetActive (true);
		equipDialogInfo.SetStatsByEquip (equip, sweepingEquip, item, true);
	}

	public void OpenDialogProcessBuffInfo(int buffID){
		equipDialog.SetActive (true);
		DeactiveAllWindows ();
		DialogController.dialogController.currentDialog = equipDialog;
		equipDialogBuffInfo.equipDialogBuffInfo.SetActive (true);
		equipDialogBuffInfo.SetProcessBuffInfo (buffID);
	}

	public void DeactiveAllWindows(){
		equipDialogInfo.equipDialogInfo.SetActive (false);
		equipDialogBuffInfo.equipDialogBuffInfo.SetActive (false);
	}




}
