﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentRune{
	public const int smallRune = 1, middleRune = 2, largeRune = 3;
	public int skinID = 0;
	public float healthPoints = 0;
	public float complexity = 0;
	public float criticalPoints = 0;
	public float physicalArmorPoints = 0;
	public float elementalArmorPoints = 0;
	public float physicalDamagePoints = 0;
	public float elementalDamagePoints = 0;


	public string GetTitle(){
		return "Rune title";
	}

	public string GetDesctiption(){
		return "Rune desctiption";
	}

	public Sprite GetIcon(){
		return SkillPanelController.skillPanelController.GetSkillTexture ("rune_" + skinID.ToString());
	}
}
