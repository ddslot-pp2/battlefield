using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalItem : Item
{
	Vector3 ArrivePos = Vector3.zero;
	Transform myTransform;

	protected override void Init () {

		base.Init();

		Type_ = Item.Type.Medal_Item;

		myTransform = transform;
	}

	public void SetMovePos(Vector3 pos)
	{
		Debug.Log ("SetMovePos: " + pos);
		ArrivePos = pos;
	}

	public override void EntityUpdate () 
	{
		
		if (Vector3.Distance (myTransform.position, ArrivePos) > 0.1f) {
			ArrivePos.y = 0.0f;
			Vector3 dir = (ArrivePos - myTransform.position).normalized;
			myTransform.Translate (dir * 1f * Time.deltaTime);

			Debug.Log ("medalpos: " + myTransform.position);
		}
        
		//transform.RotateAround(transform.position, transform.up, Time.deltaTime * 45.0f);
	}
}
