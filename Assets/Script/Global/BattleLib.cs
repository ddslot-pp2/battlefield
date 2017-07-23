using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleInfo;
using System;

namespace BattleInfo
{

	/// <summary>
	/// Room에 접속한 캐릭터 정보.
	/// </summary>
	public struct RoomEntityInfo
	{
		public int   					Type;
		public string   				Name;
		public int						killPoint;
		public int						DeathPoint;
		public int          			EnterRoom;
	}


	public struct MapBoxCollider
	{
		public float left;
		public float right;
		public float top;
		public float bottom;

	}

    // 처음 셋팅시 탱크의 기본적인 정보들 
    public struct TANK_INFO
    {
        public Vector3 Pos;
        public int MaxHp;
        public int Hp;
        public float MoveSpeed;
        public float ReloadTime;

        public TANK_INFO(Vector3 pos, int max_hp, int hp, float move_speed, float reload_time)
        {
            Pos = pos;
            MaxHp = max_hp;
            Hp = hp; ;
            MoveSpeed = move_speed;
            ReloadTime = reload_time;
        }
    }

}


public class BattleLib : MonoBehaviour {



	public BetterList <Entity > EntityList = new BetterList<Entity>();

	public Dictionary <Int64, Entity > EntityDic = new Dictionary< Int64, Entity>();



	static int MAX_ENTITY = 10;

	int m_currentTick = 0;

	int m_stageNumber = 0;

	int m_myIndex = -1;

	Int64 m_myObId = -1;

	bool m_gameStart = false;

	static 	BattleLib _Instance;


	public delegate void BattleDelegate(float value);

	public BattleDelegate FireDelegate;


	public static BattleLib Instance
	{
		get
		{
			if (_Instance == null) 
			{
				_Instance = FindObjectOfType(typeof(BattleLib)) as BattleLib;

				if(_Instance == null)
				{
					_Instance = new GameObject("BattleLib", typeof(BattleLib)).GetComponent<BattleLib>();
				}

			}

			return _Instance;
		}
	}

	void Awake()
	{
		DontDestroyOnLoad(this);
	}
		
	public void Init()
	{
		m_gameStart = false;

		for (int i = 0; i < EntityList.size ; i++) 
		{
			EntityList[i].Release();
		}

		EntityList.Clear();
		EntityDic.Clear();
	}


	public void AddEntity( Entity entity )
	{

		if (entity.ObjId > 0 && EntityDic.ContainsKey (entity.ObjId) == true) 
		{
			Debug.Log ("already exist id :" + entity.ObjId);
			return;
		}

		EntityList.Add(entity);
		EntityDic.Add (entity.ObjId, entity);
	}

	public void RemoveEnitiy( Entity entity )
	{
		EntityList.Remove(entity);

		if (entity.ObjId > 0 && EntityDic.ContainsKey (entity.ObjId) == true) 
		{
			EntityDic.Remove (entity.ObjId);
		}

		entity.Release();
	}

	public void ProgressBattle()
	{
		if (!m_gameStart)
			return;

		for (int i = 0; i < EntityList.size; i++) 
		{
			if (EntityList[i] == null)
				continue;

			EntityList[i].EntityUpdate();
		}

	}


	public void GameStart()
	{
		Debug.Log ("Battle Start");
		BattleLib.Instance.m_gameStart = true;
	}


	public void CreateEntity(Int64 obj_id, int type, string name , bool myself, Vector3 spawnPos)
	{
		
		float RotateValue = UnityEngine.Random.Range(0, 350);
	
		GameObject entity_instance = (GameObject)Instantiate(Resources.Load("Prefab/PlayerTank/PlayerTank" + type.ToString()), spawnPos, Quaternion.Euler(0, RotateValue, 0)) as GameObject;

		if (entity_instance == null)
		{
			Debug.Log("entity_instance create fail");
		}

		entity_instance.name = obj_id.ToString();
		Tank tankEntity = entity_instance.GetComponent<Tank>();

		if (myself) 
		{
			tankEntity.SetMyEntity();
			m_myObId = obj_id;
		}

		tankEntity.ObjId = obj_id;

		AddEntity(tankEntity);

	}

	public void DeleteEntity(Int64 obj_id)
	{
		if (obj_id > 0 && EntityDic.ContainsKey (obj_id) == true) {
			Entity remove_entity = EntityDic [obj_id];
			if (remove_entity == null)
				return;
	        
			RemoveEnitiy (remove_entity);
		}
    }

	public GameObject GetEntity( Int64 obj_id )
	{
		return EntityDic[obj_id].gameObject;
	}

	public void ReceiveInput( Int64 obId , float posX , float posZ , bool attack)
	{
		
		if (obId > 0 && EntityDic.ContainsKey (obId) == true) 
		{
			Tank tankEntity = EntityDic[obId] as Tank;
			tankEntity.ProgressInput(posX, posZ, attack);
		}
		
	}
		
	public bool GetMyEntityIsDead()
	{
		if (m_myObId == -1)
			return false;

		if (EntityDic [m_myObId] == null)
			return false;

		Tank tankEntity = EntityDic[m_myObId] as Tank;

		if (tankEntity == null)
			return false;
		
		return tankEntity.IsDead ();
	}

	public Vector3 GetMyEntityPos()
	{
		if (m_myObId == -1)
			return Vector3.zero;
		
		if (EntityDic [m_myObId] == null)
			return Vector3.zero;
	
		return EntityDic [m_myObId].GetEntityPos();

	}

