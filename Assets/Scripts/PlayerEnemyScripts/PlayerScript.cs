using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class PlayerScript : MonoBehaviour {

	public Vector2 direction; 
	//private Vector2 movement;

	private Animator anim;
	private bool toForward = true;

	//public bool isGrounded = true;
	//public bool inJump = false;
	bool jumpEndPlayed = true;
	public Transform groundCheck;
	public bool isJumpEnd;
	public Transform jumpEndCheck;
	public int fallIndex = 0;
	private float groundRadius = 0.1f;
	public LayerMask whatIsGround;
	public bool jumpButton;

	private GameObject player;
	private Vector3 cameraPositionVector = new Vector3 (0, 0, -5);
	private Transform cameraPosition;
	private GameObject background;

	//private RaycastHit[] rayToGroundHits;
	private RaycastHit rayToGroundHit;
	public PlayerHealthBar playerHealthBar;

	Stats stats;
	public MovementController movementController;
	public PlayerController playerController;
	public Joystick joystick;




	void Awake () {
		anim = gameObject.GetComponent<Animator> ();
		anim.SetBool ("OnGround", true);
		player = GameObject.Find ("Player");
		cameraPosition = Camera.main.transform;
		background = GameObject.Find ("BackGround");
		stats = GetComponent<Stats> ();
		stats.isPlayerStats = true;
	}
		
	void Update () {
		//Debug.Log (direction);
		if (Input.GetKeyDown (KeyCode.I)) {
			stats.MakeDamage (100, Stats.physicalDamageType, true);
		}

		if (!stats.withoutControl) {
			if (direction.x == 0.0 && direction.y == 0.0) {
				float inputX = Input.GetAxisRaw ("Horizontal");
				float inputY = Input.GetAxisRaw ("Vertical");

				anim.SetFloat ("DirectionY", inputY);

				movementController.SetMovement (new Vector2 (
					MovementController.speed.x * inputX * stats.currentSpeed, 
					MovementController.speed.y * inputY 
				));
			} else {
				var normilizeDirection = 0;
				if (direction.x > 0) {
					normilizeDirection = 1;	
				} else {
					if (direction.x < 0) {
						normilizeDirection = -1;	
					}
				}
				movementController.SetMovement (new Vector2 (
					MovementController.speed.x * normilizeDirection * stats.currentSpeed, 
					MovementController.speed.y * direction.y
				));
			}
			
			if (stats.isGrounded && (Input.GetKeyDown (KeyCode.Space) || jumpButton)) {
				movementController.Jump (new Vector3(0, 250f, 0));
			}
		}
		cameraPositionVector.x = player.transform.position.x;
		cameraPositionVector.y = player.transform.position.y;
		cameraPosition.position = cameraPositionVector;
		Vector3 backgroundPositinon = new Vector3 (player.transform.position.x - 2.5f, player.transform.position.y, 5);
		background.transform.position = backgroundPositinon;
	}

	void FixedUpdate(){
		direction = joystick.GetDirection ();
		anim.SetFloat ("DirectionY", direction.y);
	}

	private RaycastHit FindNearestGroundHits(RaycastHit[] hits){
		RaycastHit hit = new RaycastHit ();
		List<RaycastHit> newHits = new List<RaycastHit> (hits);
		float minDistance = 0;
		minDistance = hits.Where(h => h.collider.gameObject.layer == LayerMask.NameToLayer("Ground")).Min(h=> h.distance);
		hit = newHits.Find (h=> h.distance == minDistance);
		return hit;
	}

	private void Flip(){
		toForward = !toForward;
		Vector3 newScale = gameObject.transform.localScale;
		newScale.x *=  -1;
		transform.localScale = newScale;
		transform.rotation = Quaternion.Euler (new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z) * -1);
	}
}
