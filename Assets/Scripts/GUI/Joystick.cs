using UnityEngine;
using System.Collections;
using System;

public class Joystick : MonoBehaviour {

	private Vector3 mouseVector;
	private Touch[] touches;
	private System.Nullable<Touch> joystickTouch = null;
	private float maxJoystickDistance = 30.0f;

	// Use this for initialization
	void Start () {
		gameObject.transform.position = GameObject.Find ("JoystickCenter").transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {
		touches = Input.touches;
		/*for(int i=0; i < Input.touchCount; i++){
			/*Debug.Log(touches[i]);*/	
		/*}*/
	}

	void FixedUpdate(){
		var player = GameObject.Find ("Player");
		var movement = new Vector2 (
			GetDirection().x,
			GetDirection().y
		);
		/*player.direction */
		player.GetComponent<PlayerScript>().direction =  movement;
	}

	public void Drag(){
		/*joystickTouch = Array.Find (touches, touch => touch );*/
		/*Debug.Log(GUILayer.HitTest (new Vector3 (Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0)));*/
		RectTransform rt = (RectTransform)transform;
		Rect joystickRect = new Rect(0, 0,rt.rect.height*10, rt.rect.width*10);
		joystickRect.center = transform.position;

		//if (joystickTouch == null) {
			joystickTouch = Array.Find (touches, touch => joystickRect.Contains(touch.position));
		//if(joystickTouch != null){
		//	joystickRect.center = joystickTouch.Value.position;
		//}
		//}
		//Debug.Log(joystickTouch.GetType());



		if (GetDistanceBetweenTouchAndCenter () > maxJoystickDistance) {
			mouseVector =   joystickTouch.Value.position - new Vector2(GameObject.Find ("JoystickCenter").transform.position.x, GameObject.Find ("JoystickCenter").transform.position.y);
			gameObject.transform.position = (mouseVector / mouseVector.magnitude) * maxJoystickDistance + GameObject.Find ("JoystickCenter").transform.position;
		} else {
			gameObject.transform.position = joystickTouch.Value.position;
		}
	}

	public void Drop(){
		gameObject.transform.position = GameObject.Find ("JoystickCenter").transform.position;
		joystickTouch = null;
	}

	/*public void Lol(){
		Debug.Log ("azaza");
	}*/

	float GetDistanceBetweenJoystickAndCenter(){
		return (gameObject.transform.position - GameObject.Find ("JoystickCenter").transform.position).magnitude; 	
	}

	float GetDistanceBetweenTouchAndCenter(){
		return (joystickTouch.Value.position -  new Vector2(GameObject.Find ("JoystickCenter").transform.position.x, GameObject.Find ("JoystickCenter").transform.position.y)).magnitude; 	
	}

	public Vector2 GetDirection(){
		var direction = gameObject.transform.position - GameObject.Find ("JoystickCenter").transform.position;
		direction.Normalize ();
		return direction;
	}
}


