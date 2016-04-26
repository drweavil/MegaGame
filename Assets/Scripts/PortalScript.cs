using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PortalScript : MonoBehaviour {

	public Transform teleportButton;
	public int portalId;
	public int linkWithPortalId;
	public int linkWithLevel;

	void Awake(){
		teleportButton = GameObject.Find ("ButtonsController").GetComponent<ButtonsController> ().teleportButton;
		linkWithLevel = -1;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider player){
		teleportButton.GetComponent<ButtonScript> ().teleport = this;
		teleportButton.gameObject.SetActive(true);
		//Debug.Log (this.gameObject.name);
	}

	void OnTriggerExit(Collider player){
		teleportButton.gameObject.SetActive(false);
	}

	public void SetData(PortalInfo portalInfo){
		portalId = portalInfo.portalId;
		linkWithPortalId = portalInfo.linkWithPortalId;
		linkWithLevel = portalInfo.linkWithLevel;
		this.gameObject.transform.position = new Vector3(portalInfo.portalCoord.x, portalInfo.portalCoord.y, portalInfo.portalCoord.z);
	}
}
