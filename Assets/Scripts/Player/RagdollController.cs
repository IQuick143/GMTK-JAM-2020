﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
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
	private float legWidth = 0.6f;
	[SerializeField]
	private float stepLength = 1f;
	[SerializeField]
	private float legSpeed = 10f;
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
		HeadRB.velocity = (HeadTarget - this.Head.position) * 15f;

		//Move the legs
		Vector3 LegTarget = this.transform.position - this.transform.right * legWidth / 2f + this.transform.forward * stepLength / 2f;
		Vector3 offset = LegTarget - LeftFootTarget;
		if ((offset.magnitude > stepLength && this.RightFootRB.velocity.sqrMagnitude < 5f) || this.LeftFootRB.velocity.magnitude > legSpeed) {
			LeftFootTarget = LegTarget;
		}
		this.LeftFootRB.velocity = (LeftFootTarget - this.LeftFoot.position) * legSpeed;

		LegTarget = this.transform.position + this.transform.right * legWidth / 2f + this.transform.forward * stepLength / 2f;
		offset = LegTarget - RightFootTarget;
		if ((offset.magnitude > stepLength && this.LeftFootRB.velocity.sqrMagnitude < 5f) || this.RightFootRB.velocity.magnitude > legSpeed) {
			RightFootTarget = LegTarget;
		}
		this.RightFootRB.velocity = (RightFootTarget - this.RightFoot.position) * legSpeed;

		//Orient the body
		this.TorsoRB.AddTorque(Vector3.Cross(this.Torso.forward, this.transform.forward) * TorsoTorque);
	}
}
