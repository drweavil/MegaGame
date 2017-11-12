using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TalentButton : MonoBehaviour {
	public Text talentNumber;
	public int skillID;

	public GameObject active;
	public bool skillDeactive = true;


	public void UpdateSkillInfo(){
		if (PlayerController.skillStates [skillID].skillActive) {
			active.SetActive (false);
			skillDeactive = false;
		} else {
			active.SetActive (true);
			skillDeactive = true;	
		}

		if ( PlayerController.skillStates [skillID].talentStatus > 999) {
			talentNumber.text = "-";
		} else {
			talentNumber.text = PlayerController.skillStates [skillID].talentStatus.ToString ();
		}
	}

	public void ButtonClick(){
		
		DialogController.dialogController.skillTalentID = skillID;
		EquipDialog.equipDialogStatic.OpenSkillTalentDialog (skillID);

		DialogController.DeactivateButtons ();
		if (PlayerController.talentsAvailableNumber != 0) {
			DialogController.dialogController.addTalentPointButton.SetActive (true);
		}
	}
}
