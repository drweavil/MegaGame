using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Stats : MonoBehaviour {
	public const int physicalDamageType = 0, hybridDamageType = 1, elementalDamageType = 2; 
	public const int meleeSpec = 0, fireSpec=1, elementalSpec = 2;
	public int specId = 0;

	public float health;
	public float elementalShield;
	public float physicalShield;
	public float hybridShield;

	public float maximumHealth;
	public float stamina = 0;
	public const float staminaCoeff = 10;
	private static float healthPercent = 0.2f;

	public float physicalDamage;
	public float physicalDamagePoints = 0;
	public const float maximumPhysicalDamage = 100;
	private static float physicalDamagePercent = 0.15f;

	public float elementalDamage;
	public float elementalDamagePoints = 0;
	public const float maximumElementalDamage = 100;
	private static float elementalDamagePercent = 0.15f;

	public float armor;
	public float armorPoints = 0;
	public const float maximumArmor = 60;
	private static float armorPercent = 0.15f;

	public float elementalArmor;
	public float elementalArmorPoints = 0;
	public const float maximumElementalArmor = 60;
	private static float elementalArmorPercent = 0.15f;

	public float critical;
	public float criticalPoints = 0;
	public const float maximumCritical = 50;
	private static float criticalPercent = 0.2f;
	public float weaponDamage = 0;
	public float fireWeaponDamage = 0;
	public float elementalWeaponDamage = 0;





	public int meleeEnergy;
	public int maximumMeleeEnergy = 100;
	public int fireEnergy;
	public int maximumFireEnergy = 0;
	public int magicEnergy;
	public int maximumMagicEnergy = 100;

	public bool isDeath = false;
	public bool deathPlayed = false;
	public bool isGrounded = true;
	public bool withoutControl = false;
	public bool inSilence = false;
	public bool inIceStun = false;
	public int inIceStunProcess = 0;
	public bool inJump = false;
	bool itIsBurn = false;
	//float burnPercent = 0;
	float minimumSpeed;
	public float currentSpeed = 1f;
	public float maximumSpeed = 1f;
	public List<float> minimumSpeedValues = new List<float> ();

	public bool onGlobalCooldown = false;
	private List<int> skillsOnCD = new List<int> ();



	/**********timers*******************/
	Timer canRemoveMeleResourceTimer = new Timer();
	Timer canRestoreFireResourceTimer = new Timer();
	Timer canRemoveMagicResourceTimer = new Timer();

	Timer meleeResourceTimer = new Timer();
	Timer fireResourceTimer = new Timer();
	Timer magicResourceTimer = new Timer();

	Timer canRestoreHealthTimer = new Timer();
	Timer restoreHealthTimer = new Timer ();

	Timer stunTimer = new Timer();
	Timer silenceTimer = new Timer();
	Timer slowMovementTimer = new Timer();

	public Timer shieldTimer = new Timer();
	/********************************/


	private float healthRestoreTime = 2f;
	private float healthRestorePoints = 10f;
	private float healthCanRestoreTime = 5f;
	private float meleeRemoveTime = 1f;
	private int meleeRemovePoint = 2;
	private float meleeCanRemoveTime = 5f;
	private float fireRestoreTime = 1f;
	private int fireRestorePoint = 2;
	private float fireCanRestoreTime = 5f;
	private float magicRemoveTime = 1f;
	private int magicRemovePoint = 2;
	private float magicCanRemoveTime = 5f;



	public bool isPlayerStats = false;
	public PlayerHealthBar playerHealthBar;
	public CharacterAPI characterAPI;




	
	//private Object playerScript;


	void Awake(){
		//playerScript
		/*SetMaximumHealth ();
		RestoreMaximumHealth ();*/
		shieldTimer = new Timer ();
		if (isPlayerStats) {
			SetStatsByComplexity (10);
		}
		deathPlayed = false;
		//SetMaximumFireResource ();
	}



	void Update(){
		/*if (Input.GetKeyDown (KeyCode.U)) {
			SetSpec (1);
		}*/
		if (canRestoreHealthTimer.TimeIsOver()) {
			if(restoreHealthTimer.TimeIsOver() && health != GetMaximumHealth()){
				Stats.NumberParams restore = new Stats.NumberParams ();
				restore.number = GetMaximumHealth() * 0.02f;
				RestoreHealth (restore);
				restoreHealthTimer.SetTimer (healthRestoreTime);
			}
		}

		if (isPlayerStats || specId == PlayerController.melee) {
			if (canRemoveMeleResourceTimer.TimeIsOver ()) {
				if (meleeResourceTimer.TimeIsOver ()) {
					RemoveMeleeEnergyPoints (meleeRemovePoint);
					meleeResourceTimer.SetTimer (meleeRemoveTime);
				}
			}
		}

		if (isPlayerStats || specId == PlayerController.fire) {
			if (canRestoreFireResourceTimer.TimeIsOver ()) {
				if (fireResourceTimer.TimeIsOver ()) {
					RemoveFireEnergyPoints (fireRestorePoint);
					fireResourceTimer.SetTimer (fireRestoreTime);
				}
			}
		}

		if (isPlayerStats || specId == PlayerController.magic) {
			if (canRemoveMagicResourceTimer.TimeIsOver ()) {
				if (magicResourceTimer.TimeIsOver ()) {
					RemoveMagicEnergyPoints (magicRemovePoint);
					magicResourceTimer.SetTimer (magicRemoveTime);
				}
			}
		}


		if (isDeath && !deathPlayed) {
			deathPlayed = true;
			Death ();
		}
	}

	public int GetResourceBySpec(){
		int value = 0;
		if (specId == Stats.meleeSpec) {
			value = meleeEnergy;
		} else if (specId == Stats.fireSpec) {
			value = fireEnergy;
		} else if (specId == Stats.elementalSpec) {
			value = magicEnergy;
		}
		return value;
	}

	public void SetStatsToNull(){
		health = 0;
		stamina = 0;
		maximumHealth = 0;
		physicalDamage = 0;
		physicalDamagePoints = 0;
		elementalDamage = 0;
		elementalDamagePoints = 0;
		critical = 0;
		criticalPoints = 0;
		elementalArmor = 0;
		elementalArmorPoints = 0;
		armor = 0;
		armorPoints = 0;
	}


	public void SetMaximumHealth(){
		maximumHealth = GetMaximumHealth();
		healthRestorePoints = GetMaximumHealth () * 0.02f;
	}

	public void RestoreMaximumHealth(){
		health = GetMaximumHealth();
		isDeath = false;
	}

	public float GetMaximumHealth(){
		return stamina * staminaCoeff;
	}

	public void  SetMaximumFireResource(){
		fireEnergy = maximumFireEnergy;
	}



	public void MakeDamage(NumberParams damage, int damageType = -1, bool withResTimer = true, SkillSettings skillSettings = default(SkillSettings)){
		if (damageType == physicalDamageType) {
			damage.number = damage.number - ((damage.number*armor)/100f);
			if (physicalShield > 0) {
				damage.number = RemoveShieldPoints (damage.number, Stats.physicalDamageType);
			}
		}
		if (damageType == elementalDamageType) {
			damage.number = damage.number - ((damage.number*elementalArmor)/100f);
			if (elementalShield > 0) {
				damage.number = RemoveShieldPoints (damage.number, Stats.elementalDamageType);
			}
		}

		if (hybridShield > 0) {
			float residualDamage = RemoveShieldPoints (damage.number, Stats.hybridDamageType);
			damage.number = (damage.number / 2f) + residualDamage;
		}

		if (damage.number < 0) {
			damage.number = 0;
		}

		health -= damage.number;


		if (BattleInterfaceController.battleInterfaceController.battleInterface.activeInHierarchy) {
			if (playerHealthBar.gameObject.activeInHierarchy) {
				playerHealthBar.SetHealth (health);
				if (damage.number != 0) {
					int hpBarDamage = (int)damage.number;
					if (hpBarDamage == 0) {
						hpBarDamage = 1;
					}
					PlayerHealthBar.NumberParams numberParams = new PlayerHealthBar.NumberParams ();
					numberParams.damageType = damageType;
					numberParams.number = hpBarDamage;
					numberParams.isCrit = damage.isCrit;
					playerHealthBar.AddDamage (numberParams);
				}
			}
		}

		if (health <= 0) {
			isDeath = true;
		}

		if (withResTimer) {
			canRestoreHealthTimer.SetTimer (healthCanRestoreTime);
		}
	}

	public void AddShieldPoints(NumberParams shieldPoints, int shieldType){
		if (Stats.physicalDamageType == shieldType) {
			physicalShield += shieldPoints.number;
			elementalShield = 0;
			hybridShield = 0;

			if(playerHealthBar.gameObject.activeInHierarchy){
				playerHealthBar.SetShield (physicalShield);
			}
		}

		if (Stats.elementalDamageType == shieldType) {
			physicalShield = 0;
			elementalShield += shieldPoints.number;
			hybridShield = 0;

			//if(isPlayerStats){
				playerHealthBar.SetShield (elementalShield);
			//}
		}

		if (Stats.hybridDamageType == shieldType) {
			physicalShield = 0;
			elementalShield = 0;
			hybridShield += shieldPoints.number;

			//if(isPlayerStats){
			if (BattleInterfaceController.battleInterfaceController.battleInterface) {
				if (playerHealthBar.gameObject.activeInHierarchy) {
					playerHealthBar.SetShield (hybridShield);
				}
			}
			//}
		}
	}


	public float RemoveShieldPoints(float shieldPoints, int shieldType){
		float residualPoints = 0;
		if (Stats.physicalDamageType == shieldType) {
			physicalShield -= shieldPoints;
			if (physicalShield < 0) {
				shieldTimer.SetTimer (0);
				residualPoints = physicalShield * -1f;
				physicalShield += physicalShield * -1f;
			}

			if(playerHealthBar.gameObject.activeInHierarchy){
				playerHealthBar.SetShield (physicalShield);
			}
		}

		if (Stats.elementalDamageType == shieldType) {
			elementalShield -= shieldPoints;
			if (elementalShield < 0) {
				shieldTimer.SetTimer (0);
				residualPoints = elementalShield * -1f;
				elementalShield += elementalShield * -1f;
			}

			if(playerHealthBar.gameObject.activeInHierarchy){
				playerHealthBar.SetShield (elementalShield);
			}
		}

		if (Stats.hybridDamageType == shieldType) {
			hybridShield -= shieldPoints;
			if (hybridShield < 0) {
				shieldTimer.SetTimer (0);
				residualPoints = hybridShield * -1f;
				hybridShield += hybridShield * -1f;
			}

			if (BattleInterfaceController.battleInterfaceController.battleInterface.activeInHierarchy) {
				if (playerHealthBar.gameObject.activeInHierarchy) {
					playerHealthBar.SetShield (hybridShield);
				}
			}
		}
		return residualPoints;
	}

	public void RestoreHealth(NumberParams restore){
		health += restore.number;
		if (health > GetMaximumHealth ()) {
			health -= health - GetMaximumHealth ();
		}
		if (BattleInterfaceController.battleInterfaceController.battleInterface.activeInHierarchy) {
			if (playerHealthBar.gameObject.activeInHierarchy) {
				if (restore.number != 0) {
					PlayerHealthBar.NumberParams numberParams = new PlayerHealthBar.NumberParams ();
					numberParams.number = (int)restore.number;
					numberParams.isDamage = false;
					numberParams.isCrit = restore.isCrit;
					if (restore.number < 1) {
						numberParams.number = 1;
					}
					playerHealthBar.AddDamage (numberParams);
				}
				playerHealthBar.SetHealth (health);
			}
		}
	}



	public int GetMaximumResource(){
		int resource = 0;
		if(PlayerController.currentSpec == PlayerController.melee){
			resource = maximumMeleeEnergy;
		}
		if(PlayerController.currentSpec == PlayerController.fire){
			resource = maximumFireEnergy;
		}
		if(PlayerController.currentSpec == PlayerController.magic){
			resource = maximumMagicEnergy;
		}
		return resource;
	}


	public void AddMeleeEnergyPoints(int points, bool withResTimer = false){
		meleeEnergy += points;
		if (meleeEnergy > maximumMeleeEnergy) {
			meleeEnergy -= meleeEnergy - maximumMeleeEnergy;
		}
		//if (isPlayerStats) {
			SetRecourceToBar ();
		//}
		if (withResTimer) {
			canRemoveMeleResourceTimer.SetTimer (meleeCanRemoveTime);	
		}
	}

	public void AddFireEnergyPoints(int points, bool withResTimer = false){
		fireEnergy += points;
		if (fireEnergy > 100) {
			if (!itIsBurn) {
				itIsBurn = true;
				StartCoroutine (Burn());
			}
			//fireEnergy -= fireEnergy - maximumFireEnergy;
		}
		//if (isPlayerStats) {
			SetRecourceToBar ();
		//}
		if (withResTimer) {
			canRestoreFireResourceTimer.SetTimer (fireCanRestoreTime);	
		}
	}

	public void AddMagicEnergyPoints(int points, bool withResTimer = false){
		magicEnergy += points;
		if (magicEnergy > maximumMagicEnergy) {
			magicEnergy -= magicEnergy - maximumMagicEnergy;
		}
		//if (isPlayerStats) {
			SetRecourceToBar ();
		//}
		if (withResTimer) {
			canRemoveMagicResourceTimer.SetTimer (magicCanRemoveTime);	
		}
	}

	public void RemoveMeleeEnergyPoints(int points, bool withResTimer = false){
		if (meleeEnergy != 0) {
			meleeEnergy -= points;
			if (meleeEnergy < 0) {
				meleeEnergy += meleeEnergy * -1;
			}
			//if (isPlayerStats) {
				SetRecourceToBar ();
			//}
		}
		if (withResTimer) {
			canRemoveMeleResourceTimer.SetTimer (meleeCanRemoveTime);	
		}
	}

	public void RemoveFireEnergyPoints(int points, bool withResTimer = false){
		if (fireEnergy != 0) {
			fireEnergy -= points;
			if (fireEnergy <= 100) {
				itIsBurn = false;
			}
			if (fireEnergy < 0) {
				fireEnergy += fireEnergy * -1;
			}
			//if (isPlayerStats) {
				SetRecourceToBar ();
			//}
		}
		if (withResTimer) {
			canRestoreFireResourceTimer.SetTimer (fireCanRestoreTime);	
		}
	}

	public void RemoveMagicEnergyPoints(int points, bool withResTimer = false){
		if (magicEnergy != 0) {
			magicEnergy -= points;
			if (magicEnergy < 0) {
				magicEnergy += magicEnergy * -1;
			}
			//if (isPlayerStats) {
				SetRecourceToBar ();
			//}
		}
		if (withResTimer) {
			canRemoveMagicResourceTimer.SetTimer (magicCanRemoveTime);	
		}
	}


	float GetBurnPercent(){
		return (float)(fireEnergy - 100) * 0.0005f;
	}


	public void SetRecourceToBar(){
		if (BattleInterfaceController.battleInterfaceController.battleInterface.activeInHierarchy) {
			if (playerHealthBar.gameObject.activeInHierarchy) {
				if (specId == PlayerController.melee) {
					playerHealthBar.SetResource (meleeEnergy);
				}
				if (specId == PlayerController.fire) {
					playerHealthBar.SetResource (fireEnergy);
				}
				if (specId == PlayerController.magic) {
					playerHealthBar.SetResource (magicEnergy);
				}
			}
		}
	}





	public static float GetPlusHealthByPoints(float points){
		return points * staminaCoeff;
	}

	public static float GetPlusPhysicalArmorByPoints(float points){
		return (float)Math.Round((float)(((float)points / (float)GetMaximumArmorPoints ()) * (float)maximumArmor), 2);
	}

	public static float GetPlusElementalArmorByPoints(float points){
		return (float)Math.Round((float)(((float)points / (float)GetMaximumElementalArmorPoints ()) * (float)maximumElementalArmor), 2);
	}

	public static float GetPlusCriticalByPoints(float points){
		return (float)Math.Round((float)(((float)points / (float)GetMaximumCriticalPoints ()) * (float)maximumCritical), 2);
	}

	public static float GetPlusPhysicalDamageByPoints(float points){
		return (float)Math.Round((float)(((float)points / (float)GetMaximumPhysicalDamagePoints ()) * (float)maximumPhysicalDamage), 2);
	}

	public static float GetPlusElementalDamageByPoints(float points){
		return (float)Math.Round((float)(((float)points / (float)GetMaximumElementalDamagePoints ()) * (float)maximumElementalDamage), 2);
	}




	static float GetMaximumPhysicalDamagePoints(){
		return (PlayerController.maximumComplexity * physicalDamagePercent);
	}

	static float GetMaximumElementalDamagePoints(){
		return (PlayerController.maximumComplexity * elementalDamagePercent);
	}

	static float GetMaximumCriticalPoints(){
		return (PlayerController.maximumComplexity * criticalPercent);
	}

	static float GetMaximumArmorPoints(){
		return (PlayerController.maximumComplexity * armorPercent);
	}

	static float GetMaximumElementalArmorPoints(){
		return (PlayerController.maximumComplexity * elementalArmorPercent);
	}


	public void ChangeHealthPoints(float points){
		stamina = stamina + points;
		if (stamina < 0) {
			stamina = 0;
		}
		SetMaximumHealth ();
	}


	public void SetPhysicalDamageByPoints (float points){
		physicalDamage = GetPlusPhysicalDamageByPoints(points);
	}
	public void ChangePhysicalDamagePoints(float points){
		physicalDamagePoints = physicalDamagePoints + points;
		if (physicalDamagePoints > 0) {
			SetPhysicalDamageByPoints (physicalDamagePoints);
		} else {
			physicalDamage = 0;
			physicalDamagePoints = 0;
		}
	}


	public void SetElementalDamageByPoints (float points){
		elementalDamage = GetPlusElementalDamageByPoints(points);
	}
	public void ChangeElementalDamagePoints(float points){
		elementalDamagePoints = elementalDamagePoints + points;
		if (elementalDamagePoints > 0) {
			SetElementalDamageByPoints (elementalDamagePoints);
		} else {
			elementalDamage = 0;
			elementalDamagePoints = 0;
		}
	}


	public void SetArmorByPoints(float points){
		armor = GetPlusPhysicalArmorByPoints (points);
		if (armor > maximumArmor) {
			armor = maximumArmor;
		}
	}
	public void ChangeArmorPoints(float points){
		armorPoints = armorPoints + points;
		if (armorPoints > 0) {
			SetArmorByPoints (armorPoints);
		} else {
			armor = 0;
			armorPoints = 0;
		}
	}



	public void SetElementalArmorByPoints(float points){
		elementalArmor = GetPlusElementalArmorByPoints (points);
		if (elementalArmor > maximumElementalArmor) {
			elementalArmor = maximumElementalArmor;
		}
	}
	public void ChangeElementalArmorPoints(float points){
		elementalArmorPoints = elementalArmorPoints + points;
		if (elementalArmorPoints > 0) {
			SetElementalArmorByPoints (elementalArmorPoints);
		} else {
			elementalArmor = 0;
			elementalArmorPoints = 0;
		}
	}


	public void SetCriticalByPoints(float points){
		critical = GetPlusCriticalByPoints (points);
		if (critical > maximumCritical) {
			critical = maximumCritical;
		}	
	}
	public void ChangeCriticalPoints(float points){
		criticalPoints = criticalPoints + points;
		if (criticalPoints > 0) {
			SetCriticalByPoints (criticalPoints);
		} else {
			critical = 0;
			criticalPoints = 0;
		}
	}

	public void SetSpec(int newSpecId){
		specId = newSpecId;
		characterAPI.skills.anim.SetInteger ("SpecID", newSpecId); 
		StartCoroutine(StartProcess.StartActionAfterFewFrames(2, () => {
			characterAPI.reskinController.ChangeSpec (newSpecId);
		}));

		//Debug.Log (newSpecId);
		StartCoroutine(StartProcess.StartActionAfterFewFrames(7, ()=>{
			if(BattleInterfaceController.battleInterfaceController.battleInterface.activeInHierarchy){
				if(this.gameObject.activeInHierarchy){
					characterAPI.healthBar.ChangeToSpec (newSpecId);
				}
			}
		}));
	}

	public void SetStatsByComplexity (float complexity, int enemyID = -1){
		SetStatsToNull ();

		if (enemyID == -1) {
			ChangeHealthPoints ((float)Math.Round ((float)complexity * healthPercent));
			ChangePhysicalDamagePoints ((float)Math.Round ((float)complexity * physicalDamagePercent));
			ChangeElementalDamagePoints ((float)Math.Round ((float)complexity * elementalDamagePercent));
			ChangeArmorPoints ((float)Math.Round ((float)complexity * armorPercent));
			ChangeElementalArmorPoints ((float)Math.Round ((float)complexity * elementalArmorPercent));
			ChangeCriticalPoints ((float)Math.Round ((float)complexity * criticalPercent));
			RestoreMaximumHealth ();
			SetSpec (Stats.meleeSpec);
		} else {
			EnemyType enemyType = EnemyType.GetType (enemyID);
			ChangeHealthPoints ((float)Math.Round ((float)complexity * (enemyType.staminaPercent/100)));
			ChangePhysicalDamagePoints ((float)Math.Round ((float)complexity * (enemyType.physicalDamagePointsPercent/100)));
			ChangeElementalDamagePoints ((float)Math.Round ((float)complexity * (enemyType.elementalDamagePointsPercent/100)));
			ChangeArmorPoints ((float)Math.Round ((float)complexity * (enemyType.physicalArmorPointsPercent/100)));
			ChangeElementalArmorPoints ((float)Math.Round ((float)complexity * (enemyType.elementalArmorPointsPercent/100)));
			ChangeCriticalPoints ((float)Math.Round ((float)complexity * (enemyType.criticalPointsPercent/100)));
			RestoreMaximumHealth ();
			SetSpec (enemyType.specID);
		}

		weaponDamage = EquipmentGenerator.GetWeaponDamageByComplexity (complexity);
		
	}


	public IEnumerator ControlStun(float stunTime, bool withAnimation = false, bool withNullSpeed = true){
		if (withAnimation) {
			//if(characterAPI.movementController.inJump == false){
				characterAPI.skills.anim.SetBool("InStun", true);
			//}
		}
		withoutControl = true;
		//Timer stunTimer = new Timer ();
		stunTimer.SetTimer (stunTime);
		while (!stunTimer.TimeIsOver ()) {
			withoutControl = true;
			//if (withAnimation) {
			//	if(characterAPI.movementController.inJump == false){
			//		characterAPI.skills.anim.SetBool("InStun", true);
			//	}
			//}
			//Debug.Log ("stun");
			//characterAPI.movementController.SetMovement(new Vector3(0, 0, 0));
			if (withNullSpeed) {
				characterAPI.movementController.rigidbody.velocity = new Vector3 (0, 0, 0);
			}
			yield return null;
		}
		withoutControl = false;
		if (withAnimation) {
			//characterAPI.skills.anim.Play ("Idle");
			characterAPI.skills.anim.SetBool("InStun", false);
		}
		yield break;
	}

	public IEnumerator Silence(float silenceTime){
		inSilence = true;
		//Timer silenceTimer = new Timer ();
		silenceTimer.SetTimer (silenceTime);
		while (!silenceTimer.TimeIsOver ()) {
			inSilence = true;
			yield return null;
		}
		inSilence = false;
		yield break;
	}

	public void FullStun(float stunTime, bool withNullSpeed = true){
		StartCoroutine (ControlStun (stunTime, true, withNullSpeed));
		StartCoroutine (Silence (stunTime));
	}

	public void SetCurrentSpeed(float speed){
		currentSpeed = speed;
		characterAPI.skills.anim.SetFloat("RunAnimationSpeed", speed);
	}


	public void GetMovementSlowly(float time, float toSpeed){
		StartCoroutine(GetMovementSlowlyCoroutine(time, toSpeed));
	}
	IEnumerator GetMovementSlowlyCoroutine(float time, float toSpeed){
		minimumSpeedValues.Add (toSpeed);
		float minimumValueSpeed = minimumSpeedValues.Min (o => o);
		slowMovementTimer.SetTimer (time);
		minimumSpeed = minimumValueSpeed;
		while (!slowMovementTimer.TimeIsOver ()) {
			SetCurrentSpeed (minimumSpeed);
			yield return null;
		}

		if (minimumSpeedValues.Count - 1 != 0) {
			minimumSpeedValues.Remove (toSpeed);
			minimumValueSpeed = minimumSpeedValues.Min (o => o);
			minimumSpeed = minimumValueSpeed;
		} else {
			minimumSpeedValues.Remove (toSpeed);
		}
		SetCurrentSpeed(maximumSpeed);
		yield break;
	}

	public IEnumerator DealDot(NumberParams damage, int damageType, float dotTime, int ticksNumber){
		float eachTick = damage.number / ticksNumber;
		if (eachTick == 0) {
			eachTick = 1f;
		}
		NumberParams eachTickDamage = new NumberParams();
		eachTickDamage.number = eachTick;

		//Debug.Log (damage.ToString() + ":" + eachTick.ToString());

		float tickTime = dotTime / ticksNumber;

		Timer tickTimer = new Timer ();
		tickTimer.SetTimer (tickTime);
		while (ticksNumber != 0) {
			if (tickTimer.TimeIsOver ()) {
				//Debug.Log ("tick");


				MakeDamage (eachTickDamage, damageType, true);
				ticksNumber -= 1;
				if (ticksNumber == 0) {
					yield break;
				} else {
					tickTimer.SetTimer (tickTime);
					yield return null;
				}
			} else {
				yield return null;
			}
		}
	}


	public void IceStun(float time){
		
		GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/iceBlock");
		Effect effect = effectObject.GetComponent<Effect> ();
		effect.path = "Prefabs/Particles/Elemental/iceBlock";
		EffectOptions effectOptions = new EffectOptions ();
		effect.transform.parent = characterAPI.skills.nullEffectPosition.transform;
		effectOptions.transformPosition = effect.nullPositionCoords;
		effectOptions.isRandomDuration = false;
		effectOptions.duration = time;
		effectOptions.objectDuration = time;


		if (inIceStun) {
			ObjectsPool.PushObject("Prefabs/Particles/Elemental/iceBlock", characterAPI.skills.iceStunEffect.gameObject);
			characterAPI.skills.iceStunEffect = effect;
		} else {
			inIceStun = true;
			characterAPI.skills.iceStunEffect = effect;
		}

		effect.StartEffect (effectOptions);
		StartCoroutine (ControlStun (time));
		StartCoroutine (Silence(time));

		inIceStunProcess = inIceStunProcess + 1;

		Timer timer = new Timer ();
		timer.SetTimer (time);
		characterAPI.skills.anim.speed = 0f;
		Timer.TimerAction action = () => {
			if(inIceStunProcess == 1){
				characterAPI.skills.anim.speed = 1f;
				inIceStun = false;
			}
			inIceStunProcess -= 1;
		};
		StartCoroutine(timer.ActionAfterTimer (action));
	}


	IEnumerator Burn(){
		Timer burnTimer = new Timer();
		burnTimer.SetTimer (1f);
		while (itIsBurn) {
			if (burnTimer.TimeIsOver()) {
				NumberParams burnDamage = new NumberParams ();
				burnDamage.number = GetMaximumHealth () * GetBurnPercent ();
				//health -= GetMaximumHealth () * GetBurnPercent ();
				//if(isPlayerStats){
				MakeDamage(burnDamage, Stats.elementalDamageType, true);
				//	playerHealthBar.SetHealth (health);
				//}
				canRestoreHealthTimer.SetTimer (healthCanRestoreTime);
				burnTimer.SetTimer (1f);
				if (health <= 0) {
					isDeath = true;
					yield break;
				}
			}
			yield return null;
		}
		yield break;
	}

	public bool IsMaximumSpeed(){
		return currentSpeed == maximumSpeed;
	}

	public void Death(){
		if (isPlayerStats) {
		} else {
			
			withoutControl = true;
			characterAPI.movementController.rigidbody.velocity = new Vector3 (0, 0, 0);


			List<object> paramsList = new List<object> ();
			Skills.AfterAnimationAction afterAction = (List<object> actionParamsList) => {
				characterAPI.healthBarController.PushBarToPool();
				ObjectsPool.PushObject("InteractionObjects/enemyH", this.gameObject);
			};
			int random = UnityEngine.Random.Range(0, 2);
			//characterAPI.skills.anim.Stop ();
			if (random == 0) {
				characterAPI.skills.anim.Play ("Death");
				StartCoroutine (characterAPI.skills.WaitAnimationActionAndStartEvent ("Death", 0, afterAction, paramsList));
			} else {
				characterAPI.skills.anim.Play ("Death2");
				StartCoroutine (characterAPI.skills.WaitAnimationActionAndStartEvent ("Death2", 0, afterAction, paramsList));
			}
		}
	}

	public void StartGCD(float cdTime){
		StartCoroutine (StartGCDCoroutine(cdTime));
	}
	private IEnumerator StartGCDCoroutine(float cdTime){
		Timer gcdTimer = new Timer();
		gcdTimer.SetTimer(cdTime);
		onGlobalCooldown = true;
		while(!gcdTimer.TimeIsOver()){
			onGlobalCooldown = true;
			yield return null;
		}
		onGlobalCooldown = false;
		yield break;
	}

	public void StartCD(float cdTime, int skillID){
		if (cdTime != 0) {
			Timer gcdTimer = new Timer ();
			gcdTimer.SetTimer (cdTime);
			skillsOnCD.Add (skillID);
			StartCoroutine(gcdTimer.ActionAfterTimer (() => {
				skillsOnCD.Remove (skillID);
			}));
		}
	}

	public bool SkillOnCD(int skillID){
		if (skillsOnCD.FindIndex (i => i == skillID) == -1) {
			return false;
		} else {
			return true;
		}
	}

	public void Cauterization(int resource){
		NumberParams restore = new NumberParams ();
		restore.number = ((maximumHealth * 0.1f) / 100f) * (float)resource;
		RestoreHealth (restore);
	}


	public class NumberParams{
		public float number;
		public bool isCrit;
	}
}
