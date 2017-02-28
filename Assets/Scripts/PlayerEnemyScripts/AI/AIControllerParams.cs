using UnityEngine;
using System.Collections;

public class AIControllerParams {
	public float distanceToPlayer = 0;
	public Vector3 currentPosition = new Vector3();
	public bool targetInLine = false;
	public bool targetSelected = false;
	public Transform target = null;
	public CharacterAPI targetAPI = null;
	public CharacterAPI characterAPI;
	public bool inChain = true;
	public bool restAgainstLeftWall = false;
	public bool restAgainstRightWall = false;
	public bool leftPrecipice = false;
	public bool rightPrecipice = false;

	public Vector3 spawnCoords;
	public float patrolRadius;

	public Transform patrolTarget;
}
