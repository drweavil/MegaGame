using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillChanger : MonoBehaviour {
	public int panelID;
	public int buttonID;
	public int skillID;
	public Image icon;
	public LumenArea lumenArea;
	//public SkillButtonSetting skillButtonSetting;

	public void SetButton(int newSkillID){
		icon.sprite = SkillPanelController.skillPanelController.GetSkillTexture ("skill-" + newSkillID.ToString());
		skillID = newSkillID;
	}

	public void DeactivateSkillButton(){
		if (SkillInterface.currentChangingSpec == Stats.meleeSpec) {
			int index = SkillPanelController.skillPanelController.meleeSet.FindIndex (b => b.buttonID == buttonID && b.panelID == panelID);
			if (index != -1) {
				SkillPanelController.skillPanelController.meleeSet.RemoveAt (index);
			}
			SkillInterface.skillInterface.ActivateMeleeSubPanel ();
		} else if (SkillInterface.currentChangingSpec == Stats.fireSpec) {
			int index = SkillPanelController.skillPanelController.fireSet.FindIndex (b => b.buttonID == buttonID && b.panelID == panelID);
			if (index != -1) {
				SkillPanelController.skillPanelController.fireSet.RemoveAt (index);
			}
			SkillInterface.skillInterface.ActivateFireSubPanel ();
		} else if (SkillInterface.currentChangingSpec == Stats.elementalSpec) {
			int index = SkillPanelController.skillPanelController.elementalSet.FindIndex (b => b.buttonID == buttonID && b.panelID == panelID);
			if (index != -1) {
				SkillPanelController.skillPanelController.elementalSet.RemoveAt (index);
			}
			SkillInterface.skillInterface.ActivateElementalSubPanel ();
		}
	}


	public void SetSkill(){
		List<SkillButtonSetting> skillButtonSettings = new List<SkillButtonSetting> ();
		if (SkillInterface.currentChangingSpec == Stats.meleeSpec) {
			skillButtonSettings = SkillPanelController.skillPanelController.meleeSet;
		} else if(SkillInterface.currentChangingSpec == Stats.fireSpec){
			skillButtonSettings = SkillPanelController.skillPanelController.fireSet;
		} else if(SkillInterface.currentChangingSpec == Stats.elementalSpec){
			skillButtonSettings = SkillPanelController.skillPanelController.elementalSet;
		}

		int index = skillButtonSettings.FindIndex (s => s.buttonID == buttonID && s.panelID == panelID);	
		if (index == -1) {
			SkillButtonSetting settings = new SkillButtonSetting ();
			settings.panelID = panelID;
			settings.buttonID = buttonID;
			settings.skillID = SkillInterface.skillInterface.draggingSkillID;
			settings.specID = SkillInterface.currentChangingSpec;
			skillButtonSettings.Add (settings);
		} else {
			skillButtonSettings[index].skillID = SkillInterface.skillInterface.draggingSkillID;
		}
		icon.sprite = SkillPanelController.skillPanelController.GetSkillTexture ("skill-" + SkillInterface.skillInterface.draggingSkillID.ToString());

	}
}
