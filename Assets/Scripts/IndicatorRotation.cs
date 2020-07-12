using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorRotation : MonoBehaviour {
	[SerializeField] private float indicatorHeight = 2f;

	void Update() {
		this.transform.rotation = Quaternion.identity;
		this.transform.position = this.transform.parent.position + Vector3.up * indicatorHeight;
	}
}
