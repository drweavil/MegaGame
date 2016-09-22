using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {
	Timer effectTimer = new Timer();
	Timer durationTimer = new Timer();
	bool hasTimer = false;
	public string path;
	//GameObject effect;
	ParticleSystem particleSystem;
	Transform transform;

	void Update(){
		if (hasTimer) {
			if (durationTimer.TimeIsOver ()) {
				particleSystem.Stop ();
				ParticleSystem.EmissionModule emission = particleSystem.emission;
				emission.enabled = false;
				if (effectTimer.TimeIsOver ()) {
					//particleSystem.Stop ();
					ObjectsPool.PushObject (path, this.gameObject);
					hasTimer = false;
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.I)) {
			particleSystem.Pause ();
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			particleSystem.Play ();
		}
	}

	void Awake(){
		particleSystem = GetComponent<ParticleSystem> ();
		//particleSystem.Stop ();
		transform = GetComponent<Transform> ();
	}

	public void StartEffect(EffectOptions options){
		transform.localPosition = options.transformPosition;
		float duration;
		if (options.isRandomDuration) {
			duration = Random.Range (options.minRandomDuration, options.maxRandomDuration + 0.000001f);
		} else {
			duration = options.duration;
		}
		if (!options.loop) {
			hasTimer = true;
			durationTimer.SetTimer (duration);
			effectTimer.SetTimer (particleSystem.startDelay + duration + (particleSystem.startLifetime-0.05f));
		}
		particleSystem.Play ();
		ParticleSystem.EmissionModule emission = particleSystem.emission;
		emission.enabled = true;

	}

	public void StopEffect(){
		particleSystem.Stop ();
		hasTimer = true;
		durationTimer.SetTimer (0);
		effectTimer.SetTimer(particleSystem.startLifetime-0.05f);
	}
}
