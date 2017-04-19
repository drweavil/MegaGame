using UnityEngine;
using System.Collections;
using System;

public class JoystickScript : MonoBehaviour {

	private Ray rayToCenter;
	private RaycastHit hitWithCenter; 

	private Ray rayToMouse;
	private RaycastHit[] hitsWithMouse; 
	private RaycastHit hitWithMouse;

	private Vector3 direction;
	private Vector3 directionFromMouse;
	private Vector3 interfaceMousePosition;
	public int speed = 25; 
	public float distance;
	private float maxJoystickDistance = 1.5f;


	private Ray dragRay ; // Луч от камеры до мыши
	private RaycastHit dragHit;
	private Vector3 worldMousePosition;
	private GameObject dragObject;


	private Vector3 centerMouseVector;
	// Use this for initialization
	void Start () {
		SetNullPosition ();
	}
	
	// Update is called once per frame
	void Update () {

		/********************** Джой направление *******************/
		direction = gameObject.transform.position - GameObject.Find ("JoystickCenter").transform.position;
		direction.Normalize ();
		rayToCenter = new Ray(gameObject.transform.position, direction);
		Physics.Raycast (rayToCenter, out hitWithCenter);

		//Debug.Log ();

		if (Input.GetMouseButtonUp (0)) {
			SetNullPosition ();
		}
		/*********************************************/

		/************************  Луч от мыши к центру **********************************/
		interfaceMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		interfaceMousePosition.z = -5;
		directionFromMouse = GameObject.Find ("JoystickCenter").transform.position - interfaceMousePosition;
		directionFromMouse.Normalize ();
		rayToMouse = new Ray (interfaceMousePosition, directionFromMouse);
		hitsWithMouse = Physics.RaycastAll (rayToMouse, 100f);
		hitWithMouse = Array.Find(hitsWithMouse, hit => hit.collider.name == "JoystickCenter");

		/*************************************************************************************/

		/******** Движение джойстика ********/
		if (Input.GetMouseButton (0)) {
			dragRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			worldMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			Physics.Raycast (dragRay, out dragHit);
			if (dragHit.collider != null) {
				dragObject = dragHit.collider.gameObject;	
			}

			if ((dragObject != null)) {
				if (dragObject.name == gameObject.name) {
					if (hitWithMouse.distance <= maxJoystickDistance && hitWithMouse.collider.name == "JoystickCenter") {
						Vector3 newPosition = new Vector3 (worldMousePosition.x, worldMousePosition.y, gameObject.transform.position.z);
						dragObject.transform.position = newPosition;

					} else {
						worldMousePosition.z = -5;
						centerMouseVector = worldMousePosition - GameObject.Find ("JoystickCenter").transform.position;
						dragObject.transform.position =  (centerMouseVector/centerMouseVector.magnitude) * maxJoystickDistance + GameObject.Find ("JoystickCenter").transform.position;
					}
				}
			//Debug.Log(hitWithMouse.distance);
			}


		} 



		if (Input.GetMouseButtonUp(0)){
			dragObject = null;
		}
		/**************************************************************************/

	}

	/*void FixedUpdate(){
		var player = GameObject.Find ("Player");
		var movement = new Vector3 (
			direction.x*speed,
			direction.y*speed,
			direction.z* speed
		);
		player.GetComponent<Rigidbody>().velocity = movement;
	}*/

	void SetNullPosition(){
		gameObject.transform.position = GameObject.Find ("JoystickCenter").transform.position;
		hitWithCenter.distance = 0;
	}
}
