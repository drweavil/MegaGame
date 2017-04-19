using UnityEngine;
using System.Collections;

public class ButtonsController : MonoBehaviour {
	public Transform teleportButton;
	public static ButtonsController buttonsController;
	public PortalScript teleport;

	void Awake(){
		buttonsController = this;
	}

	// Use this for initialization
	/*void Start () {
		teleportButton.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
	
	}*/
}
