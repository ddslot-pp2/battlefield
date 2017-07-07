using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleInfo;




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


	public RoomEntityInfo[]	m_roomEntityList = new RoomEntityInfo[MAX_ENTITY];

	public Entity[] m_entityList = new Entity[MAX_ENTITY];

	static int MAX_ENTITY = 10;

	int m_currentTick = 0;

	int m_stageNumber = 0;

	int m_myIndex = -1;

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
		
	// Update is called once per frame
	void Update () {


	}

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

	public void CreateEntity(int type, int index, string name , bool my, Vector3 spawnPos)
	{
		Debug.Log ("CreateEntityIndex: " + type);
	
		float RotateValue = Random.Range(0, 350);

		GameObject entity_instance = (GameObject)Instantiate(Resources.Load("Prefab/PlayerTank/PlayerTank" + type.ToString()), spawnPos, Quaternion.Euler(0, RotateValue, 0)) as GameObject;

		if (entity_instance == null)
		{
			Debug.Log("entity_instance create fail");
		}

		entity_instance.name = index.ToString();

		m_entityList[index] = entity_instance.GetComponent<Entity>();
		m_entityList[index].m_index = index;

		if (index == m_myIndex)
		{
			Debug.Log ("myIndex:" + index);
			m_entityList[index].SetMyEntity();
		}

	
	}

	public void ReceiveKeyBuffer(int dirKey, int attackKey, int index, int serverTick, float posX, float posY, float posZ, int hp, int eventNumber)
	{
		
		if( m_entityList[index] != null )
			m_entityList[index].ProgressInput();
		
	}
}
