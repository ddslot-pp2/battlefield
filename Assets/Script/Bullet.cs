using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : Entity
{
    private Int64 Id_;
    private Vector3 Pos_;
    private Vector3 Dir_;

    private float Speed_;
    private float Distance_;

    void Start ()
    {
        this.transform.position = Pos_;
	}
	
	void Update ()
    {
        Debug.Log("bullet update called\n");
        this.transform.position = this.transform.position + (Dir_ * Speed_ * Time.deltaTime);
        //this.transform.position = Pos_;
    }

    public void SetProperty(Int64 Id, Vector3 Pos, Vector3 dir, float Speed, float Distance)
    {
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
