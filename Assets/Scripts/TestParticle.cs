using UnityEngine;
using System.Collections;

public class TestParticle : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			ParticleSystem parts = GetComponent<ParticleSystem> ();
			parts.Stop();
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			ParticleSystem parts = GetComponent<ParticleSystem> ();
			parts.Play();
		}
	}
}
