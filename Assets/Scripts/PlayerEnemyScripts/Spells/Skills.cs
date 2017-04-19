using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Skills : MonoBehaviour {
	public Animator anim;
	public Stats stats;
	public Rigidbody rigidbody;
	public Transform nullEffectPosition;
	public Transform nullSkillPosition;
	public Transform armsPosition;
	private string currentAction = "";
	public  List<Collider> ignoreColliders = new List<Collider>();
	private int skillProcessCount = 0;
	public CharacterAPI characterAPI;
	public delegate void AfterAnimationAction(List<object> paramsList);

	private string[] names = new string[]{ "Run", "jumpStart", "jumpEnd", "jumpIdle" };
	private List<int> actionAnimationHashes = new List<int>();

	private Vector3 shotDirectionVector;

	private Effect shieldEffect;
	public Effect iceStunEffect;

	public Dictionary<int, SkillSettings> settingsSet = new Dictionary<int, SkillSettings>();
	private Dictionary<string, object> tempSkillParams = new Dictionary<string, object>();

	public Vector3 aimCoord;



	void Awake(){
		foreach (string name in names) {
			actionAnimationHashes.Add (Animator.StringToHash(name));
		}
	}

	void Update(){
	}

	public SkillSettings GetSkillSetting(int skillID){
		if (settingsSet.ContainsKey (skillID)) {
			return settingsSet [skillID];
		} else {
			SkillSettings newSettings = SkillSettingsSet.GetSettings(skillID);
			settingsSet.Add (newSettings.skillID, newSettings);
			return settingsSet [skillID];
		}
	}

	public float MagicEfficiency (float efficiency){
		return efficiency + (efficiency * (stats.magicEnergy/100f));
	}

	public Stats.NumberParams SkillDamage(float percent, int damageType, float efficiency = 100f, int spec = 0){
		float damage = 0;
		float wd = 0;//(float)stats.weaponDamage;
		if (spec == Stats.meleeSpec) {
			wd = stats.weaponDamage;
		} else if (spec == Stats.fireSpec) {
			wd = stats.fireWeaponDamage;
		} else if (spec == Stats.elementalSpec){
			wd = stats.elementalWeaponDamage;
		}
		float ap = 0.3f; /*average enemy armor*/
		float dp = 0;
		if (damageType == Stats.elementalDamageType) {
			dp = stats.elementalDamage / 100f;
		} else {
			dp = stats.physicalDamage / 100f;
		}

		damage = wd + (wd * percent) + (wd * dp) + (wd * percent * dp);

		//damage = (int)((wd * percent) + wd + (wd * dp) + (wd * ap * percent) + (wd * ap) + (wd * dp * ap));
		damage = damage * (efficiency/100);
		if (damage == 0) {
			damage = 1;
		}
		Stats.NumberParams finalDamage = TryCriticalDamage (damage);
		finalDamage.number = finalDamage.number + (finalDamage.number * ap)/(1 - ap);
		return finalDamage;
	}
		
	Stats.NumberParams TryCriticalDamage(float damage){
		Stats.NumberParams criticalParams = new Stats.NumberParams();
		float finalDamage = damage;
		float random = Random.Range (0f, 100.01f);
		if (random <= stats.critical) {
			finalDamage = damage * 2;
			criticalParams.isCrit = true;
		} 
		criticalParams.number = finalDamage;
		return criticalParams;
	}

	public bool IsCritical(){
		bool isCriticalValue = false;
		float random = Random.Range (0f, 100.01f);
		if (random <= stats.critical) {
			isCriticalValue = true;
		}
		return isCriticalValue;
	}

	void AnimationAction(){
		//Debug.Log ("lul");
		MethodInfo methodInfo = this.GetType ().GetMethod (currentAction);
		methodInfo.Invoke (this, new object[]{true});
	}

	public void FastPunch (bool isSecondPart = false){
		currentAction = "FastPunch";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.fastPunch);
			int target;
			if (stats.isPlayerStats) {
				target = 1 << LayerMask.NameToLayer ("Enemy");
			} else {
				target = 1 << LayerMask.NameToLayer ("Player");
			}
			RaycastHit hit;
			Physics.Raycast (transform.position, new Vector3(transform.localScale.x, 0, 0), out hit, skillSettings.distance, target);
			if (hit.collider != null) {
				Stats.NumberParams damage = SkillDamage (skillSettings.damagePercent, Stats.physicalDamageType);
				//Debug.Log (damage);
				CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI>();
				targetCharacterAPI.stats.MakeDamage(damage, Stats.physicalDamageType,  true);
				stats.AddMeleeEnergyPoints (skillSettings.resourceAdd, true);

				GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/blood");
				Effect effect = effectObject.GetComponent<Effect> ();
				effect.path = "Prefabs/Particles/blood";
				EffectOptions effectOptions = new EffectOptions ();
				effect.transform.parent = targetCharacterAPI.skills.nullEffectPosition.transform;
				effect.SetNullPositionCoords(effect.transform.position);
				effectOptions.transformPosition = effect.nullPositionCoords;
				effectOptions.minRandomDuration = 0.3f;
				effectOptions.maxRandomDuration = 0.5f;
				effect.StartEffect (effectOptions);
			}
		} else {
			if (IsAction ()) {
				anim.Play ("actionFastPunch");
			} else {
				anim.Play ("fastPunch");	
			}
		}
	}

	public void MiddlePunch(bool isSecondPart = false){
		currentAction = "MiddlePunch";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.middlePunch);
			int target;
			if (stats.isPlayerStats) {
				target = 1 << LayerMask.NameToLayer ("Enemy");
			} else {
				target = 1 << LayerMask.NameToLayer ("Player");
			}
			RaycastHit hit;
			Physics.Raycast (transform.position, new Vector3(transform.localScale.x, 0, 0), out hit, skillSettings.distance, target);
			if (hit.collider != null) {
				Stats.NumberParams damage = SkillDamage (skillSettings.damagePercent, Stats.physicalDamageType);
				//Debug.Log (damage);
				CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI>();
				targetCharacterAPI.stats.MakeDamage(damage, Stats.physicalDamageType,  true);
				stats.AddMeleeEnergyPoints (skillSettings.resourceAdd, true);

				GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/blood");
				Effect effect = effectObject.GetComponent<Effect> ();
				effect.path = "Prefabs/Particles/blood";
				EffectOptions effectOptions = new EffectOptions ();
				effect.transform.parent = targetCharacterAPI.skills.nullEffectPosition.transform;
				effect.SetNullPositionCoords(effect.transform.position);
				effectOptions.transformPosition = effect.nullPositionCoords;
				effectOptions.minRandomDuration = 0.3f;
				effectOptions.maxRandomDuration = 0.5f;
				effect.StartEffect (effectOptions);
			}
		} else {
			if (IsAction ()) {
				anim.Play ("actionMiddlePunch");
			} else {
				anim.Play ("middlePunch");	
			}
		}
	}

	public void SlowPunch(bool isSecondPart = false){
		currentAction = "SlowPunch";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.slowPunch);
			int target;
			if (stats.isPlayerStats) {
				target = 1 << LayerMask.NameToLayer ("Enemy");
			} else {
				target = 1 << LayerMask.NameToLayer ("Player");
			}
			RaycastHit hit;
			Physics.Raycast (transform.position, new Vector3(transform.localScale.x, 0, 0), out hit, skillSettings.distance, target);
			if (hit.collider != null) {
				Stats.NumberParams damage = SkillDamage (skillSettings.damagePercent, Stats.physicalDamageType);
				//Debug.Log (damage);
				CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI>();
				targetCharacterAPI.stats.MakeDamage(damage, Stats.physicalDamageType,  true);
				stats.AddMeleeEnergyPoints (skillSettings.resourceAdd, true);

				GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/blood");
				Effect effect = effectObject.GetComponent<Effect> ();
				effect.path = "Prefabs/Particles/blood";
				EffectOptions effectOptions = new EffectOptions ();
				effect.transform.parent = targetCharacterAPI.skills.nullEffectPosition.transform;
				effect.SetNullPositionCoords(effect.transform.position);
				effectOptions.transformPosition = effect.nullPositionCoords;
				effectOptions.minRandomDuration = 0.3f;
				effectOptions.maxRandomDuration = 0.5f;
				effect.StartEffect (effectOptions);
			}
		} else {
			if (IsAction ()) {
				anim.Play ("actionSlowPunch");
			} else {
				anim.Play ("slowPunch");	
			}
		}
	}


	public void MeleeCone(bool isSecondPart = false){
		currentAction = "MeleeCone";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.meleeCone);
			stats.RemoveMeleeEnergyPoints (skillSettings.resourceRemove, true);
			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Melee/meleeCone");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Melee/meleeCone";
			EffectOptions effectOptions = new EffectOptions ();
			Vector3 effectPosition = nullEffectPosition.transform.position + effect.nullPositionCoords;
			if(characterAPI.transform.localScale.x < 0){
				effectPosition = nullEffectPosition.transform.position +
				new Vector3 (effect.nullPositionCoords.x * -1, effect.nullPositionCoords.y, effect.nullPositionCoords.z);
				effectOptions.revertRotationY = true;	

			}

			effectOptions.isLocalPosition = false;
			effectOptions.transformPosition = effectPosition;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 0.5f;
			effect.StartEffect (effectOptions);

			int target;
			if (stats.isPlayerStats) {
				target = LayerMask.NameToLayer ("Enemy");
			} else {
				target = LayerMask.NameToLayer ("Player");
			}

			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/MeleeConeHitbox");
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.position = nullSkillPosition.transform.position + spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			if(characterAPI.transform.localScale.x < 0){
				//Debug.Log ("lols");
				spellHitbox.FlipWorld();	
			}
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			spellHitbox.selectingLayer = target;
			spellHitbox.path = "Prefabs/SkillPrefabs/MeleeConeHitbox";


			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.physicalDamageType), Stats.physicalDamageType, true);
			};

			StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));
		} else {
			if (IsAction ()) {
				anim.Play ("actionMeleeCone");
				StartCoroutine (WaitAnimationAction ("actionMeleeCone", 1));
			} else {
				anim.Play ("meleeCone");
				StartCoroutine (WaitAnimationAction ("meleeCone", 1));
			}
		}
	}

	public void CirclePunch(int partNumber = 0){
		currentAction = "CirclePunch";
		switch(partNumber){
		case 0:
			anim.Play ("circlePunch");
			SkillSettings skillSettings0 = GetSkillSetting (SkillSettingsSet.SkillID.circlePunch);
			stats.RemoveMeleeEnergyPoints (skillSettings0.resourceRemove, true);

			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Melee/circlePunch");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Melee/circlePunch";
			EffectOptions effectOptions = new EffectOptions ();
			effect.transform.parent = nullEffectPosition.transform;
			effectOptions.transformPosition = effect.nullPositionCoords;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 1f;
			effect.StartEffect (effectOptions);
			break;
		case 1:
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.circlePunch);
			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/CirclePunchHitbox");
			hitBox.transform.parent = nullSkillPosition.transform;
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.localPosition = spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			if (stats.isPlayerStats) {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}
			HitboxDamagerOptions damagerOptions = new HitboxDamagerOptions();
			damagerOptions.hitBox = hitBox;
			damagerOptions.spellHitbox = spellHitbox;
			damagerOptions.damagePercent = skillSettings.damagePercent;
			damagerOptions.damageType = Stats.physicalDamageType;
			damagerOptions.path = "Prefabs/SkillPrefabs/CirclePunchHitbox";
			StartCoroutine(waitFewFramesAndMakeDamage (damagerOptions));
			break;
		case 2:
			SkillSettings skillSettings2 = GetSkillSetting (SkillSettingsSet.SkillID.circlePunch);
			GameObject hitBox2 = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/CirclePunchHitbox");
			hitBox2.transform.parent = nullSkillPosition.transform;
			SpellHitbox spellHitbox2 = hitBox2.GetComponent<SpellHitbox> ();
			hitBox2.transform.position = spellHitbox2.nullSpellPosition;
			hitBox2.transform.localScale = spellHitbox2.nullSpellScale;
			spellHitbox2.Flip();
			spellHitbox2.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox2.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			if (stats.isPlayerStats) {
				spellHitbox2.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox2.selectingLayer = LayerMask.NameToLayer ("Player");
			}

			HitboxDamagerOptions damagerOptions2 = new HitboxDamagerOptions();
			damagerOptions2.hitBox = hitBox2;
			damagerOptions2.spellHitbox = spellHitbox2;
			damagerOptions2.damagePercent = skillSettings2.damagePercent;
			damagerOptions2.damageType = Stats.physicalDamageType;
			damagerOptions2.path = "Prefabs/SkillPrefabs/CirclePunchHitbox";
			StartCoroutine(waitFewFramesAndMakeDamage (damagerOptions2));
			break;

		case 3: 
			StartCoroutine (waitSkillEndAndClearColliders ());
			break;
		}
	}


	public void MeleeWave(bool isSecondPart = false){
		currentAction = "MeleeWave";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.meleeWave);
			stats.RemoveMeleeEnergyPoints (skillSettings.resourceRemove, true);

			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Melee/meleeWave");
			Effect effect = effectObject.GetComponent<Effect> ();
			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/MeleeWaveHitbox");
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();



			RaycastHit hit;
			Physics.Raycast (transform.position, new Vector3(transform.localScale.x, 0, 0), out hit, effect.distance, 1 << 8/*Ground*/);
			Vector3 currentEffectNullPosition = effect.nullPositionCoords;
			Vector3 currentHitboxNullPosition = spellHitbox.nullSpellPosition;


			if (hit.collider != null) {
				float newRadius = (hit.distance * effect.nullRadius) / effect.distance;
				effect.SetMeleeWaveRadius (newRadius);
				currentEffectNullPosition = new Vector3 ((currentEffectNullPosition.x / effect.nullRadius) * newRadius, currentEffectNullPosition.y, currentEffectNullPosition.z);

				BoxCollider hitboxCollider = (BoxCollider)(spellHitbox.collider);
				hitboxCollider.size = new Vector3 ((spellHitbox.nullColliderSize.x/ effect.nullRadius)*newRadius, spellHitbox.nullColliderSize.y, spellHitbox.nullColliderSize.z);
				hitboxCollider.center = new Vector3 ((spellHitbox.nullColliderCenter.x/ effect.nullRadius)*newRadius, spellHitbox.nullColliderCenter.y, spellHitbox.nullColliderCenter.z);
				currentHitboxNullPosition = new Vector3((spellHitbox.nullSpellPosition.x/ effect.nullRadius)*newRadius, spellHitbox.nullSpellPosition.y, spellHitbox.nullSpellPosition.z);
				//Debug.Log (currentHitboxNullPosition);
			} else {
				effect.SetMeleeWaveRadius (effect.nullRadius);	
				BoxCollider hitboxCollider = (BoxCollider)(spellHitbox.collider);
				hitboxCollider.size = spellHitbox.nullColliderSize;
				hitboxCollider.center = spellHitbox.nullColliderCenter;
			}
			effect.path = "Prefabs/Particles/Melee/meleeWave";
			EffectOptions effectOptions = new EffectOptions ();
			Vector3 effectPosition = nullEffectPosition.transform.position + currentEffectNullPosition;
			hitBox.transform.position = nullEffectPosition.transform.position + currentHitboxNullPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;

			if(characterAPI.transform.localScale.x < 0){
				effectPosition = nullEffectPosition.transform.position +
					new Vector3 (currentEffectNullPosition.x * -1, currentEffectNullPosition.y, currentEffectNullPosition.z);
				effectOptions.revertRotationY = true;
				spellHitbox.currentNullSpellPosition = currentHitboxNullPosition;
				spellHitbox.FlipWorld(true);

			}
			effectOptions.transformPosition = effectPosition;
			effectOptions.isRandomDuration = false;
			effectOptions.isLocalPosition = false;
			effectOptions.duration = 1f;
			effect.StartEffect (effectOptions);



			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			if (stats.isPlayerStats) {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}
			spellHitbox.path = "Prefabs/SkillPrefabs/MeleeWaveHitbox";


			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.physicalDamageType), Stats.physicalDamageType, true);
			};
			StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));
		} else {
			if (IsAction ()) {
				anim.Play ("actionMeleeWave");
			} else {
				anim.Play ("meleeWave");	
			}
		}
	}




	public void MeleeWaveSlow(bool isSecondPart = false){
		currentAction = "MeleeWaveSlow";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.meleeWaveSlow);
			stats.RemoveMeleeEnergyPoints (skillSettings.resourceRemove, true);

			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Melee/meleeWave2");
			Effect effect = effectObject.GetComponent<Effect> ();
			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/MeleeWaveHitbox");
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();



			RaycastHit hit;
			Physics.Raycast (transform.position, new Vector3(transform.localScale.x, 0, 0), out hit, effect.distance, 1 << 8/*Ground*/);
			Vector3 currentEffectNullPosition = effect.nullPositionCoords;
			Vector3 currentHitboxNullPosition = spellHitbox.nullSpellPosition;


			if (hit.collider != null) {
				float newRadius = (hit.distance * effect.nullRadius) / effect.distance;
				effect.SetMeleeWaveRadius (newRadius);
				currentEffectNullPosition = new Vector3 ((currentEffectNullPosition.x / effect.nullRadius) * newRadius, currentEffectNullPosition.y, currentEffectNullPosition.z);

				BoxCollider hitboxCollider = (BoxCollider)(spellHitbox.collider);
				hitboxCollider.size = new Vector3 ((spellHitbox.nullColliderSize.x/ effect.nullRadius)*newRadius, spellHitbox.nullColliderSize.y, spellHitbox.nullColliderSize.z);
				hitboxCollider.center = new Vector3 ((spellHitbox.nullColliderCenter.x/ effect.nullRadius)*newRadius, spellHitbox.nullColliderCenter.y, spellHitbox.nullColliderCenter.z);
				currentHitboxNullPosition = new Vector3((spellHitbox.nullSpellPosition.x/ effect.nullRadius)*newRadius, spellHitbox.nullSpellPosition.y, spellHitbox.nullSpellPosition.z);
				//Debug.Log (currentHitboxNullPosition);
			} else {
				effect.SetMeleeWaveRadius (effect.nullRadius);	
				BoxCollider hitboxCollider = (BoxCollider)(spellHitbox.collider);
				hitboxCollider.size = spellHitbox.nullColliderSize;
				hitboxCollider.center = spellHitbox.nullColliderCenter;
			}
			effect.path = "Prefabs/Particles/Melee/meleeWave2";
			EffectOptions effectOptions = new EffectOptions ();
			Vector3 effectPosition = nullEffectPosition.transform.position + currentEffectNullPosition;
			hitBox.transform.position = nullEffectPosition.transform.position + currentHitboxNullPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;

			if(characterAPI.transform.localScale.x < 0){
				effectPosition = nullEffectPosition.transform.position +
					new Vector3 (currentEffectNullPosition.x * -1, currentEffectNullPosition.y, currentEffectNullPosition.z);
				effectOptions.revertRotationY = true;
				spellHitbox.currentNullSpellPosition = currentHitboxNullPosition;
				spellHitbox.FlipWorld(true);

			}
			effectOptions.transformPosition = effectPosition;
			effectOptions.isRandomDuration = false;
			effectOptions.isLocalPosition = false;
			effectOptions.duration = 1f;
			effect.StartEffect (effectOptions);



			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			if (stats.isPlayerStats) {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}

			spellHitbox.path = "Prefabs/SkillPrefabs/MeleeWaveHitbox";


			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.physicalDamageType), Stats.physicalDamageType, true);
				targetAPI.stats.GetMovementSlowly(skillSettings.slowTime, skillSettings.slowSpeed);
			};
			StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));


		} else {
			if (IsAction ()) {
				anim.Play ("actionMeleeWave");
			} else {
				anim.Play ("meleeWave");	
			}
		}
	}



	public void Charge(bool isSecondPart = false){
		currentAction = "Charge";
		anim.Play ("charge");

		SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.charge);
		stats.RemoveMeleeEnergyPoints (skillSettings.resourceRemove, true);

		GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/streamHitbox");
		StreamHitbox streamHitbox = hitBox.GetComponent<StreamHitbox> ();
		hitBox.transform.parent = nullSkillPosition.transform;
		//SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
		hitBox.transform.localPosition = streamHitbox.nullSpellPosition;
		hitBox.transform.localScale = streamHitbox.nullSpellScale;
		streamHitbox.ignoreColliders.Add (characterAPI.sphereCollider);
		streamHitbox.ignoreColliders.Add (characterAPI.boxCollider);
		if (stats.isPlayerStats) {
			streamHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
		} else {
			streamHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
		}
			
		streamHitbox.action = (CharacterAPI targetAPI) => {
			targetAPI.stats.MakeDamage (characterAPI.skills.SkillDamage (skillSettings.damagePercent, Stats.physicalDamageType, 100f), Stats.physicalDamageType, true);
			targetAPI.stats.FullStun(skillSettings.stunTime);
			targetAPI.stats.GetMovementSlowly(skillSettings.slowTime, skillSettings.slowSpeed);
		};

		streamHitbox.readyToTrigger = true;

		StartCoroutine (ChargeMove(hitBox, streamHitbox));
	}
	IEnumerator ChargeMove(GameObject hitbox, StreamHitbox streamHitbox){
		Vector3 currentPosition = characterAPI.transform.position;
		for(int i = 0; i < 1; i++){
			yield return null;
		}
		stats.withoutControl = true;
		Vector3 newVelocity = new Vector3(0, 0, 0);
		if (characterAPI.transform.localScale.x < 0) {
			newVelocity = new Vector3 (-6f, 0, 0);
		} else {
			newVelocity = new Vector3 (6f, 0, 0);
		}
		//int animIndex = 4;
		while (anim.GetCurrentAnimatorStateInfo (1).shortNameHash == Animator.StringToHash ("charge")) {
			characterAPI.rigidbody.velocity = newVelocity;
			characterAPI.transform.position = new Vector3 (characterAPI.transform.position.x, currentPosition.y, currentPosition.z);

			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Melee/firePart");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Melee/firePart";
			EffectOptions effectOptions = new EffectOptions ();
			Vector3 effectPosition = nullEffectPosition.transform.position + effect.nullPositionCoords;
			if(characterAPI.transform.localScale.x < 0){
				effectPosition = nullEffectPosition.transform.position +
					new Vector3 (effect.nullPositionCoords.x * -1, effect.nullPositionCoords.y, effect.nullPositionCoords.z);
			}
			effectOptions.isLocalPosition = false;
			effectOptions.transformPosition = effectPosition;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 1f;
			effect.StartEffect (effectOptions);
			yield return null;
		}
		stats.withoutControl = false;

		streamHitbox.Clear ();
		ObjectsPool.PushObject ("Prefabs/SkillPrefabs/streamHitbox", hitbox);
		//Debug.Log ("zaz");
		yield break;
	}


	public void ThunderClap(bool isSecondPart = false){
		currentAction = "ThunderClap";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.thunderClap);
			stats.RemoveMeleeEnergyPoints (skillSettings.resourceRemove, true);

			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Melee/ThunderClapAir");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Melee/ThunderClapAir";
			EffectOptions effectOptions = new EffectOptions ();
			Vector3 effectPosition = nullEffectPosition.transform.position + effect.nullPositionCoords;

			effectOptions.isLocalPosition = false;
			effectOptions.transformPosition = effectPosition;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 1f;
			effect.StartEffect (effectOptions);



			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/ThunderClapHitBoxCenter");
			ThunderClapHitbox thunderClapHitbox = hitBox.GetComponent<ThunderClapHitbox> ();
			thunderClapHitbox.characterAPI = characterAPI;
			thunderClapHitbox.spellHitbox.ignoreColliders.Add (characterAPI.sphereCollider);
			thunderClapHitbox.spellHitbox.ignoreColliders.Add (characterAPI.boxCollider);
			hitBox.transform.parent = nullSkillPosition.transform;
			hitBox.transform.localPosition = thunderClapHitbox.spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = thunderClapHitbox.spellHitbox.nullSpellScale;
			if (stats.isPlayerStats) {
				thunderClapHitbox.spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				thunderClapHitbox.spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}
			thunderClapHitbox.action = (targetAPI) => {
				targetAPI.stats.GetMovementSlowly (skillSettings.slowTime, skillSettings.slowSpeed);
				targetAPI.stats.FullStun (skillSettings.stunTime, false);
				targetAPI.stats.MakeDamage (characterAPI.skills.SkillDamage (skillSettings.damagePercent, Stats.physicalDamageType, 100), Stats.physicalDamageType, true);
			};


			StartCoroutine (thunderClapHitbox.MakeDamageAndThrow ());

		} else {
			StartCoroutine (ToGround ());
			StartCoroutine (WaitGroundedAndStartThunderClap ());
		}
	}


	public void BlackHole(bool isSecondPart = false){
		currentAction = "BlackHole";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.blackHole);
			stats.RemoveMeleeEnergyPoints (skillSettings.resourceRemove, true);

			float blackHoleExist = skillSettings.existingTime;
			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Melee/BlackHole");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Melee/BlackHole";
			EffectOptions effectOptions = new EffectOptions ();
			//effect.transform.parent = nullEffectPosition.transform;
			Vector3 effectPosition = nullEffectPosition.transform.position + effect.nullPositionCoords;
			if(characterAPI.transform.localScale.x < 0){
				effectPosition = nullEffectPosition.transform.position +
					new Vector3 (effect.nullPositionCoords.x * -1, effect.nullPositionCoords.y, effect.nullPositionCoords.z);
				effectOptions.revertRotationY = true;	

			}

			effectOptions.isLocalPosition = false;
			effectOptions.transformPosition = effectPosition;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = blackHoleExist;
			effect.StartEffect (effectOptions);

			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/blackHoleHitbox");
			StreamHitbox streamHitbox = hitBox.GetComponent<StreamHitbox> ();
			hitBox.transform.position = nullSkillPosition.transform.position + streamHitbox.nullSpellPosition;
			hitBox.transform.localScale = streamHitbox.nullSpellScale;
			if(characterAPI.transform.localScale.x < 0){
				streamHitbox.FlipWorld();	
			}
			streamHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			streamHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			if (stats.isPlayerStats) {
				streamHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				streamHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}

			streamHitbox.action = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.physicalDamageType), Stats.physicalDamageType, true);
				StartCoroutine(targetAPI.movementController.MovementToObjectWithTimer(blackHoleExist, streamHitbox.centerTransform, new Vector2(5, 5f)));
				//StartCoroutine(targetAPI.stats.Silence(2f));
			};
			streamHitbox.readyToTrigger = true;

			Timer blackHoleTimer = new Timer ();
			blackHoleTimer.SetTimer (blackHoleExist);
			blackHoleTimer.ActionAfterTimer (() => {
				ObjectsPool.PushObject("Prefabs/SkillPrefabs/blackHoleHitbox", hitBox);
			});

			//StartCoroutine(streamHitbox.MakeObjectsAction(spellHitboxAction));

		} else {
			if (IsAction ()) {
				anim.Play ("actionBlackHole", 1);
				StartCoroutine (WaitAnimationAction ("actionBlackHole", 1));
			} else {
				anim.Play ("blackHole", 1);
				StartCoroutine (WaitAnimationAction ("blackHole", 1));
			}

			//StartCoroutine(StunWhileAnimation("blackHole", 1));
		}
	}



	public void SiphonLife(bool isSecondPart = false){
		currentAction = "SiphonLife";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.siphonLife);
			stats.RemoveMeleeEnergyPoints (skillSettings.resourceRemove, true);

			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Melee/siphonLife");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Melee/siphonLife";
			EffectOptions effectOptions = new EffectOptions ();
			effect.transform.parent = nullEffectPosition.transform;
			effectOptions.transformPosition = effect.nullPositionCoords;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 1f;
			effect.StartEffect (effectOptions);

			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/siphonLifeHitbox");
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.position = nullSkillPosition.transform.position + spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			if(characterAPI.transform.localScale.x < 0){
				spellHitbox.FlipWorld();	
			}
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			spellHitbox.path = "Prefabs/SkillPrefabs/siphonLifeHitbox";
			if (stats.isPlayerStats) {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}

			stats.RestoreHealth(SkillDamage(skillSettings.restorePercent, Stats.physicalDamageType));
			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				Stats.NumberParams damage = SkillDamage(skillSettings.damagePercent, Stats.physicalDamageType);
				targetAPI.stats.MakeDamage(damage, Stats.physicalDamageType, true);

				if(IsCritical()){
					stats.RestoreHealth(damage);
				}
			};

			StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));
		} else {
			//Debug.Log ("lol");
			if (IsAction ()) {
				Debug.Log ("lol");
				anim.Play ("actionSiphonLife", 1);
				StartCoroutine (WaitAnimationAction ("actionSiphonLife", 1));
			} else {
				anim.Play ("siphonLife", 1);
				StartCoroutine (WaitAnimationAction ("siphonLife", 1));
			}

		}
	}


	public void Chains(bool isSecondPart = false){
		currentAction = "Chains";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.chains);
			stats.RemoveMeleeEnergyPoints (skillSettings.resourceRemove, true);

			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/chainsHitbox");
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.position = nullSkillPosition.transform.position + spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			if(characterAPI.transform.localScale.x < 0){
				spellHitbox.FlipWorld();	
			}
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			spellHitbox.path = "Prefabs/SkillPrefabs/chainsHitbox";
			if (stats.isPlayerStats) {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}

			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.physicalDamageType), Stats.physicalDamageType, true);
				targetAPI.stats.GetMovementSlowly(skillSettings.slowTime, skillSettings.slowSpeed);
				//targetAPI
				GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Melee/chains");
				Effect effect = effectObject.GetComponent<Effect> ();
				effect.path = "Prefabs/Particles/Melee/chains";
				EffectOptions effectOptions = new EffectOptions ();
				effect.transform.parent = targetAPI.skills.nullEffectPosition.transform;
				effect.SetNullPositionCoords(effect.transform.position);

				Vector3 effectPosition = effect.nullPositionCoords;
				if(targetAPI.transform.localScale.x < 0){
					effectPosition = new Vector3 (effect.nullPositionCoords.x * -1, effect.nullPositionCoords.y, effect.nullPositionCoords.z);
				}
				effectOptions.isLocalPosition = true;
				effectOptions.transformPosition = effectPosition;
				effectOptions.isRandomDuration = false;
				effectOptions.duration = 5f;
				effect.StartEffect (effectOptions);
			};

			StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));
		} else {
			//Debug.Log ("lol");
			if (IsAction ()) {
				anim.Play ("actionChains", 1);
				StartCoroutine (WaitAnimationAction ("actionChains", 1));
			} else {
				anim.Play ("chains", 1);
				StartCoroutine (WaitAnimationAction ("chains", 1));
			}

		}
	}


	public void MeleeStun (bool isSecondPart = false){
		currentAction = "MeleeStun";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.meleeStun);
			stats.RemoveMeleeEnergyPoints (skillSettings.resourceRemove, true);

			int target;
			if (stats.isPlayerStats) {
				target = 1 << LayerMask.NameToLayer ("Enemy");
			} else {
				target = 1 << LayerMask.NameToLayer ("Player");
			}
			RaycastHit hit;
			Physics.Raycast (transform.position, new Vector3(transform.localScale.x, 0, 0), out hit, skillSettings.distance, target);
			if (hit.collider != null) {
				Stats.NumberParams damage = SkillDamage (skillSettings.damagePercent, Stats.physicalDamageType);
				//Debug.Log (damage);
				CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI>();
				targetCharacterAPI.stats.MakeDamage(damage, Stats.physicalDamageType,  true);

				targetCharacterAPI.stats.FullStun (skillSettings.stunTime);
			}
		} else {
			if (IsAction ()) {
				anim.Play ("actionFastPunch");
			} else {
				anim.Play ("fastPunch");	
			}
		}
	}










	/************************************************* Fire *********************************************/
	public void FastShot(bool isSecondPart = false){
		currentAction = "FastShot";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.fastShot);
			RaycastHit hit;
			float scaleDirectionX = 1;
			if (transform.localScale.x < 0) {
				scaleDirectionX = -1;
			}

			int target;
			if (stats.isPlayerStats) {
				target = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ground"));
			} else {
				target = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ground"));
			}
			Physics.Raycast (
				transform.position, 
				new Vector3(scaleDirectionX, shotDirectionVector.y, shotDirectionVector.z),
				out hit, 
				skillSettings.distance, 
				target /*Enemy*/
			);
			//Physics.Raycast (new Vec);
			if (hit.collider != null) {
				if (hit.collider.gameObject.layer != LayerMask.NameToLayer ("Ground")) {
					//Debug.Log (damage);
					CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI> ();
					targetCharacterAPI.stats.MakeDamage (SkillDamage (skillSettings.damagePercent, Stats.physicalDamageType), Stats.physicalDamageType, true);
					targetCharacterAPI.stats.MakeDamage (SkillDamage (skillSettings.damagePercent, Stats.elementalDamageType), Stats.elementalDamageType, true);
					stats.RemoveFireEnergyPoints (skillSettings.resourceRemove, true);

					GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Fire/ExplosiveBullet");
					Effect effect = effectObject.GetComponent<Effect> ();
					effect.path = "Prefabs/Particles/Fire/ExplosiveBullet";
					EffectOptions effectOptions = new EffectOptions ();
					effect.transform.parent = targetCharacterAPI.skills.nullEffectPosition.transform;
					effectOptions.isRandomDuration = false;
					effectOptions.duration = 0.5f;
					float randomYposition = Random.Range (effect.nullPositionCoords.y - 0.100f, effect.nullPositionCoords.y + 0.100f);
					effectOptions.transformPosition = new Vector3 (effect.nullPositionCoords.x, randomYposition, effect.nullPositionCoords.z);
					effect.StartEffect (effectOptions);
				}
			}
		} else {
			if (characterAPI.movementController.movement.y < 0.8f && characterAPI.movementController.movement.y > -0.8f) {
				anim.Play ("fastShot90", 2);
				shotDirectionVector = new Vector3 (1f, 0, 0);
				StartCoroutine (WaitAnimationAction ("fastShot90", 2));
			} else {
				if (characterAPI.movementController.movement.y > 0.8f) {
					anim.Play ("fastShot135", 2);
					shotDirectionVector = new Vector3 (1f, 1f, 0);
					StartCoroutine (WaitAnimationAction ("fastShot135", 2));
				} else {
					if (characterAPI.movementController.movement.y < -0.8f) {
						anim.Play ("fastShot45", 2);
						shotDirectionVector = new Vector3 (1f, -1f, 0);
						StartCoroutine (WaitAnimationAction ("fastShot45", 2));
					}
				}
			}

		}
	}

	public void MiddleShot(bool isSecondPart = false){
		currentAction = "MiddleShot";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.middleShot);
			RaycastHit hit;
			float scaleDirectionX = 1;
			if (transform.localScale.x < 0) {
				scaleDirectionX = -1;
			}

			int target;
			if (stats.isPlayerStats) {
				target = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ground"));
			} else {
				target = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ground"));
			}
			Physics.Raycast (
				transform.position, 
				new Vector3(scaleDirectionX, shotDirectionVector.y, shotDirectionVector.z),
				out hit, 
				skillSettings.distance, 
				target /*Enemy*/
			);
			//Physics.Raycast (new Vec);
			if (hit.collider != null) {
				if (hit.collider.gameObject.layer != LayerMask.NameToLayer ("Ground")) {
					//Debug.Log (damage);
					CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI> ();
					targetCharacterAPI.stats.MakeDamage (SkillDamage (skillSettings.damagePercent, Stats.elementalDamageType), Stats.elementalDamageType, true);
					targetCharacterAPI.stats.MakeDamage (SkillDamage (skillSettings.damagePercent, Stats.physicalDamageType), Stats.physicalDamageType, true);
					stats.RemoveFireEnergyPoints (skillSettings.resourceRemove, true);

					GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Fire/ExplosiveBullet");
					Effect effect = effectObject.GetComponent<Effect> ();
					effect.path = "Prefabs/Particles/Fire/ExplosiveBullet";
					EffectOptions effectOptions = new EffectOptions ();
					effect.transform.parent = targetCharacterAPI.skills.nullEffectPosition.transform;
					effectOptions.isRandomDuration = false;
					effectOptions.duration = 0.5f;
					float randomYposition = Random.Range (effect.nullPositionCoords.y - 0.100f, effect.nullPositionCoords.y + 0.100f);
					effectOptions.transformPosition = new Vector3 (effect.nullPositionCoords.x, randomYposition, effect.nullPositionCoords.z);
					effect.StartEffect (effectOptions);
				}
			}
		} else {
			if (characterAPI.movementController.movement.y < 0.8f && characterAPI.movementController.movement.y > -0.8f) {
				anim.Play ("middleShot90", 2);
				shotDirectionVector = new Vector3 (1f, 0, 0);
				StartCoroutine (WaitAnimationAction ("middleShot90", 2));
			} else {
				if (characterAPI.movementController.movement.y > 0.8f) {
					anim.Play ("middleShot135", 2);
					shotDirectionVector = new Vector3 (1f, 1f, 0);
					StartCoroutine (WaitAnimationAction ("middleShot135", 2));
				} else {
					if (characterAPI.movementController.movement.y < -0.8f) {
						anim.Play ("middleShot45", 2);
						shotDirectionVector = new Vector3 (1f, -1f, 0);
						StartCoroutine (WaitAnimationAction ("middleShot45", 2));
					}
				}
			}

		}
	}

	public void SlowShot(bool isSecondPart = false){
		currentAction = "SlowShot";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.slowShot);
			RaycastHit hit;
			float scaleDirectionX = 1;
			if (transform.localScale.x < 0) {
				scaleDirectionX = -1;
			}

			int target;
			if (stats.isPlayerStats) {
				target = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ground"));
			} else {
				target = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ground"));
			}
			Physics.Raycast (
				transform.position, 
				new Vector3(scaleDirectionX, shotDirectionVector.y, shotDirectionVector.z),
				out hit, 
				skillSettings.distance, 
				target /*Enemy*/
			);
			//Physics.Raycast (new Vec);
			if (hit.collider != null) {
				if (hit.collider.gameObject.layer != LayerMask.NameToLayer ("Ground")) {
					//float damage = SkillDamage (skillSettings.damagePercent, Stats.hybridDamageType);
					//Debug.Log (damage);
					CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI> ();
					targetCharacterAPI.stats.MakeDamage (SkillDamage (skillSettings.damagePercent, Stats.elementalDamageType), Stats.elementalDamageType, true);
					targetCharacterAPI.stats.MakeDamage (SkillDamage (skillSettings.damagePercent, Stats.physicalDamageType), Stats.physicalDamageType, true);
					stats.RemoveFireEnergyPoints (skillSettings.resourceRemove, true);

					GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Fire/ExplosiveBullet");
					Effect effect = effectObject.GetComponent<Effect> ();
					effect.path = "Prefabs/Particles/Fire/ExplosiveBullet";
					EffectOptions effectOptions = new EffectOptions ();
					effect.transform.parent = targetCharacterAPI.skills.nullEffectPosition.transform;
					effectOptions.isRandomDuration = false;
					effectOptions.duration = 0.5f;
					float randomYposition = Random.Range (effect.nullPositionCoords.y - 0.100f, effect.nullPositionCoords.y + 0.100f);
					effectOptions.transformPosition = new Vector3 (effect.nullPositionCoords.x, randomYposition, effect.nullPositionCoords.z);
					effect.StartEffect (effectOptions);
				}
			}
		} else {
			if (characterAPI.movementController.movement.y < 0.8f && characterAPI.movementController.movement.y > -0.8f) {
				anim.Play ("snipe90", 2);
				shotDirectionVector = new Vector3 (1f, 0, 0);
				StartCoroutine (WaitAnimationAction ("snipe90", 2));
			} else {
				if (characterAPI.movementController.movement.y > 0.8f) {
					anim.Play ("snipe135", 2);
					shotDirectionVector = new Vector3 (1f, 1f, 0);
					StartCoroutine (WaitAnimationAction ("snipe135", 2));
				} else {
					if (characterAPI.movementController.movement.y < -0.8f) {
						anim.Play ("snipe45", 2);
						shotDirectionVector = new Vector3 (1f, -1f, 0);
						StartCoroutine (WaitAnimationAction ("snipe45", 2));
					}
				}
			}

		}
	}



	/*public void Grenade(Vector3 grenadeCoord){
		aimCoord = grenadeCoord;
		ExplosiveMine ();
		//Debug.Log ("lol");
	}*/

	public void GrenadeLaunch(bool isSecondPart = false){
		currentAction = "GrenadeLaunch";
		//Debug.Log (isSecondPart);
		if (isSecondPart) {
			//Debug.Log ("asdf");
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.grenadeLaunch);
			stats.AddFireEnergyPoints(skillSettings.resourceAdd, true);
			stats.Cauterization (skillSettings.resourceAdd);
			RaycastHit hitWithGround;
			Vector3 finalAimCoord; 
			if (Physics.Raycast (aimCoord, new Vector3 (0, -1, 0), out hitWithGround, 10f, 1 << 8/*Ground*/)) {
				//Debug.Log (hitWithGround.distance);
				finalAimCoord = new Vector3 (aimCoord.x, aimCoord.y - hitWithGround.distance + 0.3739569f, aimCoord.z);
			} else {
				finalAimCoord = aimCoord;
			}
			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Fire/Grenade");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Fire/Grenade";
			EffectOptions effectOptions = new EffectOptions ();
			//effect.transform.parent = nullEffectPosition.transform;

			effectOptions.transformPosition = finalAimCoord + effect.nullPositionCoords;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 1f;
			effect.StartEffect (effectOptions);


			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/GrenadeHitbox");
			//hitBox.transform.parent = nullSkillPosition.transform;
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.localPosition = finalAimCoord + spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			if (stats.isPlayerStats) {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}
			spellHitbox.path = "Prefabs/SkillPrefabs/GrenadeHitbox";

			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.physicalDamageType), Stats.physicalDamageType, true);
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.elementalDamageType), Stats.elementalDamageType, true);
			};

			StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));
		} else {
			//Debug.Log ("lol");
			anim.Play ("grenade");
			StartCoroutine (WaitAnimationAction("grenade", 2));
		}
	}

	public void ToxicGrenadeLaunch(bool isSecondPart = false){
		currentAction = "ToxicGrenadeLaunch";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.toxicGrenadeLaunch);
			stats.AddFireEnergyPoints(skillSettings.resourceAdd, true);
			stats.Cauterization (skillSettings.resourceAdd);
			RaycastHit hitWithGround;
			Vector3 finalAimCoord; 
			if (Physics.Raycast (aimCoord, new Vector3 (0, -1, 0), out hitWithGround, 10f, 1 << 8/*Ground*/)) {
				//Debug.Log (hitWithGround.distance);
				finalAimCoord = new Vector3 (aimCoord.x, aimCoord.y - hitWithGround.distance + 0.3739569f, aimCoord.z);
			} else {
				finalAimCoord = aimCoord;
			}
			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Fire/toxicGrenade");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Fire/toxicGrenade";
			EffectOptions effectOptions = new EffectOptions ();

			effectOptions.transformPosition = finalAimCoord + effect.nullPositionCoords;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 1f;
			effect.StartEffect (effectOptions);


			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/GrenadeHitbox");
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.localPosition = finalAimCoord + spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			if (stats.isPlayerStats) {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}
			spellHitbox.path = "Prefabs/SkillPrefabs/GrenadeHitbox";

			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.physicalDamageType), Stats.physicalDamageType, true);
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.elementalDamageType), Stats.elementalDamageType, true);
				targetAPI.stats.FullStun(skillSettings.stunTime);
			};

			StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));
		} else {
			//Debug.Log ("lol");
			anim.Play ("grenade");
			StartCoroutine (WaitAnimationAction("grenade", 2));
		}
	}


	public void RocketJump(bool isSecondPart = false){
		currentAction = "RocketJump";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.rocketJump);
			//characterAPI.stats.withoutControl = true;
			stats.AddFireEnergyPoints(skillSettings.resourceAdd, true);
			stats.Cauterization (skillSettings.resourceAdd);

			int directionIndex = -1;
			if (characterAPI.transform.localScale.x < 0) {
				directionIndex = 1;
			}



			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Fire/RocketJumpBang");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Fire/RocketJumpBang";
			EffectOptions effectOptions = new EffectOptions ();
			//effect.transform.parent = nullEffectPosition.transform;

			effectOptions.transformPosition = nullEffectPosition.transform.position + effect.nullPositionCoords;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 0.6f;
			effect.StartEffect (effectOptions);


			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/RocketJumpHitBox");
			//hitBox.transform.parent = nullSkillPosition.transform;
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.position = nullEffectPosition.transform.position + spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			if (stats.isPlayerStats) {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}
			spellHitbox.path = "Prefabs/SkillPrefabs/GrenadeHitbox";

			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.physicalDamageType), Stats.physicalDamageType, true);
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.elementalDamageType), Stats.elementalDamageType, true);
			};

			StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));
			characterAPI.movementController.AddForceWithStartAnimation(new Vector3(350 * directionIndex, 350, 0), "rocketJump", 2);

			/*HitboxDamagerOptions damagerOptions = new HitboxDamagerOptions();
			damagerOptions.hitBox = hitBox;
			damagerOptions.spellHitbox = spellHitbox;
			damagerOptions.damagePercent = 0.1025641f;
			damagerOptions.damageType = Stats.hybridDamageType;
			damagerOptions.path = "Prefabs/SkillPrefabs/GrenadeHitbox";

			StartCoroutine(waitFewFramesAndMakeDamage (damagerOptions));
			StartCoroutine (waitSkillEndAndClearColliders ());
			*/
			//stats.inJump = true;
		} else {
			anim.Play ("rocketJump");
			StartCoroutine (WaitAnimationAction("rocketJump", 2));
			StartCoroutine (StunWhileAnimation ("rocketJump", 2));
			characterAPI.rigidbody.velocity = new Vector3 (0, 0, 0);
		}
	}


	public void GrenadeWave(bool isSecondPart = false){
		currentAction = "GrenadeWave";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.grenadeWave);
			stats.AddFireEnergyPoints(skillSettings.resourceAdd, true);
			stats.Cauterization (skillSettings.resourceAdd);
			List<Collider> waveIgnoreColliders = new List<Collider> ();
			SpellHitbox.ObjectsAction action = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.elementalDamageType), Stats.elementalDamageType, true);
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.physicalDamageType), Stats.physicalDamageType, true);
				waveIgnoreColliders.Add(targetAPI.boxCollider);
				waveIgnoreColliders.Add(targetAPI.sphereCollider);
			};
			StartCoroutine (GrenadeWaveProcess("Prefabs/Particles/Fire/GrenadeWavePart", action, waveIgnoreColliders));
		} else {
			anim.Play ("grenadeWave", 2);
			StartCoroutine (WaitAnimationAction("grenadeWave", 2));
		}
	}

	public void ToxicGrenadeWave(bool isSecondPart = false){
		currentAction = "ToxicGrenadeWave";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.toxicGrenadeWave);
			stats.AddFireEnergyPoints(skillSettings.resourceAdd, true);
			stats.Cauterization (skillSettings.resourceAdd);
			List<Collider> waveIgnoreColliders = new List<Collider> ();
			SpellHitbox.ObjectsAction action = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.elementalDamageType), Stats.elementalDamageType, true);
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.physicalDamageType), Stats.physicalDamageType, true);
				targetAPI.stats.GetMovementSlowly(skillSettings.slowTime, skillSettings.slowSpeed);
				waveIgnoreColliders.Add(targetAPI.boxCollider);
				waveIgnoreColliders.Add(targetAPI.sphereCollider);
			};
			StartCoroutine (GrenadeWaveProcess("Prefabs/Particles/Fire/toxicGrenade", action, waveIgnoreColliders));
		} else {
			anim.Play ("grenadeWave", 2);
			StartCoroutine (WaitAnimationAction("grenadeWave", 2));
		}
	}

	IEnumerator GrenadeWaveProcess(string effectPath, SpellHitbox.ObjectsAction action, List<Collider>waveIgnoreColliders){
		int directionIndex = 1;
		if (transform.localScale.x < 0) {
			directionIndex = -1;
		}

		float rangeBetweenBangs = 0.6571111f;
		Vector3 waveAimCoord = nullEffectPosition.transform.position; 
		//List<Collider> waveIgnoreColliders = new List<Collider> ();
		waveIgnoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
		waveIgnoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
		Vector3 finalAimCoord;

		RaycastHit hitWithWall;
		Physics.Raycast (
			transform.position, 
			new Vector3(directionIndex, 0, 0),
			out hitWithWall, 
			6.2f, 
			(1 << LayerMask.NameToLayer("Ground")) /*Enemy*/
		);
		int rocketsCount = 7;
		if (hitWithWall.collider != null) {
			if (hitWithWall.distance < rangeBetweenBangs) { 
				rocketsCount = 1;
			} else {
				rocketsCount = (int)(hitWithWall.distance / rangeBetweenBangs);
				if ((hitWithWall.distance - ((float)rocketsCount * rangeBetweenBangs)) > rangeBetweenBangs/2) {
					rocketsCount += 1;
				}
			}
			float excessDistance  = (rocketsCount * rangeBetweenBangs + rangeBetweenBangs/2) - hitWithWall.distance;
			rangeBetweenBangs -= excessDistance / rocketsCount;
		}

		int rocketFrame = 5;
		for (int i = 0; i < rocketsCount * rocketFrame; i++) {
			if (i % rocketFrame == 0) {
				RaycastHit hitWithGround;
				GameObject effectObject = ObjectsPool.PullObject (effectPath);
				Effect effect = effectObject.GetComponent<Effect> ();
				effect.path = effectPath;
				EffectOptions effectOptions = new EffectOptions ();
				waveAimCoord = new Vector3 (waveAimCoord.x + rangeBetweenBangs * directionIndex, waveAimCoord.y, waveAimCoord.z);
				if (Physics.Raycast (waveAimCoord, new Vector3 (0, -1, 0), out hitWithGround, 10f, 1 << 8/*Ground*/)) {
					finalAimCoord = new Vector3 (waveAimCoord.x, waveAimCoord.y - hitWithGround.distance + 0.3739569f, waveAimCoord.z);
				} else {
					finalAimCoord = waveAimCoord;
				}
				effectOptions.transformPosition = finalAimCoord + effect.nullPositionCoords;
				effectOptions.isRandomDuration = false;
				effectOptions.duration = 1f;
				effect.StartEffect (effectOptions);

				GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/GrenadeWaveHitbox");
				SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
				hitBox.transform.localPosition = finalAimCoord + spellHitbox.nullSpellPosition;
				hitBox.transform.localScale = spellHitbox.nullSpellScale;
				foreach (Collider ignCol in waveIgnoreColliders) {
					spellHitbox.ignoreColliders.Add (ignCol);
				}


				spellHitbox.path = "Prefabs/SkillPrefabs/GrenadeWaveHitbox";
				if (stats.isPlayerStats) {
					spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
				} else {
					spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
				}
				StartCoroutine (spellHitbox.MakeObjectsAction(action));
			}

			yield return null;
		}
	}



	public void RocketCircle(int partNumber = 0){
		currentAction = "RocketCircle";
		//Debug.Log (partNumber);
		switch(partNumber){
		case 0:
			anim.Play ("rocketCircle", 2);
			SkillSettings skillSettings0 = GetSkillSetting (SkillSettingsSet.SkillID.rocketCircle);
			stats.AddFireEnergyPoints(skillSettings0.resourceAdd, true);
			stats.Cauterization (skillSettings0.resourceAdd);
			StartCoroutine (WaitAnimationAction ("rocketCircle", 2));
			tempSkillParams.Add ("characterScaleRocketCircle", characterAPI.transform.localScale.x);
			break;
		case 1:
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.rocketCircle);
			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Fire/rocketCircle");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Fire/rocketCircle";
			effect.transform.localScale = effect.nullScaleCoords;
			EffectOptions effectOptions = new EffectOptions ();
			Vector3 effectPosition = nullEffectPosition.transform.position + effect.nullPositionCoords;
			if (/*characterAPI.transform.localScale.x*/ (float)tempSkillParams["characterScaleRocketCircle"] < 0) {
				effectPosition = nullEffectPosition.transform.position +
				new Vector3 (effect.nullPositionCoords.x * -1, effect.nullPositionCoords.y, effect.nullPositionCoords.z);
				effectOptions.revertRotationY = true;	
				effect.transform.localScale = new Vector3 (effect.transform.localScale.x * -1, effect.transform.localScale.y, effect.transform.localScale.z);	
			}
			effectOptions.isLocalPosition = false;
			effectOptions.transformPosition = effectPosition;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 1.5f;
			effect.transform.rotation = Quaternion.Euler (effect.nullRotationCoords);
			effect.StartEffect (effectOptions);


			/************************BangEffect***************************/
			GameObject bangEffectObject = ObjectsPool.PullObject ("Prefabs/Particles/Fire/bang");
			EffectOptions bangEffectOptions = new EffectOptions ();
			Effect bangEffect = bangEffectObject.GetComponent<Effect> ();
			bangEffect.transform.rotation = Quaternion.Euler (bangEffect.nullRotationCoords);
			bangEffect.transform.localScale = bangEffect.nullScaleCoords;

			RaycastHit hitWithGround;
			Vector3 finalCoord = nullEffectPosition.position; 
			Vector3 bangEffectPosition = bangEffect.nullPositionCoords;
			if (/*characterAPI.transform.localScale.x*/(float)tempSkillParams["characterScaleRocketCircle"] < 0) {
				bangEffectOptions.revertRotationY = true;
				bangEffectPosition = new Vector3 (bangEffectPosition.x * -1, bangEffectPosition.y, bangEffectPosition.z);
			}

			if (Physics.Raycast (finalCoord + bangEffectPosition, new Vector3 (0, -1, 0), out hitWithGround, 10f, 1 << LayerMask.NameToLayer("Ground")/*Ground*/)) {
				finalCoord = new Vector3 (finalCoord.x, finalCoord.y - hitWithGround.distance + 0.03895676f /*0.3739569f*/, finalCoord.z);
			} else {
				finalCoord = nullEffectPosition.position;
			}
			bangEffect.path = "Prefabs/Particles/Fire/bang";
			bangEffectOptions.transformPosition = finalCoord + bangEffectPosition;
			bangEffectOptions.isRandomDuration = false;
			bangEffectOptions.duration = 0.6f;
			bangEffectObject.SetActive (false);
			/*******************************************/

			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/rocketCircleHitbox");
			hitBox.SetActive (false);
			//hitBox.transform.parent = nullSkillPosition.transform;
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.position = finalCoord + spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			if (stats.isPlayerStats) {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}
			//Debug.Log ("lil");
			HitboxDamagerOptions damagerOptions = new HitboxDamagerOptions ();
			damagerOptions.hitBox = hitBox;
			damagerOptions.spellHitbox = spellHitbox;
			damagerOptions.damagePercent = skillSettings.damagePercent;
			damagerOptions.damageType = Stats.hybridDamageType;
			damagerOptions.path = "Prefabs/SkillPrefabs/rocketCircleHitbox";

			Timer bangTimer = new Timer ();
			bangTimer.SetTimer (0.6f);
			StartCoroutine(bangTimer.ActionAfterTimer (() => {
				hitBox.SetActive (true);
				bangEffectObject.SetActive (true);
				bangEffect.StartEffect (bangEffectOptions);
				StartCoroutine (waitFewFramesAndMakeDamage (damagerOptions));
			}));

			break;
		case 2:
			SkillSettings skillSettings2 = GetSkillSetting (SkillSettingsSet.SkillID.rocketCircle);
			GameObject effectObject2 = ObjectsPool.PullObject ("Prefabs/Particles/Fire/rocketCircle");
			Effect effect2 = effectObject2.GetComponent<Effect> ();
			effect2.path = "Prefabs/Particles/Fire/rocketCircle";
			effect2.transform.localScale = effect2.nullScaleCoords;
			EffectOptions effectOptions2 = new EffectOptions ();
			Vector3 effectPosition2 = nullEffectPosition.transform.position + effect2.nullPositionCoords;
			if (/*characterAPI.transform.localScale.x*/(float)tempSkillParams["characterScaleRocketCircle"] > 0) {
				effectPosition2 = nullEffectPosition.transform.position +
				new Vector3 (effect2.nullPositionCoords.x * -1, effect2.nullPositionCoords.y, effect2.nullPositionCoords.z);
				effectOptions2.revertRotationY = true;	
				effect2.transform.localScale = new Vector3 (effect2.transform.localScale.x * -1, effect2.transform.localScale.y, effect2.transform.localScale.z);
			}
			effect2.transform.rotation = Quaternion.Euler (effect2.nullRotationCoords);
			effectOptions2.isLocalPosition = false;
			effectOptions2.transformPosition = effectPosition2;
			effectOptions2.isRandomDuration = false;
			effectOptions2.duration = 1.5f;
			effect2.StartEffect (effectOptions2);



			/**********************BangEffect*****************************/
			GameObject bangEffectObject2 = ObjectsPool.PullObject ("Prefabs/Particles/Fire/bang");
			EffectOptions bangEffectOptions2 = new EffectOptions ();
			Effect bangEffect2 = bangEffectObject2.GetComponent<Effect> ();
			bangEffect2.transform.rotation = Quaternion.Euler (bangEffect2.nullRotationCoords);
			bangEffect2.transform.localScale = bangEffect2.nullScaleCoords;

			RaycastHit hitWithGround2;
			Vector3 finalCoord2 = nullEffectPosition.position; 
			Vector3 bangEffectPosition2 = bangEffect2.nullPositionCoords;
			if (/*characterAPI.transform.localScale.x*/(float)tempSkillParams["characterScaleRocketCircle"] > 0) {
				bangEffectOptions2.revertRotationY = true;
				bangEffectPosition2 = new Vector3 (bangEffectPosition2.x * -1, bangEffectPosition2.y, bangEffectPosition2.z);
			}

			if (Physics.Raycast (finalCoord2 + bangEffectPosition2, new Vector3 (0, -1, 0), out hitWithGround2, 10f, 1 << LayerMask.NameToLayer ("Ground")/*Ground*/)) {
				finalCoord2 = new Vector3 (finalCoord2.x, finalCoord2.y - hitWithGround2.distance + 0.03895676f/*+ 0.3739569f*/, finalCoord2.z);
				//Debug.Log(hitWithGround2.distance);
			} else {
				finalCoord2 = nullEffectPosition.position;
			}
			bangEffect2.path = "Prefabs/Particles/Fire/bang";
			bangEffectOptions2.transformPosition = finalCoord2 + bangEffectPosition2;
			bangEffectOptions2.isRandomDuration = false;
			bangEffectOptions2.duration = 0.6f;
			bangEffectObject2.SetActive (false);

			/*******************************************/

			//Debug.Log ("lil2");

			GameObject hitBox2 = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/rocketCircleHitbox");
			hitBox2.SetActive (false);
			//hitBox2.transform.parent = nullSkillPosition.transform;
			SpellHitbox spellHitbox2 = hitBox2.GetComponent<SpellHitbox> ();
			hitBox2.transform.position = finalCoord2 + spellHitbox2.nullSpellPosition;
			hitBox2.transform.localScale = spellHitbox2.nullSpellScale;
			spellHitbox2.FlipWorld();
			spellHitbox2.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox2.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			if (stats.isPlayerStats) {
				spellHitbox2.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox2.selectingLayer = LayerMask.NameToLayer ("Player");
			}

			HitboxDamagerOptions damagerOptions2 = new HitboxDamagerOptions();
			damagerOptions2.hitBox = hitBox2;
			damagerOptions2.spellHitbox = spellHitbox2;
			damagerOptions2.damagePercent = skillSettings2.damagePercent;
			damagerOptions2.damageType = Stats.hybridDamageType;
			damagerOptions2.path = "Prefabs/SkillPrefabs/rocketCircleHitbox";
			Timer bangTimer2 = new Timer ();
			bangTimer2.SetTimer (0.6f);
			StartCoroutine(bangTimer2.ActionAfterTimer (() => {
				hitBox2.SetActive (true);
				bangEffectObject2.SetActive (true);
				bangEffect2.StartEffect (bangEffectOptions2);
				StartCoroutine (waitFewFramesAndMakeDamage (damagerOptions2));
			}));
			break;

		case 3: 
			//Debug.Log ("lil3");
			tempSkillParams.Remove ("characterScaleRocketCircle");
			StartCoroutine (waitSkillEndAndClearColliders ());
			break;
		}
	}


	public void LightingBombLaunch(bool isSecondPart = false){
		currentAction = "LightingBombLaunch";
		//Debug.Log (isSecondPart);
		if (isSecondPart) {
			//Debug.Log ("asdf");
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.lightingBombLaunch);
			float cloudExistingTime = skillSettings.existingTime;
			float slowMovementTime = skillSettings.slowTime;
			stats.AddFireEnergyPoints(skillSettings.resourceAdd, true);
			stats.Cauterization (skillSettings.resourceAdd);
			RaycastHit hitWithGround;
			Vector3 finalAimCoord; 
			if (Physics.Raycast (aimCoord, new Vector3 (0, -1, 0), out hitWithGround, 10f, 1 << LayerMask.NameToLayer("Ground"))) {
				finalAimCoord = new Vector3 (aimCoord.x, aimCoord.y - hitWithGround.distance + 0.3739569f, aimCoord.z);
			} else {
				finalAimCoord = aimCoord;
			}
			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Fire/LightingBomb");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Fire/LightingBomb";
			EffectOptions effectOptions = new EffectOptions ();
			effectOptions.transformPosition = finalAimCoord + effect.nullPositionCoords;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = cloudExistingTime;
			effect.StartEffect (effectOptions);


			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/LightingCloudStreamHitbox");
			//hitBox.transform.parent = nullSkillPosition.transform;
			StreamHitbox streamHitbox= hitBox.GetComponent<StreamHitbox> ();
			hitBox.transform.localPosition = finalAimCoord + streamHitbox.nullSpellPosition;
			hitBox.transform.localScale = streamHitbox.nullSpellScale;
			streamHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			streamHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			if (stats.isPlayerStats) {
				streamHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				streamHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}

			//streamHitbox.path = "Prefabs/SkillPrefabs/LightingBombSteamHitbox";

			streamHitbox.action = (CharacterAPI targetAPI) => {
				//targetAPI.stats.MakeDamage(SkillDamage(0.8589743f, Stats.physicalDamageType), Stats.physicalDamageType, true);
				targetAPI.stats.GetMovementSlowly(slowMovementTime, skillSettings.slowSpeed);
				Timer slowMovementTimer = new Timer(); 
				slowMovementTimer.SetTimer(slowMovementTime);
				StartCoroutine(slowMovementTimer.ActionAfterTimer(() => {
					if(targetAPI.gameObject.activeInHierarchy){
						streamHitbox.RemoveIgnoreObject(targetAPI.gameObject);
					}
				}));
			};
			streamHitbox.readyToTrigger = true;

			Timer cloudTimer = new Timer ();
			cloudTimer.SetTimer (cloudExistingTime);
			StartCoroutine(cloudTimer.ActionAfterTimer (() => {
				streamHitbox.Clear();
				ObjectsPool.PushObject("Prefabs/SkillPrefabs/LightingCloudStreamHitbox", hitBox);	
			}));
		} else {
			//Debug.Log ("lol");
			anim.Play ("grenade");
			StartCoroutine (WaitAnimationAction("grenade", 2));
		}
	}


	public void ExplosiveMine(bool isSecondPart = false){
		currentAction = "ExplosiveMine";
		//Debug.Log (isSecondPart);
		if (isSecondPart) {
			//Debug.Log ("asdf");
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.explosiveMine);
			stats.AddFireEnergyPoints(skillSettings.resourceAdd, true);
			stats.Cauterization (skillSettings.resourceAdd);
			RaycastHit hitWithGround;
			Vector3 finalAimCoord; 
			if (Physics.Raycast (aimCoord, new Vector3 (0, -1, 0), out hitWithGround, 10f, 1 << LayerMask.NameToLayer("Ground"))) {
				//Debug.Log (hitWithGround.distance);
				finalAimCoord = new Vector3 (aimCoord.x, aimCoord.y - hitWithGround.distance + 0.3739569f, aimCoord.z);
			} else {
				finalAimCoord = aimCoord;
			}
			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Fire/bangWithNullX");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Fire/bangWithNullX";
			EffectOptions effectOptions = new EffectOptions ();
			effectOptions.transformPosition = finalAimCoord + effect.nullPositionCoords;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 0.7f;
			effectObject.SetActive (false);

			GameObject mine = ObjectsPool.PullObject ("Prefabs/Particles/Fire/mine");
			Effect mineEffect = mine.GetComponent<Effect> ();
			EffectOptions mineEffectOptions = new EffectOptions ();
			mineEffectOptions.loop = true;
			mineEffectOptions.transformPosition = finalAimCoord + mineEffect.nullPositionCoords;
			mineEffect.StartEffect(mineEffectOptions);


			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/ExplosiveMineHitbox");
			//hitBox.transform.parent = nullSkillPosition.transform;
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.position = finalAimCoord + spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			if (stats.isPlayerStats) {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}
			spellHitbox.path = "Prefabs/SkillPrefabs/ExplosiveMineHitbox";
			hitBox.SetActive (false);

			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(0.8589743f, Stats.physicalDamageType), Stats.physicalDamageType, true);
				targetAPI.stats.MakeDamage(SkillDamage(0.8589743f, Stats.elementalDamageType), Stats.elementalDamageType, true);
				int randomVectorValue = Random.Range (100, 150);
				if(characterAPI.transform.position.x < targetAPI.transform.position.x){
					targetAPI.stats.FullStun (skillSettings.stunTime, false);
					targetAPI.movementController.NullSpeedWhenGround ();
					targetAPI.movementController.AddForceWithoutAnimation (new Vector3 (randomVectorValue, 250, 0));
				}else{
					targetAPI.stats.FullStun (skillSettings.stunTime, false);
					targetAPI.movementController.NullSpeedWhenGround ();
					targetAPI.movementController.AddForceWithoutAnimation (new Vector3 (randomVectorValue * -1, 250, 0));
				}
			};


			Timer mineActivateTimer = new Timer ();
			mineActivateTimer.SetTimer (1f);
			StartCoroutine (mineActivateTimer.ActionAfterTimer(() => {
				ObjectsPool.PushObject("Prefabs/Particles/Fire/mine", mine);
				hitBox.SetActive(true);
				effectObject.SetActive(true);
				StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));
				effect.StartEffect (effectOptions);
			}));
		} else {
			//Debug.Log ("lol");
			anim.Play ("grenade");
			StartCoroutine (WaitAnimationAction("grenade", 2));
		}
	}


	public void SnipeShot(bool isSecondPart = false){
		currentAction = "SnipeShot";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.snipeShot);
			stats.AddFireEnergyPoints(skillSettings.resourceAdd, true);
			stats.Cauterization (skillSettings.resourceAdd);
			RaycastHit hit;
			float scaleDirectionX = 1;
			if (transform.localScale.x < 0) {
				scaleDirectionX = -1;
			}

			int target;
			if (stats.isPlayerStats) {
				target = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ground"));
			} else {
				target = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ground"));
			}
			Physics.Raycast (
				transform.position, 
				new Vector3(scaleDirectionX, shotDirectionVector.y, shotDirectionVector.z),
				out hit, 
				skillSettings.distance, 
				target /*Enemy*/
			);
			//Physics.Raycast (new Vec);
			if (hit.collider != null) {
				if (hit.collider.gameObject.layer != LayerMask.NameToLayer ("Ground")) {
					//float damage = SkillDamage (-0.2179487f, Stats.physicalDamageType);
					//Debug.Log (damage);

					CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI> ();
					targetCharacterAPI.stats.MakeDamage (SkillDamage (skillSettings.damagePercent, Stats.physicalDamageType), Stats.physicalDamageType, true);
					targetCharacterAPI.stats.MakeDamage (SkillDamage (skillSettings.damagePercent, Stats.elementalDamageType), Stats.elementalDamageType, true);
					targetCharacterAPI.stats.FullStun (skillSettings.stunTime);

					GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Fire/ExplosiveBullet");
					Effect effect = effectObject.GetComponent<Effect> ();
					effect.path = "Prefabs/Particles/Fire/ExplosiveBullet";
					EffectOptions effectOptions = new EffectOptions ();
					effect.transform.parent = targetCharacterAPI.skills.nullEffectPosition.transform;
					effectOptions.isRandomDuration = false;
					effectOptions.duration = 0.5f;
					float randomYposition = Random.Range (effect.nullPositionCoords.y - 0.100f, effect.nullPositionCoords.y + 0.100f);
					effectOptions.transformPosition = new Vector3 (effect.nullPositionCoords.x, randomYposition, effect.nullPositionCoords.z);
					effect.StartEffect (effectOptions);
				}
			}
		} else {
			if (characterAPI.movementController.movement.y < 0.8f && characterAPI.movementController.movement.y > -0.8f) {
				anim.Play ("snipe90", 2);
				shotDirectionVector = new Vector3 (1f, 0, 0);
				StartCoroutine (WaitAnimationAction ("snipe90", 2));
			} else {
				if (characterAPI.movementController.movement.y > 0.8f) {
					anim.Play ("snipe135", 2);
					shotDirectionVector = new Vector3 (1f, 1f, 0);
					StartCoroutine (WaitAnimationAction ("snipe135", 2));
				} else {
					if (characterAPI.movementController.movement.y < -0.8f) {
						anim.Play ("snipe45", 2);
						shotDirectionVector = new Vector3 (1f, -1f, 0);
						StartCoroutine (WaitAnimationAction ("snipe45", 2));
					}
				}
			}

		}
	}

	public void FireClap(bool isSecondPart = false){
		currentAction = "FireClap";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.fireClap);
			stats.AddFireEnergyPoints(skillSettings.resourceAdd, true);
			stats.Cauterization (skillSettings.resourceAdd);
			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Fire/fireClap");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Fire/fireClap";
			EffectOptions effectOptions = new EffectOptions ();
			Vector3 effectPosition = nullEffectPosition.transform.position + effect.nullPositionCoords;

			effectOptions.isLocalPosition = false;
			effectOptions.transformPosition = effectPosition;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 1f;
			effect.StartEffect (effectOptions);



			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/ThunderClapHitBoxCenter");
			ThunderClapHitbox thunderClapHitbox = hitBox.GetComponent<ThunderClapHitbox> ();
			thunderClapHitbox.characterAPI = characterAPI;
			thunderClapHitbox.spellHitbox.ignoreColliders.Add (characterAPI.sphereCollider);
			thunderClapHitbox.spellHitbox.ignoreColliders.Add (characterAPI.boxCollider);
			hitBox.transform.parent = nullSkillPosition.transform;
			hitBox.transform.localPosition = thunderClapHitbox.spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = thunderClapHitbox.spellHitbox.nullSpellScale;
			if (stats.isPlayerStats) {
				thunderClapHitbox.spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				thunderClapHitbox.spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}
			thunderClapHitbox.action = (targetAPI) => {
				targetAPI.stats.GetMovementSlowly (skillSettings.slowTime, skillSettings.slowSpeed);
				targetAPI.stats.FullStun (skillSettings.stunTime, false);
				targetAPI.stats.MakeDamage (characterAPI.skills.SkillDamage (skillSettings.damagePercent, Stats.physicalDamageType, 100), Stats.physicalDamageType, true);
				targetAPI.stats.MakeDamage (characterAPI.skills.SkillDamage (skillSettings.damagePercent, Stats.elementalDamageType, 100), Stats.elementalDamageType, true);
			};


			StartCoroutine (thunderClapHitbox.MakeDamageAndThrow ());

		} else {
			anim.Play ("fireClap");
			StartCoroutine (WaitAnimationAction ("fireClap", 2));
		}
	}













	/************************************************Elemental****************************************************/
	public void FastCast(bool isSecondPart = false){
		currentAction = "FastCast";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.fastCast);
			RaycastHit hit;
			float scaleDirectionX = 1;
			if (transform.localScale.x < 0) {
				scaleDirectionX = -1;
			}



			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/fireball");
			Effect effect = effectObject.GetComponent<Effect> ();
			SpellBall spellBall = effectObject.GetComponent<SpellBall> ();
			spellBall.characterAPI = characterAPI;
			spellBall.direction = new Vector3 (scaleDirectionX, shotDirectionVector.y, shotDirectionVector.z);
			spellBall.speed = 0.15f;
			spellBall.time = 2f;
			spellBall.currentTime = 0;
			spellBall.ready = true;
			spellBall.path = "Prefabs/Particles/Elemental/fireball";
			spellBall.ignoreColliders.Add (characterAPI.boxCollider);
			spellBall.ignoreColliders.Add (characterAPI.sphereCollider);
			spellBall.action = (enemyTarget) => {
				enemyTarget.stats.MakeDamage (SkillDamage (skillSettings.damagePercent, Stats.elementalDamageType, MagicEfficiency(100)), Stats.elementalDamageType, true);
				characterAPI.stats.AddMagicEnergyPoints (skillSettings.resourceAdd, true);
			};
			if (stats.isPlayerStats) {
				spellBall.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellBall.selectingLayer = LayerMask.NameToLayer ("Player");
			}
			effect.path = "Prefabs/Particles/Elemental/fireball";
			EffectOptions effectOptions = new EffectOptions ();
			effectOptions.loop = true;
			effectOptions.transformPosition = armsPosition.position;
			effect.StartEffect (effectOptions);
		} else {
			if (characterAPI.movementController.movement.y < 0.8f && characterAPI.movementController.movement.y > -0.8f) {
				anim.Play ("fastCast90", 3);
				shotDirectionVector = new Vector3 (1f, 0, 0);
				StartCoroutine (WaitAnimationAction ("fastCast90", 3));
			} else {
				if (characterAPI.movementController.movement.y > 0.8f) {
					anim.Play ("fastCast135", 3);
					shotDirectionVector = new Vector3 (1f, 1f, 0);
					StartCoroutine (WaitAnimationAction ("fastCast135", 3));
				} else {
					if (characterAPI.movementController.movement.y < -0.8f) {
						anim.Play ("fastCast45", 3);
						shotDirectionVector = new Vector3 (1f, -1f, 0);
						StartCoroutine (WaitAnimationAction ("fastCast45", 3));
					}
				}
			}

		}
	}


	public void MiddleCast(bool isSecondPart = false){
		currentAction = "MiddleCast";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.middleCast);
			RaycastHit hit;
			float scaleDirectionX = 1;
			if (transform.localScale.x < 0) {
				scaleDirectionX = -1;
			}



			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/frostball");
			Effect effect = effectObject.GetComponent<Effect> ();
			SpellBall spellBall = effectObject.GetComponent<SpellBall> ();
			spellBall.characterAPI = characterAPI;
			spellBall.direction = new Vector3 (scaleDirectionX, shotDirectionVector.y, shotDirectionVector.z);
			spellBall.speed = 0.15f;
			spellBall.time = 2f;
			spellBall.currentTime = 0;
			spellBall.ready = true;
			spellBall.path = "Prefabs/Particles/Elemental/frostball";
			spellBall.ignoreColliders.Add (characterAPI.boxCollider);
			spellBall.ignoreColliders.Add (characterAPI.sphereCollider);
			spellBall.action = (enemyTarget) => {
				enemyTarget.stats.MakeDamage (SkillDamage (skillSettings.damagePercent, Stats.elementalDamageType, MagicEfficiency(100)), Stats.elementalDamageType, true);
				characterAPI.stats.AddMagicEnergyPoints (skillSettings.resourceAdd, true);
			};

			if (stats.isPlayerStats) {
				spellBall.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellBall.selectingLayer = LayerMask.NameToLayer ("Player");
			}
			effect.path = "Prefabs/Particles/Elemental/frostball";
			EffectOptions effectOptions = new EffectOptions ();
			effectOptions.loop = true;
			effectOptions.transformPosition = armsPosition.position;
			effect.StartEffect (effectOptions);
		} else {
			if (characterAPI.movementController.movement.y < 0.8f && characterAPI.movementController.movement.y > -0.8f) {
				anim.Play ("fastCast90", 3);
				shotDirectionVector = new Vector3 (1f, 0, 0);
				StartCoroutine (WaitAnimationAction ("fastCast90", 3));
			} else {
				if (characterAPI.movementController.movement.y > 0.8f) {
					anim.Play ("fastCast135", 3);
					shotDirectionVector = new Vector3 (1f, 1f, 0);
					StartCoroutine (WaitAnimationAction ("fastCast135", 3));
				} else {
					if (characterAPI.movementController.movement.y < -0.8f) {
						anim.Play ("fastCast45", 3);
						shotDirectionVector = new Vector3 (1f, -1f, 0);
						StartCoroutine (WaitAnimationAction ("fastCast45", 3));
					}
				}
			}

		}
	}


	public void SlowCast(bool isSecondPart = false){
		currentAction = "SlowCast";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.slowCast);
			RaycastHit hit;
			float scaleDirectionX = 1;
			if (transform.localScale.x < 0) {
				scaleDirectionX = -1;
			}



			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/lightingball");
			Effect effect = effectObject.GetComponent<Effect> ();
			SpellBall spellBall = effectObject.GetComponent<SpellBall> ();
			spellBall.characterAPI = characterAPI;
			spellBall.direction = new Vector3 (scaleDirectionX, shotDirectionVector.y, shotDirectionVector.z);
			spellBall.speed = 0.15f;
			spellBall.time = 2f;
			spellBall.currentTime = 0;
			spellBall.ready = true;
			spellBall.path = "Prefabs/Particles/Elemental/lightingball";
			spellBall.ignoreColliders.Add (characterAPI.boxCollider);
			spellBall.ignoreColliders.Add (characterAPI.sphereCollider);
			spellBall.action = (enemyTarget) => {
				enemyTarget.stats.MakeDamage (SkillDamage (skillSettings.damagePercent, Stats.elementalDamageType, MagicEfficiency(100)), Stats.elementalDamageType, true);
				characterAPI.stats.AddMagicEnergyPoints (skillSettings.resourceAdd, true);
			};

			if (stats.isPlayerStats) {
				spellBall.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellBall.selectingLayer = LayerMask.NameToLayer ("Player");
			}
			effect.path = "Prefabs/Particles/Elemental/lightingball";
			EffectOptions effectOptions = new EffectOptions ();
			effectOptions.loop = true;
			effectOptions.transformPosition = armsPosition.position;
			effect.StartEffect (effectOptions);
		} else {
			if (characterAPI.movementController.movement.y < 0.8f && characterAPI.movementController.movement.y > -0.8f) {
				anim.Play ("fastCast90", 3);
				shotDirectionVector = new Vector3 (1f, 0, 0);
				StartCoroutine (WaitAnimationAction ("fastCast90", 3));
			} else {
				if (characterAPI.movementController.movement.y > 0.8f) {
					anim.Play ("fastCast135", 3);
					shotDirectionVector = new Vector3 (1f, 1f, 0);
					StartCoroutine (WaitAnimationAction ("fastCast135", 3));
				} else {
					if (characterAPI.movementController.movement.y < -0.8f) {
						anim.Play ("fastCast45", 3);
						shotDirectionVector = new Vector3 (1f, -1f, 0);
						StartCoroutine (WaitAnimationAction ("fastCast45", 3));
					}
				}
			}

		}
	}




	public void ChainLavaBurst(bool isSecondPart = false){
		currentAction = "ChainLavaBurst";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.chainLavaBurst);
			RaycastHit hit;
			float scaleDirectionX = 1;
			if (transform.localScale.x < 0) {
				scaleDirectionX = -1;
			}
			int target;
			if (stats.isPlayerStats) {
				target = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ground"));
			} else {
				target = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ground"));
			}
			Physics.Raycast (
				transform.position, 
				new Vector3(scaleDirectionX, shotDirectionVector.y, shotDirectionVector.z),
				out hit, 
				skillSettings.distance, 
				target /*Enemy*/
			);
			//Physics.Raycast (new Vec);
			if (hit.collider != null) {
				if (hit.collider.gameObject.layer != LayerMask.NameToLayer ("Ground")) {
					Stats.NumberParams damage = SkillDamage (skillSettings.damagePercent, Stats.hybridDamageType, MagicEfficiency(100));
					//Debug.Log (damage);
					CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI> ();
					//targetCharacterAPI.stats.MakeDamage (damage, Stats.hybridDamageType, true);
					//stats.AddMeleeEnergyPoints (9, true);
					List<Collider> spellUserIgnoreColliders = new List<Collider>();
					spellUserIgnoreColliders.Add (characterAPI.boxCollider);
					spellUserIgnoreColliders.Add (characterAPI.sphereCollider);
					LavaBurstPart (targetCharacterAPI, spellUserIgnoreColliders, 100f);


				}
			}
			stats.RemoveMagicEnergyPoints (skillSettings.resourceRemove, true);
		} else {
			if (characterAPI.movementController.movement.y < 0.8f && characterAPI.movementController.movement.y > -0.8f) {
				anim.Play ("chainLavaBurst90", 3);
				shotDirectionVector = new Vector3 (1f, 0, 0);
				StartCoroutine (WaitAnimationAction ("chainLavaBurst90", 3));
				StartCoroutine (StunWhileAnimation ("chainLavaBurst90", 3));
			} else {
				if (characterAPI.movementController.movement.y > 0.8f) {
					anim.Play ("chainLavaBurst135", 3);
					//Debug.Log ("asdf");
					shotDirectionVector = new Vector3 (1f, 1f, 0);
					StartCoroutine (WaitAnimationAction ("chainLavaBurst135", 3));
					StartCoroutine (StunWhileAnimation ("chainLavaBurst135", 3));
				} else {
					if (characterAPI.movementController.movement.y < -0.8f) {
						anim.Play ("chainLavaBurst45", 3);
						shotDirectionVector = new Vector3 (1f, -1f, 0);
						StartCoroutine (WaitAnimationAction ("chainLavaBurst45", 3));
						StartCoroutine (StunWhileAnimation ("chainLavaBurst45", 3));
					}
				}
			}
		}
	}

	void LavaBurstPart(CharacterAPI targetAPI, List<Collider> spellUserIgnoreColliders, float percentAndDamage){
		GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/lavaBurst");
		Effect effect = effectObject.GetComponent<Effect> ();
		effect.path = "Prefabs/Particles/Elemental/lavaBurst";
		EffectOptions effectOptions = new EffectOptions ();
		effect.transform.parent = targetAPI.skills.nullEffectPosition.transform;
		effectOptions.isRandomDuration = false;
		effectOptions.duration = 1.5f;
		//float randomYposition = Random.Range (effect.nullPositionCoords.y - 0.100f, effect.nullPositionCoords.y + 0.100f);
		effectOptions.transformPosition = effect.nullPositionCoords;
		effect.StartEffect (effectOptions);

		GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/lavaBurstHitbox");
		//hitBox.transform.parent = nullSkillPosition.transform;
		SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
		//hitBox.transform.parent = targetAPI.skills.nullEffectPosition.transform;
		hitBox.transform.position =  targetAPI.transform.position + spellHitbox.nullSpellPosition;
		hitBox.transform.localScale = spellHitbox.nullSpellScale;
		spellHitbox.ignoreColliders = spellUserIgnoreColliders;


		HitboxDamagerOptions damagerOptions = new HitboxDamagerOptions();
		damagerOptions.hitBox = hitBox;
		damagerOptions.spellHitbox = spellHitbox;
		damagerOptions.damagePercent = 1.1025641f;
		damagerOptions.damageType = Stats.elementalDamageType;
		damagerOptions.path = "Prefabs/SkillPrefabs/lavaBurstHitbox";
		damagerOptions.efficienty = percentAndDamage;

		StartCoroutine(waitFewFramesAndMakeDamage (damagerOptions));
		StartCoroutine (waitSkillEndAndClearColliders ());

		float randomPercentAndDamage = Random.Range (0, percentAndDamage);
		StartCoroutine (WaitFewSecondsAndTryLavaBurst(1f, targetAPI, randomPercentAndDamage, spellUserIgnoreColliders));
	}

	IEnumerator WaitFewSecondsAndTryLavaBurst(float waitTime, CharacterAPI targetAPI, float percentAndDamage, List<Collider> spellUserIgnoreColliders){
		float randomPercentAndDamage = Random.Range (0, 100.01f);
		if (randomPercentAndDamage <= percentAndDamage) {
			Timer timer = new Timer ();
			timer.SetTimer (waitTime);
			while (!timer.TimeIsOver ()) {
				yield return null;
			}
			int target;
			if (stats.isPlayerStats) {
				target = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ground"));
			} else {
				target = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ground"));
			}
			RaycastHit[] rightHits = Physics.RaycastAll (transform.position, 
                new Vector3 (1, 0, 0), 
                3f, 
				target
			);

			RaycastHit[] leftHits = Physics.RaycastAll (transform.position, 
               new Vector3 (1, 0, 0), 
               3f, 
				target
           );
			List<RaycastHit> hits = new List<RaycastHit> ();
			foreach (RaycastHit hit in rightHits) {
				if (hit.collider.gameObject != targetAPI.gameObject) {
					hits.Add (hit);
				}
			}
			foreach (RaycastHit hit in leftHits) {
				if (hit.collider.gameObject != targetAPI.gameObject) {
					hits.Add (hit);
				}
			}

			if (hits.Count != 0) {
				int hitIndex = Random.Range (1, hits.Count + 1);
				RaycastHit hit = hits [hitIndex - 1];
				CharacterAPI newTargetAPI = hit.collider.gameObject.GetComponent<CharacterAPI> ();

				LavaBurstPart (newTargetAPI, spellUserIgnoreColliders, randomPercentAndDamage);

			}
		}
		yield break;
	}



	public void PhysicalShield(bool isSecondPart = false){
		currentAction = "PhysicalShield";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.physicalShield);
			stats.AddShieldPoints (SkillDamage(skillSettings.restorePercent, Stats.physicalDamageType, MagicEfficiency(100)), Stats.physicalDamageType);
			if (stats.shieldTimer.TimeIsOver()) {
				GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/physicalShield");
				Effect effect = effectObject.GetComponent<Effect> ();
				effect.path = "Prefabs/Particles/Elemental/physicalShield";
				EffectOptions effectOptions = new EffectOptions ();
				effect.transform.parent = nullEffectPosition.transform;
				effectOptions.transformPosition = effect.nullPositionCoords;
				effectOptions.isRandomDuration = false;
				//effectOptions.duration = 1f;
				effectOptions.loop = true;
				effect.StartEffect (effectOptions);
				stats.shieldTimer.SetTimer (skillSettings.existingTime);
				Timer.TimerAction timerAction = () => {
					stats.RemoveShieldPoints (stats.physicalShield, Stats.physicalDamageType);
					ObjectsPool.PushObject ("Prefabs/Particles/Elemental/physicalShield", effectObject);
				};

				StartCoroutine(stats.shieldTimer.ActionAfterTimer(timerAction));
			}else{
				stats.shieldTimer.SetTimer(skillSettings.existingTime);
			}
		} else {
			anim.Play ("castShield", 3);
			StartCoroutine (WaitAnimationAction("castShield", 3));
		}
	}

	public void ElementalShield(bool isSecondPart = false){
		currentAction = "ElementalShield";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.elementalShield);
			stats.AddShieldPoints (SkillDamage(skillSettings.restorePercent, Stats.elementalDamageType, MagicEfficiency(100)), Stats.elementalDamageType);
			if (stats.shieldTimer.TimeIsOver()) {
				GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/elementalShield");
				Effect effect = effectObject.GetComponent<Effect> ();
				effect.path = "Prefabs/Particles/Elemental/elementalShield";
				EffectOptions effectOptions = new EffectOptions ();
				effect.transform.parent = nullEffectPosition.transform;
				effectOptions.transformPosition = effect.nullPositionCoords;
				effectOptions.isRandomDuration = false;
				//effectOptions.duration = 1f;
				effectOptions.loop = true;
				effect.StartEffect (effectOptions);
				stats.shieldTimer.SetTimer (skillSettings.existingTime);
				Timer.TimerAction timerAction = () => {
					stats.RemoveShieldPoints (stats.elementalShield, Stats.elementalDamageType);
					ObjectsPool.PushObject ("Prefabs/Particles/Elemental/elementalShield", effectObject);
				};

				StartCoroutine(stats.shieldTimer.ActionAfterTimer(timerAction));
			}else{
				stats.shieldTimer.SetTimer(skillSettings.existingTime);
			}
		} else {
			anim.Play ("castShield", 3);
			StartCoroutine (WaitAnimationAction("castShield", 3));
		}
	}


	public void HybridShield(bool isSecondPart = false){
		currentAction = "HybridShield";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.hybridShield);
			stats.AddShieldPoints ((SkillDamage(skillSettings.restorePercent, Stats.elementalDamageType, MagicEfficiency(100))), Stats.hybridDamageType);
			stats.AddShieldPoints ((SkillDamage(skillSettings.restorePercent, Stats.physicalDamageType, MagicEfficiency(100))), Stats.hybridDamageType);
			if (stats.shieldTimer.TimeIsOver()) {
				GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/hybridShield");
				Effect effect = effectObject.GetComponent<Effect> ();
				effect.path = "Prefabs/Particles/Elemental/hybridShield";
				EffectOptions effectOptions = new EffectOptions ();
				effect.transform.parent = nullEffectPosition.transform;
				effectOptions.transformPosition = effect.nullPositionCoords;
				effectOptions.isRandomDuration = false;
				//effectOptions.duration = 1f;
				effectOptions.loop = true;
				effect.StartEffect (effectOptions);
				stats.shieldTimer.SetTimer (skillSettings.existingTime);
				Timer.TimerAction timerAction = () => {
					stats.RemoveShieldPoints (stats.hybridShield, Stats.hybridDamageType);
					ObjectsPool.PushObject ("Prefabs/Particles/Elemental/hybridShield", effectObject);
				};

				StartCoroutine(stats.shieldTimer.ActionAfterTimer(timerAction));
			}else{
				stats.shieldTimer.SetTimer(skillSettings.existingTime);
			}
		} else {
			anim.Play ("castShield", 3);
			StartCoroutine (WaitAnimationAction("castShield", 3));
		}
	}


	public void FireNova(bool isSecondPart = false){
		currentAction = "FireNova";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.fireNova);
			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/fireNova");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Elemental/fireNova";
			EffectOptions effectOptions = new EffectOptions ();
			Vector3 effectPosition = nullEffectPosition.transform.position + effect.nullPositionCoords;

			effectOptions.isLocalPosition = false;
			effectOptions.transformPosition = effectPosition;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 1f;
			effect.StartEffect (effectOptions);

			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/novaHitbox");
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.position = nullSkillPosition.transform.position + spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			if(characterAPI.transform.localScale.x < 0){
				spellHitbox.FlipWorld();	
			}
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			spellHitbox.path = "Prefabs/SkillPrefabs/novaHitbox";
			if (stats.isPlayerStats) {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}

			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				//targetAPI.stats.MakeDamage(SkillDamage(2.3717949f, Stats.elementalDamageType), Stats.elementalDamageType, true);
				StartCoroutine(targetAPI.stats.DealDot(SkillDamage(skillSettings.damagePercent, Stats.elementalDamageType, MagicEfficiency(100)), Stats.elementalDamageType, 12, 12));
			};

			StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));
			stats.RemoveMagicEnergyPoints (skillSettings.resourceRemove, true);
		} else {
			if (IsAction ()) {
				anim.Play ("actionFireNova");
				StartCoroutine (WaitAnimationAction ("actionFireNova", 3));
			} else {
				anim.Play ("fireNova");
				StartCoroutine (WaitAnimationAction ("fireNova", 3));
			}
		}
	}

	public void Blizzard(bool isSecondPart = false){
		currentAction = "Blizzard";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.blizzard);
			float blizzardTime = skillSettings.existingTime;
			float slowTime = skillSettings.slowTime;
			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/blizzard");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Elemental/blizzard";
			EffectOptions effectOptions = new EffectOptions ();
			Vector3 effectPosition = nullEffectPosition.transform.position + effect.nullPositionCoords;

			effectOptions.isLocalPosition = false;
			effectOptions.transformPosition = effectPosition;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = blizzardTime;
			effect.StartEffect (effectOptions);

			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/BlizzardHitbox");
			StreamHitbox streamHitbox = hitBox.GetComponent<StreamHitbox> ();
			hitBox.transform.position = nullSkillPosition.transform.position + streamHitbox.nullSpellPosition;
			hitBox.transform.localScale = streamHitbox.nullSpellScale;
			streamHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			streamHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			if (stats.isPlayerStats) {
				streamHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				streamHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}




			streamHitbox.action = (CharacterAPI targetAPI) => {
				targetAPI.stats.GetMovementSlowly(slowTime, 0.3f);
				targetAPI.stats.MakeDamage(SkillDamage (skillSettings.damagePercent, Stats.elementalDamageType, MagicEfficiency(100)), Stats.elementalDamageType, true);
				float random = Random.Range(0, 100.01f);
				if(random <= skillSettings.stunChance){
					targetAPI.stats.IceStun(skillSettings.stunTime);
				}
				Timer slowMovementTimer = new Timer(); 
				slowMovementTimer.SetTimer(slowTime);
				StartCoroutine(slowMovementTimer.ActionAfterTimer(() => {
					if(targetAPI.gameObject.activeInHierarchy){
						streamHitbox.RemoveIgnoreObject(targetAPI.gameObject);
					}
				}));
			};
			streamHitbox.readyToTrigger = true;

			Timer cloudTimer = new Timer ();
			cloudTimer.SetTimer (blizzardTime);
			StartCoroutine(cloudTimer.ActionAfterTimer (() => {
				streamHitbox.Clear();
				ObjectsPool.PushObject("Prefabs/SkillPrefabs/BlizzardHitbox", hitBox);	
			}));
			stats.RemoveMagicEnergyPoints (skillSettings.resourceRemove, true);
		} else {
			if (IsAction ()) {
				anim.Play ("actionFireNova");
				StartCoroutine (WaitAnimationAction ("actionFireNova", 3));
			} else {
				anim.Play ("fireNova");
				StartCoroutine (WaitAnimationAction ("fireNova", 3));
			}
		}
	}

	public void ElementalCone(bool isSecondPart = false){
		currentAction = "ElementalCone";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.elementalCone);
			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/fireKM");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Elemental/fireKM";
			EffectOptions effectOptions = new EffectOptions ();
			Vector3 effectPosition = nullEffectPosition.transform.position + effect.nullPositionCoords;
			if(characterAPI.transform.localScale.x < 0){
				effectPosition = nullEffectPosition.transform.position +
					new Vector3 (effect.nullPositionCoords.x * -1, effect.nullPositionCoords.y, effect.nullPositionCoords.z);
				effectOptions.revertRotationY = true;	

			}

			effectOptions.isLocalPosition = false;
			effectOptions.transformPosition = effectPosition;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 0.5f;
			effect.StartEffect (effectOptions);

			int target;
			if (stats.isPlayerStats) {
				target = LayerMask.NameToLayer ("Enemy");
			} else {
				target = LayerMask.NameToLayer ("Player");
			}

			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/elementalConeHitbox");
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.position = nullSkillPosition.transform.position + spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			if(characterAPI.transform.localScale.x < 0){
				//Debug.Log ("lols");
				spellHitbox.FlipWorld();	
			}
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			spellHitbox.selectingLayer = target;
			spellHitbox.path = "Prefabs/SkillPrefabs/elementalConeHitbox";


			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.physicalDamageType, MagicEfficiency(100)), Stats.physicalDamageType, true);
			};

			StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));
			stats.RemoveMagicEnergyPoints (skillSettings.resourceRemove, true);
		} else {
			if (IsAction ()) {
				anim.Play ("actionElementalCone", 3);
				StartCoroutine (WaitAnimationAction ("actionElementalCone", 3));
			} else {
				anim.Play ("elementalCone", 3);
				StartCoroutine (WaitAnimationAction ("elementalCone", 3));
			}
		}
	}




	public void FireWall(bool isSecondPart = false){
		currentAction = "FireWall";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.fireWall);
			RaycastHit hit;
			float scaleDirectionX = 1;
			if (transform.localScale.x < 0) {
				scaleDirectionX = -1;
			}

			int target;
			if (stats.isPlayerStats) {
				target = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ground"));
			} else {
				target = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ground"));
			}
			Physics.Raycast (
				transform.position, 
				new Vector3(scaleDirectionX, shotDirectionVector.y, shotDirectionVector.z),
				out hit, 
				skillSettings.distance, 
				target /*Enemy*/
			);

			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/FireWall");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Elemental/FireWall";
			EffectOptions effectOptions = new EffectOptions ();
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 0.5f;



			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/wallHitbox");
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			if(characterAPI.transform.localScale.x < 0){
				spellHitbox.FlipWorld();	
			}
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			spellHitbox.path = "Prefabs/SkillPrefabs/wallHitbox";
			if (stats.isPlayerStats) {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}

			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.elementalDamageType, MagicEfficiency(100)), Stats.elementalDamageType, true);
			};


			if (hit.collider != null) {
				if (hit.collider.gameObject.layer != LayerMask.NameToLayer ("Ground")) {
					CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI> ();
					effectOptions.transformPosition = effect.nullPositionCoords + new Vector3 (targetCharacterAPI.skills.nullEffectPosition.transform.position.x, nullEffectPosition.transform.position.y, nullEffectPosition.transform.position.z);
					hitBox.transform.position = spellHitbox.nullSpellPosition + new Vector3 (targetCharacterAPI.skills.nullEffectPosition.transform.position.x, nullEffectPosition.transform.position.y, nullEffectPosition.transform.position.z);
				} else {
					effectOptions.transformPosition = effect.nullPositionCoords + nullEffectPosition.transform.position;
					hitBox.transform.position = spellHitbox.nullSpellPosition + nullEffectPosition.transform.position;
				}
			} else {
				effectOptions.transformPosition = effect.nullPositionCoords + nullEffectPosition.transform.position;
				hitBox.transform.position = spellHitbox.nullSpellPosition + nullEffectPosition.transform.position;
			}

			StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));
			effect.StartEffect (effectOptions);
			stats.RemoveMagicEnergyPoints (skillSettings.resourceRemove, true);
		} else {
			if (characterAPI.movementController.movement.y < 0.8f && characterAPI.movementController.movement.y > -0.8f) {
				if (IsAction ()) {
					anim.Play ("actionElementalCone", 3);
					StartCoroutine (WaitAnimationAction ("actionElementalCone", 3));
				} else {
					anim.Play ("elementalCone", 3);
					StartCoroutine (WaitAnimationAction ("elementalCone", 3));
				}
				shotDirectionVector = new Vector3 (1f, 0, 0);

			} else {
				if (characterAPI.movementController.movement.y > 0.8f) {
					if (IsAction ()) {
						anim.Play ("actionElementalCone135", 3);
						StartCoroutine (WaitAnimationAction ("actionElementalCone135", 3));
					} else {
						anim.Play ("elementalCone135", 3);
						StartCoroutine (WaitAnimationAction ("elementalCone135", 3));
					}
					shotDirectionVector = new Vector3 (1f, 1f, 0);
				} else {
					if (characterAPI.movementController.movement.y < -0.8f) {
						if (IsAction ()) {
							anim.Play ("actionElementalCone45", 3);
							StartCoroutine (WaitAnimationAction ("actionElementalCone45", 3));
						} else {
							anim.Play ("elementalCone45", 3);
							StartCoroutine (WaitAnimationAction ("elementalCone45", 3));
						}
						shotDirectionVector = new Vector3 (1f, -1f, 0);
					}
				}
			}

		}
	}


	public void IceWave(bool isSecondPart = false){
		currentAction = "IceWave";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.iceWave);
			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/iceWave");
			Effect effect = effectObject.GetComponent<Effect> ();
			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/MeleeWaveHitbox");
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();



			RaycastHit hit;
			Physics.Raycast (transform.position, new Vector3(transform.localScale.x, 0, 0), out hit, effect.distance, 1 << 8/*Ground*/);
			Vector3 currentEffectNullPosition = effect.nullPositionCoords;
			Vector3 currentHitboxNullPosition = spellHitbox.nullSpellPosition;


			if (hit.collider != null) {
				float newRadius = (hit.distance * effect.nullRadius) / effect.distance;
				effect.SetMeleeWaveRadius (newRadius);
				currentEffectNullPosition = new Vector3 ((currentEffectNullPosition.x / effect.nullRadius) * newRadius, currentEffectNullPosition.y, currentEffectNullPosition.z);

				BoxCollider hitboxCollider = (BoxCollider)(spellHitbox.collider);
				hitboxCollider.size = new Vector3 ((spellHitbox.nullColliderSize.x/ effect.nullRadius)*newRadius, spellHitbox.nullColliderSize.y, spellHitbox.nullColliderSize.z);
				hitboxCollider.center = new Vector3 ((spellHitbox.nullColliderCenter.x/ effect.nullRadius)*newRadius, spellHitbox.nullColliderCenter.y, spellHitbox.nullColliderCenter.z);
				currentHitboxNullPosition = new Vector3((spellHitbox.nullSpellPosition.x/ effect.nullRadius)*newRadius, spellHitbox.nullSpellPosition.y, spellHitbox.nullSpellPosition.z);
				//Debug.Log (currentHitboxNullPosition);
			} else {
				effect.SetMeleeWaveRadius (effect.nullRadius);	
				BoxCollider hitboxCollider = (BoxCollider)(spellHitbox.collider);
				hitboxCollider.size = spellHitbox.nullColliderSize;
				hitboxCollider.center = spellHitbox.nullColliderCenter;
			}
			effect.path = "Prefabs/Particles/Melee/meleeWave";
			EffectOptions effectOptions = new EffectOptions ();
			Vector3 effectPosition = nullEffectPosition.transform.position + currentEffectNullPosition;
			hitBox.transform.position = nullEffectPosition.transform.position + currentHitboxNullPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;

			if(characterAPI.transform.localScale.x < 0){
				effectPosition = nullEffectPosition.transform.position +
					new Vector3 (currentEffectNullPosition.x * -1, currentEffectNullPosition.y, currentEffectNullPosition.z);
				effectOptions.revertRotationY = true;
				spellHitbox.currentNullSpellPosition = currentHitboxNullPosition;
				spellHitbox.FlipWorld(true);

			}
			effectOptions.transformPosition = effectPosition;
			effectOptions.isRandomDuration = false;
			effectOptions.isLocalPosition = false;
			effectOptions.duration = 0.8f;
			effect.StartEffect (effectOptions);



			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			if (stats.isPlayerStats) {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}
			spellHitbox.path = "Prefabs/SkillPrefabs/MeleeWaveHitbox";


			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.physicalDamageType, MagicEfficiency(100)), Stats.physicalDamageType, true);
				targetAPI.stats.GetMovementSlowly(skillSettings.slowTime, skillSettings.slowSpeed);
			};
			StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));
			stats.RemoveMagicEnergyPoints (skillSettings.resourceRemove, true);
		} else {
			if (IsAction ()) {
				anim.Play ("actionElementalCone");
				StartCoroutine (WaitAnimationAction ("actionElementalCone", 3));
			} else {
				anim.Play ("elementalCone");
				StartCoroutine (WaitAnimationAction ("elementalCone", 3));
			}
		}
	}



	public void EarthDisruption(bool isSecondPart = false){
		currentAction = "EarthDisruption";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.earthDisruption);
			RaycastHit hit;
			float scaleDirectionX = 1;
			if (transform.localScale.x < 0) {
				scaleDirectionX = -1;
			}

			int target;
			if (stats.isPlayerStats) {
				target = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ground"));
			} else {
				target = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ground"));
			}
			Physics.Raycast (
				transform.position, 
				new Vector3(scaleDirectionX, shotDirectionVector.y, shotDirectionVector.z),
				out hit, 
				skillSettings.distance, 
				target /*Enemy*/
			);

			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/Spikes");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Elemental/Spikes";
			EffectOptions effectOptions = new EffectOptions ();
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 0.5f;



			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/spikesHitbox");
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			if(characterAPI.transform.localScale.x < 0){
				spellHitbox.FlipWorld();	
			}
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			spellHitbox.path = "Prefabs/SkillPrefabs/spikesHitbox";
			if (stats.isPlayerStats) {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}

			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.elementalDamageType, MagicEfficiency(100)), Stats.elementalDamageType, true);

				int randomVectorValue = Random.Range (50, 75);
				if(targetAPI.transform.position.x > characterAPI.transform.position.x){
					targetAPI.movementController.NullSpeedWhenGround ();
					targetAPI.movementController.AddForceWithoutAnimation (new Vector3 (randomVectorValue, 250, 0));
				}else{
					targetAPI.movementController.NullSpeedWhenGround ();
					targetAPI.movementController.AddForceWithoutAnimation (new Vector3 (randomVectorValue * -1, 250, 0));
				}
				targetAPI.stats.FullStun(skillSettings.stunTime, false);
			};


			if (hit.collider != null) {
				if (hit.collider.gameObject.layer != LayerMask.NameToLayer ("Ground")) {
					CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI> ();
					effectOptions.transformPosition = effect.nullPositionCoords + new Vector3 (targetCharacterAPI.skills.nullEffectPosition.transform.position.x, nullEffectPosition.transform.position.y, nullEffectPosition.transform.position.z);
					hitBox.transform.position = spellHitbox.nullSpellPosition + new Vector3 (targetCharacterAPI.skills.nullEffectPosition.transform.position.x, nullEffectPosition.transform.position.y, nullEffectPosition.transform.position.z);
				} else {
					effectOptions.transformPosition = effect.nullPositionCoords + nullEffectPosition.transform.position;
					hitBox.transform.position = spellHitbox.nullSpellPosition + nullEffectPosition.transform.position;
				}
			} else {
				effectOptions.transformPosition = effect.nullPositionCoords + nullEffectPosition.transform.position;
				hitBox.transform.position = spellHitbox.nullSpellPosition + nullEffectPosition.transform.position;
			}

			StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));
			effect.StartEffect (effectOptions);
			stats.RemoveMagicEnergyPoints (skillSettings.resourceRemove, true);
		} else {
			if (characterAPI.movementController.movement.y < 0.8f && characterAPI.movementController.movement.y > -0.8f) {
				if (IsAction ()) {
					anim.Play ("actionElementalCone", 3);
					StartCoroutine (WaitAnimationAction ("actionElementalCone", 3));
				} else {
					anim.Play ("elementalCone", 3);
					StartCoroutine (WaitAnimationAction ("elementalCone", 3));
				}
				shotDirectionVector = new Vector3 (1f, 0, 0);

			} else {
				if (characterAPI.movementController.movement.y > 0.8f) {
					if (IsAction ()) {
						anim.Play ("actionElementalCone135", 3);
						StartCoroutine (WaitAnimationAction ("actionElementalCone135", 3));
					} else {
						anim.Play ("elementalCone135", 3);
						StartCoroutine (WaitAnimationAction ("elementalCone135", 3));
					}
					shotDirectionVector = new Vector3 (1f, 1f, 0);
				} else {
					if (characterAPI.movementController.movement.y < -0.8f) {
						if (IsAction ()) {
							anim.Play ("actionElementalCone45", 3);
							StartCoroutine (WaitAnimationAction ("actionElementalCone45", 3));
						} else {
							anim.Play ("elementalCone45", 3);
							StartCoroutine (WaitAnimationAction ("elementalCone45", 3));
						}
						shotDirectionVector = new Vector3 (1f, -1f, 0);
					}
				}
			}

		}
	}


	public void IceWall(bool isSecondPart = false){
		currentAction = "IceWall";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.iceWall);
			RaycastHit hit;
			float scaleDirectionX = 1;
			if (transform.localScale.x < 0) {
				scaleDirectionX = -1;
			}

			int target;
			if (stats.isPlayerStats) {
				target = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ground"));
			} else {
				target = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ground"));
			}
			Physics.Raycast (
				transform.position, 
				new Vector3(scaleDirectionX, shotDirectionVector.y, shotDirectionVector.z),
				out hit, 
				skillSettings.distance, 
				target /*Enemy*/
			);

			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/ConeOfCold");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Elemental/ConeOfCold";
			EffectOptions effectOptions = new EffectOptions ();
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 0.5f;



			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/wallHitbox");
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			if(characterAPI.transform.localScale.x < 0){
				spellHitbox.FlipWorld();	
			}
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			spellHitbox.path = "Prefabs/SkillPrefabs/wallHitbox";
			if (stats.isPlayerStats) {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Enemy");
			} else {
				spellHitbox.selectingLayer = LayerMask.NameToLayer ("Player");
			}

			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.elementalDamageType, MagicEfficiency(100)), Stats.elementalDamageType, true);
				targetAPI.stats.GetMovementSlowly(skillSettings.slowTime, skillSettings.slowSpeed);
			};


			if (hit.collider != null) {
				if (hit.collider.gameObject.layer != LayerMask.NameToLayer ("Ground")) {
					CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI> ();
					effectOptions.transformPosition = effect.nullPositionCoords + new Vector3 (targetCharacterAPI.skills.nullEffectPosition.transform.position.x, nullEffectPosition.transform.position.y, nullEffectPosition.transform.position.z);
					hitBox.transform.position = spellHitbox.nullSpellPosition + new Vector3 (targetCharacterAPI.skills.nullEffectPosition.transform.position.x, nullEffectPosition.transform.position.y, nullEffectPosition.transform.position.z);
				} else {
					effectOptions.transformPosition = effect.nullPositionCoords + nullEffectPosition.transform.position;
					hitBox.transform.position = spellHitbox.nullSpellPosition + nullEffectPosition.transform.position;
				}
			} else {
				effectOptions.transformPosition = effect.nullPositionCoords + nullEffectPosition.transform.position;
				hitBox.transform.position = spellHitbox.nullSpellPosition + nullEffectPosition.transform.position;
			}

			StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));
			effect.StartEffect (effectOptions);
			stats.RemoveMagicEnergyPoints (skillSettings.resourceRemove, true);
		} else {
			if (characterAPI.movementController.movement.y < 0.8f && characterAPI.movementController.movement.y > -0.8f) {
				if (IsAction ()) {
					anim.Play ("actionElementalCone", 3);
					StartCoroutine (WaitAnimationAction ("actionElementalCone", 3));
				} else {
					anim.Play ("elementalCone", 3);
					StartCoroutine (WaitAnimationAction ("elementalCone", 3));
				}
				shotDirectionVector = new Vector3 (1f, 0, 0);

			} else {
				if (characterAPI.movementController.movement.y > 0.8f) {
					if (IsAction ()) {
						anim.Play ("actionElementalCone135", 3);
						StartCoroutine (WaitAnimationAction ("actionElementalCone135", 3));
					} else {
						anim.Play ("elementalCone135", 3);
						StartCoroutine (WaitAnimationAction ("elementalCone135", 3));
					}
					shotDirectionVector = new Vector3 (1f, 1f, 0);
				} else {
					if (characterAPI.movementController.movement.y < -0.8f) {
						if (IsAction ()) {
							anim.Play ("actionElementalCone45", 3);
							StartCoroutine (WaitAnimationAction ("actionElementalCone45", 3));
						} else {
							anim.Play ("elementalCone45", 3);
							StartCoroutine (WaitAnimationAction ("elementalCone45", 3));
						}
						shotDirectionVector = new Vector3 (1f, -1f, 0);
					}
				}
			}

		}
	}


	public void IceStun(bool isSecondPart = false){
		currentAction = "IceStun";
		if (isSecondPart) {
			SkillSettings skillSettings = GetSkillSetting (SkillSettingsSet.SkillID.iceStun);
			RaycastHit hit;
			float scaleDirectionX = 1;
			if (transform.localScale.x < 0) {
				scaleDirectionX = -1;
			}

			int target;
			if (stats.isPlayerStats) {
				target = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ground"));
			} else {
				target = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ground"));
			}
			Physics.Raycast (
				transform.position, 
				new Vector3(scaleDirectionX, shotDirectionVector.y, shotDirectionVector.z),
				out hit, 
				skillSettings.distance, 
				target /*Enemy*/
			);

			if (hit.collider != null) {
				if (hit.collider.gameObject.layer != LayerMask.NameToLayer ("Ground")) {
					CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI> ();
					targetCharacterAPI.stats.MakeDamage(SkillDamage(skillSettings.damagePercent, Stats.elementalDamageType, MagicEfficiency(100)), Stats.elementalDamageType, true);
					targetCharacterAPI.stats.IceStun(skillSettings.stunTime);
				} 
			} 
			stats.RemoveMagicEnergyPoints (skillSettings.resourceRemove, true);
		} else {
			if (characterAPI.movementController.movement.y < 0.8f && characterAPI.movementController.movement.y > -0.8f) {
				if (IsAction ()) {
					anim.Play ("actionElementalCone", 3);
					StartCoroutine (WaitAnimationAction ("actionElementalCone", 3));
				} else {
					anim.Play ("elementalCone", 3);
					StartCoroutine (WaitAnimationAction ("elementalCone", 3));
				}
				shotDirectionVector = new Vector3 (1f, 0, 0);

			} else {
				if (characterAPI.movementController.movement.y > 0.8f) {
					if (IsAction ()) {
						anim.Play ("actionElementalCone135", 3);
						StartCoroutine (WaitAnimationAction ("actionElementalCone135", 3));
					} else {
						anim.Play ("elementalCone135", 3);
						StartCoroutine (WaitAnimationAction ("elementalCone135", 3));
					}
					shotDirectionVector = new Vector3 (1f, 1f, 0);
				} else {
					if (characterAPI.movementController.movement.y < -0.8f) {
						if (IsAction ()) {
							anim.Play ("actionElementalCone45", 3);
							StartCoroutine (WaitAnimationAction ("actionElementalCone45", 3));
						} else {
							anim.Play ("elementalCone45", 3);
							StartCoroutine (WaitAnimationAction ("elementalCone45", 3));
						}
						shotDirectionVector = new Vector3 (1f, -1f, 0);
					}
				}
			}

		}
	}




	bool IsAction(){
		int hash = anim.GetCurrentAnimatorStateInfo (0).shortNameHash;
		bool isAction = false;
		if (actionAnimationHashes.FindIndex (h => h == hash) != -1) {
			isAction = true;
		}
		return isAction;
	}




	float TestSkillDamagePercent(int fd/*finalDamage*/){
		/*withComplexity 600*/
		int wd = 60;/*weapon damage*/
		float ap = 0.3f; /*armorPercent*/
		float dp = 0.5f; /*damagePercent*/
		//return (fd - wd - (wd * dp) - (wd * ap) - (wd * dp * ap)) / (wd + (wd * ap));
		return (fd - wd - (wd * dp))/(wd + (wd * dp));
	}


	IEnumerator waitSkillEndAndClearColliders(){
		while (skillProcessCount != 0) {
			yield return null;
		}
		ignoreColliders.Clear ();
		yield break;
	}

	IEnumerator waitFewFramesAndMakeDamage(HitboxDamagerOptions damagerOptions){
		skillProcessCount = skillProcessCount + 1;
		int zero = 0;
		while (zero < 4) {
			if (damagerOptions.spellHitbox.objects.Count == 0) {
				zero += 1;
				yield return null;
			} else {
				foreach (Collider col in ignoreColliders) {
					damagerOptions.spellHitbox.ignoreColliders.Add (col);
				}

				foreach (GameObject obj in damagerOptions.spellHitbox.GetObjectsWithoutIgnoredColliders()) {
					//Debug.Log ("Lol");
					CharacterAPI enemyCharacterAPI = obj.GetComponent<CharacterAPI> ();
					if (damagerOptions.damageType == Stats.hybridDamageType) {
						enemyCharacterAPI.stats.MakeDamage (SkillDamage (damagerOptions.damagePercent, Stats.physicalDamageType, damagerOptions.efficienty), Stats.physicalDamageType, true);
						enemyCharacterAPI.stats.MakeDamage (SkillDamage (damagerOptions.damagePercent, Stats.elementalDamageType, damagerOptions.efficienty), Stats.elementalDamageType, true);
					} else {
						enemyCharacterAPI.stats.MakeDamage (SkillDamage (damagerOptions.damagePercent, damagerOptions.damageType, damagerOptions.efficienty), damagerOptions.damageType, true);
					}

					if (damagerOptions.withStun) {
						enemyCharacterAPI.stats.FullStun (damagerOptions.stunTime);
					}
					if (damagerOptions.action != null) {
						damagerOptions.action (enemyCharacterAPI);
					}
					ignoreColliders.Add (enemyCharacterAPI.sphereCollider);
					damagerOptions.spellHitbox.ignoreColliders.Add (enemyCharacterAPI.sphereCollider);
					ignoreColliders.Add (enemyCharacterAPI.boxCollider);
					damagerOptions.spellHitbox.ignoreColliders.Add (enemyCharacterAPI.boxCollider);
				}
				zero = 4;
			}
		}
		damagerOptions.spellHitbox.Clear ();
		ObjectsPool.PushObject (damagerOptions.path, damagerOptions.hitBox);
		skillProcessCount = skillProcessCount - 1;
		yield break;
	}
		


	IEnumerator StunWhileAnimation(string animationName, int layer = 1){
		stats.withoutControl = true;
		for(int i = 0; i < 1; i++){
			yield return null;
			stats.withoutControl = true;
		}
		while (anim.GetCurrentAnimatorStateInfo (layer).shortNameHash == Animator.StringToHash (animationName)) {
			stats.withoutControl = true;
			//Debug.Log ("lol");
			yield return null;
		}
		stats.withoutControl = false;
		yield break;

	}

	IEnumerator ToGround(){
		Vector3 newVelocity = new Vector3 (0, -10, 0);
		while (!characterAPI.movementController.isGrounded) {
			characterAPI.rigidbody.velocity = newVelocity;
			yield return null;
		}
		yield break;
	}

	IEnumerator WaitGroundedAndStartThunderClap(){
		while (!characterAPI.movementController.isGrounded) {
			yield return null;
		}
		anim.Play ("thunderClap");
		StartCoroutine (StunWhileAnimation("thunderClap"));
	}


	public void SetLastFrameAnimation(){
		anim.SetBool ("IsLastFrame", true);
	}
	IEnumerator WaitAnimationAction(string animationName, int layer){
		anim.SetBool("AnimationAction", true);
		for(int i = 0; i < 1; i++){
			yield return null;
		}

		while(anim.GetCurrentAnimatorStateInfo (layer).IsName(animationName)){
			yield return null;
		}

		anim.SetBool("AnimationAction", false);
		yield break;
	}

	public IEnumerator WaitAnimationActionAndStartEvent(string animationName, int layer, AfterAnimationAction action, List<object> afterActionParams){
		anim.SetBool("AnimationAction", true);
		for(int i = 0; i < 1; i++){
			yield return null;
		}

		while(anim.GetCurrentAnimatorStateInfo (layer).IsName(animationName)  && !anim.GetBool("IsLastFrame")){
			yield return null;
		}

		anim.SetBool("AnimationAction", false);
		anim.SetBool ("IsLastFrame", false);
		action (afterActionParams);
		yield break;
	}
}
