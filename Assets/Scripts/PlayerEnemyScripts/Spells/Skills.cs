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
	private string currentAction = "";
	private List<Collider> ignoreColliders = new List<Collider>();
	private int skillProcessCount = 0;
	public CharacterAPI characterAPI;

	private string[] names = new string[]{ "Run", "jumpStart", "jumpEnd", "jumpIdle" };
	private List<int> actionAnimationHashes = new List<int>();


	void Awake(){
		foreach (string name in names) {
			actionAnimationHashes.Add (Animator.StringToHash(name));
		}
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Z)) {
			Debug.Log(TestSkillDamagePercent (125));
		}
	}

	public int SkillDamage(float percent, int damageType){
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
				Debug.Log (damage);
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

			StartCoroutine(waitFewFramesAndMakeDamage (hitBox, 
				spellHitbox,
				4.910256f,
				Stats.physicalDamageType,
				"Prefabs/SkillPrefabs/MeleeConeHitbox")
			);
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

			StartCoroutine(waitFewFramesAndMakeDamage (hitBox, 
				spellHitbox, 
				1.705128f,
				Stats.physicalDamageType,
				"Prefabs/SkillPrefabs/CirclePunchHitbox")
			);
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

			StartCoroutine(waitFewFramesAndMakeDamage (hitBox2, 
				spellHitbox2, 
				1.705128f,
				Stats.physicalDamageType,
				"Prefabs/SkillPrefabs/CirclePunchHitbox")
			);
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

			StartCoroutine(waitFewFramesAndMakeDamage (hitBox, 
				spellHitbox,
				2.98718f,
				Stats.physicalDamageType,
				"Prefabs/SkillPrefabs/MeleeWaveHitbox")
			);
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
			//Debug.Log ("azaz");
			GameObject effectObject = ObjectsPool.PullObject ("Prefabs/Particles/Melee/ThunderClapAir");
			Effect effect = effectObject.GetComponent<Effect> ();
			effect.path = "Prefabs/Particles/Melee/ThunderClapAir";
			EffectOptions effectOptions = new EffectOptions ();
			//effect.transform.parent = nullEffectPosition.transform;
			Vector3 effectPosition = nullEffectPosition.transform.position + effect.nullPositionCoords;
			//if(characterAPI.transform.localScale.x < 0){
				//Debug.Log ("lols");
				//effectPosition = nullEffectPosition.transform.position +
				//	new Vector3 (effect.nullPositionCoords.x * -1, effect.nullPositionCoords.y, effect.nullPositionCoords.z);
				//effectOptions.revertRotationY = true;	

			//}

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
			//SpellHitbox spellHitbox = hitBox.GetComponent<SpellHitbox> ();
			hitBox.transform.localPosition = thunderClapHitbox.spellHitbox.nullSpellPosition;
			hitBox.transform.localScale = thunderClapHitbox.spellHitbox.nullSpellScale;


			StartCoroutine (thunderClapHitbox.MakeDamageAndThrow ());

		} else {
			StartCoroutine (ToGround ());
			StartCoroutine (WaitGroundedAndStartThunderClap ());
		}
	}












	/************************************************* Fire *********************************************/
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

			StartCoroutine(waitFewFramesAndMakeDamage (hitBox, 
				spellHitbox, 
				1.064103f,
				Stats.hybridDamageType,
				"Prefabs/SkillPrefabs/GrenadeHitbox")
			);
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

			StartCoroutine(waitFewFramesAndMakeDamage (hitBox, 
				spellHitbox, 
				0.1025641f,
				Stats.hybridDamageType,
				"Prefabs/SkillPrefabs/RocketJumpHitBox")
			);
			StartCoroutine (waitSkillEndAndClearColliders ());
			//characterAPI.rigidbody.AddForce (350 * directionIndex, 350, 0);
			characterAPI.movementController.AddForceWithStartAnimation(new Vector3(350 * directionIndex, 350, 0), "rocketJump", 2);
			//stats.inJump = true;
		} else {
			anim.Play ("rocketJump");
			StartCoroutine (WaitAnimationAction("rocketJump", 2));
			StartCoroutine (StunWhileAnimation ("rocketJump", 2));
			characterAPI.rigidbody.velocity = new Vector3 (0, 0, 0);
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

	IEnumerator waitFewFramesAndMakeDamage(GameObject hitBox, SpellHitbox spellHitbox, float damagePercent, int damageType, string path, bool withStun = false, float stunTime = 0){
		skillProcessCount = skillProcessCount + 1;
		int zero = 0;
		while (zero < 4) {
			if (spellHitbox.objects.Count == 0) {
				zero += 1;
				yield return null;
			} else {
				foreach (Collider col in ignoreColliders) {
					spellHitbox.ignoreColliders.Add (col);
				}

				foreach (GameObject obj in spellHitbox.GetObjectsWithoutIgnoredColliders()) {
					CharacterAPI enemyCharacterAPI = obj.GetComponent<CharacterAPI> ();
					if (damageType == Stats.hybridDamageType) {
						enemyCharacterAPI.stats.MakeDamage (SkillDamage (damagePercent, Stats.physicalDamageType), Stats.physicalDamageType, true);
						enemyCharacterAPI.stats.MakeDamage (SkillDamage (damagePercent, Stats.elementalDamageType), Stats.elementalDamageType, true);
					} else {
						enemyCharacterAPI.stats.MakeDamage (SkillDamage (damagePercent, damageType), damageType, true);
					}

					if (withStun) {
						enemyCharacterAPI.stats.FullStun (stunTime);
					}
					ignoreColliders.Add (enemyCharacterAPI.sphereCollider);
					spellHitbox.ignoreColliders.Add (enemyCharacterAPI.sphereCollider);
					ignoreColliders.Add (enemyCharacterAPI.boxCollider);
					spellHitbox.ignoreColliders.Add (enemyCharacterAPI.boxCollider);
				}

				spellHitbox.Clear ();
				zero = 4;
			}
		}
		ObjectsPool.PushObject (path, hitBox);
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
