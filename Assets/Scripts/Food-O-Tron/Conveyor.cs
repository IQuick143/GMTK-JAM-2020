using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour {
	[SerializeField]
	private float speed = 1f;
	
	void OnTriggerStay(Collider other) {
		Item item = other.GetComponent<Item>();
		if (item != null) {
			float horizontalCorrection = Vector3.Dot(this.transform.position - other.transform.position, this.transform.up);
			var rb = item.GetComponent<Rigidbody>();
			rb.velocity = new Vector3(0, rb.velocity.y, 0) + horizontalCorrection * this.transform.up - speed * this.transform.right;
		}
	}
}
