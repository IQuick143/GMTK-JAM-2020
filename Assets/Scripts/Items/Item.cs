using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handle an item object on the floor
public class Item : MonoBehaviour {
	public ItemType type;

	public PlayerItem Grab() {
		return this.gameObject.AddComponent<PlayerItem>();
	}
}

// All the items we need need to have an entry in the enum
public enum ItemType {
	PerfectlyGenericObject,
	Cheese
}
