using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SkillInterface : MonoBehaviour {
	public static SkillInterface skillInterface;

	public GameObject subPanel;
	public GameObject framePart;


	public GameObject commonPanelActive;
	public GameObject meleePanelActive;
	public GameObject firePanelActive;
	public GameObject elementalPanelActive;

	public GameObject meleeSubPanelActive;
	public GameObject fireSubPanelActive;
	public GameObject elementalSubPanelActive;

	public int currentSpec;
	public int currentPage = 1;
	public int maximumPage;	

	static int pageItemsCount = 5;

	public List<InterfaceSkillButton> skillButtons = new List<InterfaceSkillButton>();
	public List<SkillChanger> skillChangers = new List<SkillChanger> ();

	public static int currentChangingSpec = 1;

	public Image draggingIcon;
	public int draggingSkillID;


	public void Awake(){
		skillInterface = this;
	}


	public void ActivateChangersLumenArea(){
		foreach (SkillChanger changer in skillChangers) {
			changer.lumenArea.lumen.SetActive (true);
		}
	}

	public void DeactivateChangersLumenArea(){
		foreach (SkillChanger changer in skillChangers) {
			changer.lumenArea.lumen.SetActive (false);
		}
	}


	public void ActivateSubPanelChanger(){
		subPanel.SetActive (true);
		framePart.SetActive (false);
	}
	public void DeactivateSubPanelChanger(){
		subPanel.SetActive (false);
		framePart.SetActive (true);
	}

	public void ActivateInterface(){
		this.gameObject.SetActive (true);	
		ActivateCommon ();
		DeactivateChangersLumenArea ();
		//PlayerController.playerCharacterAPI.stats.specId;
	}

	public void DeactivateInterface(){
		this.gameObject.SetActive (false);
	}


	public void ActivateSpec(int specID){
		currentPage = 1;
		maximumPage = GetPagesCount (specID);
		DrawSkills ();
	}

	public void ActivateCommon(){
		currentSpec = Stats.commonSpec;
		commonPanelActive.SetActive (true);
		meleePanelActive.SetActive (false);
		firePanelActive.SetActive (false);
		elementalPanelActive.SetActive (false);
		ActivateSpec (Stats.commonSpec);
		ActivateSubPanelChanger ();
		ActivateMeleeSubPanel ();
	}

	public void ActivateMelee(){
		currentSpec = Stats.meleeSpec;
		commonPanelActive.SetActive (false);
		meleePanelActive.SetActive (true);
		firePanelActive.SetActive (false);
		elementalPanelActive.SetActive (false);
		ActivateSpec (Stats.meleeSpec);
		DeactivateSubPanelChanger ();
		currentChangingSpec = Stats.meleeSpec;
		ActivateMeleeSubPanel ();
	}

	public void ActivateFire(){
		currentSpec = Stats.fireSpec;
		commonPanelActive.SetActive (false);
		meleePanelActive.SetActive (false);
		firePanelActive.SetActive (true);
		elementalPanelActive.SetActive (false);
		ActivateSpec (Stats.fireSpec);
		DeactivateSubPanelChanger ();
		currentChangingSpec = Stats.fireSpec;
		ActivateFireSubPanel ();
	}

	public void ActivateElemental(){
		currentSpec = Stats.elementalSpec;
		commonPanelActive.SetActive (false);
		meleePanelActive.SetActive (false);
		firePanelActive.SetActive (false);
		elementalPanelActive.SetActive (true);
		ActivateSpec (Stats.elementalSpec);
		DeactivateSubPanelChanger ();
		currentChangingSpec = Stats.elementalSpec;
		ActivateElementalSubPanel ();
	}


	public void ActivateMeleeSubPanel(){
		DeactivateChangers ();
		meleeSubPanelActive.SetActive (true);
		fireSubPanelActive.SetActive (false);
		elementalSubPanelActive.SetActive (false);
		currentChangingSpec = Stats.meleeSpec;
		foreach (SkillButtonSetting buttonSetting in SkillPanelController.skillPanelController.meleeSet) {
			SkillChanger skillChanger = skillChangers.Find (s => s.buttonID == buttonSetting.buttonID && s.panelID == buttonSetting.panelID);
			skillChanger.SetButton (buttonSetting.skillID);
		}
	}

	public void ActivateFireSubPanel(){
		DeactivateChangers ();
		meleeSubPanelActive.SetActive (false);
		fireSubPanelActive.SetActive (true);
		elementalSubPanelActive.SetActive (false);
		currentChangingSpec = Stats.fireSpec;
		foreach (SkillButtonSetting buttonSetting in SkillPanelController.skillPanelController.fireSet) {
			SkillChanger skillChanger = skillChangers.Find (s => s.buttonID == buttonSetting.buttonID && s.panelID == buttonSetting.panelID);
			skillChanger.SetButton (buttonSetting.skillID);
		}
	}

	public void ActivateElementalSubPanel(){
		DeactivateChangers ();
		meleeSubPanelActive.SetActive (false);
		fireSubPanelActive.SetActive (false);
		elementalSubPanelActive.SetActive (true);
		currentChangingSpec = Stats.elementalSpec;
		foreach (SkillButtonSetting buttonSetting in SkillPanelController.skillPanelController.elementalSet) {
			SkillChanger skillChanger = skillChangers.Find (s => s.buttonID == buttonSetting.buttonID && s.panelID == buttonSetting.panelID);
			skillChanger.SetButton (buttonSetting.skillID);
		}
	}

	public void DeactivateChangers(){
		foreach (SkillChanger changer in skillChangers) {
			changer.skillID = -1;
			changer.icon.sprite = SkillPanelController.skillPanelController.GetSkillTexture ("skill--1");
		}
	}

	public void ButtonDown(){
		if (currentPage == 1) {
			currentPage = maximumPage;
		} else {
			currentPage -= 1;
		}

		DrawSkills ();
	}

	public void ButtonUp(){
		if (currentPage == maximumPage) {
			currentPage = 1;
		} else {
			currentPage += 1;
		}

		DrawSkills ();
	}


	public void DrawSkills(){
		DeactivateButtons ();
		List<CurrentSkillState> states = GetSkillStatesByPage (currentPage, currentSpec);
		for (int i = 0; i< states.Count; i++) {
			skillButtons [i].gameObject.SetActive (true);
			skillButtons [i].SetButton (states [i]);
		}	
	}

	void DeactivateButtons(){
		foreach (InterfaceSkillButton button in skillButtons) {
			button.gameObject.SetActive (false);
		}
	}


	public static List<CurrentSkillState> GetSkillStatesByPage(int page, int specID){
		//List<BackpackItem> items = new List<BackpackItem> ();
		int size = 0;
		List<CurrentSkillState> states = new List<CurrentSkillState> ();
		foreach (KeyValuePair<int, CurrentSkillState> pair in PlayerController.skillStates.ToList().FindAll (s => s.Value.specID == specID)) {
			states.Add (pair.Value);
		}
		//Debug.Log (states.Count);
		size = states.Count;
		int index = (page * pageItemsCount) - pageItemsCount;
		int count = 0;
		int deltaPageSize = size - page * pageItemsCount;
		if (deltaPageSize >= 0) {
			count = pageItemsCount;
		}
		if (deltaPageSize < 0) {
			count = pageItemsCount + deltaPageSize;
		}

		return states.GetRange (index, count);

	}


	public static int GetPagesCount(int specID){
		int value = 0;
		int size = 0;
		size = PlayerController.skillStates.ToList().FindAll(s => s.Value.specID == specID).Count;
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
}
