using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FoodSpawner : MonoBehaviour {
	[Serializable]
	public class FoodSpawnpoint {
		public ItemType food;
		public Transform spawnpoint;

		public float interval;
		public float current_time;
	}

	[SerializeField] private List<FoodSpawnpoint> spawnpoints = new List<FoodSpawnpoint>();

	void Update() {
		foreach (var spawnpoint in spawnpoints) {
			spawnpoint.current_time += Time.deltaTime;

			if (spawnpoint.current_time > spawnpoint.interval) {
				Instantiate(ItemManager.GetPrefab(spawnpoint.food), spawnpoint.spawnpoint.position, Quaternion.identity);

				spawnpoint.current_time = 0.0f;
			}
		}
	}
}
