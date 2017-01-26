using UnityEngine;
using System.Collections;

public class EffectOptions : MonoBehaviour {
	public float minRandomDuration;
	public float maxRandomDuration;
	public bool isRandomDuration = true;
	public float duration;
	public float objectDuration = 0;
	public bool loop = false;
	public Vector3 transformPosition;
	public bool isLocalPosition = true;
	public bool revertRotationY = false;
}
