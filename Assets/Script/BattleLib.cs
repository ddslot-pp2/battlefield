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

}


public class BattleLib : MonoBehaviour {


	//public RoomEntityInfo[]	m_roomEntityList = new RoomEntityInfo[MAX_ENTITY];

	public BetterList <Entity > EntityList = new BetterList<Entity>();

	public Dictionary <Int64, Entity > EntityDic = new Dictionary< Int64, Entity>();

	//public BetterList <Entity > EntityList = new BetterList<Entity>();

	//public Entity[] m_entityList = new Entity[MAX_ENTITY];

	static int MAX_ENTITY = 10;

	int m_currentTick = 0;

	int m_stageNumber = 0;

	int m_myIndex = -1;

	Int64 m_myObId = -1;

	bool m_gameStart = false;

	static 	BattleLib _Instance;


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

		/*
		for (int i = 0; i < m_entityList.Length; i++) 
		{
			m_entityList[i] = null;
		}
		*/

		for (int i = 0; i < EntityList.size ; i++) 
		{
			EntityList[i].Release();
		}

		EntityList.Clear();
		EntityDic.Clear();
	}


	public void AddEntity( Entity entity )
	{
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

		/*
		for (int i = 0; i < m_entityList.Length; i++) 
		{
			if (m_entityList[i] == null)
				continue;

			m_entityList [i].EntityUpdate();

		}
		*/
	}

	/*
	public void EnterRoom(int index, int type, string name)
	{
		
		if( index >= MAX_ENTITY)
		{
			Debug.Log ("EnterRoom max player over" );
		}

		if( m_roomEntityList[index].EnterRoom == 1 )
		{
			Debug.Log ("already Enitity exist" );
			return;
		}

		m_roomEntityList[index].Type = type;
		m_roomEntityList[index].Name = name;
		m_roomEntityList[index].EnterRoom = 1;
	
	}

	public void ExitRoom(int index)
	{
		m_roomEntityList[index].EnterRoom = 0;
	}
	*/


	public void GameStart()
	{
		Debug.Log ("Battle Start");
		BattleLib.Instance.m_gameStart = true;
	}

	/*
	public void InitBattleEntity()
	{
		Debug.Log("InitBattleEntity");

		for (int i = 0; i < MAX_ENTITY; i++)
		{
			if (m_roomEntityList[i].EnterRoom == 1)
			{
				CreateEntity(m_roomEntityList[i].Type, i , m_roomEntityList[i].Name);
			}
		}
	}
	*/

	public void CreateBullet(int index, float distance, float posx, float posz, float speed)
	{
		//Tank tankobject = m_entityList[index] as Tank;
		//tankobject.CreateBullet (posx, posz, speed, distance);
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
		Entity tankEntity = entity_instance.GetComponent<Entity>();

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

		/*
        if (bullet_type == Bullet.Type.DirectBullet)
        {
			
            var bullet_object = Instantiate(Resources.Load("Prefab/Bullet/DirectBullet"), pos, fire_transform.rotation) as GameObject;
            bullet_object.transform.localScale = new Vector3(bullet_object.transform.localScale.x * size.x, bullet_object.transform.localScale.y * size.y, bullet_object.transform.localScale.z * size.z);
			Bullet bulletEntity = bullet_object.GetComponent<Bullet> ();
			bulletEntity.SetProperty(bullet_id, pos, bullet_dir, speed, distance);
			AddEntity (bulletEntity);
            //tankObject.AddBullet(bullet_id, bullet_object);
        }
        */
    }
    public void DestroyBullet(Int64 bullet_id)
    {
        //Tank tankObject = EntityDic[owner_id] as Tank;
        //tankObject.RemoveBullet(bullet_id);
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

    public void TryFire(Int64 obj_id, float x, float z)
    {
        // 이것또한 탱크로 밀어 넣자
        Tank tankObject = EntityDic[obj_id] as Tank;

		Debug.Log ("TryFire");
		tankObject.SetMove (false);

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
