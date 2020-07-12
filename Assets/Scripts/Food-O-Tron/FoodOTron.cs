using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FoodOTron : MonoBehaviour {
	[SerializeField]
	private ItemType Product;
	[SerializeField]
	private int ProductCount = 1;
	[SerializeField]
	private ItemType[] RequiredItems;
	[SerializeField]
	private Conveyor conveyor;
	[SerializeField]
	private Transform ItemSpawn;
	[SerializeField]
	private Transform RequireBox;
	private GameObject RequireModel = null;
	[SerializeField]
	private Transform ProductBox;
    [SerializeField]
    private FireController FireController;
    [SerializeField]
    private float fireChance = 0.4f;
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
        FireController.fireAmount = -1.0f;

    }
	
	void OnTriggerEnter(Collider other) {
        if (FireController.fireAmount <= 0.0f)
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                var type = item.type;
                for (int i = 0; i < RequiredItems.Length; i++)
                {
                    if (!itemsSatisfied[i] && type == RequiredItems[i])
                    {
                        itemsSatisfied[i] = true;
                        break;
                    }
                }
                bool makeItem = true;
                for (int i = 0; i < RequiredItems.Length; i++)
                {
                    if (!itemsSatisfied[i])
                    {
                        makeItem = false;
                        missing = RequiredItems[i];
                        break;
                    }
                }
                if (makeItem)
                {
                    StartCoroutine(MakeItems());
                    itemsSatisfied = new bool[RequiredItems.Length];
                    missing = RequiredItems[0];
                }
                Destroy(other.gameObject);
            }
        }
	}

	private IEnumerator MakeItems() {
		for (int i = 0; i < ProductCount; i++) {
			Instantiate(ItemManager.GetPrefab(Product), ItemSpawn.position, Quaternion.identity);
			yield return new WaitForSeconds(1f / conveyor.speed);
		}

        float random_chance = UnityEngine.Random.Range(0.0f, 1.0f);
        if (random_chance < fireChance)
        {
            FireController.fireAmount = 75.0f;
        }
	}
}
