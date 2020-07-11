using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class CustomerRandomController : EntityController
{
    public Vector2 minPoint;
    public Vector2 maxPoint;

    private AudioClip eatOrderSFX;
    private AudioClip eatSFX;

    override protected void ChooseLocation()
    {
        targetLocation = new Vector3(Random.Range(minPoint.x, maxPoint.x), 0f, Random.Range(minPoint.y, maxPoint.y));
    }
    override protected void TimePassed()
    {
        ChooseLocation();
    }
}
