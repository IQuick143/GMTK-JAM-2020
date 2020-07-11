using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour {
	public Vector3 MoveVector = Vector3.zero;
	private new Rigidbody rigidbody;
	[SerializeField]
	private float speed = 2f;

	void Awake() {
		this.rigidbody = this.GetComponent<Rigidbody>();
	}

	void Update() {
		MoveVector = Input.GetAxis("Horizontal") * this.transform.right + Input.GetAxis("Vertical") * this.transform.forward;
		if (MoveVector.sqrMagnitude > 1) MoveVector.Normalize();
		this.rigidbody.AddForce(MoveVector * speed);
		//this.rigidbody.velocity = MoveVector * speed;
	}
}
