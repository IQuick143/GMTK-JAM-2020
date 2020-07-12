using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All the items we need need to have an entry in the enum
public enum ItemType {
	Extinguisher,
	PerfectlyGenericObject,
	Dough,
	Donut,
	Burger,
	Potato,
	Fries,
	Steak
}

// Handle an item object
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour {
	public ItemType type;

	public float carryDistance = 0.9f;
	public float carryHeight = 1f;
	public bool orientItem = false;	

	public bool held = false;
	public Vector3 targetPosition = Vector3.zero;

	public Transform left;
	public Transform right;

	public new Rigidbody rigidbody;
	public new Collider collider;

	void Start() {
		//Set the collider layer
		this.gameObject.layer = 8;
		this.collider = this.GetComponent<Collider>();
		this.rigidbody = this.GetComponent<Rigidbody>();
		if (left == null) {
			left = this.transform.Find("Left");
		}
		if (right == null) {
			right = this.transform.Find("Right");
		}
	}

	void Update() {
		//No longer used
		//if (held) this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, 0.25f);
		if (held) this.rigidbody.velocity = (targetPosition - this.transform.position) * 20;
	}

	public void Grab() {
		if (!held) {
			held = true;
			collider.enabled = false;
		}
	}

	public void Drop() {
		if (held) {
			held = false;
			collider.enabled = true;
		}
	}
}
