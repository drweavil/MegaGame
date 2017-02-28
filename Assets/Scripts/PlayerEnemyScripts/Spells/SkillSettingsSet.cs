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
		settings.distance = 0.8f;
		settings.resourceAdd = 9;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_1(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.middlePunch;
		settings.distance = 0.8f;
		settings.resourceAdd = 15;
		settings.globalCooldown = 1f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_2(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.slowPunch;
		settings.distance = 0.8f;
		settings.resourceAdd = 20;
		settings.globalCooldown = 1.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_4(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.circlePunch;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_5(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.meleeWave;
		settings.distance = 6f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_6(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.blackHole;
		settings.distance = 6f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 2f;
		return settings;
	}

	private static SkillSettings Settings_7(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.chains;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 5f;
		settings.slowSpeed = 0.7f;
		return settings;
	}

	private static SkillSettings Settings_8(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.meleeStun;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.stunTime = 10f;
		return settings;
	}

	private static SkillSettings Settings_9(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.meleeWaveSlow;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 4f;
		settings.slowSpeed = 0.4f;
		return settings;
	}

	private static SkillSettings Settings_10(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.charge;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.stunTime = 2f;
		settings.slowTime = 4f;
		settings.slowSpeed = 0.8f;
		return settings;
	}

	private static SkillSettings Settings_11(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.thunderClap;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 4f;
		settings.slowSpeed = 0.2f;
		settings.stunTime = 2f;
		return settings;
	}

	private static SkillSettings Settings_12(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.siphoneLife;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_13(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fastShot;
		settings.distance = 8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_14(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.middleShot;
		settings.distance = 8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_15(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.slowShot;
		settings.distance = 8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_16(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.grenadeLaunch;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_17(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.grenadeWave;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_18(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.rocketCircle;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_19(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.explosiveMine;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.stunTime = 5f;
		return settings;
	}

	private static SkillSettings Settings_20(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.lightingBombLaunch;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 15f;
		settings.slowTime = 4f;
		settings.slowSpeed = 0.5f;
		return settings;
	}

	private static SkillSettings Settings_21(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.snipeShot;
		settings.distance = 8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.stunTime = 10f;
		return settings;
	}

	private static SkillSettings Settings_22(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.toxicGrenadeWave;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 10f;
		settings.slowSpeed = 0.2f;
		return settings;
	}

	private static SkillSettings Settings_23(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.rocketJump;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_24(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fireClap;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 4f;
		settings.slowSpeed = 0.2f;
		settings.stunTime = 2f;
		return settings;
	}

	private static SkillSettings Settings_25(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.toxicGrenadeLaunch;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.stunTime = 4f;
		return settings;
	}

	private static SkillSettings Settings_26(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fastCast;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_27(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.middleCast;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_28(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.slowCast;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_29(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.elementalCone;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_30(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fireWall;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_31(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.iceWave;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 3f;
		settings.slowSpeed = 0.8f;
		return settings;
	}

	private static SkillSettings Settings_32(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.iceStun;
		settings.distance = 6f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.stunTime = 15f;
		return settings;
	}

	private static SkillSettings Settings_33(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.blizzard;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 4f;
		settings.slowTime = 0.5f;
		settings.stunTime = 5f;
		return settings;
	}

	private static SkillSettings Settings_34(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.chainLavaBurst;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_35(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.fireNova;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_36(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.physicalShield;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 10f;
		settings.restorePercent = 2f;
		return settings;
	}

	private static SkillSettings Settings_37(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.elementalShield;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 4f;
		settings.restorePercent = 2f;
		return settings;
	}

	private static SkillSettings Settings_38(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.hybridShield;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.existingTime = 4f;
		settings.restorePercent = 1f;
		return settings;
	}

	private static SkillSettings Settings_39(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.earthDisruption;
		settings.distance = 0.8f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.stunTime = 2f;
		return settings;
	}

	private static SkillSettings Settings_40(){
		SkillSettings settings = new SkillSettings();
		settings.skillID = SkillID.iceWall;
		settings.distance = 6f;
		settings.globalCooldown = 0.5f;
		settings.damagePercent = 1f;
		settings.slowTime = 8f;
		settings.slowSpeed = 0.2f;
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
		public const int siphoneLife = 12;

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
