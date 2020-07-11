using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handle an item object
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour {
	public ItemType type;
	public bool held;

	public bool orientItem = false;	
	public Vector3 targetPosition = Vector3.zero;

	public new Rigidbody rigidbody;
	public new Collider collider;

	void Start() {
		//Set the collider layer
		this.gameObject.layer = 8;
		this.collider = this.GetComponent<Collider>();
		this.rigidbody = this.GetComponent<Rigidbody>();
	}

	void Update() {
		if (held) this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, 0.25f);
	}

	public void Grab() {
		if (!held) {
			held = true;
			collider.enabled = false;
			rigidbody.isKinematic = true;
		}
	}

	public void Drop() {
		if (held) {
			held = false;
			collider.enabled = true;
			rigidbody.isKinematic = false;
		}
	}
}

// All the items we need need to have an entry in the enum
public enum ItemType {
	PerfectlyGenericObject,
	Cheese
}
