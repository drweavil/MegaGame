using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class PlayerScript : MonoBehaviour {

	public Vector2 speed = new Vector2 (10, 0);
	public Vector2 direction; 
	private Vector2 movement;

	private Animator anim;
	private bool toForward = true;

	public bool isGrounded = true;
	bool inJump = false;
	public Transform groundCheck;
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




	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		anim.SetBool ("OnGround", true);
		player = GameObject.Find ("Player");
		cameraPosition = Camera.main.transform;
		background = GameObject.Find ("BackGround");
		stats = GetComponent<Stats> ();
		stats.isPlayerStats = true;
		//stats.playerHealthBar = 

	}
		
	void Update () {
		if (!stats.withoutControl) {
			if (isGrounded) {
				inJump = true;
			}

			if (Physics.Raycast (transform.position, new Vector3 (0, -1, 0), out rayToGroundHit, 20f, 1 << 8/*Ground*/)) {
				if ((rayToGroundHit.distance < 0.5f) && (isGrounded == false) && inJump) {
					anim.Play ("jumpEnd");
					anim.SetBool ("isJump", false);
					inJump = false;
				}
			}



			if (direction.x == 0.0 && direction.y == 0.0) {
				float inputX = Input.GetAxisRaw ("Horizontal");
				float inputY = Input.GetAxisRaw ("Vertical");

				movement = new Vector2 (
					speed.x * inputX / 4, 
					speed.y * inputY / 4
				);
				anim.SetFloat ("Speed", Math.Abs (inputX));
			} else {
				var normilizeDirection = 0;
				if (direction.x > 0) {
					normilizeDirection = 1;	
				} else {
					if (direction.x < 0) {
						normilizeDirection = -1;	
					}
				}


				movement = new Vector2 (
					speed.x * normilizeDirection / 4, 
					speed.y * direction.y
				);
				anim.SetFloat ("Speed", Math.Abs (direction.x));
			}

			if (movement.x < 0 && toForward) {
				Flip ();
			} else {
				if (movement.x > 0 && !toForward) {
					Flip ();
				}
			}
			
			if (isGrounded && (Input.GetKeyDown (KeyCode.Space) || jumpButton)) {
				anim.SetBool ("OnGround", false);
				anim.SetBool ("isJump", true);
				anim.Play ("jumpStart");
				inJump = true;
				gameObject.GetComponent<Rigidbody> ().AddForce (new Vector2 (0, 250));
			}

			//Debug.Log (GetComponent<Rigidbody> ().velocity.y);
		} //else {
		cameraPositionVector.x = player.transform.position.x;
		cameraPositionVector.y = player.transform.position.y;
		cameraPosition.position = cameraPositionVector;
		Vector3 backgroundPositinon = new Vector3 (player.transform.position.x - 2.5f, player.transform.position.y, 5);
		background.transform.position = backgroundPositinon;
		//	anim.Play ("Death");
		//}
	}

	void FixedUpdate(){
		
		isGrounded = Physics.CheckSphere (groundCheck.position, groundRadius, whatIsGround);

		anim.SetBool ("OnGround", isGrounded);
		anim.SetFloat ("vSpeed", GetComponent<Rigidbody> ().velocity.y);

		if (!stats.withoutControl) {
			gameObject.GetComponent<Rigidbody> ().velocity = new Vector2 (movement.x, GetComponent<Rigidbody> ().velocity.y);
		}	
	}

	private RaycastHit FindNearestGroundHits(RaycastHit[] hits){
		RaycastHit hit = new RaycastHit ();
		List<RaycastHit> newHits = new List<RaycastHit> (hits);
		//newHits.Min(h => h.distance);
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
