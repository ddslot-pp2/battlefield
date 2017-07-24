
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

	public Tank_State state;
	HpBar hpBar;

	GameObject DustEffect;

	Vector3 ArrivePos = Vector3.zero;
	Vector3 AttackDir = Vector3.zero;

	public  float nextfire = 0.0f;
    private float FireTime_;

    private float SlowdownSpeed_ = 0.0f;
    private float SlowdownTime_ = 0.0f;

	Vector3 controllDir = Vector3.zero;

	//private Controller controller;

    protected override void Init () {

		base.Init();

		myTransform = this.transform;

		state = gameObject.GetComponent<Tank_State>();

		hpBar = GetComponent<HpBar>();

		dead = false;

        SlowdownSpeed_ = 1.0f;
    }

	public void SetMyEntity()
	{
		m_my = true;
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
		UpdateFireTime();
		MoveEntity();
	}

	/*
	public void SetControllDir(Vector3 Dir)
	{
		//controllDir = Dir;
	}
	*/

	void MoveEntity()
	{
		if (IsDead()) return;


		if (IsMyEntity ()) 
		{
			Vector3 RightDir = DualJoystickPlayerController.Instance.GetRightJoystickDirection();
			if (RightDir.x != 0 && RightDir.y != 0)
			{
				lookDirection = RightDir.x * Vector3.right + RightDir.y * Vector3.forward;
				fireTransform.rotation = Quaternion.LookRotation(state.direct * lookDirection);

				SendFire (RightDir.x, RightDir.y, true);
			}
				
			Vector3 LeftDir = DualJoystickPlayerController.Instance.GetleftJoystickDirection();

			if (LeftDir.x != 0 && LeftDir.y != 0)
			{
				lookDirection = LeftDir.x * Vector3.right + LeftDir.y * Vector3.forward;
				myTransform.rotation = Quaternion.LookRotation(state.direct * lookDirection);
				if (LeftDir != new Vector3(0, 0, 0))
					myTransform.Translate(state.forward * state.moveSpeed * Time.deltaTime * SlowdownSpeed_);
				move = false;

			}
				
		}
			
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

	void UpdateFireTime()
	{
		FireTime_ = FireTime_ + (Time.deltaTime * 1000.0f);

		/*
		if (SlowdownSpeed_ < 1.0f)
		{
			SlowdownSpeed_ = SlowdownSpeed_ + 0.02f;
		}



		if (SlowdownTime_ > 0.0f)
		{
            SlowdownTime_ = SlowdownTime_ - Time.deltaTime;
			if (SlowdownTime_ <= 0.0f)
			{
				Debug.Log("슬로우 다운 풀림");
				SlowdownSpeed_ = 1.0f;
                SlowdownTime_  = 0.0f;
			}
		}
		*/
	}

	public void SetMove(bool bMove)
	{
		move = bMove;
	}
		
	public virtual void  Fire()
	{
		if( IsMyEntity () )
		{
			//GameCamera.ToggleShake (0.1f);
		}
	}

    protected void Update()
    {
		// EntityUpdate 에서 사용 Update 사용금지.
    }

    public bool CheckFire()
    {
        if (FireTime_ < state.fireRate) return false;

        FireTime_ = 0.0f;
        return true;
    }
		
	public virtual void ProgressInput(float posX, float posZ, bool attack)
	{
		if (attack) 
		{
			//Attack (posX, posZ);
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


    public void CreateBullet(float posX, float posZ, float speed, float distance)
    {
		AttackDir = (new Vector3(posX, 0.0f, posZ) - myTransform.position).normalized;
        AttackDir.y = 0;
        fireTransform.rotation = Quaternion.LookRotation(AttackDir);

        Fire();

		//GameCamera.ToggleShake (0.1f);
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
		if( IsMyEntity () )
		{
			GameCamera.ToggleShake (0.2f);
		}

		state.GetDamage(damage);
		hpBar.UpdateHpBar();
	}

	public void SetHp(int hp)
	{
		state.hp = hp;
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
        //Debug.Log("슬로우 다운 시작");
        SlowdownSpeed_ = 0.0f;
        SlowdownTime_ += delta;
    }  

	public bool SendFire(float x, float z, bool joystick)
	{
		if (IsDead())
			return false;

		if (!CheckFire())
			return false;

		//AddSlowdownTime(1.0f);
	
		var look_dir = (new Vector3(x, 0.0f, z) - myTransform.position).normalized;

		if (joystick) 
		{
			look_dir = x * Vector3.right + z * Vector3.forward;
		} 
	
		var fire_dirs = GetFireDirs(look_dir);

		var Send = new GAME.CS_FIRE();
		// 추후 탱크마다 불렛 타입도 지정해줌
		Send.BulletType = 0;
		Send.PosX = myTransform.position.x;
		Send.PosY = myTransform.position.y;
		Send.PosZ = myTransform.position.z;
		Send.DirX = look_dir.x;
		Send.DirY = look_dir.y;
		Send.DirZ = look_dir.z;

		foreach (var dir in fire_dirs)
		{
			GAME.BULLET_INFO bullet = new GAME.BULLET_INFO();
			bullet.DirX = dir.x;
			bullet.DirY = dir.y;
			bullet.DirZ = dir.z;

			Send.BulletInfos.Add(bullet);
		}
			
		ProtobufManager.Instance().Send(opcode.CS_FIRE, Send);

		return true;
	}
   
}
