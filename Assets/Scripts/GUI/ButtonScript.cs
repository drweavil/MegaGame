using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

	public bool isJump = false;
	public int jumpTime =0;
	private PlayerScript playerScript; 

	// Use this for initialization
	void Start () {
		playerScript = GameObject.Find ("Player").GetComponent<PlayerScript> ();
		//playerScript = GameObject.Find ("Player").GetComponent<LevelController> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (playerScript.jumpButton) {
			jumpTime += 1;
			if (jumpTime == 1) {
				jumpTime = 0;
				playerScript.jumpButton = false;
			}
		}
	
	}

	public void Jump(){
		playerScript.jumpButton = true;
	}

	/*public void Lol(){
		LevelController generator = GameObject.Find ("LevelController").GetComponent<LevelController>();
		generator.Test ();
	}*/


	public PortalScript teleport;
	public void Teleport(){
		LevelController levelController = GameObject.Find ("LevelController").GetComponent<LevelController> ();
		//Debug.Log (levelController.gameObject.name);
		levelController.Teleport (teleport.GetComponent<PortalScript>().portalId);
		//teleport.portalId;
	}
}
