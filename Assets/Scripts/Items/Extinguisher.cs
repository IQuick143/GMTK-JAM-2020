﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : MonoBehaviour {
	[SerializeField]
	private Item item;
	[SerializeField]
	private new ParticleSystem particleSystem;

	void Update() {
		var emission = particleSystem.emission;
		emission.enabled = item.held;
	}
}
