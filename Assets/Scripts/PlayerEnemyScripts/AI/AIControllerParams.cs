using UnityEngine;
using System.Collections;

public class AIControllerParams {
	public float distanceToPlayer = 0;
	public Vector3 currentPosition = new Vector3();
	public bool targetInLine = false;
	public bool targetSelected = false;
	public Transform target = null;
	public CharacterAPI targetAPI = null;
	public Vector3 targetDirection = new Vector3();
	public CharacterAPI characterAPI;
	public bool inChain = true;
	public bool restAgainstLeftWall = false;
	public bool restAgainstRightWall = false;
	public bool leftPrecipice = false;
	public bool rightPrecipice = false;
	public bool inMovement = false;

	public Vector3 spawnCoords;
	public float patrolRadius;

	public Vector3 patrolTarget;
	public Vector3 rangeTarget;

	public int resourceValue = 0;

	public bool rangeTargetSelected = false;
	public bool rangeTargetReselected = true;

	public bool rangeTargetIsPlayer = false;
	public float rangeDistance = 0;

	public float minimumRangeDistance = 1;
	public float maximumRangeDistance = 4;
}
