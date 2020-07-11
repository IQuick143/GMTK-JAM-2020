using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to move a carried item around
public class PlayerItem : MonoBehaviour {
	public Vector3 targetPosition = Vector3.zero;

	void Update() {
		this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, 0.5f);
	}

	void OnEnable() {
		var col = this.GetComponent<Collider>();
		if (col != null) col.enabled = false;
	}

	void OnDisable() {
		var col = this.GetComponent<Collider>();
		if (col != null) col.enabled = false;
	}
}
