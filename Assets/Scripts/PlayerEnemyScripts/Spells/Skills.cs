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
			Debug.Log(TestSkillDamagePercent (350));
		}
	}

	public int SkillDamage(float percent, float plusDamage){
		int damage = 0;
		damage = (int)(System.Math.Round (2.5f * stats.weaponDamage * percent) + plusDamage);
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
		MethodInfo methodInfo = this.GetType ().GetMethod (currentAction);
		methodInfo.Invoke (this, new object[]{true});
	}

	public void FastPunch (bool isSecondPart = false){
		currentAction = "FastPunch";
		if (isSecondPart) {
			RaycastHit hit;
			Physics.Raycast (transform.position, new Vector3(transform.localScale.x, 0, 0), out hit, 0.8f, 1 << 9/*Enemy*/);
			if (hit.collider != null) {
				int damage = SkillDamage (0.666f, stats.physicalDamage);
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
				int damage = SkillDamage (1.333f, stats.physicalDamage);
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
				int damage = SkillDamage (2f, stats.physicalDamage);
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
				SkillDamage (3.333f, stats.physicalDamage),
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
				SkillDamage (1.666f, stats.physicalDamage), 
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
				SkillDamage (1.666f, stats.physicalDamage), 
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
				SkillDamage (2.333f, stats.physicalDamage),
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
		streamHitbox.damagePercent = 1.66f;
		streamHitbox.damageType = Stats.physicalDamageType;
		streamHitbox.readyToTrigger = true;

		StartCoroutine (ChargeMove(hitBox, streamHitbox));
	}


	public void ThunderClap(bool isSecondPart = false){
		currentAction = "ThunderClap";
		if (isSecondPart) {
			Debug.Log ("azaz");
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


		} else {
			anim.Play ("thunderClap");
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




	float TestSkillDamagePercent(int finalDamage){
		/*withComplexity 600*/
		return finalDamage / (2.5f * 60);
	}


	IEnumerator waitSkillEndAndClearColliders(){
		while (skillProcessCount != 0) {
			yield return null;
		}
		ignoreColliders.Clear ();
		yield break;
	}

	IEnumerator waitFewFramesAndMakeDamage(GameObject hitBox, SpellHitbox spellHitbox, int damage, int damageType, string path){
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
					enemyCharacterAPI.stats.MakeDamage (damage, damageType, true);
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
}
