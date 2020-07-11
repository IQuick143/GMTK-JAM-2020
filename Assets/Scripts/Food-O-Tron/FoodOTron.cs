using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FoodOTron : MonoBehaviour {
	[SerializeField]
	private ItemType Product;
	[SerializeField]
	private ItemType[] RequiredItems;
	[SerializeField]
	private Transform ItemSpawn;
	[SerializeField]
	private Transform RequireBox;
	private GameObject RequireModel = null;
	[SerializeField]
	private Transform ProductBox;
	private bool[] itemsSatisfied;
	private ItemType missing {
		set {
			if (RequireModel != null) Destroy(RequireModel);
			RequireModel = Instantiate(ItemManager.GetModel(value), RequireBox);
			RequireModel.transform.SetParent(RequireBox, true);
			RequireModel.transform.localPosition = Vector3.zero;
		}
	}

	// Start is called before the first frame update
	void Start() {
		itemsSatisfied = new bool[RequiredItems.Length];
		var ProductModel = Instantiate(ItemManager.GetModel(Product));
			ProductModel.transform.SetParent(ProductBox, true);
			ProductModel.transform.localPosition = Vector3.zero;
		missing = RequiredItems[0];
	}
	
	void OnTriggerEnter(Collider other) {
		Item item = other.GetComponent<Item>();
		if (item != null) {
			var type = item.type;
			for (int i = 0; i < RequiredItems.Length; i++) {
				if (!itemsSatisfied[i] && type == RequiredItems[i]) {
					itemsSatisfied[i] = true;
					break;
				}
			}
			bool makeItem = true;
			for (int i = 0; i < RequiredItems.Length; i++) {
				if (!itemsSatisfied[i]) {
					makeItem = false;
					missing = RequiredItems[i];
					break;
				}
			}
			if (makeItem) {
				Instantiate(ItemManager.GetPrefab(Product), ItemSpawn.position, Quaternion.identity);
				itemsSatisfied = new bool[RequiredItems.Length];
				missing = RequiredItems[0];
			}
			Destroy(other.gameObject);
		}
	}
}
