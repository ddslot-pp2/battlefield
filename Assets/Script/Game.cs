﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using GameCore;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	public GameCamera gameCamera;
	TouchDispatcher _TouchDispatcher = new TouchDispatcher();

	protected RaycastHit TFire;
	Vector3 Click;
	Vector3 BeginPos;

	float lastSendTime;

    //int Index;
    Int64 MyObjId;
    //int MyIndex;

    Vector3 LastPos_;
    public Button RespawnButton_;
    public Button[] BuffButton_;
    public GameObject BuffButtons_;
    enum BuffType { MaxHpUp, TankSpeedUp, BulletSpeedUp, BulletPowerUp, BulletDistanceUp, BulletReloadTimeDown };

    Dictionary<Int64, int> IndexInfos_;
    Queue<int> IndexQueue_;

    // 1초에 5번만
    private const float UPDATE_MOVE_INTERVAL = 1.0f / 5.0f;
    // 터치랑 가끔 구별이 안되 길이를 늘림
    private const float DRAG_AS_FIRE_DISTANCE = 80.0f;
    private float LastUpdateMoveTime_ = 0.0f;

    public void onConnect()
    {
        Debug.Log("OnConnected called\n");
    }

    public void onDisconnect(SocketError ErrorCode)
    {
        Debug.Log("OnDisonnected called");
        Debug.Log("ErrorCode: " + ErrorCode);
    }
    public void handler_SC_SYNC_FIELD(GAME.SC_SYNC_FIELD read)
    {
        foreach (var other_info in read.OtherInfos)
        {
            var other_obj_id = other_info.ObjId;
            var other_pos = new Vector3(other_info.PosX, other_info.PosY, other_info.PosZ);
            var other_nickname = other_info.Nickname;
            var other_tanktype = other_info.TankType;
            //IndexInfos_[other_obj_id] = Index;
            EnterUser(other_obj_id, other_tanktype, other_nickname, false, other_pos);
        }

        // index 현재는 줄어들지 않게 함; 
        //IndexInfos_[read.ObjId] = Index;
        //MyIndex = Index;
        var spawn_pos = new Vector3(read.PosX, read.PosY, read.PosZ);
        LastPos_ = spawn_pos;

        EnterUser(read.ObjId, read.TankType, read.Nickname, true, spawn_pos);
        MyObjId = read.ObjId;
        
        // 씬 관련 서버에서 정보를 가져옴
        OnLoadingComplete();
    }

    public void handler_SC_NOTI_OTHER_ENTER_FIELD(GAME.SC_NOTI_OTHER_ENTER_FIELD read)
    {
        //IndexInfos_[read.ObjId] = Index;
        EnterUser(read.ObjId, read.TankType, read.Nickname, false, new Vector3(read.PosX, read.PosY, read.PosZ));
    }

    public void handler_SC_NOTI_OTHER_LEAVE_FIELD(GAME.SC_NOTI_OTHER_LEAVE_FIELD read)
    {
        BattleLib.Instance.DeleteEntity(read.ObjId);

		if (read.ObjId == MyObjId) 
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
		}
    }

    public void handler_SC_NOTI_OTHER_MOVE(GAME.SC_NOTI_OTHER_MOVE read)
    {
        var obj_id = read.ObjId;
        //var index = IndexInfos_[obj_id];

		ReceiveUserPos(obj_id, read.PosX, read.PosZ);
		//ReceiveUserClickInfo(obj_id, read.PosX, read.PosZ, false);
        //Debug.Log("다른 유저가 움직임\n");
    }

    // 나를 포함한 모든 탱크가 미사일 발사시 브로드캐스팅해서 전달됨
    public void handler_SC_NOTI_FIRE(GAME.SC_NOTI_FIRE read)
    {
        // 발사한 탱크의 고유 아이디
        var obj_id = read.ObjId;

        // 어떤 bullet인지; bullet의 메시정보 구별을 위해 필요. 값들은 동적으로 변함(파워, 스피드, etc)
        var bullet_type = read.BulletType;

        // 탱크가 미사일을 발사 했었던 위치
        var pos = new Vector3(read.PosX, read.PosY, read.PosZ);
        var look_dir = new Vector3(read.DirX, read.DirY, read.DirZ);

        for (var i = 0; i < read.BulletInfos.Count; ++i)
        {
            var bullet_info = read.BulletInfos[i];
            var bullet_id = bullet_info.BulletId;

            var bullet_dir = new Vector3(bullet_info.DirX, bullet_info.DirY, bullet_info.DirZ);
            var size = new Vector3(bullet_info.SizeX, bullet_info.SizeY, bullet_info.SizeZ);
            var speed = bullet_info.Speed;
            var distance = bullet_info.Distance;

            // bullet 생성 
			BattleLib.Instance.CreateBullet(obj_id, (Bullet.Type)bullet_type, bullet_id, pos, look_dir, bullet_dir, size, speed, distance);
        }
    }
    public void handler_SC_NOTI_DESTROY_BULLET(GAME.SC_NOTI_DESTROY_BULLET read)
    {
        //Debug.Log("미사일 제거 호출");
        //Debug.Log("bullet_id: " + read.BulletId);
        BattleLib.Instance.DestroyBullet(read.BulletId);
        foreach (var damage_info in read.DamageInfos)
        {
            var target_obj_id = damage_info.TargetId;
            var damage = damage_info.Damage;
            //var hp = damage_info.Damage;
            BattleLib.Instance.GetDamage(target_obj_id, (int)damage);
        }
    }
    public void handler_SC_NOTI_DESTROY_CHARACTER(GAME.SC_NOTI_DESTROY_CHARACTER read)
    {
        // 현재 케릭터가 hp가 0이라서; 폭파한 상태 스모그 이펙트 나오면서 3초후 respawn 나오면 되겠징;;
        // 헐 내가 죽었네 ㅜㅜ;
        // 미사일 발사 금지 이동 금지 등등 핸들링

		BattleLib.Instance.EntityDead (read.ObjId);

        if (MyObjId == read.ObjId)
        {
            RespawnButton_.gameObject.SetActive(true);
            return;
        }


        // 휴 다행이 난 아니다 다른 탱크군 후훗 여기부분 처리해주자
        var OtherObjId = read.ObjId;
        
        // bullet destroy는 따로 올테니 일단은 신경안씀; 이 오브젝트 기준으로 bullet을 다 제거 할수도 있지만... 보류
    }

    public void handler_SC_NOTI_RESPAWN_CHARACTER(GAME.SC_NOTI_RESPAWN_CHARACTER read)
    {
        BattleInfo.TANK_INFO tank_info = new BattleInfo.TANK_INFO(new Vector3(read.PosX, read.PosY, read.PosZ), read.MaxHp, read.Hp, read.Speed, read.ReloadTime);
        
        // 내가 respawn 됨
        if (MyObjId == read.ObjId)
        {
            
        }

        BattleLib.Instance.EntityRevive(read.ObjId, tank_info);
    }

    public void handler_SC_SELECT_BUFF(GAME.SC_SELECT_BUFF read)
    {
        // 버프 가능 횟수 증가해줘야함 
        // 유저가 바빠 바로 못하면 2번 3번 쌓았다가 1개씩 처리;
        ShowBuffButtons();
    }

    public void handler_SC_NOTI_UPDATE_CHARACTER_STATUS(GAME.SC_NOTI_UPDATE_CHARACTER_STATUS read)
    {
        if (MyObjId == read.ObjId)
        {
            // 내꺼 버프 업데이트 된거라서 respawnTime은 업데이트
            var reloadTime = read.ReloadTime;
        }
        // 나머지는 공통
        var tank_speed = read.Speed;
        var max_hp     = read.MaxHp;
        var hp         = read.Hp;
    }

    public void RegisterPacketHandler()
    {
        // 이번 패킷에 사용할 패킷관련 핸들러를 지정
        ProtobufManager.Instance().SetHandler<GAME.SC_SYNC_FIELD>(opcode.SC_SYNC_FIELD, handler_SC_SYNC_FIELD);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_OTHER_ENTER_FIELD>(opcode.SC_NOTI_OTHER_ENTER_FIELD, handler_SC_NOTI_OTHER_ENTER_FIELD);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_OTHER_LEAVE_FIELD>(opcode.SC_NOTI_OTHER_LEAVE_FIELD, handler_SC_NOTI_OTHER_LEAVE_FIELD);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_OTHER_MOVE>(opcode.SC_NOTI_OTHER_MOVE, handler_SC_NOTI_OTHER_MOVE);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_FIRE>(opcode.SC_NOTI_FIRE, handler_SC_NOTI_FIRE);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_DESTROY_BULLET>(opcode.SC_NOTI_DESTROY_BULLET, handler_SC_NOTI_DESTROY_BULLET);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_DESTROY_CHARACTER>(opcode.SC_NOTI_DESTROY_CHARACTER, handler_SC_NOTI_DESTROY_CHARACTER);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_RESPAWN_CHARACTER>(opcode.SC_NOTI_RESPAWN_CHARACTER, handler_SC_NOTI_RESPAWN_CHARACTER);
        ProtobufManager.Instance().SetHandler<GAME.SC_SELECT_BUFF>(opcode.SC_SELECT_BUFF, handler_SC_SELECT_BUFF);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_UPDATE_CHARACTER_STATUS>(opcode.SC_NOTI_UPDATE_CHARACTER_STATUS, handler_SC_NOTI_UPDATE_CHARACTER_STATUS);
    }

	void OnTouchBegan(Vector3 pos)
	{
		//Debug.Log ("OnTouchBegan:");
		BeginPos = pos;
	}

	void OnTouchMoved(Vector3 pos)
	{
		//Debug.Log ("OnTouchMoved:");
	}

	void OnTouchPressed(Vector3 pos)
	{
		//Debug.Log ("OnTouchPressed:");
	}

	void OnTouchEnded(Vector3 pos)
	{

		if (BattleLib.Instance.GetMyEntityIsDead ())
			return;
		
		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out TFire);


		if (Vector3.Distance (BeginPos, pos) > DRAG_AS_FIRE_DISTANCE) 
		{
            BattleLib.Instance.TryFire(MyObjId, TFire.point.x, TFire.point.z);
            //TryFire(TFire.point.x, TFire.point.z);
            //SendUserClickInfo(TFire.point.x, TFire.point.z, true);
        } 
		else 
		{
			SendUserClickInfo(TFire.point.x, TFire.point.z, false);

		}

	}

	void Awake ()
	{
		BattleLib.Instance.Init();
	}

    // Use this for initialization
    void Start () 
	{
        IndexInfos_ = new Dictionary<Int64, int>();
        IndexQueue_ = new Queue<int>();
     
        MyObjId = 0;
        RespawnButton_.gameObject.SetActive(false);
        HideBuffButtons();
        //ShowBuffButtons();
        //RespawnButton_.enabled = false;

        RegisterPacketHandler();

        SendSyncField();

		_TouchDispatcher.BeganDelegate = new TouchDispatcher.TouchDelegate(OnTouchBegan);
		_TouchDispatcher.MovedDelegate = new TouchDispatcher.TouchDelegate(OnTouchMoved);
		_TouchDispatcher.PressedDelegate = new TouchDispatcher.TouchDelegate(OnTouchPressed);
		_TouchDispatcher.EndedDelegate = new TouchDispatcher.TouchDelegate(OnTouchEnded);

	}
	// Update is called once per frame

	void LateUpdate()
	{
		_TouchDispatcher.Update();
	}
	void Update () 
	{
        // 업데이트 할때마다 패킷을 처리해 핸들러를 호출
        ProtobufManager.Instance().ProcessPacket();

		BattleLib.Instance.ProgressBattle();

        LastUpdateMoveTime_ = LastUpdateMoveTime_ + Time.deltaTime;
        if (LastUpdateMoveTime_ >= UPDATE_MOVE_INTERVAL)
        {
            //Debug.Log("보냄\n");
            SendUserPos();
            LastUpdateMoveTime_ = 0.0f;
        }
    }

	public void SendSyncField()
	{
        var Send = new GAME.CS_SYNC_FIELD();
        Send.FieldId = 0;
        ProtobufManager.Instance().Send(opcode.CS_SYNC_FIELD, Send);
    }

    public void OnLoadingComplete()
    {
        BattleLib.Instance.GameStart();

        //var obj = GameObject.Find(MyObjId.ToString());
        //var script = obj.GetComponent<Tank_State>();
        //script.hp = 50;
    }

	public void SendUserPos()
	{
		// 내위치 얻어오기
		Vector3 pos = BattleLib.Instance.GetMyEntityPos();

        if (LastPos_ == pos)
        {
            return;
        }

        // 서버 샌드 로직
        var Send = new GAME.CS_NOTI_MOVE();
        Send.PosX = pos.x;
        Send.PosY = pos.y;
        Send.PosZ = pos.z;
        ProtobufManager.Instance().Send(opcode.CS_NOTI_MOVE, Send);

        LastPos_ = pos;
    }

	public void SendUserClickInfo(float posX, float posZ, bool attack)
	{
		// 서버로 send


		//Debug.Log ("SendUserClickInfo");
		// 내유닛은 바로 받음
		ReceiveUserClickInfo(MyObjId, posX, posZ, attack);
	}

	public void ReceiveUserClickInfo(Int64 obId, float posX, float posZ, bool attack)
	{
        //Debug.Log("PosX: " + posX + ", PosZ: " + posZ);
		BattleLib.Instance.ReceiveInput(obId, posX, posZ, attack);
	}

	public void EnterUser(Int64 obj_id, int type, string name, bool myself, Vector3 pos)
	{
		BattleLib.Instance.CreateEntity (obj_id, type, name, myself, pos);

		if (myself) 
		{
			gameCamera.SetTarget(BattleLib.Instance.GetEntity (obj_id).transform);		
		}
	}
		
    // 유저 나갔을 경우 삭제
    public void LeaveUser(Int64 obj_id)
    {
		BattleLib.Instance.DeleteEntity (obj_id);

        //IndexQueue_.Enqueue(index);
		if (obj_id == MyObjId) 
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
		}
    }

	public void ReceiveUserPos(Int64 obj_id, float posX, float posZ)
	{
		BattleLib.Instance.ReceivePos(obj_id, posX, posZ);
	}

	public void ReceiveUserDamage(Int64 obj_id, int hp)
	{
		Debug.Log ("damage hp:" + hp);
	}

	public void Dead(Int64 obj_id)
	{

	}

	public void Revive(Int64 obj_id)
	{

	}

    public void onRespawnButton()
    {
        RespawnButton_.gameObject.SetActive(false);
        var Send = new GAME.CS_RESPAWN_CHARACTER();
        ProtobufManager.Instance().Send(opcode.CS_RESPAWN_CHARACTER, Send);
    }

    public void ShowBuffButtons()
    {
        BuffButtons_.SetActive(true);
    }

    public void HideBuffButtons()
    {
        BuffButtons_.SetActive(false);
    }

    public void onBuffButton(int type)
    {
        // 밑에는 클라이언트 버튼 max 이상 방지를 위한 작업 및 이펙트 효과 + 해줌
        if (type == (int)BuffType.MaxHpUp)
        {

        }
        else if (type == (int)BuffType.TankSpeedUp)
        {

        }
        else if (type == (int)BuffType.BulletSpeedUp)
        {

        }
        else if (type == (int)BuffType.BulletPowerUp)
        {

        }
        else if (type == (int)BuffType.BulletDistanceUp)
        {

        }
        else if (type == (int)BuffType.BulletReloadTimeDown)
        {

        }

        // 서버에 전송
        var Send = new GAME.CS_ENHANCE_BUFF();
        Send.BuffType = type;
        ProtobufManager.Instance().Send(opcode.CS_ENHANCE_BUFF, Send);
    }
}
