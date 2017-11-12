using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInterfaceController : MonoBehaviour {
	public static BattleInterfaceController battleInterfaceController;
	public SkillPanelController skillPanelController;
	public SkillPanelChanger skillPanelChanger;

	public GameObject battleInterface;

	void Awake(){
		battleInterfaceController = this;

		List<SkillButtonSetting> skillButtonSettings = new List<SkillButtonSetting> ();
		skillButtonSettings.Add (Lol (1, 0, 1, Stats.meleeSpec));
		skillButtonSettings.Add (Lol (2, 1, 1, Stats.meleeSpec));
		skillButtonSettings.Add (Lol (3, 2, 1, Stats.meleeSpec));
		skillButtonSettings.Add (Lol (4, 3, 1, Stats.meleeSpec));
		skillButtonSettings.Add (Lol (1, 4, 2, Stats.meleeSpec));
		skillButtonSettings.Add (Lol (2, 5, 2, Stats.meleeSpec));
		skillButtonSettings.Add (Lol (3, 6, 2, Stats.meleeSpec));
		skillButtonSettings.Add (Lol (4, 7, 2, Stats.meleeSpec));
		skillButtonSettings.Add (Lol (1, 8, 3, Stats.meleeSpec));
		skillButtonSettings.Add (Lol (2, 9, 3, Stats.meleeSpec));
		skillButtonSettings.Add (Lol (3, 10, 3, Stats.meleeSpec));
		skillButtonSettings.Add (Lol (4, 11, 3, Stats.meleeSpec));
		skillButtonSettings.Add (Lol (1, 12, 4, Stats.meleeSpec));

		skillButtonSettings.Add (Lol (1, 13, 1, Stats.fireSpec));
		skillButtonSettings.Add (Lol (2, 14, 1, Stats.fireSpec));
		skillButtonSettings.Add (Lol (3, 15, 1, Stats.fireSpec));
		skillButtonSettings.Add (Lol (4, 16, 1, Stats.fireSpec));
		skillButtonSettings.Add (Lol (1, 17, 2, Stats.fireSpec));
		skillButtonSettings.Add (Lol (2, 18, 2, Stats.fireSpec));
		skillButtonSettings.Add (Lol (3, 19, 2, Stats.fireSpec));
		skillButtonSettings.Add (Lol (4, 20, 2, Stats.fireSpec));
		skillButtonSettings.Add (Lol (1, 21, 3, Stats.fireSpec));
		skillButtonSettings.Add (Lol (2, 22, 3, Stats.fireSpec));
		skillButtonSettings.Add (Lol (3, 23, 3, Stats.fireSpec));
		skillButtonSettings.Add (Lol (4, 24, 3, Stats.fireSpec));
		skillButtonSettings.Add (Lol (1, 25, 4, Stats.fireSpec));

		skillButtonSettings.Add (Lol (1, 26, 1, Stats.elementalSpec));
		skillButtonSettings.Add (Lol (2, 27, 1, Stats.elementalSpec));
		skillButtonSettings.Add (Lol (3, 28, 1, Stats.elementalSpec));
		skillButtonSettings.Add (Lol (4, 29, 1, Stats.elementalSpec));
		skillButtonSettings.Add (Lol (1, 30, 2, Stats.elementalSpec));
		skillButtonSettings.Add (Lol (2, 31, 2, Stats.elementalSpec));
		skillButtonSettings.Add (Lol (3, 32, 2, Stats.elementalSpec));
		skillButtonSettings.Add (Lol (4, 33, 2, Stats.elementalSpec));
		skillButtonSettings.Add (Lol (1, 34, 3, Stats.elementalSpec));
		skillButtonSettings.Add (Lol (2, 35, 3, Stats.elementalSpec));
		skillButtonSettings.Add (Lol (3, 36, 3, Stats.elementalSpec));
		skillButtonSettings.Add (Lol (4, 37, 3, Stats.elementalSpec));
		skillButtonSettings.Add (Lol (1, 38, 4, Stats.elementalSpec));
		skillButtonSettings.Add (Lol (2, 39, 4, Stats.elementalSpec));
		skillButtonSettings.Add (Lol (3, 40, 4, Stats.elementalSpec));
		skillPanelController.currentPanelId = Stats.meleeSpec; 


		skillPanelController.SetSettings (skillButtonSettings);
		StartCoroutine (StartProcess.StartActionAfterFewFrames (10, () => {
			if(BattleInterfaceController.battleInterfaceController.battleInterface.activeInHierarchy){
				skillPanelController.SetPanel (1);
				skillPanelChanger.SetPanel (1);
				SpecChangeButton.staticButton.SetSpecVisual(Stats.meleeSpec);
			}
		}));
	}



	public void DeactivateInterface(){
		battleInterface.SetActive (false);
	}

	public void ActivateInterface(){
		battleInterface.SetActive (true);

		skillPanelController.SetPanel (skillPanelController.currentPanelId);
		skillPanelChanger.SetPanel (skillPanelController.currentPanelId);
		SpecChangeButton.staticButton.SetSpecVisual(PlayerController.playerCharacterAPI.stats.specId);

		HealthBarPool.PublishDeferredHealthBars ();

		foreach (HealthBar bar in HealthBarPool.healthBars) {
			if (bar.playerHealthBar.gameObject.activeInHierarchy) {
				bar.playerHealthBar.UpdateNumbers ();
			}
		}
		PlayerController.playerCharacterAPI.healthBar.UpdateNumbers ();
	}





	SkillButtonSetting Lol(int buttonID, int skillID, int panelID, int specID){
		SkillButtonSetting lol = new SkillButtonSetting ();
		lol.buttonID = buttonID;
		lol.skillID = skillID;
		lol.panelID = panelID; 
		lol.specID = specID;
		return lol;
	}

}
