using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarPool : MonoBehaviour {
	public static HealthBarPool healthBarPool;
	public static bool isLoaded = false;
	public static List<HealthBar> healthBars = new List<HealthBar> ();
	public static List<HealthBar> deferredHealthBars = new List<HealthBar> ();
	static int frames = 0;
	public RectTransform commonGUIRect;

	public GameObject barsPool;
	


	void Awake () {
		healthBarPool = this.gameObject.GetComponent<HealthBarPool>();
		isLoaded = true;
	}

	void LateUpdate(){
		//Debug.Log(healthBars.Count);
		if (BattleInterfaceController.battleInterfaceController.battleInterface.activeInHierarchy) {
			foreach (HealthBar bar in healthBars) {
				bar.playerHealthBar.transform.position = Camera.main.WorldToScreenPoint (bar.objectPosition.position);
			}
			foreach (HealthBar bar in healthBars) {
				foreach (HealthBar bar2 in healthBars) {
					if (bar.playerHealthBar.resourcesBarRect.gameObject != bar2.playerHealthBar.resourcesBarRect.gameObject) {
						Rect largeRect = new Rect ();
						largeRect.position = bar.playerHealthBar.resourcesBarRect.position;
						largeRect.width = bar.playerHealthBar.resourcesBarRect.rect.width;
						largeRect.height = bar.playerHealthBar.resourcesBarRect.rect.height;

						Rect smallRect = new Rect ();
						smallRect.position = bar2.playerHealthBar.resourcesBarRect.position;
						smallRect.width = bar2.playerHealthBar.resourcesBarRect.rect.width;
						smallRect.height = bar2.playerHealthBar.resourcesBarRect.rect.height;
						if (HealthBar.RectContainsRect (largeRect, smallRect)) {
							bar2.playerHealthBar.transform.position = new Vector2 (
								bar2.playerHealthBar.resourcesBarRect.position.x,
								bar2.playerHealthBar.resourcesBarRect.position.y + (bar2.playerHealthBar.resourcesBarRect.rect.height + 0.001f)
							);
						} else {
						}
					}
				}
			}
		}
	}

	public static void PublishDeferredHealthBars(){
		foreach(HealthBar bar in deferredHealthBars){
			bar.bar.transform.parent = HealthBarPool.healthBarPool.transform;
			healthBars.Add (bar);
		}
		deferredHealthBars.Clear ();
	}
}
