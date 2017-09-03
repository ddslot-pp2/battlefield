using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Entity
{
    public enum Type { Hp_Item = 0, Medal_Item = 1, Coin_Item = 2, Shield_Item = 3, Speed_Item = 4 };
	protected Transform MyTransform;


    protected override void Init ()
    {
		base.Init();
		MyTransform = this.transform;
	}

	public override void Release()
	{
		ItemManager.Instance.ItemGain (new Vector3(MyTransform.position.x, 3.0f, MyTransform.position.z));
		gameObject.Recycle();
	}

	public override void EntityUpdate () 
	{
		
	}
		
    protected Type Type_;
}
