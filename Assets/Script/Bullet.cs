using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : Entity
{
    public enum Type { DirectBullet = 0 };
	public GameObject ExpEffect;
    private Int64 Id_;
    private Vector3 Pos_;
    private Vector3 Dir_;

    private float Speed_;
    private float Distance_;

	Transform myTransform;

    void Start ()
    {
        //this.transform.rotation = Quaternion.LookRotation(Dir_);
		myTransform = this.transform;
		myTransform.position = Pos_;
	}


	public override void Release()
	{
		ExpEffect.Spawn (myTransform.position, myTransform.rotation);
		//GameObject exp = Instantiate(ExpEffect, myTransform.position, myTransform.rotation);
		//Destroy(exp, 1.0f);
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

	/*
	void Update ()
    {
        //Debug.Log("bullet update called\n");
        this.transform.position = this.transform.position + (Dir_ * Speed_ * Time.deltaTime);
        //this.transform.position = Pos_;
        //Debug.Log("X: " + this.transform.position.x + ", z: " + this.transform.position.z);
    }
    */

    public void SetProperty(Int64 Id, Vector3 Pos, Vector3 dir, float Speed, float Distance)
    {
        ObjId = Id;
        Id_ = Id;
        Pos_ = Pos;
        Dir_ = dir;
        Speed_ = Speed;
        Distance_ = Distance;
    }

    public Int64 GetId()
    {
        return Id_;
    }

}
