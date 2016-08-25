using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {

	public GameObject healthBar;
	public GameObject resourceBar;
	public GameObject player;

	private Stats stats;

	private RectTransform healthBarRect;
	private RectTransform resourceBarRect;

	private float maximumHealthWidth;
	private float maximumResourceWidth;

	private Vector2 healthRectPosition;
	private Vector2 resourceRectPosition;
	private Rect newHealthRectPosition = new Rect ();
	private Rect newResourceRectPosition = new Rect ();


	//float time = 30f;

	// Use this for initialization
	void Start () {
		SetBarRects ();
		stats = player.GetComponent<Stats> ();
	}
	
	// Update is called once per frame
	void Update () {
		



		//time -= Time.realtimeSinceStartup;

		//Debug.Log (Time.time);





		//Debug.Log (maximumHealthWidth);
	}

	void SetBarRects(){
		healthBarRect = healthBar.GetComponent<RectTransform> ();
		resourceBarRect = resourceBar.GetComponent<RectTransform> ();

		maximumHealthWidth = healthBarRect.rect.width;
		maximumResourceWidth = resourceBarRect.rect.width;

		healthRectPosition = healthBarRect.rect.position;
		resourceRectPosition = resourceBarRect.rect.position;

		newHealthRectPosition.height = healthBarRect.rect.height;
		newResourceRectPosition.height = resourceBarRect.rect.height;
	}



	public void SetHealth(int currentHealth){
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
}
