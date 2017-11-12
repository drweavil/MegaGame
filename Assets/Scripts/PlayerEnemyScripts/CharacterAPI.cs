using UnityEngine;
using System.Collections;

public class CharacterAPI : MonoBehaviour {
	public ReskinController reskinController;
	public Skills skills;
	public Stats stats;
	public Transform transform;
	public SphereCollider sphereCollider;
	public BoxCollider boxCollider;
	public Rigidbody rigidbody;
	public MovementController movementController;
	public AIController aiController;
	public Animator animator;
	public PlayerHealthBar healthBar;
	public HealthBar healthBarController;


	public void SetEnemyType(int complexity, int typeId){
		aiController.SetActionsByEnemyID (typeId);
		stats.SetStatsByComplexity (complexity, typeId);

		StartCoroutine(StartProcess.StartActionAfterFewFrames(5, ()=>{
			if(BattleInterfaceController.battleInterfaceController.battleInterface.activeInHierarchy){
				if(this.gameObject.activeInHierarchy){
					healthBar.SetEnemyAvatar();
				}
			}
		}));
	}
}
