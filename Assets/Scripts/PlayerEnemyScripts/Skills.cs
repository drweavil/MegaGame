using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Skills : MonoBehaviour {
	public Animator anim;
	public Stats stats;
	public Rigidbody rigidbody;
	private string currentAction = "";

	private string[] names = new string[]{ "Run", "jumpStart", "jumpEnd", "jumpIdle" };
	private List<int> actionAnimationHashes = new List<int>();


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

	int SkillDamage(float percent, float plusDamage){
		int damage = 0;
		damage = (int)System.Math.Round (2.5f * stats.weaponDamage * percent);
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
				hit.collider.gameObject.GetComponent<Stats>().MakeDamage(damage, Stats.physicalDamageType,  true);
				stats.AddMeleeEnergyPoints (9, true);
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
				hit.collider.gameObject.GetComponent<Stats>().MakeDamage(damage, Stats.physicalDamageType,  true);
				stats.AddMeleeEnergyPoints (15, true);
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
				hit.collider.gameObject.GetComponent<Stats>().MakeDamage(damage, Stats.physicalDamageType,  true);
				stats.AddMeleeEnergyPoints (20, true);
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
			RaycastHit hit;
			Physics.Raycast (transform.position, new Vector3(transform.localScale.x, 0, 0), out hit, 0.8f, 1 << 9/*Enemy*/);
			if (hit.collider != null) {
				int damage = SkillDamage (2f, stats.physicalDamage);
				//Debug.Log (damage);
				hit.collider.gameObject.GetComponent<Stats>().MakeDamage(damage, Stats.physicalDamageType,  true);
				stats.AddMeleeEnergyPoints (20, true);
			}
		} else {
			if (IsAction ()) {
				anim.Play ("actionMeleeCone");
			} else {
				anim.Play ("meleeCone");	
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




	float TestSkillDamagePercent(int finalDamage){
		/*return (finalDamage - 2.5f * 60) / (2.5f * 60);*/
		return finalDamage / (2.5f * 60);
	}
}
