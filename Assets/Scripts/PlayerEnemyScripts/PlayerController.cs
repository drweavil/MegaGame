using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	public const int melee = 0, fire = 1, magic = 2;
	public static int currentSpec = 0;
	public static  GameObject player;
	public GameObject nonStaticPlayer;
	public static float maximumComplexity = 1200;
	public static int healthInjectionPoolX = 10;
	public static CharacterAPI playerCharacterAPI;
	public CharacterAPI nonStaticCharacterAPI;
	public static List<CharacterAPI> leftEnemyChain = new List<CharacterAPI> ();
	public static List<CharacterAPI> rightEnemyChain = new List<CharacterAPI> ();


	public static Equipment head;
	public static Equipment chest;
	public static Equipment legs;
	public static Equipment trinket;
	public static Equipment finger;
	public static Equipment neck;
	public static Equipment meleeWeapon;
	public static Equipment fireWeapon;
	public static Equipment elementalWeapon;

	public static int currentHealthInjectionPool = 327;
	public static int lifes = 12;
	public static int gold = 288522;
	public static int donatGold = 234; 

	public static List<BackpackItem> backPackItems = new List<BackpackItem> ();
	public static float currentWeight = 0;
	public static float maximumWeight = 100f; 





	void Awake(){
		player = nonStaticPlayer;
		playerCharacterAPI = nonStaticCharacterAPI;
		//ChangePlayerSpec (currentSpec);

		StartCoroutine(StartProcess.StartActionAfterFewFrames (10, () => {
			PlayerController.SetEquip(EquipmentGenerator.GetTestRandomEquipment(600, Equipment.head));
			PlayerController.SetEquip(EquipmentGenerator.GetTestRandomEquipment(600, Equipment.chest));
			PlayerController.SetEquip(EquipmentGenerator.GetTestRandomEquipment(600, Equipment.legs));
			PlayerController.SetEquip(EquipmentGenerator.GetTestRandomEquipment(600, Equipment.trinket));
			PlayerController.SetEquip(EquipmentGenerator.GetTestRandomEquipment(600, Equipment.finger));
			PlayerController.SetEquip(EquipmentGenerator.GetTestRandomEquipment(600, Equipment.neck));
			PlayerController.SetEquip(EquipmentGenerator.GetTestRandomEquipment(600, Equipment.meleeWeapon));
			PlayerController.SetEquip(EquipmentGenerator.GetTestRandomEquipment(600, Equipment.fireWeapon));
			PlayerController.SetEquip(EquipmentGenerator.GetTestRandomEquipment(600, Equipment.elementalWeapon));

			for(int i = 0; i < 10; i++){
				BackpackController.AddBackpackItem(BackpackItemParamsController.GetNewBackpackItem(EquipmentGenerator.GetTestRandomEquipment(600, (int)Random.Range(0, 9))), false);
			}

		}));
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Q)) {
			//Debug.Log ("lol");
			/*Tools.RenameFiles("C://megaGameWorkDirectory/icons/smot/finger/", "C://megaGameWorkDirectory/icons/smot/renamed/finger/", "finger_");
			Tools.RenameFiles("C://megaGameWorkDirectory/icons/smot/gun/", "C://megaGameWorkDirectory/icons/smot/renamed/gun/", "r_w_");
			Tools.RenameFiles("C://megaGameWorkDirectory/icons/smot/head/", "C://megaGameWorkDirectory/icons/smot/renamed/head/", "head_");
			Tools.RenameFiles("C://megaGameWorkDirectory/icons/smot/legs/", "C://megaGameWorkDirectory/icons/smot/renamed/legs/", "legs_");*/
			//Tools.RenameFiles("C://megaGameWorkDirectory/icons/smot/mili/", "C://megaGameWorkDirectory/icons/smot/renamed/mili/", "m_w_");
			/*Tools.RenameFiles("C://megaGameWorkDirectory/icons/smot/neck/", "C://megaGameWorkDirectory/icons/smot/renamed/neck/", "neck_");
			Tools.RenameFiles("C://megaGameWorkDirectory/icons/smot/tors/", "C://megaGameWorkDirectory/icons/smot/renamed/tors/", "chest_");*/
			//Tools.RenameFiles("C://megaGameWorkDirectory/icons/smot/tpm/", "C://megaGameWorkDirectory/icons/smot/renamed/tpm/", "e_w_");
			//Tools.RenameFiles("C://megaGameWorkDirectory/icons/smot/trink/", "C://megaGameWorkDirectory/icons/smot/renamed/trink/", "trinket_");



		    //List<Sprite> armorSprites = new List<Sprite> (Resources.LoadAll<Sprite>("Textures/humanSprite 1"));
			//List<Sprite> weaponSprites = new List<Sprite> (Resources.LoadAll<Sprite>("Textures/kower_weap"));
			//Debug.Log (weaponSprites.FindIndex (s => s.name == "m_w_6"));

			/*Tools.RenameSprites (ref weaponSprites, "C://megaGameWorkDirectory/icons/smot/mili/", "m_w_");
			Tools.RenameSprites (ref weaponSprites, "C://megaGameWorkDirectory/icons/smot/gun/", "r_w_");
			Tools.RenameSprites (ref weaponSprites, "C://megaGameWorkDirectory/icons/smot/tpm/", "e_w_");
			Tools.RenameSprite (ref weaponSprites, "s10_3", "e_w_40");*/

			//Tools.SaveSprites (weaponSprites, "C://megaGameWorkDirectory/weaponSprites");
			//Tools.SavePivots (weaponSprites, "C://megaGameWorkDirectory/weaponSprites");
			//Debug.Log(Application.dataPath);
			//Tools.SetPivots("Textures/weaponSprites", "C://megaGameWorkDirectory/weaponSprites/pivots.dct");
			//Tools.RenameArmorSprites(ref armorSprites, "C://megaGameWorkDirectory/icons/smot/head/", "-head", "head_");

			//Debug.Log (BackpackController.GetPagesCount());

		}
	}


	public static void SetEquip(Equipment equip){
		Equipment oldEquip = new Equipment();
		oldEquip.slotID = -1;
		if (equip.slotID == Equipment.head) {
			oldEquip = head;
			head = equip;
			playerCharacterAPI.reskinController.SetHelm (equip.skinID);
		} else if (equip.slotID == Equipment.chest) {
			oldEquip = chest;
			chest = equip;
			playerCharacterAPI.reskinController.SetChest (equip.skinID);
		} else if (equip.slotID == Equipment.legs) {
			oldEquip = legs;
			legs = equip;
			playerCharacterAPI.reskinController.SetLegs (equip.skinID);
		} else if (equip.slotID == Equipment.trinket) {
			oldEquip = trinket;
			trinket = equip;
		} else if (equip.slotID == Equipment.finger) {
			oldEquip = finger;
			finger = equip;
		} else if (equip.slotID == Equipment.neck) {
			oldEquip = neck;
			neck = equip;
		}

		if (equip.slotID == Equipment.meleeWeapon) {
			oldEquip = meleeWeapon;
			meleeWeapon = equip;
			playerCharacterAPI.stats.weaponDamage = equip.weaponDamage;
			playerCharacterAPI.reskinController.meleeWeaponSkinID = equip.skinID;
			if (playerCharacterAPI.stats.specId == Stats.meleeSpec) {
				playerCharacterAPI.reskinController.SetMeleeWeapon (equip.skinID);
			}
		} else if (equip.slotID == Equipment.fireWeapon) {
			oldEquip = fireWeapon;
			fireWeapon = equip;
			playerCharacterAPI.stats.fireWeaponDamage = equip.weaponDamage;
			playerCharacterAPI.reskinController.fireWeaponSkinID = equip.skinID;
			if (playerCharacterAPI.stats.specId == Stats.fireSpec) {
				playerCharacterAPI.reskinController.SetFireWeapon (equip.skinID);
			}
		} else if (equip.slotID == Equipment.elementalWeapon) {
			oldEquip = elementalWeapon;
			elementalWeapon = equip;
			playerCharacterAPI.stats.elementalWeaponDamage = equip.weaponDamage;
			playerCharacterAPI.reskinController.elementalWeaponSkinID = equip.skinID;
			if (playerCharacterAPI.stats.specId == Stats.elementalSpec) {
				playerCharacterAPI.reskinController.SetElementalWeapon (equip.skinID);
			}
		}

		if (oldEquip != null) {
			if (oldEquip.slotID != -1) {
				playerCharacterAPI.stats.ChangeHealthPoints (-oldEquip.GetOverallHealthPoints ());
				playerCharacterAPI.stats.ChangeArmorPoints (-oldEquip.GetOverallPhysicalArmorPoints ());
				playerCharacterAPI.stats.ChangeElementalArmorPoints (-oldEquip.GetOverallElementalArmorPoints ());
				playerCharacterAPI.stats.ChangePhysicalDamagePoints (-oldEquip.GetOverallPhysicalDamagePoints ());
				playerCharacterAPI.stats.ChangeElementalDamagePoints (-oldEquip.GetOverallElementalDamagePoints ());
				playerCharacterAPI.stats.ChangeCriticalPoints (-oldEquip.GetOverallCriticalPoints ());
			}
		}


		playerCharacterAPI.stats.ChangeHealthPoints (equip.GetOverallHealthPoints());
		playerCharacterAPI.stats.ChangeArmorPoints (equip.GetOverallPhysicalArmorPoints());
		playerCharacterAPI.stats.ChangeElementalArmorPoints (equip.GetOverallElementalArmorPoints());
		playerCharacterAPI.stats.ChangePhysicalDamagePoints (equip.GetOverallPhysicalDamagePoints());
		playerCharacterAPI.stats.ChangeElementalDamagePoints (equip.GetOverallElementalDamagePoints());
		playerCharacterAPI.stats.ChangeCriticalPoints (equip.GetOverallCriticalPoints());
	}


	public static void RemoveEquip(Equipment equip){
		playerCharacterAPI.stats.ChangeHealthPoints (-equip.GetOverallHealthPoints());
		playerCharacterAPI.stats.ChangeArmorPoints (-equip.GetOverallPhysicalArmorPoints());
		playerCharacterAPI.stats.ChangeElementalArmorPoints (-equip.GetOverallElementalArmorPoints());
		playerCharacterAPI.stats.ChangePhysicalDamagePoints (-equip.GetOverallPhysicalDamagePoints());
		playerCharacterAPI.stats.ChangeElementalDamagePoints (-equip.GetOverallElementalDamagePoints());
		playerCharacterAPI.stats.ChangeCriticalPoints (-equip.GetOverallCriticalPoints());



		SetEquip (GetNullEquipment(equip.slotID));
	}

	public static Equipment GetNullEquipment(int slotID){
		Equipment nullEquip = new Equipment ();
		nullEquip.slotID = slotID;
		nullEquip.isNullEquip = true;
		nullEquip.skinID = 0;
		nullEquip.healthPoints = 1;
		nullEquip.physicalArmorPoints = 1;
		nullEquip.elementalArmorPoints = 1;
		nullEquip.criticalPoints = 1;
		nullEquip.physicalDamagePoints = 1;
		nullEquip.elementalDamagePoints = 1;
		return nullEquip;
	}

	public static void ChangePlayerSpec(int specId){
		playerCharacterAPI.stats.SetSpec (specId);
		playerCharacterAPI.healthBar.ChangeToSpec (specId);
	}

	public static float GetMaximumHealthInjectionPool(){
		return healthInjectionPoolX * playerCharacterAPI.stats.GetMaximumHealth();
	}
		
}
