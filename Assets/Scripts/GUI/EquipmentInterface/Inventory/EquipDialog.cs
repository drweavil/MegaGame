using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipDialog : MonoBehaviour {
	public GameObject equipDialog;
	public static EquipDialog equipDialogStatic;
	public EquipDialogInfo equipDialogInfo;

	void Awake(){
		equipDialogStatic = this;
	}

	public void OpenDialogInfo(Equipment equip, Equipment sweepingEquip){
		equipDialog.SetActive (true);
		DeactiveAllWindows ();
		DialogController.dialogController.currentDialog = equipDialog;
		equipDialogInfo.equipDialogInfo.SetActive (true);
		equipDialogInfo.SetStatsByEquip (equip, sweepingEquip);
	}

	public void DeactiveAllWindows(){
		equipDialogInfo.equipDialogInfo.SetActive (false);
	}




}
