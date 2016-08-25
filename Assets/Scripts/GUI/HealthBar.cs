using UnityEngine;
using System.Collections;
using System;

public class HealthBar : MonoBehaviour {
	public Texture2D barTexture;
	GUIStyle style = new GUIStyle();
	//public GameObject barObject;
	public Transform objectPosition;
	Vector3 screenPosition;
	Rect barRect;
	Stats stats;
	int barWidth = 100;

	public GUIStyle labelStyle;// = new GUIStyle ();

	float oneWidthPoint;
	//int currentHealth;

	// Use this for initialization
	void Awake () {
		stats = gameObject.GetComponent<Stats> ();
		oneWidthPoint = stats.GetMaximumHealth ()/barWidth;
		//stats.SetMaximumHealth ();
		//objectPosition = barObject.transform;
		style.normal.background = barTexture;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.L)) {
			/*stats.MakeDamage(100, true);
			stats.AddMeleeEnergyPoints(100);*/
			stats.ChangePhysicalDamagePoints (95);
			//Debug.Log (Math.Round(10.123456f, 2));

		}
	}

	void OnGUI(){
		//Camera
		int currentBarWidth = (int)(stats.health/oneWidthPoint);
		screenPosition = Camera.main.WorldToScreenPoint (objectPosition.position);
		barRect = new Rect (0, 0, currentBarWidth, 10);
		barRect.x = screenPosition.x;//new Vector2 (screenPosition.x, Screen.height - screenPosition.y);
		barRect.y = Screen.height - screenPosition.y;

		GUI.Box (barRect, "", style);
		/*

		Rect textRect = new Rect (0, 0, currentBarWidth, 40);
		textRect.x = screenPosition.x;//new Vector2 (screenPosition.x, Screen.height - screenPosition.y);
		textRect.y = Screen.height - screenPosition.y+ 20;

		//GUIStyle = new GUIStyle ();
		GUI.Label (textRect, "1000069", labelStyle);*/
	}
}
