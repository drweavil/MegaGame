using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {

	public GameObject healthBar;
	public GameObject resourceBar;
	//public GameObject player;
	public GameObject shieldBar;
	public GameObject rageBar;
	public GameObject fireBar;
	public GameObject elementalBar;
	public RectTransform resourcesBarRect;
	public GameObject damageNumber;
	public GameObject damageNumberNullSlot;
	private Text damageNumberText;
	private bool damageDetonatorActive = false;
	private List<NumberParams> damageNumbers = new List<NumberParams> ();
	public bool checkRects = true;

	public Stats stats;

	private RectTransform healthBarRect;
	private RectTransform resourceBarRect;
	private RectTransform shieldBarRect;


	private float maximumHealthWidth;
	private float maximumResourceWidth;
	private float maximumShieldWidth;

	private Vector2 healthRectPosition;
	private Vector2 resourceRectPosition;
	private Vector2 shieldRectPosition;

	private Rect newHealthRectPosition = new Rect ();
	private Rect newResourceRectPosition = new Rect ();
	private Rect newShieldRectPosition = new Rect ();

	private int frames = 0;




	//float time = 30f;

	// Use this for initialization
	void Awake () {
		SetBarRects ();
		//stats = player.GetComponent<Stats> ();

		damageNumberText = damageNumber.GetComponent<Text> ();
		damageNumber.SetActive (false);
		//damageNumberNullPosition = damageNumber.transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (damageNumbers.Count);
		if (checkRects) {
			//Debug.Log(resourcesBarRect.rect.position);
			//Debug.Log(resourcesBarRect.rect);
			//Debug.Log(Camera.main.ViewportToScreenPoint(new Vector3(1, 1, Camera.main.nearClipPlane)));
			//Debug.Log ((HealthBar.RectContainsRect (Camera.main.rect, resourcesBarRect.rect)));
		}

		if (damageNumbers.Count != 0 && !damageDetonatorActive) {
			StartCoroutine (DamageNumberBang ());
		}
	}


	public void ChangeToSpec(int specId){
		//Debug.Log ("lul");
		if (Stats.meleeSpec == specId) {
			rageBar.SetActive (true);
			elementalBar.SetActive (false);
			fireBar.SetActive (false);
			SetResource (stats.meleeEnergy);
		} else {
			if (Stats.fireSpec == specId) {
				rageBar.SetActive (false);
				elementalBar.SetActive (false);
				fireBar.SetActive (true);
				SetResource (stats.fireEnergy);
			} else {
				if (Stats.elementalSpec == specId) {
					rageBar.SetActive (false);
					elementalBar.SetActive (true);
					fireBar.SetActive (false);
					SetResource (stats.magicEnergy);
				}
			}
		}
	}

	void SetBarRects(){
		healthBarRect = healthBar.GetComponent<RectTransform> ();
		resourceBarRect = resourceBar.GetComponent<RectTransform> ();
		shieldBarRect = shieldBar.GetComponent<RectTransform> ();

		maximumHealthWidth = healthBarRect.rect.width;
		maximumResourceWidth = resourceBarRect.rect.width;
		maximumShieldWidth = shieldBarRect.rect.width;

		healthRectPosition = new Vector2(healthBarRect.rect.position.x, healthBarRect.rect.position.y);
		//Debug.Log (healthRectPosition);
		resourceRectPosition = resourceBarRect.rect.position;
		shieldRectPosition = shieldBarRect.rect.position;

		newHealthRectPosition.height = healthBarRect.rect.height;
		newResourceRectPosition.height = resourceBarRect.rect.height;
		newShieldRectPosition.height = shieldBarRect.rect.height;
	}



	public void SetHealth(float currentHealthFloat){
		int currentHealth = (int)(System.Math.Round(currentHealthFloat, 0));
		float oneHealthWidthPoint = stats.maximumHealth / maximumHealthWidth;
		healthBarRect.sizeDelta = new Vector2(currentHealth / oneHealthWidthPoint, healthBarRect.rect.height);
		newHealthRectPosition.position = healthRectPosition;
		newHealthRectPosition.width = healthBarRect.rect.width;
		healthBarRect.localPosition = newHealthRectPosition.center;
	}

	public void SetResource(int currentResource){
		float oneResourceWidthPoint = stats.GetMaximumResource() / maximumResourceWidth;
		resourceBarRect.sizeDelta = new Vector2(currentResource / oneResourceWidthPoint, resourceBarRect.rect.height);
		newResourceRectPosition.position = resourceRectPosition;
		newResourceRectPosition.width = resourceBarRect.rect.width;
		resourceBarRect.localPosition = newResourceRectPosition.center;
	}

	public void SetShield(float currentShield){
		if (currentShield > 0) {
			shieldBar.SetActive (true);
		} else {
			shieldBar.SetActive (false);
		}
		float oneShieldWidthPoint = stats.maximumHealth / maximumShieldWidth;
		if(currentShield >= stats.maximumHealth){
			currentShield = stats.maximumHealth;
		}
		shieldBarRect.sizeDelta = new Vector2(currentShield / oneShieldWidthPoint, shieldBarRect.rect.height);
		newShieldRectPosition.position = shieldRectPosition;
		newShieldRectPosition.width = shieldBarRect.rect.width;
		shieldBarRect.localPosition = newShieldRectPosition.center;
	}

	public void AddDamage(NumberParams damage){
		damageNumbers.Add (damage);
	}

	IEnumerator DamageNumberBang(){
		//Debug.Log (stats);
		damageDetonatorActive = true;
		damageNumber.transform.position = damageNumberNullSlot.transform.position;
		damageNumber.SetActive(true);
		//Vector2 nullPosition = damageNumber.transform.position;
		damageNumberText.text = damageNumbers [0].number.ToString ();
		if (damageNumbers [0].isCrit) {
			damageNumber.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
		} else {
			damageNumber.transform.localScale = new Vector3 (1f, 1f, 1f);
		}
		if (damageNumbers [0].isDamage) {
			if (damageNumbers [0].damageType == Stats.physicalDamageType) {
				damageNumberText.color = new Color (255, 195, 0);
			} else {
				damageNumberText.color = new Color (0, 255, 255);
			}
		} else {
			damageNumberText.color = Color.green;
		}
		if (stats.isPlayerStats) {
			float frameTime = 0;
			while (frameTime < 10) {
				frameTime += (0.3f * damageNumbers.Count);
				yield return null;
			}
		} else {
			damageNumber.transform.SetParent(HealthBarPool.healthBarPool.gameObject.transform);
			float slotYPosition = 0;
			while (slotYPosition < 10f) {
				slotYPosition += 0.3f * damageNumbers.Count;
				damageNumber.transform.position = new Vector2 (damageNumberNullSlot.transform.position.x, damageNumberNullSlot.transform.position.y + (slotYPosition));
				yield return null;
			}
		}
		damageNumber.transform.SetParent(this.gameObject.transform);
		damageNumbers.RemoveAt (0);
		damageNumber.SetActive(false);
		if (damageNumbers.Count != 0) {
			StartCoroutine (DamageNumberBang ());
		} else {
			damageDetonatorActive = false;
		}
		yield break;
	}


	public void IsActive(bool active){
		if (!active) {
			damageNumber.transform.SetParent(this.transform);
			gameObject.SetActive (false);
		} else {
			gameObject.SetActive (true);
		}
	}

	public void UpdateNumbers(){
		SetHealth (stats.health);
		stats.SetRecourceToBar ();
	}

	public class NumberParams{
		public int number = 0;
		public bool isDamage = true;
		public int damageType = 0;
		public bool isCrit = false;
	}
}
