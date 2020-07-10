using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {
	[SerializeField]
	private PlayerMove move;
	// Start is called before the first frame update
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {
		if (move.MoveVector.sqrMagnitude > 0.0001) {
			this.transform.rotation = Quaternion.LookRotation(Vector3.Lerp(this.transform.forward, move.MoveVector, 0.2f), this.transform.up);
		}
	}
}
