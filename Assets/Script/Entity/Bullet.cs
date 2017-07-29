using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : Entity
{
    public enum Type { DirectBullet = 0 };
	public GameObject ExpEffect;
    private Vector3 Pos_;
    private Vector3 Dir_;

    private float Speed_;
    private float Distance_;

	Transform myTransform;


	protected override void Init () {

		base.Init();

		myTransform = this.transform;
		myTransform.position = Pos_;

	}
   
	public override void Release()
	{
		//ExpEffect.Spawn (myTransform.position, myTransform.rotation);
		gameObject.Recycle();
	}

	public override void EntityUpdate () 
	{
		MoveEntity();
	}


	void MoveEntity()
	{
		this.transform.position = this.transform.position + (Dir_ * Speed_ * Time.deltaTime);
	}
		

    public void SetProperty(Int64 Id, Vector3 Pos, Vector3 dir, float Speed, float Distance)
    {
        ObjId = Id;
        Pos_ = Pos;
		Dir_ = dir.normalized;
        Speed_ = Speed;
        Distance_ = Distance;
    }

    

}
