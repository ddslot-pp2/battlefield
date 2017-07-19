using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpItem : Item
{

	protected override void Init () {

		base.Init();

		Type_ = Item.Type.Hp_Item;
	}
		
	public override void EntityUpdate () 
	{
		transform.RotateAround(transform.position, transform.up, Time.deltaTime * 45.0f);
	}


}
