
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Entity {

	bool move = false;
	public Vector3 lookDirection;
	public Transform fireTransform;

	Transform playertank1;

	float h, v;
	Controller controller;
	protected Tank_State state;

	protected RaycastHit TFire;
	Vector3 Click;
	Vector3 ArrivePos = Vector3.zero;
	Vector3 AttackDir = Vector3.zero;


	public float nextfire = 0.0f;


	protected override void Init () {

		base.Init();

		playertank1 = GetComponent<Transform>();

		state = gameObject.GetComponent<Tank_State>();
	}

	public override void EntityUpdate () 
	{
		MoveEntity();
	}


	void MoveEntity()
	{
		if (move) 
		{
			if (Vector3.Distance (transform.position, ArrivePos) > 0.1f) {
				ArrivePos.y = 0.0f;
				Vector3 dir = (ArrivePos - transform.position).normalized;
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (dir), Time.deltaTime * 10);
				playertank1.Translate (Vector3.forward * state.moveSpeed * Time.deltaTime);

			} else 
			{
				move = false;
			}
		}
	}
		
	virtual public void Fire()
	{
		
	}

	public override void ProgressInput(float posX, float posZ, bool attack)
	{
		if (attack) 
		{
			Attack (posX, posZ);
		} 
		else
		{
			ArrivePos = new Vector3 (posX, 0.0f,  posZ);
			move = true;
		}
	}

	public override void ProgressPos(float posX, float posZ)
	{
		if (IsMyEntity ())
			return;
		ArrivePos = new Vector3 (posX, 0.0f,  posZ);
	}

	public void Attack(float posX, float posZ)
	{
		if (fireTransform == null)
		return;

		AttackDir = ( new Vector3(posX, 0.0f, posZ ) - transform.position ).normalized;
		AttackDir.y = 0;
		fireTransform.rotation = Quaternion.LookRotation(AttackDir); 
		Fire ();
	}
}
