using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : Item
{

	protected override void Init () {

		base.Init();

		Type_ = Item.Type.Shield_Item;
	}

	public override void EntityUpdate () 
	{
		transform.RotateAround(transform.position, transform.up, Time.deltaTime * 45.0f);
	}
}
