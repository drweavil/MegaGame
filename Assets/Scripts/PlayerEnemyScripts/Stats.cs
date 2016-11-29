using UnityEngine;
using System.Collections;
using System;

public class Stats : MonoBehaviour {
	public const int physicalDamageType = 0, hybridDamageType = 1, elementalDamageType = 2; 
	public const int meleeSpec = 0, fireSpec=1, elementalSpec = 2;
	public int specId;

	public int health;

	public int maximumHealth;
	public int stamina = 0;
	const int staminaCoeff = 10;
	private static float healthPercent = 0.2f;

	public float physicalDamage;
	public int physicalDamagePoints = 0;
	public float maximumPhysicalDamage = 100;
	private static float physicalDamagePercent = 0.15f;

	public float elementalDamage;
	public int elementalDamagePoints = 0;
	public float maximumElementalDamage = 100;
	private static float elementalDamagePercent = 0.15f;

	public float armor;
	public int armorPoints = 0;
	public float maximumArmor = 60;
	private static float armorPercent = 0.15f;

	public float elementalArmor;
	public int elementalArmorPoints = 0;
	public float maximumElementalArmor = 60;
	private static float elementalArmorPercent = 0.15f;

	public float critical;
	public int criticalPoints = 0;
	public float maximumCritical = 50;
	private static float criticalPercent = 0.2f;
	public int weaponDamage = 0;





	public int meleeEnergy;
	public int maximumMeleeEnergy = 100;
	public int fireEnergy;
	public int maximumFireEnergy = 100;
	public int magicEnergy;
	public int maximumMagicEnergy = 100;

	public bool isDeath = false;
	public bool isGrounded = true;
	public bool withoutControl = false;
	public bool inSilence = false;
	public bool inJump = false;
	float minimumSpeed;
	public float currentSpeed = 1f;



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
	/********************************/


