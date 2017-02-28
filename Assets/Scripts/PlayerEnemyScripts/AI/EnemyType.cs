using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class EnemyType {
	public float staminaPercent;
	public float physicalDamagePointsPercent;
	public float elementalDamagePointsPercent;
	public float criticalPointsPercent;
	public float physicalArmorPointsPercent;
	public float elementalArmorPointsPercent;
	public int id;


	public static EnemyType GetType(int id){
		MethodInfo method = typeof(EnemyType).GetMethod ("Type_" + id.ToString (), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		EnemyType type = (EnemyType)method.Invoke (null, null);
		return (EnemyType)(type);
	}

	private static EnemyType Type_1(){
		EnemyType type = new EnemyType ();
		type.id = 1;
		type.staminaPercent = 8f;
		type.physicalDamagePointsPercent = 8f;
		type.elementalDamagePointsPercent = 8f;
		type.physicalArmorPointsPercent = 34f;
		type.criticalPointsPercent = 34f;
		type.elementalArmorPointsPercent = 8f;
		return type;
	}

	private static EnemyType Type_2(){
		EnemyType type = new EnemyType ();
		type.id = 2;
		type.staminaPercent = 34f;
		type.physicalDamagePointsPercent = 8f;
		type.elementalDamagePointsPercent = 8f;
		type.physicalArmorPointsPercent = 34f;
		type.criticalPointsPercent = 8f;
		type.elementalArmorPointsPercent = 8f;
		return type;
	}

	private static EnemyType Type_3(){
		EnemyType type = new EnemyType ();
		type.id = 3;
		type.staminaPercent = 34f;
		type.physicalDamagePointsPercent = 8f;
		type.elementalDamagePointsPercent = 8f;
		type.physicalArmorPointsPercent = 8f;
		type.criticalPointsPercent = 8f;
		type.elementalArmorPointsPercent = 34f;
		return type;
	}

	private static EnemyType Type_4(){
		EnemyType type = new EnemyType ();
		type.id = 4;
		type.staminaPercent = 8f;
		type.physicalDamagePointsPercent = 34f;
		type.elementalDamagePointsPercent = 8f;
		type.physicalArmorPointsPercent = 8f;
		type.criticalPointsPercent = 34f;
		type.elementalArmorPointsPercent = 8f;
		return type;
	}

	private static EnemyType Type_5(){
		EnemyType type = new EnemyType ();
		type.id = 5;
		type.staminaPercent = 8f;
		type.physicalDamagePointsPercent = 8f;
		type.elementalDamagePointsPercent = 34f;
		type.physicalArmorPointsPercent = 8f;
		type.criticalPointsPercent = 34f;
		type.elementalArmorPointsPercent = 8f;
		return type;
	}

	private static EnemyType Type_6(){
		EnemyType type = new EnemyType ();
		type.id = 6;
		type.staminaPercent = 8f;
		type.physicalDamagePointsPercent = 8f;
		type.elementalDamagePointsPercent = 60f;
		type.physicalArmorPointsPercent = 8f;
		type.criticalPointsPercent = 8f;
		type.elementalArmorPointsPercent = 8f;
		return type;
	}

	private static EnemyType Type_7(){
		EnemyType type = new EnemyType ();
		type.id = 7;
		type.staminaPercent = 16f;
		type.physicalDamagePointsPercent = 52f;
		type.elementalDamagePointsPercent = 8f;
		type.physicalArmorPointsPercent = 8f;
		type.criticalPointsPercent = 8f;
		type.elementalArmorPointsPercent = 8f;
		return type;
	}

	private static EnemyType Type_8(){
		EnemyType type = new EnemyType ();
		type.id = 8;
		type.staminaPercent = 90f;
		type.physicalDamagePointsPercent = 6f;
		type.elementalDamagePointsPercent = 1f;
		type.physicalArmorPointsPercent = 1f;
		type.criticalPointsPercent = 1f;
		type.elementalArmorPointsPercent = 1f;
		return type;
	}

	private static EnemyType Type_9(){
		EnemyType type = new EnemyType ();
		type.id = 9;
		type.staminaPercent = 1f;
		type.physicalDamagePointsPercent = 1f;
		type.elementalDamagePointsPercent = 95f;
		type.physicalArmorPointsPercent = 1f;
		type.criticalPointsPercent = 1f;
		type.elementalArmorPointsPercent = 1f;
		return type;
	}




	public class ID {
		public const int dodger = 1;
		public const int meleeTank = 2;
		public const int fireTank = 3;
		public const int fireDPS = 4;
		public const int elementalDPS = 5;
		public const int shieldTank = 6;
		public const int jackie = 7;
		public const int fireFat = 8;
		public const int elementalMegaDPS = 9;
	}
}
