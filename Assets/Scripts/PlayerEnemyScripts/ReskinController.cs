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

	/*****************Sprites************/
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
	/*****************************************/


	List<Sprite> armorSprites;
	List<Sprite> weaponSprites;


	public int meleeWeaponSkinID = 0;
	public int fireWeaponSkinID = 0;
	public int elementalWeaponSkinID = 0;
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

		armorSprites = new List<Sprite> (Resources.LoadAll<Sprite>("Textures/armorSprites"));
		weaponSprites = new List<Sprite> (Resources.LoadAll<Sprite>("Textures/weaponSprites"));


		//headRenderer.sprite = armorSprites.Find (s => s.name == "head");
		SetHelm (0);
		SetChest (0);
		SetLegs (0);
		SetMeleeWeapon (6);
		SetSpritesToRenderers ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.X)) {
			//int lol = Random.Range (0, 64);
			SetHelm (38);
			SetChest (38);
			SetLegs (38);
			//SetWeapon ("m_w_13");
		}
	}
		

	void LateUpdate(){
		headRenderer.sprite = headSprite;
		helmRenderer.sprite = helmSprite;
		torsRenderer.sprite = torsSprite;

		leftArmRenderer.sprite = armSprite;
		leftForearmRenderer.sprite = forearmSprite;

		if (leftWristRenderer.sprite == null) {
			leftWristRenderer.sprite = wristSprite;
		} else {
			string[] leftWristType = leftWristRenderer.sprite.name.Split ('-');
			if (leftWristType [leftWristType.Length - 1] == "wrist") {
				leftWristRenderer.sprite = wristSprite;
			}
			if (leftWristType [leftWristType.Length - 1] == "wrist90") {
				leftWristRenderer.sprite = wrist90Sprite;
			}
			if (leftWristType [leftWristType.Length - 1] == "wrist180") {
				leftWristRenderer.sprite = wrist180Sprite;
			}
		}


		rightArmRenderer.sprite = armSprite;
		rightForearmRenderer.sprite = forearmSprite;
		if (rightWristRenderer.sprite == null) {
			rightWristRenderer.sprite = wristSprite;
		} else {
			//Debug.Log (rightWristRenderer.sprite);
			string[] rightWristType = rightWristRenderer.sprite.name.Split ('-');
			if (rightWristType [rightWristType.Length - 1] == "wrist") {
				rightWristRenderer.sprite = wristSprite;
			}
			if (rightWristType [rightWristType.Length - 1] == "wrist90") {
				rightWristRenderer.sprite = wrist90Sprite;
			}
			if (rightWristType [rightWristType.Length - 1] == "wrist180") {
				rightWristRenderer.sprite = wrist180Sprite;
			}
		}

		weaponRenderer.sprite = weaponSprite;

		rightLegRenderer.sprite = legSprite;
		rightShinRenderer.sprite = shinSprite;
		if (rightFeetRenderer.sprite == null) {
			
			rightFeetRenderer.sprite = feetSpriteA;
		} else {
			string[] rightFeetType = rightFeetRenderer.sprite.name.Split ('-');
			if (rightFeetType [rightFeetType.Length - 1] == "a" || rightFeetType [rightFeetType.Length - 1] == "feet_a") {
				rightFeetRenderer.sprite = feetSpriteA;
				//Debug.Log ("lol2");
			}
			if (rightFeetType [rightFeetType.Length - 1] == "b" || rightFeetType [rightFeetType.Length - 1] == "feet_b") {
				rightFeetRenderer.sprite = feetSpriteB;
			}
		}

		leftLegRenderer.sprite = legSprite;
		leftShinRenderer.sprite = shinSprite;
		if (leftFeetRenderer.sprite == null) {
			leftFeetRenderer.sprite = feetSpriteA;
		} else {
			string[] leftFeetType = leftFeetRenderer.sprite.name.Split ('-');
			if (leftFeetType [leftFeetType.Length - 1] == "a" || leftFeetType [leftFeetType.Length - 1] == "feet_a") {
				leftFeetRenderer.sprite = feetSpriteA;
			}
			if (leftFeetType [leftFeetType.Length - 1] == "b" || leftFeetType [leftFeetType.Length - 1] == "feet_b") {
				leftFeetRenderer.sprite = feetSpriteB;
			}
		}
		//rightFeetRenderer.sprite = fee
	}





	public void SetHelm(int id){
		//if (setName == "standart") {
		headSprite = armorSprites.Find (s => s.name == "head");
		//	helmSprite = armorSprites.Find (s => s.name == "hairWF");
		//}else{
		helmSprite = armorSprites.Find (s => s.name == "head_" + id.ToString());
		//}
	}

	public void SetChest(int id){
		/*if (setName == "standart") {
			torsSprite = armorSprites.Find (s => s.name == "tors_m");
			armSprite = armorSprites.Find (s => s.name == "legs_m");
			forearmSprite = armorSprites.Find (s => s.name == "elbow_m");
			wristSprite = armorSprites.Find (s => s.name == "wrist"); 
			wrist90Sprite = armorSprites.Find (s => s.name == "wrist90"); 
			wrist180Sprite = armorSprites.Find (s => s.name == "wrist180");
		} else {*/
		torsSprite = armorSprites.Find (s => s.name == "tors_" + id.ToString());
		armSprite = armorSprites.Find (s => s.name == "arm_" + id.ToString());
		forearmSprite = armorSprites.Find (s => s.name == "elbow_" + id.ToString());
		wristSprite = armorSprites.Find (s => s.name == "wrist_" + id.ToString()); 
		wrist90Sprite = armorSprites.Find (s => s.name == "wrist90_" + id.ToString()); 
		wrist180Sprite = armorSprites.Find (s => s.name == "wrist180_" + id.ToString());
		//}
	}

	public void SetLegs(int id){
		/*if (setName == "standart") {
			legSprite = armorSprites.Find (s => s.name ==  "r_m_leg");
			shinSprite = armorSprites.Find (s => s.name == "shin_m");
			feetSpriteA = armorSprites.Find (s => s.name ==  "feet_a");
			feetSpriteB = armorSprites.Find (s => s.name ==  "feet_b");
		} else {*/
		legSprite = armorSprites.Find (s => s.name == "leg_" + id.ToString());
		shinSprite = armorSprites.Find (s => s.name == "shin_" + id.ToString());
		feetSpriteA = armorSprites.Find (s => s.name == "feet_a_" + id.ToString());
		feetSpriteB = armorSprites.Find (s => s.name == "feet_b_" + id.ToString());
		//}
	}

	public void ChangeSpec(int specId){
		if (specId == Stats.meleeSpec) {
			SetMeleeWeapon (meleeWeaponSkinID);
		} else if (specId == Stats.fireSpec) {
			SetFireWeapon (fireWeaponSkinID);
		} else if (specId == Stats.elementalSpec) {
			SetElementalWeapon (elementalWeaponSkinID);
		}
	}

	public void SetMeleeWeapon(int id){
		weaponSprite = weaponSprites.Find (s => s.name == "m_w_"+ id.ToString());
	}

	public void SetFireWeapon(int id){
		weaponSprite = weaponSprites.Find (s => s.name == "r_w_" + id.ToString());
	}

	public void SetElementalWeapon(int id){
		weaponSprite = weaponSprites.Find (s => s.name == "e_w_" + id.ToString());
	}

	void SetSpritesToRenderers(){
		headRenderer.sprite = headSprite;
		helmRenderer.sprite = helmSprite;
		torsRenderer.sprite = torsSprite;

		leftArmRenderer.sprite = armSprite;
		leftForearmRenderer.sprite = forearmSprite;
		leftWristRenderer.sprite = wristSprite;

		rightArmRenderer.sprite = armSprite;
		rightForearmRenderer.sprite = forearmSprite;
		rightWristRenderer.sprite = wristSprite;

		weaponRenderer.sprite = weaponSprite;

		rightLegRenderer.sprite = legSprite;
		rightShinRenderer.sprite = shinSprite;
		rightFeetRenderer.sprite = feetSpriteA;

		leftLegRenderer.sprite = legSprite;
		leftShinRenderer.sprite = shinSprite;
		leftFeetRenderer.sprite = feetSpriteA;
	}


	public void SetRightWrist(){
		rightWristRenderer.sprite = wristSprite;		
	}

	public void SetRightWrist90(){
		rightWristRenderer.sprite = wrist90Sprite;
	}

	public void SetRightWrist180(){
		rightWristRenderer.sprite = wrist180Sprite;
	}

	public void SetLeftWrist(){
		leftWristRenderer.sprite = wristSprite;
	}

	public void SetLeftWrist90(){
		leftWristRenderer.sprite = wrist90Sprite;
	}

	public void SetLeftWrist180(){
		leftWristRenderer.sprite = wrist180Sprite;
	}

	public void SetLeftFeetA(){
		leftFeetRenderer.sprite = feetSpriteA;
	}

	public void SetLeftFeetB(){
		leftFeetRenderer.sprite = feetSpriteB;
	}

	public void SetRightFeetA(){
		rightFeetRenderer.sprite = feetSpriteA;
		//Debug.Log ("lol");
		//Debug.Log ("lol2");
	}

	public void SetRightFeetB(){
		rightFeetRenderer.sprite = feetSpriteB;
		//Debug.Log ("lol");
	}
}
