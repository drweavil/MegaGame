using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecChangeButton : MonoBehaviour {
	public int buttonID;
	public GameObject active;
	public SpecChangeButton melee;
	public SpecChangeButton fire;
	public SpecChangeButton elemental;
	public SkillPanelChanger panelChanger;

	public static SpecChangeButton staticButton;

	void Awake(){
		staticButton = melee;
	}



	public void ChangeSpec(){
		if (buttonID == 0) {
			melee.active.SetActive (true);
			fire.active.SetActive (false);
			elemental.active.SetActive (false);
			PlayerController.playerCharacterAPI.stats.SetSpec (Stats.meleeSpec);
			SkillPanelController.skillPanelController.SetPanel (panelChanger.currentPanel, Stats.meleeSpec);
		}else if(buttonID == 1){
			melee.active.SetActive (false);
			fire.active.SetActive (true);
			elemental.active.SetActive (false);
			PlayerController.playerCharacterAPI.stats.SetSpec (Stats.fireSpec);
			SkillPanelController.skillPanelController.SetPanel (panelChanger.currentPanel, Stats.fireSpec);
		}else if(buttonID == 2){
			melee.active.SetActive (false);
			fire.active.SetActive (false);
			elemental.active.SetActive (true);
			PlayerController.playerCharacterAPI.stats.SetSpec (Stats.elementalSpec);
			SkillPanelController.skillPanelController.SetPanel (panelChanger.currentPanel, Stats.elementalSpec);
		}
	}

	public void SetSpecVisual(int specID){
		if (specID == Stats.meleeSpec) {
			melee.active.SetActive (true);
			fire.active.SetActive (false);
			elemental.active.SetActive (false);
		}else if(specID == Stats.fireSpec){
			melee.active.SetActive (false);
			fire.active.SetActive (true);
			elemental.active.SetActive (false);
		}else if(specID == Stats.elementalSpec){
			melee.active.SetActive (false);
			fire.active.SetActive (false);
			elemental.active.SetActive (true);
		}	
	}
}
