using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuExtends : EditorWindow {

	[MenuItem("Tools/UseTool")]
	public static void UseTool(){
		//Tools.SetPivots("Textures/weaponSprites", "C://megaGameWorkDirectory/weaponSprites/pivots.dct");
		List<Sprite> armorSprites = new List<Sprite> (Resources.LoadAll<Sprite>("Textures/humanSprite 1"));
		//Tools.RenameArmorSprites(resoursePath, "C://megaGameWorkDirectory/icons/smot/head/", "-head", "head_");
		string resoursePath = "Textures/humanSprite 1";
		//Tools.RenameArmorSprites(resoursePath, "C://megaGameWorkDirectory/icons/smot/head/", "-head", "head_");

		/*Tools.RenameArmorSprites(resoursePath, "C://megaGameWorkDirectory/icons/smot/head/", "-tors", "tors_");
		Tools.RenameArmorSprites(resoursePath, "C://megaGameWorkDirectory/icons/smot/head/", "-legs-r", "arm_");
		Tools.RenameArmorSprites(resoursePath, "C://megaGameWorkDirectory/icons/smot/head/", "-elbow-r", "elbow_");
		Tools.RenameArmorSprites(resoursePath, "C://megaGameWorkDirectory/icons/smot/head/", "-wrist", "wrist_");
		Tools.RenameArmorSprites(resoursePath, "C://megaGameWorkDirectory/icons/smot/head/", "-wrist90", "wrist90_");
		Tools.RenameArmorSprites(resoursePath, "C://megaGameWorkDirectory/icons/smot/head/", "-wrist180", "wrist180_");
		Tools.RenameArmorSprites(resoursePath, "C://megaGameWorkDirectory/icons/smot/head/", "-leg-r", "leg_");
		Tools.RenameArmorSprites(resoursePath, "C://megaGameWorkDirectory/icons/smot/head/", "-shin", "shin_");
		Tools.RenameArmorSprites(resoursePath, "C://megaGameWorkDirectory/icons/smot/head/", "-feet-a", "feet_a_");
		Tools.RenameArmorSprites(resoursePath, "C://megaGameWorkDirectory/icons/smot/head/", "-feet-b", "feet_b_");

		Tools.RenameSprite (resoursePath, "hair_1", "head_0");
		Tools.RenameSprite (resoursePath, "tors_m", "tors_0");
		Tools.RenameSprite (resoursePath, "legs_m", "arm_0");
		Tools.RenameSprite (resoursePath, "elbow_m", "elbow_0");
		Tools.RenameSprite (resoursePath, "wrist", "wrist_0");
		Tools.RenameSprite (resoursePath, "wrist90", "wrist90_0");
		Tools.RenameSprite (resoursePath, "wist180", "wrist180_0");
		Tools.RenameSprite (resoursePath, "r_m_leg", "leg_0");
		Tools.RenameSprite (resoursePath, "shin_m", "shin_0");
		Tools.RenameSprite (resoursePath, "feet_a", "feet_a_0");
		Tools.RenameSprite (resoursePath, "feet_b", "feet_b_0");*/


		//Tools.SaveSprites (armorSprites, "C://megaGameWorkDirectory/armorSprites");
		//Tools.SavePivots (armorSprites, "C://megaGameWorkDirectory/armorSprites");
		Tools.SetPivots("Textures/armorSprites", "C://megaGameWorkDirectory/armorSprites/pivots.dct");
	}

	[MenuItem("Tools/Test")]
	public static void Test(){
		Debug.Log (System.Math.Round (0.001, 2));
	}
}
