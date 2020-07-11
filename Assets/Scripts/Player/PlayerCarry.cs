using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarry : MonoBehaviour {
	[SerializeField]
	private Transform Torso;
	[SerializeField]
	private Transform LeftHand;
	private Rigidbody LeftHandRB;
	[SerializeField]
	private Transform RightHand;
	private Rigidbody RightHandRB;
	[SerializeField]
	private float pickUpDistance = 3f;
	[SerializeField]
	private float grabSpeed = 10f;
	[SerializeField]
	private float itemTurningPower = 10f;
	[SerializeField]
	private Item item;

	void Start() {
		this.RightHandRB = this.RightHand.GetComponent<Rigidbody>();
		this.LeftHandRB = this.LeftHand.GetComponent<Rigidbody>();
	}

	void Update() {
		if (this.item != null) {
			this.item.targetPosition = this.transform.position + this.Torso.transform.forward * this.item.carryDistance + this.transform.up * this.item.carryHeight;
			if (this.item.right != null) this.RightHandRB.velocity = grabSpeed * (this.item.right.position - RightHand.position);
			if (this.item.left  != null) this.LeftHandRB.velocity  = grabSpeed * (this.item.left.position  - LeftHand.position);

			if (item.orientItem) {
				Vector3 difference = (this.item.transform.position - this.transform.position).normalized;
				Vector3 itemForward = difference - Vector3.Dot(difference, this.transform.up) * this.transform.up;
				this.item.rigidbody.AddTorque(Vector3.Cross(this.item.transform.forward, itemForward) * itemTurningPower);
			}
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				this.item.Drop();
				this.item = null;
			}
		} else if (Input.GetKeyDown(KeyCode.Mouse0)) {
			//Grab an item
			var items = Physics.OverlapSphere(this.transform.position, pickUpDistance, 1 << 8);
			Item closest = null;
			float smallestSqrDistance = float.MaxValue;
			for (int i = 0; i < items.Length; i++) {
				float sqrDistance = (items[i].transform.position - this.transform.position).sqrMagnitude;
				Item item = items[i].GetComponent<Item>();
				if (smallestSqrDistance > sqrDistance &&
					item != null && !item.held) {
					closest = items[i].GetComponent<Item>();
					smallestSqrDistance = sqrDistance;
				}
			}

			if (closest != null) {
				this.item = closest;
				this.item.Grab();
			}
		}
	}
}
