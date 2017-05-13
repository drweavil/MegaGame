using UnityEngine;
using System.Collections;
using System;

public class HealthBar : MonoBehaviour {
	public Transform objectPosition;
	public GameObject bar;
	public CharacterAPI characterAPI;
	public PlayerHealthBar playerHealthBar;
	public int barShift = 0;
	public float healthBarPoolFrames = 0;
	public Rect poolRect = new Rect();
	public GUIStyle labelStyle;
	private bool inPool = false;

	float oneWidthPoint;
	//int currentHealth;

	// Use this for initialization
	void Awake () {
		bar = ObjectsPool.PullObject("Prefabs/UI/healthBar");

		playerHealthBar = bar.GetComponent<PlayerHealthBar> ();
		characterAPI.stats.playerHealthBar = playerHealthBar;
		playerHealthBar.stats = characterAPI.stats;
		//playerHealthBar.ChangeToSpec (characterAPI.stats.specId);
		characterAPI.healthBar = playerHealthBar;
		StartCoroutine(StartProcess.StartActionAfterFewFrames (5, () => {
			if(BattleInterfaceController.battleInterfaceController.battleInterface.activeInHierarchy){
				bar.transform.SetParent(HealthBarPool.healthBarPool.transform);
				HealthBarPool.healthBars.Add (this);
			}else{
				HealthBarPool.deferredHealthBars.Add (this);
			}
		}));

		poolRect.width = playerHealthBar.resourcesBarRect.rect.width;
		poolRect.height = playerHealthBar.resourcesBarRect.rect.height;
		poolRect.position = new Vector2 (0, 0);

	}

	void LateUpdate(){
		//Debug.Log (this.gameObject.activeInHierarchy);
		if (BattleInterfaceController.battleInterfaceController.battleInterface.activeInHierarchy) {
			if (this.gameObject.activeInHierarchy) {
				if (healthBarPoolFrames == 60) {
					Rect guiRect = new Rect ();
					guiRect.position = new Vector2 (
						HealthBarPool.healthBarPool.commonGUIRect.position.x - HealthBarPool.healthBarPool.commonGUIRect.rect.width / 2,
						HealthBarPool.healthBarPool.commonGUIRect.position.y - HealthBarPool.healthBarPool.commonGUIRect.rect.height / 2
					);
					guiRect.width = HealthBarPool.healthBarPool.commonGUIRect.rect.width;
					guiRect.height = HealthBarPool.healthBarPool.commonGUIRect.rect.height;

					Rect barRect = new Rect ();
					barRect.position = playerHealthBar.resourcesBarRect.position;
					barRect.width = playerHealthBar.resourcesBarRect.rect.width;
					barRect.height = playerHealthBar.resourcesBarRect.rect.height;

					//Debug.Log (guiRect.position);
					//Debug.Log (Camera.main.WorldToScreenPoint (objectPosition.position));
					if (guiRect.Contains (Camera.main.WorldToScreenPoint (objectPosition.position))) {
						if (HealthBarPool.healthBars.FindIndex (h => h == this) == -1) {
							HealthBarPool.healthBars.Add (this);	
							inPool = true;
							playerHealthBar.IsActive (true);
						}
						//Debug.Log ("lol");
					} else {
						if (HealthBarPool.healthBars.FindIndex (h => h == this) != -1) {
							HealthBarPool.healthBars.Remove (this);	
							inPool = false;
							playerHealthBar.IsActive (false);
						}
						//Debug.Log ("lol2");
					}
					healthBarPoolFrames = 0;
				} else {
					healthBarPoolFrames += 1;
				}
			}
		}
	}

	public void PushBarToPool(){
		playerHealthBar.damageNumber.transform.parent = playerHealthBar.transform;
		if (HealthBarPool.healthBars.FindIndex (h => h == this) != -1) {
			HealthBarPool.healthBars.Remove (this);	
			inPool = false;
		}
		if (HealthBarPool.deferredHealthBars.FindIndex (h => h == this) != -1) {
			HealthBarPool.healthBars.Remove (this);	
			inPool = false;
		}
		ObjectsPool.PushObject ("Prefabs/UI/healthBar", bar);
	}


	public void UpPosition(int shift){
		Vector2 newPosition = new Vector2 (
			playerHealthBar.resourcesBarRect.position.x,
			playerHealthBar.resourcesBarRect.position.y + (playerHealthBar.resourcesBarRect.rect.height * shift)
		);
		bar.transform.position = newPosition;
	}

	public Vector2 UpRectPosition(int shift){
		Vector2 newPosition = new Vector2 (
			playerHealthBar.resourcesBarRect.position.x,
			playerHealthBar.resourcesBarRect.position.y + (playerHealthBar.resourcesBarRect.rect.height * shift)
		);
		return newPosition;
	}


	public static bool RectContainsRect(Rect largeRect, Rect smallRect){
		bool value = false;
		//smallRectLeftTop
		if(largeRect.Contains(new Vector2 (smallRect.x, smallRect.y + smallRect.height))){
			value = true;
		}
		//smallRectRightTop
		if(largeRect.Contains(new Vector2 (smallRect.x + smallRect.width, smallRect.y + smallRect.height))){
			value = true;
		}
		//smallRectLeftBottom
		if(largeRect.Contains(new Vector2 (smallRect.x, smallRect.y))){
			value = true;
		}
		//smallRectRightBottom
		if(largeRect.Contains(new Vector2 (smallRect.x + smallRect.width, smallRect.y))){
			value = true;
		}
		return value;
	}
}
