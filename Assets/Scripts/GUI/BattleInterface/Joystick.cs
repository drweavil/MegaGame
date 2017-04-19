using UnityEngine;
using System.Collections;
using System;

public class Joystick : MonoBehaviour {

	private Vector3 mouseVector;
	private Touch[] touches;
	private System.Nullable<Touch> joystickTouch = null;
	private float maxJoystickdistance = 30.0f;
	public GameObject joystickCenter;

	// Use this for initialization
	void Start () {
		gameObject.transform.localPosition = new Vector3 (0, 0, 0);//joystickCenter.transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {
		touches = Input.touches;
		/*for(int i=0; i < Input.touchCount; i++){
			/*Debug.Log(touches[i]);*/	
		/*}*/
	}

	void FixedUpdate(){
		/*var player = GameObject.Find ("Player");
		var movement = new Vector2 (
			GetDirection().x,
			GetDirection().y
		);*/
		/*player.direction */
		/*player.GetComponent<PlayerScript>().direction =  movement;*/
	}

	public void Drag(){
		/*joystickTouch = Array.Find (touches, touch => touch );*/
		/*Debug.Log(GUILayer.HitTest (new Vector3 (Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0)));*/
		RectTransform rt = (RectTransform)transform;
		Rect joystickRect = new Rect(0, 0,rt.rect.height*10, rt.rect.width*10);
		joystickRect.center = transform.position;
		//Debug.Log ("asdfg");
		//if (joystickTouch == null) {
		joystickTouch = Array.Find (touches, touch => joystickRect.Contains(touch.position));
		//if(joystickTouch != null){
		//	joystickRect.center = joystickTouch.Value.position;
		//}
		//}
		//Debug.Log(joystickTouch.GetType());



		if (GetdistanceBetweenTouchAndCenter () > maxJoystickdistance) {
			mouseVector =   joystickTouch.Value.position - new Vector2(joystickCenter.transform.position.x, joystickCenter.transform.position.y);
			gameObject.transform.position = (mouseVector / mouseVector.magnitude) * maxJoystickdistance + joystickCenter.transform.position;
		} else {
			gameObject.transform.position = joystickTouch.Value.position;
		}
	}

	public void Drop(){
		gameObject.transform.localPosition = new Vector3 (0, 0, 0);//joystickCenter.transform.position;
		joystickTouch = null;
	}

	/*public void Lol(){
		Debug.Log ("azaza");
	}*/

	float GetdistanceBetweenJoystickAndCenter(){
		return (gameObject.transform.position - joystickCenter.transform.position).magnitude; 	
	}

	float GetdistanceBetweenTouchAndCenter(){
		return (joystickTouch.Value.position -  new Vector2(joystickCenter.transform.position.x, joystickCenter.transform.position.y)).magnitude; 	
	}

	public Vector2 GetDirection(){
		var direction = gameObject.transform.position - joystickCenter.transform.position;
		direction.Normalize ();
		return direction;
	}
}


