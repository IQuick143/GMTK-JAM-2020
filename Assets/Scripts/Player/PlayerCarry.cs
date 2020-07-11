using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarry : MonoBehaviour {
	[SerializeField]
	private float carryDistance = 1f;
	[SerializeField]
	private float carryHeight = 1f;
	[SerializeField]
	private float pickUpDistance = 3f;
	[SerializeField]
	private Item item;

	void Update() {
		if (this.item != null) {
			this.item.targetPosition = this.transform.position + this.transform.forward * this.carryDistance + this.transform.up * this.carryHeight;
			if (item.orientItem) {
				Vector3 difference = (this.item.transform.position - this.transform.position).normalized;
				Vector3 itemForward = difference - Vector3.Dot(difference, this.transform.up) * this.transform.up;
				this.item.transform.rotation = Quaternion.LookRotation(itemForward, this.transform.up);
			}
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				this.item.Drop();
				this.item = null;
			}
		} else if (Input.GetKeyDown(KeyCode.Mouse0)) {
			//Grab an item
			var items = Physics.OverlapSphere(this.transform.position, pickUpDistance, 1 << 8);
			Debug.Log(items.Length);
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
