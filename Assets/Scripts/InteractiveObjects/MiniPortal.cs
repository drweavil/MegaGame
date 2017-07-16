using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniPortal : MonoBehaviour {
	public int chunkId;
	public SerializableVector2 coordInChunkRudimentInfoForMiniPortalsArray;
	public int type;
	public Transform playerTransform;
	public bool isActive = true;
	//public BoxCollider teleportToCollider;
	public static int left = 0, right = 1, up = 2, down = 3; 

	void Awake(){
		playerTransform = LevelController.player.transform;
	}

	void OnTriggerEnter(Collider col){
		//Debug.Log ("lol");
		PlayerScript player = col.gameObject.GetComponent<PlayerScript> ();
		if (isActive) {
			if (type == left) {
				GameObject[] tmp = GameObject.FindGameObjectsWithTag ("PortalR");
				List<GameObject> objs = new List<GameObject> (tmp);
				int toPortalId = LevelController.levelChunkRudimentsInfoForMiniPortals [(int)coordInChunkRudimentInfoForMiniPortalsArray.y + 1, (int)coordInChunkRudimentInfoForMiniPortalsArray.x].chunkId;
				if (toPortalId != -1) {
					GameObject teleportTo = objs.Find (p => p.GetComponent<MiniPortal> ().chunkId == toPortalId);
					if (teleportTo != null) {
						teleportTo.GetComponent<MiniPortal> ().isActive = false;
						playerTransform.position = teleportTo.transform.position;
					}
				}
			}

			if (type == right) {
				GameObject[] tmp = GameObject.FindGameObjectsWithTag ("PortalL");
				List<GameObject> objs = new List<GameObject> (tmp);
				int toPortalId = LevelController.levelChunkRudimentsInfoForMiniPortals [(int)coordInChunkRudimentInfoForMiniPortalsArray.y + 1, (int)coordInChunkRudimentInfoForMiniPortalsArray.x + 2].chunkId;
				if (toPortalId != -1) {
					GameObject teleportTo = objs.Find (p => p.GetComponent<MiniPortal> ().chunkId == toPortalId);
					if (teleportTo != null) {
						teleportTo.GetComponent<MiniPortal> ().isActive = false;
						playerTransform.position = teleportTo.transform.position;
					}
				}
			}

			if (type == up) {
				GameObject[] tmp = GameObject.FindGameObjectsWithTag ("PortalD");
				List<GameObject> objs = new List<GameObject> (tmp);
				int toPortalId = LevelController.levelChunkRudimentsInfoForMiniPortals [(int)coordInChunkRudimentInfoForMiniPortalsArray.y, (int)coordInChunkRudimentInfoForMiniPortalsArray.x + 1].chunkId;
				if (toPortalId != -1) {
					GameObject teleportTo = objs.Find (p => p.GetComponent<MiniPortal> ().chunkId == toPortalId);
					if (teleportTo != null) {
						teleportTo.GetComponent<MiniPortal> ().isActive = false;
						playerTransform.position = teleportTo.transform.position;
					}
				}
			}

			if (type == down) {
				GameObject[] tmp = GameObject.FindGameObjectsWithTag ("PortalU");
				List<GameObject> objs = new List<GameObject> (tmp);
				int toPortalId = LevelController.levelChunkRudimentsInfoForMiniPortals [(int)coordInChunkRudimentInfoForMiniPortalsArray.y + 2, (int)coordInChunkRudimentInfoForMiniPortalsArray.x + 1].chunkId;
				if (toPortalId != -1) {
					GameObject teleportTo = objs.Find (p => p.GetComponent<MiniPortal> ().chunkId == toPortalId);
					if (teleportTo != null) {
						teleportTo.GetComponent<MiniPortal> ().isActive = false;
						playerTransform.position = teleportTo.transform.position;
					}
				}
			}
		}
	}

	void OnTriggerExit(Collider col){
		GetComponent<MiniPortal> ().isActive = true;
	}

}
