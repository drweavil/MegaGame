using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	public const int melee = 0, fire = 1, magic = 2;
	public static int currentSpec = 0;
	public static  GameObject player;
	public GameObject nonStaticPlayer;
	public static float minimumPrice = 0.00001f;
	public static float maximumPrice = 100f;
	public static float minimumComplexity = 0.00001f;
	public static float currentComplexity = 600f;
	public static float maximumComplexity = 1200f;
	public static int maximumBackpackID = 0;
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

	public static float currentHealthInjectionPool = 2000f;
	public static int lifes = 12;
	public static int gold = 288522;
	public static int donatGold = 234; 

	public static float currentExpValue = 50;
	public static float nextTalentExpValue = 100f;
	public static int talentsAvailableNumber = 50;

	public static List<BackpackItem> backPackItems = new List<BackpackItem> ();
	public static float currentWeight = 0;
	public static float maximumWeight = 100f; 

	public static Dictionary<int, CurrentSkillState> skillStates = new Dictionary<int, CurrentSkillState> ();



	public DialogRectResizer testResizer;
	int testInt = 1;

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

			for(int i = 0; i < 8; i++){
				BackpackController.AddBackpackItem(BackpackItemParamsController.GetNewBackpackItem(EquipmentGenerator.GetTestRandomEquipment(600, (int)Random.Range(0, 9))), false);
			}

			for(int i = 0; i < 8; i++){
				BackpackController.AddBackpackItem (BackpackItemParamsController.GetNewBackpackItem (EquipmentGenerator.GetEquipmentRuneByComplexity (Random.Range (1, maximumComplexity), EquipmentGenerator.smallRunePercent)), false);
			}
			BackpackController.AddBackpackItem(BackpackItemParamsController.GetNewBackpackItem(EquipmentGenerator.GetRandomBuff()), false);
			InventorySkill testSkill = new InventorySkill();
			testSkill.skillID = 1;
			BackpackController.AddBackpackItem(BackpackItemParamsController.GetNewBackpackItem(testSkill), false);

			InventorySkill testSkill2 = new InventorySkill();
			testSkill2.skillID = 2;
			BackpackController.AddBackpackItem(BackpackItemParamsController.GetNewBackpackItem(testSkill2), false);

			BackpackController.AddBackpackItem (BackpackItemParamsController.GetNewBackpackItem (EquipmentGenerator.GetHammer(1)), false);
			BackpackController.AddBackpackItem (BackpackItemParamsController.GetNewBackpackItem (EquipmentGenerator.GetHammer(2)), false);
			BackpackController.AddBackpackItem (BackpackItemParamsController.GetNewBackpackItem (EquipmentGenerator.GetHammer(3)), false);
		
		


			for(int i = 0; i < 8; i++){
				SkillActivator act = new SkillActivator();
				act.skillID = i;
				BackpackController.AddBackpackItem (BackpackItemParamsController.GetNewBackpackItem (act), false);
			}






			skillStates = SkillSettingsSet.GetAllSkillStates();
		
		}));
		StartCoroutine (StartProcess.StartActionAfterFewFrames (12, () => {
			
		}));
	}

	void Update(){
		//Debug.Log (playerCharacterAPI.stats.controlDiminishingParam);
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

			/*BuffsController.StartBuff (Buffs.GetBuff(testInt));
			testInt += 1;*/

			/*BuffsController.StartBuff (Buffs.GetBuff(2));
			BuffsController.StartBuff (Buffs.GetBuff(3));
			BuffsController.StartBuff (Buffs.GetBuff(4));
			BuffsController.StartBuff (Buffs.GetBuff(5));
			BuffsController.StartBuff (Buffs.GetBuff(6));*/

			//Debug.Log ((LanguageController.jsonFile["buffs"]["buff-1"]["patternsProcess"][0].ToString()));

			/*BackpackItem lol = new BackpackItem ();
			BackpackItem lol2 = lol;

			lol = new BackpackItem ();

			lol.itemID = 1;
			lol2.itemID = 2;

			Debug.Log (lol.itemID);
			Debug.Log (lol2.itemID);*/

			/*Buff buff1 = Buffs.GetBuff (1);
			Buff buff2 = (Buff)ObjectCloneTool.CloneObject (buff1);

			buff1.buffID = 234;

			Debug.Log (buff1.buffID);
			Debug.Log (buff2.buffID);*/
			//Debug.Log (backPackItems.Count);
			//BackpackController.AddBackpackItem (BackpackItemParamsController.GetNewBackpackItem (EquipmentGenerator.GetEquipmentRuneByComplexity (Random.Range (1, maximumComplexity), EquipmentGenerator.smallRunePercent)));
			/*InventorySkill testSkill = new InventorySkill();
			testSkill.skillID = 1;
			BackpackController.AddBackpackItem(BackpackItemParamsController.GetNewBackpackItem(testSkill));*/
			//BackpackController.AddBackpackItem (BackpackItemParamsController.GetNewBackpackItem (EquipmentGenerator.GetHammer(1)));
			//BackpackController.AddBackpackItem (BackpackItemParamsController.GetNewBackpackItem (EquipmentGenerator.GetHammer(2)));
			BackpackController.AddBackpackItem (BackpackItemParamsController.GetNewBackpackItem (EquipmentGenerator.GetHammer(3)));

		}
		if (Input.GetKeyDown (KeyCode.B)) {
			//BattleInterfaceController.battleInterfaceController.aimLine.transform.position = Camera.main.WorldToScreenPoint(BattleInterfaceController.battleInterfaceController.playerAimLineStart.position);
			BuffsView.AddBuff(Buffs.GetBuff(7));
		}
		if (Input.GetKeyDown (KeyCode.N)) {
			//BattleInterfaceController.battleInterfaceController.aimLine.transform.position = Camera.main.WorldToScreenPoint(BattleInterfaceController.battleInterfaceController.playerAimLineStart.position);
			BuffsView.RemoveBuff(Buffs.GetBuff(7));
		}
		if (Input.GetKeyDown (KeyCode.M)) {
			//BattleInterfaceController.battleInterfaceController.aimLine.transform.position = Camera.main.WorldToScreenPoint(BattleInterfaceController.battleInterfaceController.playerAimLineStart.position);
			//Debug.Log(BuffsView.buffsView.currentBuffs.Count);
			LevelController.levelController.SetTileAtlas(1);
		}
	}

	public void SpawnEnemy(){
		GameObject enemy = ObjectsPool.PullObject ("InteractiveObjects/enemyH");
		Stats enemyStats = enemy.GetComponent<Stats> ();
		enemyStats.isDeath = false;
		enemyStats.RestoreMaximumHealth ();
		enemy.transform.position = playerCharacterAPI.transform.position;
	}

	public static int GetBackpackItemID(){
		int value = maximumBackpackID;
		maximumBackpackID = maximumBackpackID + 1;
		return value;
	}

	public static void AddTalentExp(float plus){
		currentExpValue += plus;
		while(currentExpValue >= nextTalentExpValue){
			currentExpValue = currentExpValue - nextTalentExpValue;
			talentsAvailableNumber += 1;
			nextTalentExpValue = GetNextTalentExp ();
		}

		if (TalentsInterface.talentsInterface.gameObject.activeInHierarchy) {
			TalentsInterface.talentsInterface.DrawTalentBarInfo ();
		}
	}


	public static float GetNextTalentExp(){
		return nextTalentExpValue + nextTalentExpValue * 0.1f;
	}

	public static Equipment GetEquipmentBySlotID(int id){
		Equipment equip = new Equipment ();
		if (id == Equipment.head) {
			equip = head;
		} else if(id == Equipment.chest){
			equip = chest;
		} else if(id == Equipment.legs){
			equip = legs;
		} else if(id == Equipment.trinket){
			equip = trinket;
		} else if(id == Equipment.finger){
			equip = finger;
		} else if(id == Equipment.neck){
			equip = neck;
		} else if(id == Equipment.meleeWeapon){
			equip = meleeWeapon;
		} else if(id == Equipment.fireWeapon){
			equip = fireWeapon;
		} else if(id == Equipment.elementalWeapon){
			equip = elementalWeapon;
		}
		return equip;
	}

	public static void SetEquip(Equipment equip, bool withRedrawIcon = false){
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
