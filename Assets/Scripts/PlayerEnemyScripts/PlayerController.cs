using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public const int melee = 0, fire = 1, magic = 2;
	public static int currentSpec = 0;
	public static  GameObject player;
	public GameObject nonStaticPlayer;
	public static int maximumComplexity = 1200;
	public static CharacterAPI playerCharacterAPI;
	public CharacterAPI nonStaticCharacterAPI;

	void Awake(){
		player = nonStaticPlayer;
		playerCharacterAPI = nonStaticCharacterAPI;
	}
}
