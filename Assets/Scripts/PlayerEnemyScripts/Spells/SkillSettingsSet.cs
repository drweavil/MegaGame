using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class SkillSettingsSet{


	public static SkillSettings GetSettings(int id){
		MethodInfo method = typeof(SkillSettingsSet).GetMethod ("Settings_" + id.ToString (), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		SkillSettings type = (SkillSettings)method.Invoke (null, null);
		return type;
	}

	private static SkillSettings Settings_0(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fastPunch;
		settings.skillMethod = "FastPunch";
		settings.distance = 0.8f;
		settings.botUseDistance = 0.8f;
		settings.resourceAdd = 9;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 0.1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		settings.isAddResourceSkill = true;
		return settings;
	}

	private static SkillSettings Settings_1(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.middlePunch;
		settings.skillMethod = "MiddlePunch";
		settings.distance = 0.8f;
		settings.botUseDistance = 0.8f;
		settings.resourceAdd = 15;
		settings.globalCooldown = 1f;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		settings.isAddResourceSkill = true;
		return settings;
	}

	private static SkillSettings Settings_2(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.slowPunch;
		settings.skillMethod = "SlowPunch";
		settings.distance = 0.8f;
		settings.botUseDistance = 0.8f;
		settings.resourceAdd = 20;
		settings.globalCooldown = 1.5f;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		settings.isAddResourceSkill = true;
		return settings;
	}

	private static SkillSettings Settings_3(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.meleeCone;
		settings.skillMethod = "MeleeCone";
		settings.distance = 1.2f;
		settings.botUseDistance = 1.2f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 5f;
		return settings;
	}

	private static SkillSettings Settings_4(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.circlePunch;
		settings.skillMethod = "CirclePunch";
		settings.distance = 1.2f;
		settings.botUseDistance = 1.2f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 5f;
		return settings;
	}

	private static SkillSettings Settings_5(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.meleeWave;
		settings.skillMethod = "MeleeWave";
		settings.distance = 6f;
		settings.botUseDistance = 6f;
		settings.minimumDistanceToUse = 2f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 2f;
		return settings;
	}

	private static SkillSettings Settings_6(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.blackHole;
		settings.skillMethod = "BlackHole";
		settings.distance = 6f;
		settings.botUseDistance = 6f;
		settings.minimumDistanceToUse = 3f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 2f;
		settings.chance = 1f;
		return settings;
	}

	private static SkillSettings Settings_7(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.chains;
		settings.skillMethod = "Chains";
		settings.distance = 1.2f;
		settings.botUseDistance = 1.2f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 0f;
		settings.slowTime = 5f;
		settings.slowSpeed = 0.7f;
		settings.chance = 2f;
		return settings;
	}

	private static SkillSettings Settings_8(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.meleeStun;
		settings.skillMethod = "MeleeStun";
		settings.distance = 0.8f;
		settings.botUseDistance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.resourceRemove = 1;
		settings.damagePercent = 1f;
		settings.stunTime = 10f;
		settings.chance = 0.01f;
		return settings;
	}

	private static SkillSettings Settings_9(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.meleeWaveSlow;
		settings.skillMethod = "MeleeWaveSlow";
		settings.distance = 6f;
		settings.botUseDistance = 6f;
		settings.minimumDistanceToUse = 2f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 4f;
		settings.slowSpeed = 0.4f;
		settings.chance = 2f;
		return settings;
	}

	private static SkillSettings Settings_10(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.charge;
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
		return settings;
	}

	private static SkillSettings Settings_11(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.thunderClap;
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
		return settings;
	}

	private static SkillSettings Settings_12(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.siphonLife;
		settings.skillMethod = "SiphonLife";
		settings.distance = 0.7f;
		settings.botUseDistance = 0.7f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.restorePercent = 1f;
		settings.chance = 8f;
		return settings;
	}

	private static SkillSettings Settings_13(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fastShot;
		settings.skillMethod = "FastShot";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 0.5f;
		settings.resourceRemove = 9;
		settings.damagePercent = 0.1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_14(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.middleShot;
		settings.skillMethod = "MiddleShot";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 1f;
		settings.resourceRemove = 15;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_15(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.slowShot;
		settings.skillMethod = "SlowShot";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 1.5f;
		settings.resourceRemove = 20;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_16(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.grenadeLaunch;
		settings.skillMethod = "GrenadeLaunch";
		settings.distance = 7f;
		settings.botUseDistance = 6f;
		settings.resourceAdd = 50;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.withSlideAIM = true;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_17(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.grenadeWave;
		settings.skillMethod = "GrenadeWave";
		settings.distance = 6f;
		settings.botUseDistance = 6f;
		settings.minimumDistanceToUse = 2f;
		settings.resourceAdd = 50;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 3f;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_18(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.rocketCircle;
		settings.skillMethod = "RocketCircle";
		settings.distance = 0.8f;
		settings.botUseDistance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.resourceAdd = 50;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_19(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.explosiveMine;
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
		return settings;
	}

	private static SkillSettings Settings_20(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.lightingBombLaunch;
		settings.skillMethod = "LightingBombLaunch";
		settings.distance = 7f;
		settings.botUseDistance = 6f;
		settings.resourceAdd = 50;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 15f;
		settings.slowTime = 4f;
		settings.slowSpeed = 0.5f;
		settings.chance = 1f;
		settings.withSlideAIM = true;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_21(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.snipeShot;
		settings.skillMethod = "SnipeShot";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.resourceAdd = 50;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.stunTime = 10f;
		settings.chance = 0.4f;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_22(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.toxicGrenadeWave;
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
		return settings;
	}

	private static SkillSettings Settings_23(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.rocketJump;
		settings.skillMethod = "RocketJump";
		settings.distance = 0.8f;
		settings.botUseDistance = 0.8f;
		settings.resourceAdd = 50;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 1f;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_24(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fireClap;
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
		return settings;
	}

	private static SkillSettings Settings_25(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.toxicGrenadeLaunch;
		settings.skillMethod = "ToxicGrenadeLaunch";
		settings.distance = 7f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.stunTime = 4f;
		settings.chance = 0.5f;
		settings.withSlideAIM = true;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_26(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fastCast;
		settings.skillMethod = "FastCast";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 0.5f;
		settings.resourceAdd = 9;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_27(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.middleCast;
		settings.skillMethod = "MiddleCast";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 1f;
		settings.resourceAdd = 15;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_28(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.slowCast;
		settings.skillMethod = "SlowCast";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 1.5f;
		settings.resourceAdd = 20;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_29(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.elementalCone;
		settings.skillMethod = "ElementalCone";
		settings.distance = 1.1f;
		settings.botUseDistance = 1.1f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.resourceRemove = 1;
		settings.chance = 8f;
		return settings;
	}

	private static SkillSettings Settings_30(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fireWall;
		settings.skillMethod = "FireWall";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		return settings;
	}

	private static SkillSettings Settings_31(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.iceWave;
		settings.skillMethod = "IceWave";
		settings.distance = 6f;
		settings.botUseDistance = 6f;
		settings.minimumDistanceToUse = 2.5f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 3f;
		settings.slowSpeed = 0.8f;
		settings.chance = 2f;
		return settings;
	}

	private static SkillSettings Settings_32(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.iceStun;
		settings.skillMethod = "IceStun";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.globalCooldown = 0.5f;
		settings.resourceRemove = 1;
		settings.damagePercent = 1f;
		settings.stunTime = 10f;
		settings.chance = 0.4f;
		return settings;
	}

	private static SkillSettings Settings_33(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.blizzard;
		settings.skillMethod = "Blizzard";
		settings.distance = 0.8f;
		settings.botUseDistance = 0.8f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 0.1f;
		settings.existingTime = 4f;
		settings.slowTime = 0.5f;
		settings.stunTime = 2f;
		settings.chance = 5f;
		settings.stunChance = 0.5f;
		return settings;
	}

	private static SkillSettings Settings_34(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.chainLavaBurst;
		settings.skillMethod = "ChainLavaBurst";
		settings.distance = 8f;
		settings.botUseDistance = 6f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 7f;
		return settings;
	}

	private static SkillSettings Settings_35(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fireNova;
		settings.skillMethod = "FireNova";
		settings.distance = 0.8f;
		settings.botUseDistance = 0.8f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.chance = 8f;
		return settings;
	}

	private static SkillSettings Settings_36(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.physicalShield;
		settings.skillMethod = "PhysicalShield";
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 10f;
		settings.restorePercent = 2f;
		settings.cooldown = 10f;
		settings.chance = 2f;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_37(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.elementalShield;
		settings.skillMethod = "ElementalShield";
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 10f;
		settings.restorePercent = 2f;
		settings.cooldown = 10f;
		settings.chance = 2f;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_38(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.hybridShield;
		settings.skillMethod = "HybridShield";
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 10f;
		settings.restorePercent = 2f;
		settings.cooldown = 10f;
		settings.chance = 2f;
		settings.canUseWithoutResource = true;
		return settings;
	}

	private static SkillSettings Settings_39(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.earthDisruption;
		settings.skillMethod = "EarthDisruption";
		settings.distance = 8f;
		settings.botUseDistance = 3f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.stunTime = 2f;
		settings.chance = 2f;
		return settings;
	}

	private static SkillSettings Settings_40(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.iceWall;
		settings.skillMethod = "IceWall";
		settings.distance = 8f;
		settings.botUseDistance = 3f;
		settings.resourceRemove = 1;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 8f;
		settings.slowSpeed = 0.2f;
		settings.chance = 1f;
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
	}


}
