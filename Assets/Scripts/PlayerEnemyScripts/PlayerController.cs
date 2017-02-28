using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	public const int melee = 0, fire = 1, magic = 2;
	public static int currentSpec = 0;
	public static  GameObject player;
	public GameObject nonStaticPlayer;
	public static int maximumComplexity = 1200;
	public static CharacterAPI playerCharacterAPI;
	public CharacterAPI nonStaticCharacterAPI;
	public static List<CharacterAPI> leftEnemyChain = new List<CharacterAPI> ();
	public static List<CharacterAPI> rightEnemyChain = new List<CharacterAPI> ();

	void Awake(){
		player = nonStaticPlayer;
		playerCharacterAPI = nonStaticCharacterAPI;
	}
}
