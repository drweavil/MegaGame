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
	private List<Collider> ignoreColliders = new List<Collider>();
	private int skillProcessCount = 0;
	public CharacterAPI characterAPI;

	private string[] names = new string[]{ "Run", "jumpStart", "jumpEnd", "jumpIdle" };
	private List<int> actionAnimationHashes = new List<int>();

	private Vector3 shotDirectionVector;

	private Effect shieldEffect;


	void Awake(){
		foreach (string name in names) {
			actionAnimationHashes.Add (Animator.StringToHash(name));
		}
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Z)) {
			Debug.Log(TestSkillDamagePercent (300));
		}
	}

	public int SkillDamage(float percent, int damageType, float efficiency = 100f){
		int damage = 0;
		int wd = stats.weaponDamage;
		float ap = 0.3f; /*average enemy armor*/
		float dp = 0;
		if (damageType == Stats.elementalDamageType) {
			dp = stats.elementalDamage / 100f;
		} else {
			dp = stats.physicalDamage / 100f;
		}
		//damage = (int)(System.Math.Round (2.5f * stats.weaponDamage * percent) + damageType);

		damage = (int)((wd * percent) + wd + (wd * dp) + (wd * ap * percent) + (wd * ap) + (wd * dp * ap));
		damage = (int)(damage * (efficiency/100));
		if (damage == 0) {
			damage = 1;
		}
		damage = TryCriticalDamage (damage);
		return damage;
	}
		
	int TryCriticalDamage(int damage){
		int finalDamage = damage;
		float random = Random.Range (0f, 100.01f);
		if (random <= stats.critical) {
			finalDamage = damage * 2;
		}
		return finalDamage;
	}

	bool IsCritical(){
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
			RaycastHit hit;
			Physics.Raycast (transform.position, new Vector3(transform.localScale.x, 0, 0), out hit, 0.8f, 1 << 9/*Enemy*/);
			if (hit.collider != null) {
				int damage = SkillDamage (-0.2179487f, Stats.physicalDamageType);
				//Debug.Log (damage);
				CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI>();
				targetCharacterAPI.stats.MakeDamage(damage, Stats.physicalDamageType,  true);
				stats.AddMeleeEnergyPoints (9, true);

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
			RaycastHit hit;
			Physics.Raycast (transform.position, new Vector3(transform.localScale.x, 0, 0), out hit, 0.8f, 1 << 9/*Enemy*/);
			if (hit.collider != null) {
				int damage = SkillDamage (1.064103f, Stats.physicalDamageType);
				//Debug.Log (damage);
				CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI>();
				targetCharacterAPI.stats.MakeDamage(damage, Stats.physicalDamageType,  true);
				stats.AddMeleeEnergyPoints (15, true);

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
			RaycastHit hit;
			Physics.Raycast (transform.position, new Vector3(transform.localScale.x, 0, 0), out hit, 0.8f, 1 << 9/*Enemy*/);
			if (hit.collider != null) {
				int damage = SkillDamage (2.346154f, Stats.physicalDamageType);
				//Debug.Log (damage);
				CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI>();
				targetCharacterAPI.stats.MakeDamage(damage, Stats.physicalDamageType,  true);
				stats.AddMeleeEnergyPoints (20, true);

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
			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Melee/meleeCone");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Melee/meleeCone";
			EffectOptions effectOptions = new EffectOptions ();
			//effect.transform.parent = nullEffectPosition.transform;
			Vector3 effectPosition = nullEffectPosition.transform.position + effect.nullPositionCoords;
			if(characterAPI.transform.localScale.x < 0){
				//Debug.Log ("lols");
				effectPosition = nullEffectPosition.transform.position +
				new Vector3 (effect.nullPositionCoords.x * -1, effect.nullPositionCoords.y, effect.nullPositionCoords.z);
				effectOptions.revertRotationY = true;	

			}

			effectOptions.isLocalPosition = false;
			effectOptions.transformPosition = effectPosition;
			effectOptions.isRandomDuration = false;
			effectOptions.duration = 0.5f;
			effect.StartEffect (effectOptions);

			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/MeleeConeHitbox");
			//hitBox.transform.parent = nullSkillPosition.transform;
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.position = nullSkillPosition.transform.position + spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			if(characterAPI.transform.localScale.x < 0){
				//Debug.Log ("lols");
				spellHitbox.FlipWorld();	
			}
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());


			HitboxDamagerOptions damagerOptions = new HitboxDamagerOptions();
			damagerOptions.hitBox = hitBox;
			damagerOptions.spellHitbox = spellHitbox;
			damagerOptions.damagePercent = 4.910256f;
			damagerOptions.damageType = Stats.physicalDamageType;
			damagerOptions.path = "Prefabs/SkillPrefabs/MeleeConeHitbox";
			StartCoroutine(waitFewFramesAndMakeDamage (damagerOptions));
			StartCoroutine (waitSkillEndAndClearColliders ());


		} else {
			if (IsAction ()) {
				anim.Play ("actionMeleeCone");
			} else {
				anim.Play ("meleeCone");	
			}
		}
	}

	public void CirclePunch(int partNumber = 0){
		currentAction = "CirclePunch";
		switch(partNumber){
		case 0:
			anim.Play ("circlePunch");

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
			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/CirclePunchHitbox");
			hitBox.transform.parent = nullSkillPosition.transform;
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.localPosition = spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());

			HitboxDamagerOptions damagerOptions = new HitboxDamagerOptions();
			damagerOptions.hitBox = hitBox;
			damagerOptions.spellHitbox = spellHitbox;
			damagerOptions.damagePercent = 1.705128f;
			damagerOptions.damageType = Stats.physicalDamageType;
			damagerOptions.path = "Prefabs/SkillPrefabs/CirclePunchHitbox";
			StartCoroutine(waitFewFramesAndMakeDamage (damagerOptions));
			break;
		case 2:
			GameObject hitBox2 = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/CirclePunchHitbox");
			hitBox2.transform.parent = nullSkillPosition.transform;
			SpellHitbox spellHitbox2 = hitBox2.GetComponent<SpellHitbox> ();
			hitBox2.transform.position = spellHitbox2.nullSpellPosition;
			hitBox2.transform.localScale = spellHitbox2.nullSpellScale;
			spellHitbox2.Flip();
			spellHitbox2.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox2.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());


			HitboxDamagerOptions damagerOptions2 = new HitboxDamagerOptions();
			damagerOptions2.hitBox = hitBox2;
			damagerOptions2.spellHitbox = spellHitbox2;
			damagerOptions2.damagePercent = 1.705128f;
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


			HitboxDamagerOptions damagerOptions = new HitboxDamagerOptions();
			damagerOptions.hitBox = hitBox;
			damagerOptions.spellHitbox = spellHitbox;
			damagerOptions.damagePercent = 2.98718f;
			damagerOptions.damageType = Stats.physicalDamageType;
			damagerOptions.path = "Prefabs/SkillPrefabs/MeleeWaveHitbox";

			StartCoroutine(waitFewFramesAndMakeDamage (damagerOptions));
			StartCoroutine (waitSkillEndAndClearColliders ());


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


		GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/streamHitbox");
		StreamHitbox streamHitbox = hitBox.GetComponent<StreamHitbox> ();
		hitBox.transform.parent = nullSkillPosition.transform;
		//SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
		hitBox.transform.localPosition = streamHitbox.nullSpellPosition;
		hitBox.transform.localScale = streamHitbox.nullSpellScale;
		streamHitbox.ignoreColliders.Add (characterAPI.sphereCollider);
		streamHitbox.ignoreColliders.Add (characterAPI.boxCollider);

		streamHitbox.characterAPI = characterAPI;
		streamHitbox.damagePercent = 2.346154f;
		streamHitbox.damageType = Stats.physicalDamageType;
		streamHitbox.readyToTrigger = true;

		StartCoroutine (ChargeMove(hitBox, streamHitbox));
	}


	public void ThunderClap(bool isSecondPart = false){
		currentAction = "ThunderClap";
		if (isSecondPart) {
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


			StartCoroutine (thunderClapHitbox.MakeDamageAndThrow ());

		} else {
			StartCoroutine (ToGround ());
			StartCoroutine (WaitGroundedAndStartThunderClap ());
		}
	}


	public void BlackHole(bool isSecondPart){
		currentAction = "BlackHole";
		if (isSecondPart) {
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
			effectOptions.duration = 2f;
			effect.StartEffect (effectOptions);

			GameObject hitBox = ObjectsPool.PullObject ("Prefabs/SkillPrefabs/blackHoleHitbox");
			SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.position = nullSkillPosition.transform.position + spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = spellHitbox.nullSpellScale;
			if(characterAPI.transform.localScale.x < 0){
				spellHitbox.FlipWorld();	
			}
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<SphereCollider> ());
			spellHitbox.ignoreColliders.Add (gameObject.GetComponent<BoxCollider> ());
			spellHitbox.path = "Prefabs/SkillPrefabs/blackHoleHitbox";


			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				targetAPI.stats.MakeDamage(SkillDamage(-0.3717949f, Stats.physicalDamageType), Stats.physicalDamageType, true);
				StartCoroutine(targetAPI.movementController.MovementToObjectWithTimer(2f, spellHitbox.centerTransform, new Vector2(5, 5f)));
			};

			StartCoroutine(spellHitbox.MakeObjectsAction(spellHitboxAction));

		} else {
			if (IsAction ()) {
				anim.Play ("actionBlackHole", 1);
			} else {
				anim.Play ("blackHole", 1);
			}
			StartCoroutine (WaitAnimationAction ("blackHole", 1));
			//StartCoroutine(StunWhileAnimation("blackHole", 1));
		}
	}



	public void SiphonLife(bool isSecondPart = false){
		currentAction = "SiphonLife";
		if (isSecondPart) {
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


			SpellHitbox.ObjectsAction spellHitboxAction = (CharacterAPI targetAPI) => {
				int damage = SkillDamage(-0.8589743f, Stats.physicalDamageType);
				targetAPI.stats.MakeDamage(damage, Stats.physicalDamageType, true);
				stats.RestoreHealth(SkillDamage(2.346154f, Stats.physicalDamageType));

				if(IsCritical()){
					stats.RestoreHealth(damage);
				}
				//StartCoroutine(targetAPI.movementController.MovementToObjectWithTimer(2f, spellHitbox.centerTransform, new Vector2(5, 5f)));
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









	/************************************************* Fire *********************************************/
	public void FastShot(bool isSecondPart = false){
		currentAction = "FastShot";
		if (isSecondPart) {
			RaycastHit hit;
			float scaleDirectionX = 1;
			if (transform.localScale.x < 0) {
				scaleDirectionX = -1;
			}
			Physics.Raycast (
				transform.position, 
				new Vector3(scaleDirectionX, shotDirectionVector.y, shotDirectionVector.z),
				out hit, 
				8f, 
				(1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ground")) /*Enemy*/
			);
			//Physics.Raycast (new Vec);
			if (hit.collider != null) {
				if (hit.collider.gameObject.layer != LayerMask.NameToLayer ("Ground")) {
					int damage = SkillDamage (-0.2179487f, Stats.hybridDamageType);
					//Debug.Log (damage);
					CharacterAPI targetCharacterAPI = hit.collider.gameObject.GetComponent<CharacterAPI> ();
					targetCharacterAPI.stats.MakeDamage (damage, Stats.hybridDamageType, true);
					stats.AddMeleeEnergyPoints (9, true);

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


	private Vector3 aimCoord;
	public void Grenade(Vector3 grenadeCoord){
		aimCoord = grenadeCoord;
		GrenadeLaunch ();
		//Debug.Log ("lol");
	}

	public void GrenadeLaunch(bool isSecondPart = false){
		currentAction = "GrenadeLaunch";
		//Debug.Log (isSecondPart);
		if (isSecondPart) {
			//Debug.Log ("asdf");
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


			HitboxDamagerOptions damagerOptions = new HitboxDamagerOptions();
			damagerOptions.hitBox = hitBox;
			damagerOptions.spellHitbox = spellHitbox;
			damagerOptions.damagePercent = 1.064103f;
			damagerOptions.damageType = Stats.hybridDamageType;
			damagerOptions.path = "Prefabs/SkillPrefabs/GrenadeHitbox";

			StartCoroutine(waitFewFramesAndMakeDamage (damagerOptions));
			StartCoroutine (waitSkillEndAndClearColliders ());
		} else {
			//Debug.Log ("lol");
			anim.Play ("grenade");
			StartCoroutine (WaitAnimationAction("grenade", 2));
		}
	}


	public void RocketJump(bool isSecondPart = false){
		currentAction = "RocketJump";
		if (isSecondPart) {
			//characterAPI.stats.withoutControl = true;
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


			HitboxDamagerOptions damagerOptions = new HitboxDamagerOptions();
			damagerOptions.hitBox = hitBox;
			damagerOptions.spellHitbox = spellHitbox;
			damagerOptions.damagePercent = 0.1025641f;
			damagerOptions.damageType = Stats.hybridDamageType;
			damagerOptions.path = "Prefabs/SkillPrefabs/GrenadeHitbox";

			StartCoroutine(waitFewFramesAndMakeDamage (damagerOptions));
			StartCoroutine (waitSkillEndAndClearColliders ());
			characterAPI.movementController.AddForceWithStartAnimation(new Vector3(350 * directionIndex, 350, 0), "rocketJump", 2);
			//stats.inJump = true;
		} else {
			anim.Play ("rocketJump");
			StartCoroutine (WaitAnimationAction("rocketJump", 2));
			StartCoroutine (StunWhileAnimation ("rocketJump", 2));
			characterAPI.rigidbody.velocity = new Vector3 (0, 0, 0);
		}
	}














	/************************************************Elemental****************************************************/
	public void FastCast(bool isSecondPart = false){
		currentAction = "FastCast";
		if (isSecondPart) {
			RaycastHit hit;
			float scaleDirectionX = 1;
			if (transform.localScale.x < 0) {
				scaleDirectionX = -1;
			}



			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Elemental/fireball");
			Effect effect = effectObject.GetComponent<Effect> ();
			SpellBall spellBall = effectObject.GetComponent<SpellBall> ();
			spellBall.characterAPI = characterAPI;
			spellBall.damagePercent = -0.2179487f;
			spellBall.damageType = Stats.elementalDamageType;
			spellBall.direction = new Vector3 (scaleDirectionX, shotDirectionVector.y, shotDirectionVector.z);
			spellBall.speed = 0.15f;
			spellBall.time = 2f;
			spellBall.currentTime = 0;
			spellBall.ready = true;
			spellBall.path = "Prefabs/Particles/Elemental/fireball";
			spellBall.energyPoints = 9;
			spellBall.ignoreColliders.Add (characterAPI.boxCollider);
			spellBall.ignoreColliders.Add (characterAPI.sphereCollider);
			spellBall.efficiency = 100f + (float)(characterAPI.stats.magicEnergy);
			effect.path = "Prefabs/Particles/Elemental/fireball";
			EffectOptions effectOptions = new EffectOptions ();
			//effect.transform.parent = targetCharacterAPI.skills.nullEffectPosition.transform;
			//effectOptions.isRandomDuration = false;
			//effectOptions.duration = 0.5f;
			effectOptions.loop = true;
			//float randomYposition = Random.Range (effect.nullPositionCoords.y - 0.100f, effect.nullPositionCoords.y + 0.100f);
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
					//Debug.Log ("asdf");
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
			RaycastHit hit;
			float scaleDirectionX = 1;
			if (transform.localScale.x < 0) {
				scaleDirectionX = -1;
			}
			Physics.Raycast (
				transform.position, 
				new Vector3(scaleDirectionX, shotDirectionVector.y, shotDirectionVector.z),
				out hit, 
				8f, 
				(1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ground")) /*Enemy*/
			);
			//Physics.Raycast (new Vec);
			if (hit.collider != null) {
				if (hit.collider.gameObject.layer != LayerMask.NameToLayer ("Ground")) {
					int damage = SkillDamage (-0.2179487f, Stats.hybridDamageType);
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
		damagerOptions.damagePercent = 0.1025641f;
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
			RaycastHit[] rightHits = Physics.RaycastAll (transform.position, 
				                        new Vector3 (1, 0, 0), 
				                        3f, 
				                        (1 << LayerMask.NameToLayer ("Enemy")) | (1 << LayerMask.NameToLayer ("Ground"))
			                        );

			RaycastHit[] leftHits = Physics.RaycastAll (transform.position, 
				                       new Vector3 (1, 0, 0), 
				                       3f, 
				                       (1 << LayerMask.NameToLayer ("Enemy")) | (1 << LayerMask.NameToLayer ("Ground"))
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
			stats.AddShieldPoints (SkillDamage(2.346154f, Stats.physicalDamageType), Stats.physicalDamageType);
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
				stats.shieldTimer.SetTimer (10f);
				Timer.TimerAction timerAction = () => {
					stats.RemoveShieldPoints (stats.physicalShield, Stats.physicalDamageType);
					ObjectsPool.PushObject ("Prefabs/Particles/Elemental/physicalShield", effectObject);
				};

				StartCoroutine(stats.shieldTimer.ActionAfterTimer(timerAction));
			}else{
				stats.shieldTimer.SetTimer(10f);
			}
		} else {
			anim.Play ("castShield", 3);
			StartCoroutine (WaitAnimationAction("castShield", 3));
		}
	}

	public void ElementalShield(bool isSecondPart = false){
		currentAction = "ElementalShield";
		if (isSecondPart) {
			stats.AddShieldPoints (SkillDamage(2.346154f, Stats.elementalDamageType), Stats.elementalDamageType);
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
				stats.shieldTimer.SetTimer (4f);
				Timer.TimerAction timerAction = () => {
					stats.RemoveShieldPoints (stats.elementalShield, Stats.elementalDamageType);
					ObjectsPool.PushObject ("Prefabs/Particles/Elemental/elementalShield", effectObject);
				};

				StartCoroutine(stats.shieldTimer.ActionAfterTimer(timerAction));
			}else{
				stats.shieldTimer.SetTimer(4f);
			}
		} else {
			anim.Play ("castShield", 3);
			StartCoroutine (WaitAnimationAction("castShield", 3));
		}
	}


	public void HybridShield(bool isSecondPart = false){
		currentAction = "HybridShield";
		if (isSecondPart) {
			stats.AddShieldPoints ((int)(SkillDamage(2.346154f, Stats.elementalDamageType)/2), Stats.hybridDamageType);
			stats.AddShieldPoints ((int)(SkillDamage(2.346154f, Stats.physicalDamageType)/2), Stats.hybridDamageType);
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
				stats.shieldTimer.SetTimer (4f);
				Timer.TimerAction timerAction = () => {
					stats.RemoveShieldPoints (stats.hybridShield, Stats.hybridDamageType);
					ObjectsPool.PushObject ("Prefabs/Particles/Elemental/hybridShield", effectObject);
				};

				StartCoroutine(stats.shieldTimer.ActionAfterTimer(timerAction));
			}else{
				stats.shieldTimer.SetTimer(4f);
			}
		} else {
			anim.Play ("castShield", 3);
			StartCoroutine (WaitAnimationAction("castShield", 3));
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
		return (fd - wd - (wd * dp) - (wd * ap) - (wd * dp * ap)) / (wd + (wd * ap));
		//return finalDamage / (2.5f * 60);
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
					ignoreColliders.Add (enemyCharacterAPI.sphereCollider);
					damagerOptions.spellHitbox.ignoreColliders.Add (enemyCharacterAPI.sphereCollider);
					ignoreColliders.Add (enemyCharacterAPI.boxCollider);
					damagerOptions.spellHitbox.ignoreColliders.Add (enemyCharacterAPI.boxCollider);
				}

				damagerOptions.spellHitbox.Clear ();
				zero = 4;
			}
		}
		ObjectsPool.PushObject (damagerOptions.path, damagerOptions.hitBox);
		skillProcessCount = skillProcessCount - 1;
		yield break;
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
}
