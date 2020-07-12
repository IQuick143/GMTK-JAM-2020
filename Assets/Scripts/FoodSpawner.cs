using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FoodSpawner : MonoBehaviour {
	
	public ItemType food;
	public Transform spawnpoint;
	public Transform preview;
	public float trigger_zone_size;

	private GameObject spawnedItem = null;

	void Start() {
		Instantiate(ItemManager.GetModel(food), preview).transform.position = preview.position;
	}

	void Update() {
		if (spawnedItem == null || (spawnedItem.transform.position - spawnpoint.position).magnitude > trigger_zone_size) {
			spawnedItem = Instantiate(ItemManager.GetPrefab(food), spawnpoint.position, Quaternion.identity);
		}
	}
}
