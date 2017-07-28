using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Entity
{
    public enum Type { Hp_Item = 0, Medal_Item = 1, Coin_Item = 2, Shield_Item = 3 };

    protected override void Init ()
    {
		base.Init();
	}

	public override void Release()
	{
		gameObject.Recycle();
	}

	public override void EntityUpdate () 
	{
		
	}
		
    protected Type Type_;
}
