using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

	public Transform shotPrefab;
	public float shootingRate = 0.25f;

	private float shootColdown = 0f;

	// Use this for initialization
	void Start () {
		shootColdown = 0f;	
	}
	
	// Update is called once per frame
	void Update () {
		if(shootColdown > 0){
			shootColdown -= Time.deltaTime;			
		}
	
	}

	public void Attack(bool isEnemy){
		shootColdown = shootingRate;
		var shotTransform = Instantiate (shotPrefab) as Transform; 
		shotTransform.position = transform.position;
		ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript> ();

	}
}
