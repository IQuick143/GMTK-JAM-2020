using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item Data", menuName = "Create Item Data", order = 51)]
public class ItemData : ScriptableObject {
	public List<ItemDataPoint> data = new List<ItemDataPoint>();
}

[System.Serializable]
public class ItemDataPoint {
	public ItemType Key;
	public GameObject Prefab;
	public GameObject Model;
}
