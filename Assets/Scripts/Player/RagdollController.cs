using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
	[SerializeField]
	private PlayerMove move;
	[SerializeField]
	private Transform Head;
	private Rigidbody HeadRB;
	[SerializeField]
	private Transform LeftHand;
	private Rigidbody LeftHandRB;
	[SerializeField]
	private Transform Torso;
	private Rigidbody TorsoRB;
	[SerializeField]
	private Transform RightHand;
	private Rigidbody RightHandRB;
	[SerializeField]
	private Transform LeftFoot;
	private Rigidbody LeftFootRB;
	private Vector3 LeftFootTarget;
	[SerializeField]
	private Transform RightFoot;
	private Rigidbody RightFootRB;
	private Vector3 RightFootTarget;
	[SerializeField]
	private float height = 2f;
	[SerializeField]
	private Step step;
	[SerializeField]
	private float legWidth = 0.6f;
	[SerializeField]
	private float stepLength = 1f;
	[SerializeField]
	private float legSpeedMultiplier = 10f;
	private float legSpeed {get {return legSpeedMultiplier * this.move.speed;}}
	[SerializeField]
	private float TorsoTorque = 3f;

	// Start is called before the first frame update
	void Start() {
		this.HeadRB = this.Head.GetComponent<Rigidbody>();
		this.TorsoRB = this.Torso.GetComponent<Rigidbody>();
		this.LeftFootRB = this.LeftFoot.GetComponent<Rigidbody>();
		this.RightFootRB = this.RightFoot.GetComponent<Rigidbody>();
		this.LeftHandRB = this.LeftHand.GetComponent<Rigidbody>();
		this.RightHandRB = this.RightHand.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void FixedUpdate() {
		//Hold the head up
		Vector3 HeadTarget = this.transform.position + this.transform.up * this.height;
		HeadRB.velocity = (HeadTarget - this.Head.position) * 17.5f;

		//Move the legs
		float walking = this.move.MoveVector.magnitude;
		Vector3 LegTarget = this.transform.position - this.transform.right * legWidth / 2f + this.transform.up * 0.1f + walking * this.transform.forward * stepLength / 4f;
		Vector3 offset = LegTarget - LeftFootTarget;
		if ((offset.magnitude > stepLength && this.step == Step.Left) || (LeftFootTarget - this.LeftFoot.position).magnitude > 2*stepLength) {
			LeftFootTarget = LegTarget;
			this.step = Step.LeftMoving;
		}
		this.LeftFootRB.velocity = (LeftFootTarget - this.LeftFoot.position) * legSpeed;
		if (this.step == Step.LeftMoving && this.LeftFootRB.velocity.magnitude < legSpeed / 3f) {
			this.step = Step.Right;
		}

		LegTarget = this.transform.position + this.transform.right * legWidth / 2f + this.transform.up * 0.1f + walking * this.transform.forward * stepLength / 4f;
		offset = LegTarget - RightFootTarget;
		if ((offset.magnitude > stepLength && this.step == Step.Right) || (RightFootTarget - this.RightFoot.position).magnitude > stepLength) {
			RightFootTarget = LegTarget;
			this.step = Step.RightMoving;
		}
		this.RightFootRB.velocity = (RightFootTarget - this.RightFoot.position) * legSpeed;
		if (this.step == Step.RightMoving && this.RightFootRB.velocity.magnitude < legSpeed / 3f) {
			this.step = Step.Left;
		}

		//Orient the body
		this.TorsoRB.AddTorque(Vector3.Cross(this.Torso.forward, this.transform.forward) * TorsoTorque);
	}

	private enum Step {
		Left,
		LeftMoving,
		Right,
		RightMoving
	}
}
