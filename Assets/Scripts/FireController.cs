using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(Collider))]
public class FireController : MonoBehaviour {
	[SerializeField]
	public float fireAmount = 1f;
	[SerializeField]
	private float extenguishmentFactor = 0.01f;
	private ParticleSystem fireSystem;

	// Start is called before the first frame update
	void Start() {
		fireSystem = this.GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void Update() {
		var emission = fireSystem.emission;
		emission.rateOverTime = 1f + fireAmount;
		if (fireAmount < 0f) {
			//Fire extenguished
		}
	}

	void OnParticleCollision(GameObject other) {
		if (other.GetComponent<Extinguisher>() != null) {
			var part = other.GetComponent<ParticleSystem>();
			var col = new List<ParticleCollisionEvent>();
			int num_col = part.GetCollisionEvents(this.gameObject, col);
			fireAmount -= num_col * extenguishmentFactor;
		}
	}
}
