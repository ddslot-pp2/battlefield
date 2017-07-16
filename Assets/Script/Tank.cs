
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tank : Entity {

	bool dead = false;
	bool move = false;

	public Vector3 lookDirection;
	public Transform fireTransform;

	Transform myTransform;

	float h, v;
	Controller controller;
	public Tank_State state;
	HpBar hpBar;

	GameObject DustEffect;

	protected RaycastHit TFire;
	Vector3 Click;
	Vector3 ArrivePos = Vector3.zero;
	Vector3 AttackDir = Vector3.zero;

	public float nextfire = 0.0f;

    private float FireTime_;

    private float SlowdownSpeed_ = 0.0f;
    private float SlowdownTimer_ = 0.0f;

    protected override void Init () {

		base.Init();

		myTransform = this.transform;

		state = gameObject.GetComponent<Tank_State>();

		hpBar = GetComponent<HpBar>();

		dead = false;

        SlowdownSpeed_ = 1.0f;
    }

	public override void Release()
	{
		hpBar.DeleteHpobject();
		Destroy(gameObject);
	}

	public bool IsDead()
	{
		return dead;
	}

	public override void EntityUpdate () 
	{
		//Debug.Log ("move" + move);
		MoveEntity();
	}


	void MoveEntity()
	{
		if (IsDead()) return;

		if (move) 
		{
			if (Vector3.Distance (myTransform.position, ArrivePos) > 0.4f) {
				ArrivePos.y = 0.0f;
				Vector3 dir = (ArrivePos - myTransform.position).normalized;
				myTransform.rotation = Quaternion.Lerp (myTransform.rotation, Quaternion.LookRotation (dir), Time.deltaTime * 20);

                var speed = state.moveSpeed * SlowdownSpeed_;
                myTransform.Translate (Vector3.forward * speed * Time.deltaTime);

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

    protected void Update()
    {
        //Debug.Log("탱크 업데이트 콜\n");
        FireTime_ = FireTime_ + (Time.deltaTime * 1000.0f);

        if (SlowdownTimer_ > 0.0f)
        {
            SlowdownTimer_ = SlowdownTimer_ - Time.deltaTime;
            if (SlowdownTimer_ <= 0.0f)
            {
                Debug.Log("슬로우 다운 풀림");
                SlowdownSpeed_ = 1.0f;
                SlowdownTimer_ = 0.0f;
            }
        }
    }

    public bool CheckFire()
    {
        Debug.Log("FireTime: " + FireTime_);
        Debug.Log("FireRate: " + state.fireRate);

        if (FireTime_ < state.fireRate) return false;

        FireTime_ = 0.0f;
        return true;
    }
		
	public virtual void ProgressInput(float posX, float posZ, bool attack)
	{
		if (attack) 
		{
			Attack (posX, posZ);
		} 
		else
		{
			Vector3 newPos = new Vector3 (posX, 0.0f,  posZ);

			if (Vector3.Distance (myTransform.position, newPos) < 2.0f)
			{
				return;
			}

			ArrivePos = newPos;
			SetMove (true);
		}
	}

	public virtual void ProgressPos(float posX, float posZ)
	{
		//if (IsMyEntity ())
		//	return;
		ArrivePos = new Vector3 (posX, 0.0f,  posZ);
		SetMove (true);
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
		AttackDir = (new Vector3(posX, 0.0f, posZ) - myTransform.position).normalized;
        AttackDir.y = 0;
        fireTransform.rotation = Quaternion.LookRotation(AttackDir);

        Fire();
    }



    public virtual Transform GetFirePosition()
    {
		return myTransform;
    }

    public virtual Vector3[] GetFireDirs(Vector3 Vector3)
    {
		return new Vector3[] { Vector3 };
    }
		
	public void GetDamage(int damage)
	{
		state.GetDamage(damage);
		hpBar.UpdateHpBar();
	}

	public void Dead()
	{
		dead = true;
		DustEffect = EffectManager.Instance.DustEffect.Spawn(transform.position);
	}

	public void Revive(BattleInfo.TANK_INFO TankInfo)
	{
		myTransform.position = TankInfo.Pos;

        state.hp = TankInfo.Hp;
        state.maxHp = TankInfo.MaxHp;
        state.moveSpeed = TankInfo.MoveSpeed;
        state.fireRate = TankInfo.ReloadTime;

        hpBar.UpdateHpBar();

		dead = false;

		DustEffect.Recycle();
    }

    public void SetTankInfo(BattleInfo.TANK_INFO TankInfo)
    {
        Debug.Log("SetTankInfo 받음");
        //transform.position = TankInfo.Pos;

        state.hp = TankInfo.Hp;
        state.maxHp = TankInfo.MaxHp;

        state.moveSpeed = TankInfo.MoveSpeed;
        state.fireRate = TankInfo.ReloadTime;

        hpBar.UpdateHpBar();
    }

    public void AddSlowdownTime(float delta)
    {
        Debug.Log("슬로우 다운 시작");
        SlowdownSpeed_ = 0.4f;
        SlowdownTimer_ += delta;
    }  
   
}
