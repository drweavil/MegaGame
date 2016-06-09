using UnityEngine;
using System.Collections;
using System;

public class PlayerScript : MonoBehaviour {

	public Vector2 speed = new Vector2 (10, 0);
	public Vector2 direction; 
	private Vector2 movement;

	private Animator anim;
	private bool toForward = true;

	public bool isGrounded = false;
	public Transform groundCheck;
	private float groundRadius = 0.1f;
	public LayerMask whatIsGround;
	public bool jumpButton;

	private GameObject player;
	private Vector3 cameraPositionVector = new Vector3 (0, 0, -5);
	private Transform cameraPosition;
	private GameObject background;



	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		player = GameObject.Find ("Player");
		cameraPosition = Camera.main.transform;
		background = GameObject.Find ("BackGround");

	}

	// Update is called once per frame
	void Update () {
		if (direction.x == 0.0 && direction.y == 0.0) {
			float inputX = Input.GetAxisRaw ("Horizontal");
			float inputY = Input.GetAxisRaw ("Vertical");

			movement = new Vector2 (
				speed.x * inputX/4, 
				speed.y * inputY/4
			);
			anim.SetFloat ("Speed", Math.Abs(inputX));
		} else {
			var normilizeDirection = 0;
			if (direction.x > 0) {
				normilizeDirection = 1;	
			} else {
				if(direction.x < 0){
					normilizeDirection = -1;	
				}
			}


			movement = new Vector2 (
				speed.x * normilizeDirection/4, 
				speed.y * direction.y
			);
			anim.SetFloat ("Speed", Math.Abs(direction.x));
		}

		if (movement.x < 0 && toForward) {
			Flip ();
		} else {
			if(movement.x > 0 && !toForward){
				Flip ();
			}
		}
			
		if (isGrounded && (Input.GetKeyDown (KeyCode.Space) || jumpButton)) {
			anim.SetBool ("Ground", false);
			gameObject.GetComponent<Rigidbody> ().AddForce (new Vector2(0,250));
		}

		cameraPositionVector.x = player.transform.position.x;
		cameraPositionVector.y = player.transform.position.y;
		cameraPosition.position = cameraPositionVector;
		Vector3 backgroundPositinon = new Vector3 (player.transform.position.x - 2.5f, player.transform.position.y, 5);
		background.transform.position = backgroundPositinon;
	}

	void FixedUpdate(){

		isGrounded = Physics.CheckSphere (groundCheck.position, groundRadius, whatIsGround);

		anim.SetBool ("Ground", isGrounded);
		anim.SetFloat ("vSpeed", GetComponent<Rigidbody> ().velocity.y);


		gameObject.GetComponent<Rigidbody> ().velocity = new Vector2(movement.x, GetComponent<Rigidbody> ().velocity.y);
		//GetComponent<Rigidbody2D>().velocity = movement;
	
	
	}

	private void Flip(){
		toForward = !toForward;
		Vector3 newScale = gameObject.transform.localScale;
		newScale.x *=  -1;
		transform.localScale = newScale;
	}
}
