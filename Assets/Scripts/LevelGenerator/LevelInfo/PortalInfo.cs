using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class PortalInfo{
	public int portalId;
	public int linkWithPortalId;
	public int linkWithLevel;
	public SerializableVector3 portalCoord;

	public void SetData(PortalScript portal){
		portalId = portal.portalId;
		linkWithPortalId = portal.linkWithPortalId;
		linkWithLevel = portal.linkWithLevel;
		Vector3 poralPosition = portal.gameObject.transform.position;
		portalCoord = new SerializableVector3(poralPosition.x, poralPosition.y, poralPosition.z);
	}
}
