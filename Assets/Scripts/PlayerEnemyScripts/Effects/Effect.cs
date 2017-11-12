using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {
	public Timer effectTimer = new Timer();
	public Timer durationTimer = new Timer();
	public bool hasTimer = false;
	public string path;
	//GameObject effect;
	public ParticleSystem particleSystem;
	public float nullRadius;
	public float distance;
	Transform transform;

	public ParticleSystem childParticle;
	public float nullChildRadius;



	public Vector3 nullPositionCoords;
	public Vector3 nullRotationCoords;
	public Vector3 nullScaleCoords;



	//public GameObject lol;

	void Update(){
		if (hasTimer) {
			if (durationTimer.TimeIsOver ()) {
				particleSystem.Stop ();
				particleSystem.Clear ();
				//particleSystem.Clear ();
				//ParticleSystem.EmissionModule emission = particleSystem.emission;
				//emission.enabled = false;
				//particleSystem.loop = false;
				if (effectTimer.TimeIsOver ()) {
					//particleSystem.Stop ();
					//particleSystem.Clear ();
					ObjectsPool.PushObject (path, this.gameObject);
					hasTimer = false;
				}
			}
		}

		/*if (Input.GetKeyDown (KeyCode.I)) {
			ParticleSystem.ShapeModule shape = particleSystem.shape;
			shape.radius = 10;
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			//lol.SetActive (true);
			particleSystem.Play ();
		}*/
	}

	void Awake(){
		particleSystem = GetComponent<ParticleSystem> ();
		particleSystem.Stop ();
		transform = GetComponent<Transform> ();
	}

	public void StartEffect(EffectOptions options){
		
		if (options.isLocalPosition) {
			transform.localPosition = options.transformPosition;
		} else {
			transform.position = options.transformPosition;
		}
		transform.rotation = Quaternion.Euler (nullRotationCoords);
		if (options.revertRotationY) {
			transform.rotation = Quaternion.Euler(new Vector3(nullRotationCoords.x, nullRotationCoords.y * -1, nullRotationCoords.z));
		}
		//transform.localPosition = options.transformPosition;
		float duration;
		if (options.isRandomDuration) {
			duration = Random.Range (options.minRandomDuration, options.maxRandomDuration + 0.000001f);
		} else {
			duration = options.duration;
		}
		if (!options.loop) {
			hasTimer = true;
			durationTimer.SetTimer (duration);
			if (options.objectDuration != 0) {
				effectTimer.SetTimer (options.objectDuration);
			} else {
				effectTimer.SetTimer (duration + (particleSystem.startLifetime));
			}
		}
		particleSystem.Play ();
		//particleSystem.loop = true;
		//ParticleSystem.EmissionModule emission = particleSystem.emission;
		//emission.enabled = true;

	}

	public void StopEffect(bool urgently = false){
		particleSystem.Stop ();
		hasTimer = true;
		durationTimer.SetTimer (0);
		if (urgently) {
			effectTimer.SetTimer (0);
		} else {
			effectTimer.SetTimer (particleSystem.startLifetime);
		}
	}

	public void NewDuration(float time){
		durationTimer.SetTimer (time);
		effectTimer.SetTimer(time + particleSystem.startLifetime);
	}

	public void SetNullPositionCoords(Vector3 coords){
		if (nullPositionCoords == null) {
			nullPositionCoords = coords;
		}
	}

	public void ExtendDuration(float seconds){
		durationTimer.SetTimer (durationTimer.ResidualTime () + seconds);
		effectTimer.SetTimer (effectTimer.ResidualTime () + seconds);
	}

	public void SetMeleeWaveRadius(float radius){
		ParticleSystem.ShapeModule shape = particleSystem.shape; 
		shape.radius = radius;
		ParticleSystem.ShapeModule childShape = childParticle.shape; 
		childShape.radius = radius;

	}
}
