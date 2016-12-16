using UnityEngine;
using System.Collections;
using System;

public class MovementController : MonoBehaviour {
	public static Vector2 speed = new Vector2 (2.5f, 1f);

	public Vector2 movement;
	public Vector2 externalMovement = new Vector2(0, 0);

	private Animator anim;
	private bool toForward = true;

	public Transform groundCheck;
	public bool isJumpEnd;
	public Transform jumpEndCheck;

	private float groundRadius = 0.1f;
	public LayerMask whatIsGround;

	public Rigidbody rigidbody;
	public bool isJumpIdleProcess = false;
	public bool startJumpAnimationInAction = false;

	public bool inJump = false;


	public Stats stats;
	public bool isGrounded = true;
	//bool onlyMovementStun = false;
	bool dontUseVelocityY = false;




	void Awake () {
		anim = gameObject.GetComponent<Animator> ();
		anim.SetBool ("OnGround", true);
	}

	void Update () {
		//Debug.Log (movement);
		if (!isGrounded  && !inJump ) {
			anim.Play ("jumpIdle");
			inJump = true;
		}

		if (((anim.GetCurrentAnimatorStateInfo (0).shortNameHash == Animator.StringToHash ("jumpIdle")) ||
			(anim.GetCurrentAnimatorStateInfo (0).shortNameHash == Animator.StringToHash ("jumpStartIdle"))) && isJumpEnd && !startJumpAnimationInAction) {
			anim.Play ("jumpEnd");
			anim.SetBool ("isJump", false);
			inJump = false;
		}

		if (!stats.withoutControl) {
			if (movement.x < 0 && toForward) {
				Flip ();
			} else {
				if (movement.x > 0 && !toForward) {
					Flip ();
				}
			}
		}
	}

	void FixedUpdate(){
		isGrounded = Physics.CheckSphere (groundCheck.position, groundRadius, whatIsGround);
		isJumpEnd = Physics.CheckSphere (jumpEndCheck.position, groundRadius, whatIsGround);

		anim.SetBool ("isJump", inJump);
		anim.SetBool ("OnGround", isGrounded);
		if (!stats.withoutControl) {
			float velocityY = externalMovement.y + rigidbody.velocity.y;
			if (dontUseVelocityY) {
				velocityY = externalMovement.y;
			}
			rigidbody.velocity = new Vector2 (externalMovement.x + movement.x, velocityY);
		}	
	}
				

	private void Flip(){
		toForward = !toForward;
		Vector3 newScale = gameObject.transform.localScale;
		newScale.x *=  -1;
		transform.localScale = newScale;
		transform.rotation = Quaternion.Euler (new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z) * -1);
	}

	public void SetMovement(Vector2 newMovement){
		movement = newMovement;
		anim.SetFloat ("Speed", Math.Abs (movement.x));
	}

	public IEnumerator MovementToObjectWithTimer(float time, Transform objTransform, Vector2 movementSpeed){
		Timer timer = new Timer ();
		timer.SetTimer (time);
		//onlyMovementStun = true;
		rigidbody.useGravity = false;
		dontUseVelocityY = true;
		rigidbody.velocity = new Vector2 (rigidbody.velocity.x, 0);
		while (!timer.TimeIsOver ()) {
			Vector3 movementDirection = objTransform.position - gameObject.transform.position;
			if (Math.Abs(movementDirection.x) < 0.4f && Math.Abs(movementDirection.y) < 0.4f) {
				gameObject.transform.position = objTransform.position;
				externalMovement = new Vector2 (0, 0);
				rigidbody.velocity = new Vector2 (0, 0);
			} else {
				movementDirection.Normalize ();
				//Debug.Log (movementDirection);
				externalMovement = new Vector2 (movementDirection.x * movementSpeed.x, movementDirection.y * movementSpeed.y);
			}
			//rigidbody.velocity = new Vector2 (rigidbody.velocity.x, 0);
			yield return null;
		}
		dontUseVelocityY = false;
		rigidbody.useGravity = true;
		externalMovement = new Vector2 (0, 0);
		yield break;
	}

	public void Jump(Vector3 jump){
		anim.Play ("jumpStart");
		AddForceWithStartAnimation(jump, "jumpStart", 0);
	}


	public void AddForceWithStartAnimation(Vector3 force, string animation, int layer){
		rigidbody.velocity = new Vector3 (0, 0, 0);
		rigidbody.AddForce (force);
		inJump = true;
		StartCoroutine(WaitEndOfStartAnimation(animation, layer));
	}

	public void AddForceWithoutAnimation(Vector3 force){
		rigidbody.AddForce (force);
		inJump = true;
		StartCoroutine (WaitSeveralFramesWhileIdleAnimation(10));
	}

	IEnumerator WaitSeveralFramesWhileIdleAnimation(int frames){
		anim.Play ("jumpIdle", 0);
		for(int i = 0; i < frames; i++){
			startJumpAnimationInAction = true;
			if (i + 1 == frames) {	
				startJumpAnimationInAction = false;
				yield break;
			} else {
				yield return null;
			}
		}
	}

	IEnumerator WaitEndOfStartAnimation(string animation, int layer){
		startJumpAnimationInAction = true;
		for(int i = 0; i < 1; i++){
			yield return null;
			startJumpAnimationInAction = true;
		}
		bool iterationProcessed = anim.GetCurrentAnimatorStateInfo (layer).shortNameHash == Animator.StringToHash (animation);
		while (iterationProcessed) {
			if (anim.GetCurrentAnimatorStateInfo (layer).shortNameHash != Animator.StringToHash (animation)) {
				if (!isJumpEnd) {
					anim.Play ("jumpIdle", 0);
				}
				iterationProcessed = false;
				//Debug.Log ("aziz");
			} else {
				startJumpAnimationInAction = true;
				yield return null;
			}
		}
		startJumpAnimationInAction = false;
		yield break;
	}

	/*IEnumerator StartIdleAndJumpEndAnimation(){
		anim.Play ("jumpIdle");
	}*/
}
