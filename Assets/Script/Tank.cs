
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tank : Entity {

	bool move = false;
	public Vector3 lookDirection;
	public Transform fireTransform;

	Transform playertank1;

	float h, v;
	Controller controller;
	public Tank_State state;
	HpBar hpBar;


	protected RaycastHit TFire;
	Vector3 Click;
	Vector3 ArrivePos = Vector3.zero;
	Vector3 AttackDir = Vector3.zero;

	public float nextfire = 0.0f;

    protected Dictionary <Int64, GameObject> Bullets_;


	protected override void Init () {

		base.Init();

        Bullets_ = new Dictionary<long, GameObject>();

        playertank1 = GetComponent<Transform>();

		state = gameObject.GetComponent<Tank_State>();

		hpBar = GetComponent<HpBar>();

		//hpBar.UpdateHpBar();
	}

	public override void Release()
	{

	}

	public override void EntityUpdate () 
	{
		//Debug.Log ("move" + move);
		MoveEntity();
	}


	void MoveEntity()
	{
		if (move) 
		{
			if (Vector3.Distance (transform.position, ArrivePos) > 0.1f) {
				ArrivePos.y = 0.0f;
				Vector3 dir = (ArrivePos - transform.position).normalized;
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (dir), Time.deltaTime * 20);
				playertank1.Translate (Vector3.forward * state.moveSpeed * Time.deltaTime);

			} else 
			{
				SetMove (false);
			}
		}
	}

	public void SetMove(bool bMove)
	{
		move = bMove;
	}
		
	public virtual void  Fire()
	{
		
	}

	/*
	public virtual GameObject CreateBullet()
	{
		SetMove (true);

		Fire();

		return state.bullet.Spawn();
	}
	*/
		
	public virtual void ProgressInput(float posX, float posZ, bool attack)
	{
		if (attack) 
		{
			Attack (posX, posZ);
		} 
		else
		{
			Vector3 newPos = new Vector3 (posX, 0.0f,  posZ);

			if (Vector3.Distance (transform.position, newPos) < 2.0f)
			{
				return;
			}

			ArrivePos = newPos;
			SetMove (true);
		}
	}

	public virtual void ProgressPos(float posX, float posZ)
	{
		
		if (IsMyEntity ())
			return;
		ArrivePos = new Vector3 (posX, 0.0f,  posZ);
	}

	public void Attack(float posX, float posZ)
	{
		if (fireTransform == null)
		return;

        //AttackDir = ( new Vector3(posX, 0.0f, posZ ) - transform.position ).normalized;
        //AttackDir.y = 0;
        AttackDir = new Vector3(posX, 0.0f, posZ);
        fireTransform.rotation = Quaternion.LookRotation(AttackDir); 

		Fire ();
	}

    public void CreateBullet(float posX, float posZ, float speed, float distance)
    {
        AttackDir = (new Vector3(posX, 0.0f, posZ) - transform.position).normalized;
        AttackDir.y = 0;
        fireTransform.rotation = Quaternion.LookRotation(AttackDir);

        Fire();
    }



    public virtual Transform GetFirePosition()
    {
        return transform;
    }

    public virtual Vector3[] GetFireDirs(Vector3 Vector3)
    {
        return new Vector3[] {};
    }
	
	public void GetDamage(int damage)
	{
		state.GetDamage(damage);
		hpBar.UpdateHpBar();
	}

    public void AddBullet(Int64 bullet_id, GameObject bullet_obj)
    {
        Bullets_[bullet_id] = bullet_obj;
    }

    public void RemoveBullet(Int64 bullet_id)
    {
        // bullet 사리질때 파티클이라도 생기기

        Bullets_.Remove(bullet_id);
    }
}