	public void ReceivePos(Int64 obId , float posX , float posZ )
	{
		if (obId > 0 && EntityDic.ContainsKey (obId) == true) 
		{
			Tank tankEntity = EntityDic[obId] as Tank;
			tankEntity.ProgressPos(posX, posZ);
		}
	}

	public void CreateItem( Int64 obId, int itemType, float posX, float posY, float posZ)
	{
		//Debug.Log ("CreateItem itemId:" + obId);
		GameObject createItem = ItemManager.Instance.CreateItem (itemType, posX, posY, posZ);
		if (createItem == null)
			return;

		Item itemEntity = createItem.GetComponent<Item> ();
		itemEntity.ObjId = obId;

		AddEntity (itemEntity);
	}

	public void DestroyItem(Int64 itemId, Int64 tankId, int itemType, int tankHp)
	{
		//Debug.Log ("DestroyItem itemId:" + itemId);
		DeleteEntity(itemId);

		Tank tankObject = EntityDic[tankId] as Tank;
		if (tankObject == null)
			return;

		if (itemType == 0) 
		{
			tankObject.SetHp(tankHp);
		}


	}

	public void CreateBullet(Int64 obId , Bullet.Type bullet_type, Int64 bullet_id, Vector3 pos, Vector3 look_dir, Vector3 bullet_dir, Vector3 size, float speed, float distance)
    {
		Tank tankObject = EntityDic[obId] as Tank;
		if (tankObject == null)
			return;

		tankObject.Fire();

        // bullet 발사의 시작 위치를 나이스하게 가져오고 싶음;
		Transform fire_transform = tankObject.fireTransform;
        fire_transform.rotation = Quaternion.LookRotation(look_dir);

        var y = tankObject.GetFirePosition().position.y;
        pos.y = y;

		GameObject bullet_object = tankObject.state.bullet.Spawn(pos,fire_transform.rotation);
		bullet_object.transform.localScale = new Vector3(bullet_object.transform.localScale.x * size.x, bullet_object.transform.localScale.y * size.y, bullet_object.transform.localScale.z * size.z);
		Bullet bulletEntity = bullet_object.GetComponent<Bullet> ();
		bulletEntity.SetProperty(bullet_id, pos, bullet_dir, speed, distance);
        
		AddEntity (bulletEntity);
    }


    public void DestroyBullet(Int64 bullet_id)
    {
		DeleteEntity(bullet_id);
    }

    public void GetDamage(Int64 obId, int damage)
	{
		if (obId > 0 && EntityDic.ContainsKey (obId) == true) {
			Tank tankObject = EntityDic [obId] as Tank;
			if (tankObject == null)
				return;

			tankObject.GetDamage(damage);
		}
	}

	public void EntityDead(Int64 obId)
	{
		if (obId > 0 && EntityDic.ContainsKey (obId) == true) {
			Tank tankObject = EntityDic [obId] as Tank;
			if (tankObject == null)
				return;

			tankObject.Dead ();
		}
	}

	public void EntityRevive(Int64 obId, TANK_INFO TankInfo)
	{
		if (obId > 0 && EntityDic.ContainsKey (obId) == true) {
			Tank tankObject = EntityDic [obId] as Tank;
			if (tankObject == null)
				return;

			tankObject.Revive (TankInfo);
		}
	}

    public void SetTankInfo(Int64 obId, TANK_INFO TankInfo)
    {
        if (obId > 0 && EntityDic.ContainsKey(obId) == true)
        {
            Tank tankObject = EntityDic[obId] as Tank;
            if (tankObject == null)
                return;

            tankObject.SetTankInfo(TankInfo);
        }
    }

	public void CheckRightPad(Vector3 padDir)
	{

		Tank tankObject = EntityDic[m_myObId] as Tank;

		if (tankObject.IsDead ())
			return;

		if (!tankObject.CheckFire())
			return;

		if (null != FireDelegate) FireDelegate(tankObject.state.fireRate);

		tankObject.AddSlowdownTime(1.0f);
		//tankObject.SetMove (false);

		var tank_pos = tankObject.transform.position;
		Vector3 look_dir = padDir.x * Vector3.right + padDir.y * Vector3.forward;

		var fire_dirs = tankObject.GetFireDirs(look_dir);

		var Send = new GAME.CS_FIRE();
		// 추후 탱크마다 불렛 타입도 지정해줌
		Send.BulletType = 0;

		Send.PosX = tank_pos.x;
		Send.PosY = tank_pos.y;
		Send.PosZ = tank_pos.z;

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


	}

    public void TryFire(Int64 obj_id, float x, float z)
    {
        // 이것또한 탱크로 밀어 넣자
        Tank tankObject = EntityDic[obj_id] as Tank;

		if (tankObject.IsDead ())
			return;

        if (!tankObject.CheckFire())
            return;

		if (null != FireDelegate) FireDelegate(tankObject.state.fireRate);

        tankObject.AddSlowdownTime(1.0f);
		//tankObject.SetMove (false);

        var tank_pos = tankObject.transform.position;
        var look_dir = (new Vector3(x, 0.0f, z) - tank_pos).normalized;

        var fire_dirs = tankObject.GetFireDirs(look_dir);

        var Send = new GAME.CS_FIRE();
        // 추후 탱크마다 불렛 타입도 지정해줌
        Send.BulletType = 0;

        Send.PosX = tank_pos.x;
        Send.PosY = tank_pos.y;
        Send.PosZ = tank_pos.z;

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
    }
}
