using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class SkillPanelController : MonoBehaviour {
	public int currentPanelId = 1;


	public SkillButton button1;
	public SkillButton button2;
	public SkillButton button3;
	public SkillButton button4;
	public static SkillPanelController skillPanelController;
	public List<Sprite> skillSprites;

	List<SkillButtonSetting> meleeSet = new List<SkillButtonSetting> ();
	List<SkillButtonSetting> fireSet = new List<SkillButtonSetting> ();
	List<SkillButtonSetting> elementalSet = new List<SkillButtonSetting> ();



	void Awake(){
		skillPanelController = this;
		skillSprites = new List<Sprite> (Resources.LoadAll<Sprite>("Textures/skills"));

		/*button1.SetSkill (10);
		button2.SetSkill (37);
		button3.SetSkill (2);
		button4.SetSkill (3);*/
	}


	public Sprite GetSkillTexture(string spriteName){
		return skillSprites.Find (s => s.name == spriteName);
	}


	public void StartCdFor(int buttonID, float cd){
		SkillButton button = new SkillButton ();
		if (buttonID == 1) {
			button = button1;
		}
		if (buttonID == 2) {
			button = button2;
		}
		if (buttonID == 3) {
			button = button3;
		}
		if (buttonID == 4) {
			button = button4;
		}
		StartCoroutine (StartCdCoroutine(button, cd));
	}

	IEnumerator StartCdCoroutine(SkillButton button, float cd){
		int skillID = button.skillID;
		Timer cdTimer = new Timer ();
		cdTimer.SetTimer (cd);
		while (!cdTimer.TimeIsOver ()) {
			if (button.skillID == skillID) {
				button.SetCd (cdTimer.ResidualTime (), cd);
			}
			yield return null;
		}
		if (button.skillID == skillID) {
			button.SetCd (0, cd);
		}
		yield break;
	}

	public void StartGcd(int buttonID, float gcd){
		StartCoroutine (StartGcdCoroutine(button1, gcd));
		StartCoroutine (StartGcdCoroutine(button2, gcd));
		StartCoroutine (StartGcdCoroutine(button3, gcd));
		StartCoroutine (StartGcdCoroutine(button4, gcd));
	}

	IEnumerator StartGcdCoroutine(SkillButton button, float gcd){
		Timer cdTimer = new Timer ();
		cdTimer.SetTimer (gcd);
		while (!cdTimer.TimeIsOver ()) {
			button.SetGcd (cdTimer.ResidualTime(), gcd);
			yield return null;
		}
		button.SetGcd (0, gcd);
		yield break;
	}



	public void SetPanel(int newPanelID, int currentSpecID = -1){
		int specID = 0;
		if (currentSpecID == -1) {
			specID = PlayerController.playerCharacterAPI.stats.specId;
		} else {
			specID = currentSpecID;
		}

		List<SkillButtonSetting> settings = new List<SkillButtonSetting>();
		if (specID == Stats.meleeSpec) {
			settings = meleeSet;
		} else if (specID == Stats.fireSpec) {
			settings = fireSet;
		} else if (specID == Stats.elementalSpec) {
			settings = elementalSet;
		}

		int index1 = settings.FindIndex (s => s.buttonID == 1 && s.panelID == newPanelID);
		if (index1 != -1) {
			button1.SetSkill (settings [index1].skillID);
		}else{
			button1.SetSkill (-1);
		}

		int index2 = settings.FindIndex (s => s.buttonID == 2 && s.panelID == newPanelID);
		if (index2 != -1) {
			button2.SetSkill (settings [index2].skillID);
		}else{
			button2.SetSkill (-1);
		}

		int index3 = settings.FindIndex (s => s.buttonID == 3 && s.panelID == newPanelID);
		if (index3 != -1) {
			button3.SetSkill (settings [index3].skillID);
		}else{
			button3.SetSkill (-1);
		}

		int index4 = settings.FindIndex (s => s.buttonID == 4 && s.panelID == newPanelID);
		if (index4 != -1) {
			button4.SetSkill (settings [index4].skillID);
		}else{
			button4.SetSkill (-1);
		}

		button1.SetCd (0, 1);
		button2.SetCd (0, 1);
		button3.SetCd (0, 1);
		button4.SetCd (0, 1);
	}



	public void SetSettings(List<SkillButtonSetting> settings){
		meleeSet = settings.FindAll (s => s.specID == Stats.meleeSpec);
		fireSet = settings.FindAll (s => s.specID == Stats.fireSpec);
		elementalSet = settings.FindAll (s => s.specID == Stats.elementalSpec);
	}

	public List<SkillButtonSetting> GetSettings(){
		List<SkillButtonSetting> settings = new List<SkillButtonSetting> ();
		foreach (SkillButtonSetting skillSettings in meleeSet) {
			settings.Add (skillSettings);
		}

		foreach (SkillButtonSetting skillSettings in fireSet) {
			settings.Add (skillSettings);
		}

		foreach (SkillButtonSetting skillSettings in elementalSet) {
			settings.Add (skillSettings);
		}
		return settings;
	}
}
