using UnityEngine;
using System.Collections;

[System.Serializable]
public class EffectOptions  {
	public float minRandomDuration;
	public float maxRandomDuration;
	public bool isRandomDuration = true;
	public float duration;
	public float objectDuration = 0;
	public bool loop = false;
	public Vector3 transformPosition;
	public bool isLocalPosition = true;
	public bool revertRotationY = false;
	public bool revertScaleX = false;
}
