using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReskinController : MonoBehaviour {
	public GameObject head;
	public GameObject helm;

	public GameObject tors;
	public GameObject leftArm;
	public GameObject leftForearm;
	public GameObject leftWrist;
	public GameObject rightArm;
	public GameObject rightForearm;
	public GameObject rightWrist;
	public GameObject weapon;

	public GameObject rightLeg;
	public GameObject rightShin;
	public GameObject rightFeet;
	public GameObject leftLeg;
	public GameObject leftShin;
	public GameObject leftFeet;



	SpriteRenderer headRenderer;
	SpriteRenderer helmRenderer;

	SpriteRenderer torsRenderer;
	SpriteRenderer leftArmRenderer;
	SpriteRenderer leftForearmRenderer;
	SpriteRenderer leftWristRenderer;
	SpriteRenderer rightArmRenderer;
	SpriteRenderer rightForearmRenderer;
	SpriteRenderer rightWristRenderer;
	SpriteRenderer weaponRenderer;

	SpriteRenderer rightLegRenderer;
	SpriteRenderer rightShinRenderer;
	SpriteRenderer rightFeetRenderer;
	SpriteRenderer leftLegRenderer;
	SpriteRenderer leftShinRenderer;
	SpriteRenderer leftFeetRenderer;


	Sprite headSprite;
	Sprite helmSprite;

	Sprite torsSprite;
	Sprite armSprite;
	Sprite forearmSprite;
	Sprite wristSprite;
	Sprite wrist90Sprite;
	Sprite wrist180Sprite;


	Sprite weaponSprite;

	Sprite legSprite;
	Sprite shinSprite;
	Sprite feetSpriteA;
	Sprite feetSpriteB;


	List<Sprite> armorSprites;
	List<Sprite> weaponSprites;

	// Use this for initialization
	void Awake () {
		/************************Initialize renderers*************************************/
		headRenderer = head.GetComponent<SpriteRenderer> ();
		helmRenderer = helm.GetComponent<SpriteRenderer> ();
		torsRenderer = tors.GetComponent<SpriteRenderer> ();

		leftArmRenderer = leftArm.GetComponent<SpriteRenderer> ();
		leftForearmRenderer = leftForearm.GetComponent<SpriteRenderer> ();
		leftWristRenderer = leftWrist.GetComponent<SpriteRenderer> ();

		rightArmRenderer = rightArm.GetComponent<SpriteRenderer> ();
		rightForearmRenderer = rightForearm.GetComponent<SpriteRenderer> ();
		rightWristRenderer = rightWrist.GetComponent<SpriteRenderer> ();

		weaponRenderer = weapon.GetComponent<SpriteRenderer> ();

		rightLegRenderer = rightLeg.GetComponent<SpriteRenderer> ();
		rightShinRenderer = rightShin.GetComponent<SpriteRenderer> ();
		rightFeetRenderer = rightFeet.GetComponent<SpriteRenderer> ();

		leftLegRenderer = leftLeg.GetComponent<SpriteRenderer> ();
		leftShinRenderer = leftShin.GetComponent<SpriteRenderer> ();
		leftFeetRenderer = leftFeet.GetComponent<SpriteRenderer> ();
		/********************************************************************/

		armorSprites = new List<Sprite> (Resources.LoadAll<Sprite>("Textures/humanSprite 1"));
		weaponSprites = new List<Sprite> (Resources.LoadAll<Sprite>("Textures/kower_weap"));

		SetHelm ("standart");
		SetChest ("standart");
		SetLegs ("standart");
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown (KeyCode.Z)) {
			SetHelm ("s1-1");
			SetChest ("s1-1");
			SetLegs ("s1-1");
			SetWeapon ("r_w_13_1");
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			SetHelm ("s1-1");
			SetChest ("s1-1");
			SetLegs ("s1-1");
			SetWeapon ("m_w_13_1");
		}*/
	}
		

	void LateUpdate(){
		helmRenderer.sprite = helmSprite;
		torsRenderer.sprite = torsSprite;

		leftArmRenderer.sprite = armSprite;
		leftForearmRenderer.sprite = forearmSprite;
		string[] leftWristType = leftWristRenderer.sprite.name.Split ('-');
		if ( leftWristType[leftWristType.Length - 1] == "wrist") {
			leftWristRenderer.sprite = wristSprite;
		}
		if (leftWristType[leftWristType.Length - 1] == "wrist90") {
			leftWristRenderer.sprite = wrist90Sprite;
		}
		if (leftWristType[leftWristType.Length - 1] == "wrist180") {
			leftWristRenderer.sprite = wrist180Sprite;
		}


		rightArmRenderer.sprite = armSprite;
		rightForearmRenderer.sprite = forearmSprite;
		string[] rightWristType = rightWristRenderer.sprite.name.Split ('-');
		if ( rightWristType[rightWristType.Length - 1] == "wrist") {
			rightWristRenderer.sprite = wristSprite;
		}
		if (rightWristType[rightWristType.Length - 1] == "wrist90") {
			rightWristRenderer.sprite = wrist90Sprite;
		}
		if (rightWristType[rightWristType.Length - 1] == "wrist180") {
			rightWristRenderer.sprite = wrist180Sprite;
		}

		weaponRenderer.sprite = weaponSprite;

		rightLegRenderer.sprite = legSprite;
		rightShinRenderer.sprite = shinSprite;
		string[] rightFeetType = rightFeetRenderer.sprite.name.Split ('-');
		if(rightFeetType[rightFeetType.Length - 1] == "a" || rightFeetType[rightFeetType.Length - 1] == "feet_a"){
			rightFeetRenderer.sprite = feetSpriteA;
		}
		if(rightFeetType[rightFeetType.Length - 1] == "b" || rightFeetType[rightFeetType.Length - 1] == "feet_b"){
			rightFeetRenderer.sprite = feetSpriteB;
		}

		leftLegRenderer.sprite = legSprite;
		leftShinRenderer.sprite = shinSprite;
		string[] leftFeetType = leftFeetRenderer.sprite.name.Split ('-');
		if(leftFeetType[leftFeetType.Length - 1] == "a" || rightFeetType[rightFeetType.Length - 1] == "feet_a"){
			leftFeetRenderer.sprite = feetSpriteA;
		}
		if(leftFeetType[leftFeetType.Length - 1] == "b" || rightFeetType[rightFeetType.Length - 1] == "feet_b"){
			leftFeetRenderer.sprite = feetSpriteB;
		}
		//rightFeetRenderer.sprite = fee
	}





	void SetHelm(string setName){
		if (setName == "standart") {
			helmSprite = armorSprites.Find (s => s.name == "hair_1");
		}else{
			helmSprite = armorSprites.Find (s => s.name == setName +"-head");
		}
	}

	void SetChest(string setName){
		if (setName == "standart") {
			torsSprite = armorSprites.Find (s => s.name == "tors_m");
			armSprite = armorSprites.Find (s => s.name == "legs_m");
			forearmSprite = armorSprites.Find (s => s.name == "elbow_m");
			wristSprite = armorSprites.Find (s => s.name == "wrist"); 
			wrist90Sprite = armorSprites.Find (s => s.name == "wrist90"); 
			wrist180Sprite = armorSprites.Find (s => s.name == "wrist180");
		} else {
			torsSprite = armorSprites.Find (s => s.name == setName + "-tors");
			armSprite = armorSprites.Find (s => s.name == setName + "-legs-r");
			forearmSprite = armorSprites.Find (s => s.name == setName + "-elbow-r");
			wristSprite = armorSprites.Find (s => s.name == setName + "-wrist"); 
			wrist90Sprite = armorSprites.Find (s => s.name == setName + "-wrist90"); 
			wrist180Sprite = armorSprites.Find (s => s.name == setName + "-wrist180");
		}
	}

	void SetLegs(string setName){
		if (setName == "standart") {
			legSprite = armorSprites.Find (s => s.name ==  "r_m_leg");
			shinSprite = armorSprites.Find (s => s.name == "shin_m");
			feetSpriteA = armorSprites.Find (s => s.name ==  "feet_a");
			feetSpriteB = armorSprites.Find (s => s.name ==  "feet_b");
		} else {
			legSprite = armorSprites.Find (s => s.name == setName + "-leg-r");
			shinSprite = armorSprites.Find (s => s.name == setName + "-shin");
			feetSpriteA = armorSprites.Find (s => s.name == setName + "-feet-a");
			feetSpriteB = armorSprites.Find (s => s.name == setName + "-feet-b");
		}
	}

	void SetWeapon(string setName){
		weaponSprite = weaponSprites.Find (s => s.name == setName);
	}
}
