using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {

	public GameObject healthBar;
	public GameObject resourceBar;
	public GameObject player;
	public GameObject shieldBar;

	private Stats stats;

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


	//float time = 30f;

	// Use this for initialization
	void Awake () {
		SetBarRects ();
		stats = player.GetComponent<Stats> ();
	}
	
	// Update is called once per frame
	void Update () {
		

		/*if (Input.GetKeyDown (KeyCode.Z)) {
			SetShield (600);
		}
		if (Input.GetKeyDown (KeyCode.X)) {
			SetShield (0);
		}*/

		//time -= Time.realtimeSinceStartup;

		//Debug.Log (Time.time);





		//Debug.Log (maximumHealthWidth);
	}

	void SetBarRects(){
		healthBarRect = healthBar.GetComponent<RectTransform> ();
		resourceBarRect = resourceBar.GetComponent<RectTransform> ();
		shieldBarRect = shieldBar.GetComponent<RectTransform> ();

		maximumHealthWidth = healthBarRect.rect.width;
		maximumResourceWidth = resourceBarRect.rect.width;
		maximumShieldWidth = shieldBarRect.rect.width;

		healthRectPosition = healthBarRect.rect.position;
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
}
