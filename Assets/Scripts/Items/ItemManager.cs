using System.Collections.Generic;
using UnityEngine;

public static class ItemManager {
	private static Dictionary<ItemType, GameObject> model = null;
	private static Dictionary<ItemType, GameObject> prefabs = null;

	public static GameObject GetPrefab(ItemType type) {
		if (prefabs == null) {
			LoadData();
		}
		return prefabs[type];
	}

	public static GameObject GetModel(ItemType type) {
		if (model == null) {
			LoadData();
		}
		return model[type];
	}

	private static void LoadData() {
		ItemData data = Resources.Load<ItemData>("Item Data");
		model   = new Dictionary<ItemType, GameObject>();
		prefabs = new Dictionary<ItemType, GameObject>();
		foreach (ItemDataPoint dat in data.data) {
			model[dat.Key] = dat.Model;
			prefabs[dat.Key] = dat.Prefab;
		}
	}
}
