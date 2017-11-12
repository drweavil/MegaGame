using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class Buffs : MonoBehaviour {
	public const int physicalDamageBuff = 1, 
	elementalDamgeBuff = 2, 
	healthBuff = 3, 
	criticalBuff = 4, 
	physicalArmorBuff = 5, 
	elementalArmorBuff = 6;
	public static List<int> meleeBuffs = new List<int>(new int[]{9}); 
	public static List<int> fireBuffs = new List<int>(new int[]{7}); 
	public static List<int> elementalBuffs = new List<int>(new int[]{8}); 

	public delegate void BuffAction();

	public static Buff GetBuff (int id){
		Buff buff = new Buff ();
		MethodInfo method = typeof(Buffs).GetMethod ("Buff_" + id.ToString (), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		buff = (Buff)method.Invoke (null, null);
		return buff ;
	}

	public static Dictionary<string, BuffAction> GetBuffActions (int id){
		MethodInfo method = typeof(Buffs).GetMethod ("BuffAction_" + id.ToString (), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

		return (Dictionary<string, BuffAction>)method.Invoke (null, null);
	}

	static Buff Buff_1(){
		Buff buff = new Buff ();
		buff.buffID = 1;
		buff.buffTime = 55f;
		buff.originBuffTime = buff.buffTime;
		buff.buffPercent = 27f;
		return buff;
	}
	static Dictionary<string, BuffAction> BuffAction_1(){
		Buff buff = GetBuff (1);
		Dictionary<string, BuffAction> actions = new Dictionary<string, BuffAction> ();
		Stats playerStats = PlayerController.playerCharacterAPI.stats;
		buff.currentBuffNumber = playerStats.physicalDamagePoints * (buff.buffPercent / 100f);

		actions.Add("startAction", () => {
			playerStats.ChangePhysicalDamagePoints (buff.currentBuffNumber);
			BuffsController.buffsController.buffs.Add(buff.buffID, buff);
		});
		actions.Add("stopAction", () => {
			playerStats.ChangePhysicalDamagePoints (-buff.currentBuffNumber);
			BuffsController.buffsController.buffs.Remove(buff.buffID);
		});

		return actions;
	}

	static Buff Buff_2(){
		Buff buff = new Buff ();
		buff.buffID = 2;
		buff.buffTime = 62f;
		buff.originBuffTime = buff.buffTime;
		buff.buffPercent = 22f;
		return buff;
	}
	static Dictionary<string, BuffAction> BuffAction_2(){
		Buff buff = GetBuff (2);
		Dictionary<string, BuffAction> actions = new Dictionary<string, BuffAction> ();

		Stats playerStats = PlayerController.playerCharacterAPI.stats;
		buff.currentBuffNumber = playerStats.elementalDamagePoints * (buff.buffPercent / 100f);

		actions.Add("startAction", () => {
			playerStats.ChangeElementalDamagePoints (buff.currentBuffNumber);
			BuffsController.buffsController.buffs.Add(buff.buffID, buff);
		});
		actions.Add("stopAction", () => {
			playerStats.ChangeElementalDamagePoints (-buff.currentBuffNumber);
			BuffsController.buffsController.buffs.Remove(buff.buffID);
		});
		return actions;
	}

	static Buff Buff_3(){
		Buff buff = new Buff ();
		buff.buffID = 3;
		buff.buffTime = 50f;
		buff.originBuffTime = buff.buffTime;
		buff.buffPercent = 28f;
		return buff;
	}
	static Dictionary<string, BuffAction> BuffAction_3(){
		Buff buff = GetBuff (3);
		Dictionary<string, BuffAction> actions = new Dictionary<string, BuffAction> ();

		Stats playerStats = PlayerController.playerCharacterAPI.stats;
		buff.currentBuffNumber = playerStats.stamina * (buff.buffPercent / 100f);

		actions.Add("startAction", () => {
			playerStats.ChangeHealthPoints (buff.currentBuffNumber);
			BuffsController.buffsController.buffs.Add(buff.buffID, buff);
		});
		actions.Add("stopAction", () => {
			playerStats.ChangeHealthPoints (-buff.currentBuffNumber);
			BuffsController.buffsController.buffs.Remove(buff.buffID);
		});
		return actions;
	}

	static Buff Buff_4(){
		Buff buff = new Buff ();
		buff.buffID = 4;
		buff.buffTime = 75f;
		buff.originBuffTime = buff.buffTime;
		buff.buffPercent = 30f;
		return buff;
	}
	static Dictionary<string, BuffAction> BuffAction_4(){
		Buff buff = GetBuff (4);
		Dictionary<string, BuffAction> actions = new Dictionary<string, BuffAction> ();

		Stats playerStats = PlayerController.playerCharacterAPI.stats;
		buff.currentBuffNumber = playerStats.criticalPoints * (buff.buffPercent / 100f);

		actions.Add("startAction", () => {
			playerStats.ChangeCriticalPoints (buff.currentBuffNumber);
			BuffsController.buffsController.buffs.Add(buff.buffID, buff);
		});
		actions.Add("stopAction", () => {
			playerStats.ChangeCriticalPoints (-buff.currentBuffNumber);
			BuffsController.buffsController.buffs.Remove(buff.buffID);
		});
		return actions;
	}

	static Buff Buff_5(){
		Buff buff = new Buff ();
		buff.buffID = 5;
		buff.buffTime = 48f;
		buff.originBuffTime = buff.buffTime;
		buff.buffPercent = 35f;
		return buff;
	}
	static Dictionary<string, BuffAction> BuffAction_5(){
		Buff buff = GetBuff (5);
		Dictionary<string, BuffAction> actions = new Dictionary<string, BuffAction> ();

		Stats playerStats = PlayerController.playerCharacterAPI.stats;
		buff.currentBuffNumber = playerStats.armorPoints * (buff.buffPercent / 100f);

		actions.Add("startAction", () => {
			playerStats.ChangeArmorPoints (buff.currentBuffNumber);
			BuffsController.buffsController.buffs.Add(buff.buffID, buff);
		});
		actions.Add("stopAction", () => {
			playerStats.ChangeArmorPoints (-buff.currentBuffNumber);
			BuffsController.buffsController.buffs.Remove(buff.buffID);
		});
		return actions;
	}

	static Buff Buff_6(){
		Buff buff = new Buff ();
		buff.buffID = 6;
		buff.buffTime = 46f;
		buff.originBuffTime = buff.buffTime;
		buff.buffPercent = 24f;
		return buff;
	}
	static Dictionary<string, BuffAction> BuffAction_6(){
		Buff buff = GetBuff (6);
		Dictionary<string, BuffAction> actions = new Dictionary<string, BuffAction> ();

		Stats playerStats = PlayerController.playerCharacterAPI.stats;
		buff.currentBuffNumber = playerStats.elementalArmor * (buff.buffPercent / 100f);

		actions.Add("startAction", () => {
			playerStats.ChangeElementalArmorPoints (buff.currentBuffNumber);
			BuffsController.buffsController.buffs.Add(buff.buffID, buff);
		});
		actions.Add("stopAction", () => {
			playerStats.ChangeElementalArmorPoints (-buff.currentBuffNumber);
			BuffsController.buffsController.buffs.Remove(buff.buffID);
		});
		return actions;
	}

	static Buff Buff_7(){
		Buff buff = new Buff ();
		buff.buffID = 7;
		buff.isInfinityBuff = true;
		buff.canDeactivate = false;
		return buff;
	}

	static Buff Buff_8(){
		Buff buff = new Buff ();
		buff.buffID = 8;
		buff.isInfinityBuff = true;
		buff.canDeactivate = false;
		return buff;
	}

	static Buff Buff_9(){
		Buff buff = new Buff ();
		buff.buffID = 9;
		buff.isInfinityBuff = true;
		buff.canDeactivate = false;
		return buff;
	}

}
