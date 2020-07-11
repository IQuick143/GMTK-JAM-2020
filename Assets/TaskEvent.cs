using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TaskEvent : MonoBehaviour
{
    //[SerializeField] private GameObject indicator;

    [Serializable]
    public class IndicatorReference
    {
        public string food_name;
        public GameObject indicator_object;
    }

    [SerializeField] private List<IndicatorReference> food_indicators = new List<IndicatorReference>();

    private List<string> food_orders = new List<string>();

    // Update is called once per frame
    void Update()
    {
        if (food_orders.Count > 0)
        {
            foreach (var indicators in food_indicators)
            {
                if (indicators.food_name == food_orders[0])
                {
                    indicators.indicator_object.SetActive(true);
                }
                else
                {
                    indicators.indicator_object.SetActive(false);
                }
            }
        }
        else
        {
            foreach (var indicators in food_indicators)
            {
                indicators.indicator_object.SetActive(false);
            }
        }
    }

    private bool IsFood(string _food)
    {
        foreach (var indicator in food_indicators)
        {
            if (indicator.food_name == _food)
                return true;
        }
        return false;
    }

    public void SetFood(string _food)
    {
        food_orders.Add(_food);
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (food_orders.Count > 0)
        {
            if ((_other.tag == food_orders[0]) && (enabled == true))
            {
                food_orders.RemoveAt(0);
                Destroy(_other.gameObject);
            }
            else if (IsFood(_other.tag))
            {
                // Ate the food but didn't order it
                Destroy(_other.gameObject);
            }
        }
        else if (IsFood(_other.tag))
        {
            // Ate the food but didn't order it
            Destroy(_other.gameObject);
        }
    }
}
