using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class SkillSettingsSet{
	static List<int> allSkillIDs = new List<int> (new int[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,
		15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47});

	public static SkillSettings GetSettings(int id){
		MethodInfo method = typeof(SkillSettingsSet).GetMethod ("Settings_" + id.ToString (), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		SkillSettings type = (SkillSettings)method.Invoke (null, null);
		return type;
	}

	public static Dictionary<int, CurrentSkillState> GetAllSkillStates(){
		Dictionary<int, CurrentSkillState> states = new Dictionary<int, CurrentSkillState> ();
		foreach (int id in allSkillIDs) {
			SkillSettings settings = SkillSettingsSet.GetSettings (id);
			CurrentSkillState state = new CurrentSkillState ();
			state.specID = settings.specID;
			state.skillID = id;
			state.skillActive = true;
			states.Add (id, state);
		}
		return states;
	}

	private static SkillSettings Settings_0(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fastPunch;
		settings.specID = Stats.meleeSpec;
		settings.skillMethod = "FastPunch";
		settings.distance = 0.8f;
		settings.botUseDistance = 0.8f;
		settings.resourceAdd = 9;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 0.1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		settings.isAddResourceSkill = true;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_1(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.middlePunch;
		settings.specID = Stats.meleeSpec;
		settings.skillMethod = "MiddlePunch";
		settings.distance = 0.8f;
		settings.botUseDistance = 0.8f;
		settings.resourceAdd = 15;
		settings.globalCooldown = 1f;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		settings.isAddResourceSkill = true;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_2(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.slowPunch;
		settings.specID = Stats.meleeSpec;
		settings.skillMethod = "SlowPunch";
		settings.distance = 5f;
		settings.botUseDistance = 0.8f;
		settings.resourceAdd = 100;
		settings.globalCooldown = 0.5f;
		settings.cooldown = 7f;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		settings.isAddResourceSkill = true;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_3(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.meleeCone;
		settings.specID = Stats.meleeSpec;
		settings.skillMethod = "MeleeCone";
		settings.distance = 1.2f;
		settings.botUseDistance = 1.2f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 5f;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_4(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.circlePunch;
		settings.specID = Stats.meleeSpec;
		settings.skillMethod = "CirclePunch";
		settings.distance = 1.2f;
		settings.botUseDistance = 1.2f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 5f;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_5(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.meleeWave;
		settings.specID = Stats.meleeSpec;
		settings.skillMethod = "MeleeWave";
		settings.distance = 6f;
		settings.botUseDistance = 6f;
		settings.minimumDistanceToUse = 2f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 2f;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_6(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.blackHole;
		settings.specID = Stats.meleeSpec;
		settings.skillMethod = "BlackHole";
		settings.distance = 6f;
		settings.botUseDistance = 6f;
		settings.minimumDistanceToUse = 3f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 2f;
		settings.chance = 1f;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_7(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.chains;
		settings.specID = Stats.meleeSpec;
		settings.skillMethod = "Chains";
		settings.distance = 1.2f;
		settings.botUseDistance = 1.2f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 0f;
		settings.slowTime = 5f;
		settings.slowSpeed = 0.2f;
		settings.chance = 2f;
		settings.descriptionType = 1;

		settings.percentTalentStep = 10;
		settings.percentMaximumTalentStatus = 5;
		return settings;
	}

	private static SkillSettings Settings_8(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.meleeStun;
		settings.specID = Stats.meleeSpec;
		settings.skillMethod = "MeleeStun";
		settings.distance = 0.8f;
		settings.botUseDistance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.resourceRemove = 1;
		settings.damagePercent = 1f;
		settings.stunTime = 10f;
		settings.chance = 0.01f;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_9(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.meleeWaveSlow;
		settings.specID = Stats.meleeSpec;
		settings.skillMethod = "MeleeWaveSlow";
		settings.distance = 6f;
		settings.botUseDistance = 6f;
		settings.minimumDistanceToUse = 2f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 4f;
		settings.slowSpeed = 0.7f;
		settings.chance = 2f;
		settings.descriptionType = 1;

		settings.percentTalentStep = 10;
		settings.percentMaximumTalentStatus = 3;
		return settings;
	}

	private static SkillSettings Settings_10(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.charge;
		settings.specID = Stats.meleeSpec;
		settings.skillMethod = "Charge";
		settings.distance = 7f;
		settings.botUseDistance = 7f;
		settings.minimumDistanceToUse = 3f;
		settings.resourceRemove = 0;
		settings.globalCooldown = 0.5f;
		settings.cooldown = 5f;
		settings.damagePercent = 1f;
		settings.stunTime = 2f;
		settings.slowTime = 4f;
		settings.slowSpeed = 0.8f;
		settings.chance = 4f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		settings.cdTalentStep = 0.5f;
		settings.cdMaximumTalentStatus = 5;
		return settings;
	}

	private static SkillSettings Settings_11(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.thunderClap;
		settings.specID = Stats.meleeSpec;
		settings.skillMethod = "ThunderClap";
		settings.distance = 2f;
		settings.botUseDistance = 2f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 4f;
		settings.slowSpeed = 0.2f;
		settings.stunTime = 2f;
		settings.chance = 0.5f;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_12(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.siphonLife;
		settings.specID = Stats.meleeSpec;
		settings.skillMethod = "SiphonLife";
		settings.distance = 0.7f;
		settings.botUseDistance = 0.7f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.restorePercent = 1f;
		settings.chance = 8f;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_13(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fastShot;
		settings.specID = Stats.fireSpec;
		settings.skillMethod = "FastShot";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 0.5f;
		settings.resourceRemove = 9;
		settings.damagePercent = 0.1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;
		settings.withAimLine = true;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_14(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.middleShot;
		settings.specID = Stats.fireSpec;
		settings.skillMethod = "MiddleShot";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 1f;
		settings.resourceRemove = 15;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;
		settings.withAimLine = true;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_15(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.slowShot;
		settings.specID = Stats.fireSpec;
		settings.skillMethod = "SlowShot";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 1.5f;
		settings.resourceRemove = Stats.maximumFireEnergy;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;
		settings.withAimLine = true;
		settings.autoTarget = true;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_16(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.grenadeLaunch;
		settings.specID = Stats.fireSpec;
		settings.skillMethod = "GrenadeLaunch";
		settings.distance = 7f;
		settings.botUseDistance = 6f;
		settings.resourceAdd = 50;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.withSlideAIM = true;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_17(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.grenadeWave;
		settings.specID = Stats.fireSpec;
		settings.skillMethod = "GrenadeWave";
		settings.distance = 6f;
		settings.botUseDistance = 6f;
		settings.minimumDistanceToUse = 2f;
		settings.resourceAdd = 50;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 3f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_18(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.rocketCircle;
		settings.specID = Stats.fireSpec;
		settings.skillMethod = "RocketCircle";
		settings.distance = 0.8f;
		settings.botUseDistance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.resourceAdd = 50;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_19(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.explosiveMine;
		settings.specID = Stats.fireSpec;
		settings.skillMethod = "ExplosiveMine";
		settings.distance = 6f;
		settings.botUseDistance = 6f;
		settings.resourceAdd = 50;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.stunTime = 5f;
		settings.chance = 1f;
		settings.withSlideAIM = true;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_20(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.lightingBombLaunch;
		settings.specID = Stats.fireSpec;
		settings.skillMethod = "LightingBombLaunch";
		settings.distance = 7f;
		settings.botUseDistance = 6f;
		settings.resourceAdd = 50;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 15f;
		settings.slowTime = 4f;
		settings.slowSpeed = 0.9f;
		settings.chance = 1f;
		settings.withSlideAIM = true;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;

		settings.percentTalentStep = 10;
		settings.percentMaximumTalentStatus = 4;
		return settings;
	}

	private static SkillSettings Settings_21(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.snipeShot;
		settings.specID = Stats.fireSpec;
		settings.skillMethod = "SnipeShot";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.resourceAdd = 50;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.stunTime = 10f;
		settings.chance = 0.4f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;
		settings.withAimLine = true;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_22(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.toxicGrenadeWave;
		settings.specID = Stats.fireSpec;
		settings.skillMethod = "ToxicGrenadeWave";
		settings.distance = 6f;
		settings.botUseDistance = 6f;
		settings.minimumDistanceToUse = 2f;
		settings.resourceAdd = 50;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 10f;
		settings.slowSpeed = 0.2f;
		settings.chance = 1f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 5;
		settings.damageTalentStatusSoftCap = 3;
		return settings;
	}

	private static SkillSettings Settings_23(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.rocketJump;
		settings.specID = Stats.fireSpec;
		settings.skillMethod = "RocketJump";
		settings.distance = 0.8f;
		settings.botUseDistance = 0.8f;
		settings.resourceAdd = 50;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 1f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_24(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fireClap;
		settings.specID = Stats.fireSpec;
		settings.skillMethod = "FireClap";
		settings.distance = 2f;
		settings.botUseDistance = 2f;
		settings.resourceAdd = 50;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 4f;
		settings.slowSpeed = 0.2f;
		settings.stunTime = 2f;
		settings.chance = 4f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_25(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.toxicGrenadeLaunch;
		settings.specID = Stats.fireSpec;
		settings.skillMethod = "ToxicGrenadeLaunch";
		settings.distance = 7f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.stunTime = 4f;
		settings.chance = 0.5f;
		settings.withSlideAIM = true;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_47(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.emergencyCooling;
		settings.specID = Stats.fireSpec;
		settings.skillMethod = "EmergencyCooling";
		settings.distance = 8f;
		settings.botUseDistance = 3f;
		//settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		//settings.chance = 1f;
		settings.descriptionType = 1;
		settings.cooldown = 10f;
		//settings.existingTime = 5f;
		//settings.damageTalentPercentStep = 10;
		//settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_26(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fastCast;
		settings.specID = Stats.elementalSpec;
		settings.skillMethod = "FastCast";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 0.5f;
		settings.resourceAdd = 9;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;
		settings.withAimLine = true;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_27(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.middleCast;
		settings.specID = Stats.elementalSpec;
		settings.skillMethod = "MiddleCast";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 0.5f;
		settings.resourceAdd = 15;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;
		settings.withAimLine = true;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_28(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.slowCast;
		settings.specID = Stats.elementalSpec;
		settings.skillMethod = "SlowCast";
		settings.distance = 4f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 0.5f;
		settings.cooldown = 7f;
		settings.resourceAdd = Stats.maximumMagicEnergy;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;
		//settings.withAimLine = true;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_29(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.elementalCone;
		settings.specID = Stats.elementalSpec;
		settings.skillMethod = "ElementalCone";
		settings.distance = 1.1f;
		settings.botUseDistance = 1.1f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.resourceRemove = 1;
		settings.chance = 8f;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_30(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fireWall;
		settings.specID = Stats.elementalSpec;
		settings.skillMethod = "FireWall";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.descriptionType = 1;
		settings.withAimLine = true;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_31(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.iceWave;
		settings.specID = Stats.elementalSpec;
		settings.skillMethod = "IceWave";
		settings.distance = 6f;
		settings.botUseDistance = 6f;
		settings.minimumDistanceToUse = 2.5f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 3f;
		settings.slowSpeed = 0.4f;
		settings.chance = 2f;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 4;
		return settings;
	}

	private static SkillSettings Settings_32(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.iceStun;
		settings.specID = Stats.elementalSpec;
		settings.skillMethod = "IceStun";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 0.5f;
		settings.resourceRemove = 1;
		settings.damagePercent = 1f;
		settings.stunTime = 10f;
		settings.chance = 0.4f;
		settings.descriptionType = 1;
		settings.withAimLine = true;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_33(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.blizzard;
		settings.specID = Stats.elementalSpec;
		settings.skillMethod = "Blizzard";
		settings.distance = 0.8f;
		settings.botUseDistance = 0.8f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 0.1f;
		settings.existingTime = 4f;
		settings.slowTime = 0.5f;
		settings.slowSpeed = 0.3f;
		settings.stunTime = 2f;
		settings.chance = 5f;
		settings.stunChance = 0.7f;
		settings.descriptionType = 1;

		settings.percentTalentStep = 5;
		settings.percentMaximumTalentStatus = 4;
		return settings;
	}

	private static SkillSettings Settings_34(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.chainLavaBurst;
		settings.specID = Stats.elementalSpec;
		settings.skillMethod = "ChainLavaBurst";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 7f;
		settings.descriptionType = 1;
		settings.withAimLine = true;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_35(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fireNova;
		settings.specID = Stats.elementalSpec;
		settings.skillMethod = "FireNova";
		settings.distance = 0.8f;
		settings.botUseDistance = 0.8f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_36(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.physicalShield;
		settings.specID = Stats.elementalSpec;
		settings.skillMethod = "PhysicalShield";
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 15f;
		settings.restorePercent = 2f;
		settings.cooldown = 10f;
		settings.chance = 2f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_37(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.elementalShield;
		settings.specID = Stats.elementalSpec;
		settings.skillMethod = "ElementalShield";
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 15f;
		settings.restorePercent = 2f;
		settings.cooldown = 10f;
		settings.chance = 2f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_38(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.hybridShield;
		settings.specID = Stats.elementalSpec;
		settings.skillMethod = "HybridShield";
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 15f;
		settings.restorePercent = 2f;
		settings.cooldown = 10f;
		settings.chance = 2f;
		settings.canUseWithoutResource = true;
		settings.descriptionType = 1;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_39(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.earthDisruption;
		settings.specID = Stats.elementalSpec;
		settings.skillMethod = "EarthDisruption";
		settings.distance = 8f;
		settings.botUseDistance = 3f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.stunTime = 2f;
		settings.chance = 2f;
		settings.descriptionType = 1;
		settings.withAimLine = true;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_40(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.iceWall;
		settings.specID = Stats.elementalSpec;
		settings.skillMethod = "IceWall";
		settings.distance = 8f;
		settings.botUseDistance = 3f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 8f;
		settings.slowSpeed = 0.2f;
		settings.chance = 1f;
		settings.descriptionType = 1;
		settings.withAimLine = true;

		settings.damageTalentPercentStep = 10;
		settings.damageTalentStatusSoftCap = 5;
		return settings;
	}


	private static SkillSettings Settings_41(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.healthInjection1;
		settings.specID = Stats.commonSpec;
		settings.skillMethod = "HealthInjection1";
		settings.distance = 8f;
		settings.botUseDistance = 3f;
		//settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		//settings.damagePercent = 1f;
		//settings.slowTime = 8f;
		//settings.slowSpeed = 0.2f;
		//settings.chance = 1f;
		settings.descriptionType = 1;

		//settings.damageTalentPercentStep = 10;
		//settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_42(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.healthInjection2;
		settings.specID = Stats.commonSpec;
		settings.skillMethod = "HealthInjection2";
		settings.distance = 8f;
		settings.botUseDistance = 3f;
		//settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		//settings.damagePercent = 1f;
		//settings.slowTime = 8f;
		//settings.slowSpeed = 0.2f;
		//settings.chance = 1f;
		settings.descriptionType = 1;

		//settings.damageTalentPercentStep = 10;
		//settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_43(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.healthInjection3;
		settings.specID = Stats.commonSpec;
		settings.skillMethod = "HealthInjection3";
		settings.distance = 8f;
		settings.botUseDistance = 3f;
		//settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		//settings.damagePercent = 1f;
		//settings.slowTime = 8f;
		//settings.slowSpeed = 0.2f;
		//settings.chance = 1f;
		settings.descriptionType = 1;

		//settings.damageTalentPercentStep = 10;
		//settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_44(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.speedRun;
		settings.specID = Stats.commonSpec;
		settings.skillMethod = "SpeedRun";
		settings.distance = 8f;
		settings.botUseDistance = 3f;
		//settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		//settings.damagePercent = 1f;
		settings.slowTime = 3f;
		settings.slowSpeed = 0.7f;
		//settings.chance = 1f;
		settings.descriptionType = 1;
		settings.cooldown = 10f;

		//settings.damageTalentPercentStep = 10;
		//settings.damageTalentStatusSoftCap = 5;
		return settings;
	}


	private static SkillSettings Settings_45(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.antiroot;
		settings.specID = Stats.commonSpec;
		settings.skillMethod = "Antiroot";
		settings.distance = 8f;
		settings.botUseDistance = 3f;
		//settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		//settings.chance = 1f;
		settings.descriptionType = 1;
		settings.cooldown = 5f;
		settings.ignoreSilence = true;
		//settings.damageTalentPercentStep = 10;
		//settings.damageTalentStatusSoftCap = 5;
		return settings;
	}

	private static SkillSettings Settings_46(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.vanish;
		settings.specID = Stats.commonSpec;
		settings.skillMethod = "Vanish";
		settings.distance = 8f;
		settings.botUseDistance = 3f;
		//settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		//settings.chance = 1f;
		settings.descriptionType = 1;
		settings.cooldown = 30f;
		settings.existingTime = 5f;
		//settings.damageTalentPercentStep = 10;
		//settings.damageTalentStatusSoftCap = 5;
		return settings;
	}






	public class SkillID{
		public const int fastPunch = 0;
		public const int middlePunch = 1;
		public const int slowPunch = 2;
		public const int meleeCone = 3;
		public const int circlePunch = 4;
		public const int meleeWave = 5;
		public const int blackHole = 6;
		public const int chains = 7;
		public const int meleeStun = 8;
		public const int meleeWaveSlow = 9;
		public const int charge = 10;
		public const int thunderClap = 11;
		public const int siphonLife = 12;

		public const int fastShot = 13;
		public const int middleShot = 14;
		public const int slowShot = 15;
		public const int grenadeLaunch = 16;
		public const int grenadeWave = 17;
		public const int rocketCircle = 18;
		public const int explosiveMine = 19;
		public const int lightingBombLaunch = 20;
		public const int snipeShot = 21;
		public const int toxicGrenadeWave = 22;
		public const int rocketJump = 23;
		public const int fireClap = 24;
		public const int toxicGrenadeLaunch = 25;
		public const int emergencyCooling = 47;

		public const int fastCast = 26;
		public const int middleCast = 27;
		public const int slowCast = 28;
		public const int elementalCone = 29;
		public const int fireWall = 30;
		public const int iceWave = 31;
		public const int iceStun = 32;
		public const int blizzard = 33;
		public const int chainLavaBurst = 34;
		public const int fireNova = 35;
		public const int physicalShield = 36;
		public const int elementalShield = 37;
		public const int hybridShield = 38;
		public const int earthDisruption = 39;
		public const int iceWall = 40;

		public const int healthInjection1 = 41;
		public const int healthInjection2 = 42;
		public const int healthInjection3 = 43;
		public const int speedRun = 44;
		public const int antiroot = 45;
		public const int vanish = 46;
	}


}
