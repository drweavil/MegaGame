using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class SkillButton : MonoBehaviour {
	public SlideAimButton slideButtonScript;
	public int skillID = -1;
	public int buttonID;
	public bool readyToPush = true;
	SkillSettings skillSettings;
	public Image buttonImage;

	public GameObject lumen;
	public GameObject active;
	public GameObject coolDown;
	public GameObject gcd;
	public GameObject notActive;


	private RectTransform coolDownRect;
	private RectTransform gcdRect;

	private float maximumCooldownHeight;
	private float maximumGcdHeight;

	private Vector2 cooldownRectPosition;
	private Vector2 gcdRectPosition;

	private Rect newCooldownRectPosition = new Rect ();
	private Rect newGcdRectPosition = new Rect ();




	void Awake(){
		//StartCoroutine (StartProcess.StartActionAfterFewFrames (() => {
		SetBarRects ();
		//SetSkill (buttonID);
		//}));

	}

	void Update(){
	}

	public void Down(){
		if (readyToPush) {
			if (skillID != -1) {
				if (CanPress()) {
					if (skillSettings.withSlideAIM) {
						slideButtonScript.ButtonDown ();
					}
					active.SetActive (true);
				} else {
					notActive.SetActive (true);
				}
			} else {
				notActive.SetActive (true);
			}
			if(skillSettings.withAimLine){
				AimLineController.aimLineController.StartAimLine (skillSettings);
			}
		}
	}

	public void Up(){
		if (readyToPush) {
			if (skillID != -1) {
				if (CanPress ()) {
					if (skillSettings.withSlideAIM) {
						slideButtonScript.ButtonUp ();
					} else {
						UseSkill ();
					}
					//active.SetActive (false);
				} else {
					//notActive.SetActive (false);
				}
			} else {
				//notActive.SetActive (false);
			}

		}

		if(skillSettings.withAimLine){
			AimLineController.aimLineController.StopAimLine ();
		}

		active.SetActive (false);
		notActive.SetActive (false);
	}


	bool CanPress(){
		bool value = false;
		if (skillID != -1) {
			if (!(((PlayerController.playerCharacterAPI.stats.withoutControl ||
				PlayerController.playerCharacterAPI.stats.inSilence) && !skillSettings.ignoreSilence) ||
			   PlayerController.playerCharacterAPI.stats.onGlobalCooldown ||
			   PlayerController.playerCharacterAPI.stats.SkillOnCD (skillSettings.skillID))) {
				if (skillSettings.canUseWithoutResource) {
					value = true;
				} else {
					if (skillSettings.isAddResourceSkill) {
						value = true;
					} else {
						if (skillSettings.resourceRemove <= PlayerController.playerCharacterAPI.stats.GetResourceBySpec ()) {
							value = true;
						}
					}
				}
			}
		}
		return value;
	}



	public void UseSkill(){
		if (skillSettings.GetCd() != 0 && !PlayerController.playerCharacterAPI.stats.SkillOnCD(skillSettings.skillID)) {
			SkillPanelController.skillPanelController.StartCdFor (buttonID, skillSettings.GetCd());
			PlayerController.playerCharacterAPI.stats.StartCD (skillSettings.GetCd(), skillSettings.skillID);
		}
		if (skillSettings.globalCooldown != 0 && !PlayerController.playerCharacterAPI.stats.onGlobalCooldown) {
			SkillPanelController.skillPanelController.StartGcd (buttonID, skillSettings.globalCooldown);
			PlayerController.playerCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		}
		MethodInfo method = typeof(Skills).GetMethod (skillSettings.skillMethod);
		//Debug.Log (skillSettings.skillMethod);
		ParameterInfo[] methodParams = method.GetParameters();
		List<object> finalMethodParams = new List<object> ();
		foreach (ParameterInfo methodParam in methodParams) {
			finalMethodParams.Add(methodParam.DefaultValue);
		}
		method.Invoke ( PlayerController.playerCharacterAPI.skills, finalMethodParams.ToArray());
	}


	void SetBarRects(){
		coolDown.SetActive (true);
		gcd.SetActive (true);

		coolDownRect = coolDown.GetComponent<RectTransform> ();
		gcdRect = gcd.GetComponent<RectTransform> ();

		maximumCooldownHeight = coolDownRect.rect.height;
		maximumGcdHeight = gcdRect.rect.height;

		cooldownRectPosition = new Vector2(coolDownRect.rect.position.x, coolDownRect.rect.position.y);
		gcdRectPosition = gcdRect.rect.position;

		newCooldownRectPosition.height = coolDownRect.rect.height;
		newGcdRectPosition.height = gcdRect.rect.height;

		coolDown.SetActive (false);
		gcd.SetActive (false);
	}

	public void SetCd(float cd, float maximumCd){
		if (cd != 0) {
			coolDown.SetActive (true);
		} else {
			coolDown.SetActive (false);
		}
		//int currentHealth = (int)(System.Math.Round(cd, 0));
		float oneHeightPoint = maximumCd / maximumCooldownHeight;
		coolDownRect.sizeDelta = new Vector2(coolDownRect.rect.width, cd / oneHeightPoint);
		newCooldownRectPosition.position = new Vector2(cooldownRectPosition.x + coolDownRect.rect.width/2, cooldownRectPosition.y + (maximumCooldownHeight - coolDownRect.rect.height));
		newCooldownRectPosition.height = coolDownRect.rect.height;
		coolDownRect.localPosition = newCooldownRectPosition.center;
	}

	public void SetGcd(float gcdValue, float maximumGcd){
		if (gcdValue != 0) {
			gcd.SetActive (true);
		} else {
			gcd.SetActive (false);
		}
		//int currentHealth = (int)(System.Math.Round(cd, 0));
		float oneHeightPoint = maximumGcd / maximumGcdHeight;
		gcdRect.sizeDelta = new Vector2(gcdRect.rect.width, gcdValue / oneHeightPoint);
		newGcdRectPosition.position = new Vector2(gcdRectPosition.x + gcdRect.rect.width/2, gcdRectPosition.y + (maximumGcdHeight - gcdRect.rect.height));
		newGcdRectPosition.height = gcdRect.rect.height;
		gcdRect.localPosition = newGcdRectPosition.center;
	}


	public void SetSkill(int id){
		skillID = id;
		if (skillID != -1) {
			skillSettings = SkillSettingsSet.GetSettings (skillID);
		} else {
			skillSettings = null;
		}
		buttonImage.sprite = SkillPanelController.skillPanelController.GetSkillTexture ("skill-" + id.ToString());
	}


}
