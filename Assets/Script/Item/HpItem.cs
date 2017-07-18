using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpItem : Item
{
	void Start ()
    {
        Type_ = Item.Type.HpItem;
    }
	
	void Update ()
    {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * 45.0f);
    }
}