	private float healthRestoreTime = 2f;
	private int healthRestorePoints = 10;
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
		SetStatsByComplexity (600);
		SetMaximumFireResource ();
	}



	void Update(){
		if (canRestoreHealthTimer.TimeIsOver()) {
			if(restoreHealthTimer.TimeIsOver() && health != GetMaximumHealth()){
				RestoreHealth (healthRestorePoints);
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
					AddFireEnergyPoints (fireRestorePoint);
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
	}


	public void SetMaximumHealth(){
		maximumHealth = GetMaximumHealth();
		healthRestorePoints = (int)Math.Round((float)GetMaximumHealth () * 0.02f);
		if (healthRestorePoints == 0) {
			healthRestorePoints = 1;
		}
	}

	public void RestoreMaximumHealth(){
		health = GetMaximumHealth();
		isDeath = false;
	}

	public int GetMaximumHealth(){
		return stamina * staminaCoeff;
	}

	public void  SetMaximumFireResource(){
		fireEnergy = maximumFireEnergy;
	}



	public void MakeDamage(int damage, int damageType = -1, bool withResTimer = false){
		if (damageType == physicalDamageType) {
			damage = damage - (int)Math.Round((((float)damage*armor)/100f));
		}
		if (damageType == elementalDamageType) {
			damage = damage - (int)Math.Round((((float)damage*elementalArmor)/100f));
		}
		if (damage < 0) {
			damage = 0;
		}

		health -= damage;


		if(isPlayerStats){
			playerHealthBar.SetHealth (health);
		}

		if (health <= 0) {
			isDeath = true;
		}

		if (withResTimer) {
			canRestoreHealthTimer.SetTimer (healthCanRestoreTime);
		}
	}

	public void RestoreHealth(int restore){
		health += restore;
		if (health > GetMaximumHealth ()) {
			health -= health - GetMaximumHealth ();
		}
		if(isPlayerStats){
			playerHealthBar.SetHealth (health);
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
		if (isPlayerStats) {
			SetRecourceToBar ();
		}
		if (withResTimer) {
			canRemoveMeleResourceTimer.SetTimer (meleeCanRemoveTime);	
		}
	}

	public void AddFireEnergyPoints(int points, bool withResTimer = false){
		fireEnergy += points;
		if (fireEnergy > maximumFireEnergy) {
			fireEnergy -= fireEnergy - maximumFireEnergy;
		}
		if (isPlayerStats) {
			SetRecourceToBar ();
		}
		if (withResTimer) {
			canRestoreFireResourceTimer.SetTimer (fireCanRestoreTime);	
		}
	}

	public void AddMagicEnergyPoints(int points, bool withResTimer = false){
		magicEnergy += points;
		if (magicEnergy > maximumMagicEnergy) {
			magicEnergy -= magicEnergy - maximumMagicEnergy;
		}
		if (isPlayerStats) {
			SetRecourceToBar ();
		}
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
			if (isPlayerStats) {
				SetRecourceToBar ();
			}
		}
		if (withResTimer) {
			canRemoveMeleResourceTimer.SetTimer (meleeCanRemoveTime);	
		}
	}

	public void RemoveFireEnergyPoints(int points, bool withResTimer = false){
		if (fireEnergy != 0) {
			fireEnergy -= points;
			if (fireEnergy < 0) {
				fireEnergy += fireEnergy * -1;
			}
			if (isPlayerStats) {
				SetRecourceToBar ();
			}
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
			if (isPlayerStats) {
				SetRecourceToBar ();
			}
		}
		if (withResTimer) {
			canRemoveMagicResourceTimer.SetTimer (magicCanRemoveTime);	
		}
	}


	private void SetRecourceToBar(){
		if(PlayerController.currentSpec == PlayerController.melee){
			playerHealthBar.SetResource (meleeEnergy);
		}
		if(PlayerController.currentSpec == PlayerController.fire){
			playerHealthBar.SetResource (fireEnergy);
		}
		if(PlayerController.currentSpec == PlayerController.magic){
			playerHealthBar.SetResource (magicEnergy);
		}
	}




	static int GetMaximumPhysicalDamagePoints(){
		return (int)(PlayerController.maximumComplexity * physicalDamagePercent);
	}

	static int GetMaximumElementalDamagePoints(){
		return (int)(PlayerController.maximumComplexity * elementalDamagePercent);
	}

	static int GetMaximumCriticalPoints(){
		return (int)(PlayerController.maximumComplexity * criticalPercent);
	}

	static int GetMaximumArmorPoints(){
		return (int)(PlayerController.maximumComplexity * armorPercent);
	}

	static int GetMaximumElementalArmorPoints(){
		return (int)(PlayerController.maximumComplexity * elementalArmorPercent);
	}


	public void ChangeHealthPoints(int points){
		stamina = stamina + points;
		if (stamina < 0) {
			stamina = 0;
		}
		SetMaximumHealth ();
	}

	public void SetPhysicalDamageByPoints (int points){
		physicalDamage = (float)Math.Round((float)(((float)points / (float)GetMaximumPhysicalDamagePoints ()) * (float)maximumPhysicalDamage), 2);
	}
	public void ChangePhysicalDamagePoints(int points){
		physicalDamagePoints = physicalDamagePoints + points;
		if (physicalDamagePoints > 0) {
			SetPhysicalDamageByPoints (physicalDamagePoints);
		} else {
			physicalDamage = 0;
			physicalDamagePoints = 0;
		}
	}

	public void SetElementalDamageByPoints (int points){
		elementalDamage = (float)Math.Round((float)(((float)points / (float)GetMaximumElementalDamagePoints ()) * (float)maximumElementalDamage), 2);
	}
	public void ChangeElementalDamagePoints(int points){
		elementalDamagePoints = elementalDamagePoints + points;
		if (elementalDamagePoints > 0) {
			SetElementalDamageByPoints (elementalDamagePoints);
		} else {
			elementalDamage = 0;
			elementalDamagePoints = 0;
		}
	}

	public void SetArmorByPoints(int points){
		armor = (float)Math.Round((float)(((float)points / (float)GetMaximumArmorPoints ()) * (float)maximumArmor), 2);
		if (armor > maximumArmor) {
			armor = maximumArmor;
		}
	}
	public void ChangeArmorPoints(int points){
		armorPoints = armorPoints + points;
		if (armorPoints > 0) {
			SetArmorByPoints (armorPoints);
		} else {
			armor = 0;
			armorPoints = 0;
		}
	}


	public void SetElementalArmorByPoints(int points){
		elementalArmor = (float)Math.Round((float)(((float)points / (float)GetMaximumElementalArmorPoints ()) * (float)maximumElementalArmor), 2);
		if (elementalArmor > maximumElementalArmor) {
			elementalArmor = maximumElementalArmor;
		}
	}
	public void ChangeElementalArmorPoints(int points){
		elementalArmorPoints = elementalArmorPoints + points;
		if (elementalArmorPoints > 0) {
			SetElementalArmorByPoints (elementalArmorPoints);
		} else {
			elementalArmor = 0;
			elementalArmorPoints = 0;
		}
	}


	public void SetCriticalByPoints(int points){
		critical = (float)Math.Round((float)(((float)points / (float)GetMaximumCriticalPoints ()) * (float)maximumCritical), 2);
		if (critical > maximumCritical) {
			critical = maximumCritical;
		}	
	}
	public void ChangeCriticalPoints(int points){
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
	}

	public void SetStatsByComplexity (int complexity){
		ChangeHealthPoints ((int)Math.Round((float)complexity * healthPercent));
		ChangePhysicalDamagePoints ((int)Math.Round((float)complexity * physicalDamagePercent));
		ChangeElementalDamagePoints ((int)Math.Round((float)complexity * elementalDamagePercent));
		ChangeArmorPoints ((int)Math.Round((float)complexity * armorPercent));
		ChangeElementalArmorPoints ((int)Math.Round((float)complexity * elementalArmorPercent));
		ChangeCriticalPoints ((int)Math.Round((float)complexity * criticalPercent));
		RestoreMaximumHealth ();

		weaponDamage = EquipmentGenerator.GetWeaponDamageByComplexity (complexity);
		SetSpec (Stats.meleeSpec);
	}


	public IEnumerator ControlStun(float stunTime, bool withAnimation = false){
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

	public void FullStun(float stunTime){
		StartCoroutine (ControlStun (stunTime, true));
		StartCoroutine (Silence (stunTime));
	}

	public void SetCurrentSpeed(float speed){
		currentSpeed = speed;
		characterAPI.skills.anim.SetFloat("RunAnimationSpeed", speed);
	}

	public IEnumerator GetMovementSlowly(float time, float toSpeed){
		slowMovementTimer.SetTimer (time);
		minimumSpeed = toSpeed;
		while (!slowMovementTimer.TimeIsOver ()) {
			SetCurrentSpeed (minimumSpeed);
			yield return null;
		}
		SetCurrentSpeed(1f);
		yield break;
	}

}
