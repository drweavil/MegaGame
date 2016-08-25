using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public const int melee = 0, fire = 1, magic = 2;
	public static int currentSpec = 0;
	public  GameObject player;
	public static int maximumComplexity = 1200;
}
