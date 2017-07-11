using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using GameCore;

public class Game : MonoBehaviour {

	public GameCamera gameCamera;
	TouchDispatcher _TouchDispatcher = new TouchDispatcher();

	protected RaycastHit TFire;
	Vector3 Click;
	Vector3 BeginPos;

	float lastSendTime;

    int Index;
    Int64 MyObjId;
    int MyIndex;


    Dictionary<Int64, int> IndexInfos_;
    Queue<int> IndexQueue_;

    // 1초에 5번만
    private const float UPDATE_MOVE_INTERVAL = 1.0f / 5.0f;
    private const float DRAG_AS_FIRE_DISTANCE = 30.0f;
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
            IndexInfos_[other_obj_id] = Index;
            EnterUser(other_obj_id, other_tanktype, Index++, other_nickname, false, other_pos);
        }

        // index 현재는 줄어들지 않게 함; 
        IndexInfos_[read.ObjId] = Index;
        MyIndex = Index;
        EnterUser(read.ObjId, read.TankType, Index++, read.Nickname, true, new Vector3(read.PosX, read.PosY, read.PosZ));
        MyObjId = read.ObjId;
        
        // 씬 관련 서버에서 정보를 가져옴
        OnLoadingComplete();
    }

    public void handler_SC_NOTI_OTHER_ENTER_FIELD(GAME.SC_NOTI_OTHER_ENTER_FIELD read)
    {
        // 인덱스 재활용
        if (IndexQueue_.Count > 0)
        {
            var index = IndexQueue_.Dequeue();

            IndexInfos_[read.ObjId] = index;
            EnterUser(read.ObjId, read.TankType, index, read.Nickname, false, new Vector3(read.PosX, read.PosY, read.PosZ));
            return;
        }

        IndexInfos_[read.ObjId] = Index;
        EnterUser(read.ObjId, read.TankType, Index++, read.Nickname, false, new Vector3(read.PosX, read.PosY, read.PosZ));
    }

    public void handler_SC_NOTI_OTHER_LEAVE_FIELD(GAME.SC_NOTI_OTHER_LEAVE_FIELD read)
    {

    }

    public void handler_SC_NOTI_OTHER_MOVE(GAME.SC_NOTI_OTHER_MOVE read)
    {
        var obj_id = read.ObjId;
        var index = IndexInfos_[obj_id];

        ReceiveUserClickInfo(index, read.PosX, read.PosZ, false);
        Debug.Log("다른 유저가 움직임\n");

    }
    public void handler_SC_NOTI_FIRE(GAME.SC_NOTI_FIRE read)
    {
        var obj_id = read.ObjId;
        var bullet_type = read.BulletType;

        for (var i = 0; i < read.BulletInfos.Count; ++i)
        {
            var bullet_info = read.BulletInfos[i];
            var dir = new Vector3(bullet_info.DirX, bullet_info.DirY, bullet_info.DirZ);
            var speed = bullet_info.Speed;
            Debug.Log("Dir x: " + dir.x + ", Dir z: " + dir.z);
            SendUserClickInfo(dir.x, dir.z, true);
        }

    }
    public void RegisterPacketHandler()
    {
        // 이번 패킷에 사용할 패킷관련 핸들러를 지정
        ProtobufManager.Instance().SetHandler<GAME.SC_SYNC_FIELD>(opcode.SC_SYNC_FIELD, handler_SC_SYNC_FIELD);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_OTHER_ENTER_FIELD>(opcode.SC_NOTI_OTHER_ENTER_FIELD, handler_SC_NOTI_OTHER_ENTER_FIELD);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_OTHER_LEAVE_FIELD>(opcode.SC_NOTI_OTHER_LEAVE_FIELD, handler_SC_NOTI_OTHER_LEAVE_FIELD);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_OTHER_MOVE>(opcode.SC_NOTI_OTHER_MOVE, handler_SC_NOTI_OTHER_MOVE);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_FIRE>(opcode.SC_NOTI_FIRE, handler_SC_NOTI_FIRE);
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
		
		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out TFire);


		if (Vector3.Distance (BeginPos, pos) > 30.0f) 
		{
			SendUserClickInfo(TFire.point.x, TFire.point.z, true);
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
        Index = 0;
        MyObjId = 0;

        RegisterPacketHandler();

        SendSyncField();

		_TouchDispatcher.BeganDelegate = new TouchDispatcher.TouchDelegate(OnTouchBegan);
		_TouchDispatcher.MovedDelegate = new TouchDispatcher.TouchDelegate(OnTouchMoved);
		_TouchDispatcher.PressedDelegate = new TouchDispatcher.TouchDelegate(OnTouchPressed);
		_TouchDispatcher.EndedDelegate = new TouchDispatcher.TouchDelegate(OnTouchEnded);

		// 테스트용 생성
		//EnterUser (1,0,"aaa", false, new Vector3(0.0f,0.0f,0.0f));
		//EnterUser (2,1,"bbb", false, new Vector3(10.0f,0.0f,0.0f));
		//EnterUser (3,2,"bbb", false, new Vector3(20.0f,0.0f,0.0f));
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

        // 서버 샌드 로직
        var Send = new GAME.CS_NOTI_MOVE();
        Send.PosX = pos.x;
        Send.PosY = pos.y;
        Send.PosZ = pos.z;
        ProtobufManager.Instance().Send(opcode.CS_NOTI_MOVE, Send);
    }

	public void SendUserClickInfo(float posX, float posZ, bool attack)
	{
		// 서버로 send



		// 움직임 테스트를 위해 바로 받음
		ReceiveUserClickInfo(MyIndex, posX, posZ, attack);
	}

	public void ReceiveUserClickInfo(int index, float posX, float posZ, bool attack)
	{
        //Debug.Log("PosX: " + posX + ", PosZ: " + posZ);
		BattleLib.Instance.ReceiveInput(index, posX, posZ, attack);
	}

	public void EnterUser(Int64 obj_id, int type, int index, string name, bool myself, Vector3 pos)
	{
		BattleLib.Instance.CreateEntity (obj_id, type, index, name, myself, pos);

		if (myself) 
		{
			gameCamera.SetTarget(BattleLib.Instance.GetEntity (index).transform);		
		}
	}
		
    // 유저 나갔을 경우 삭제
    public void LeaveUser(Int64 obj_id, int index)
    {
		BattleLib.Instance.DeleteEntity (index);

        IndexQueue_.Enqueue(index);

        if (index == MyIndex) 
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
		}
    }

    public void ReceiveUserPos(int index, float posX, float posZ)
	{
		BattleLib.Instance.ReceivePos(index, posX, posZ);
	}

	public void ReceiveUserDamage(int index, int hp)
	{
		Debug.Log ("damage index:" + index);
		Debug.Log ("damage hp:" + hp);
	}

	public void Dead()
	{

	}

	public void Revive()
	{

	}

    void TryFire(float x, float z)
    {
        var obj = BattleLib.Instance.GetEntity(MyIndex);
        var dir = (new Vector3(x, 0.0f, z) - obj.transform.position).normalized;

        var Send = new GAME.CS_FIRE();

        Send.BulletType = 0;

        GAME.BULLET_INFO bullet_0 = new GAME.BULLET_INFO();
        bullet_0.DirX = dir.x;
        bullet_0.DirY = dir.y;
        bullet_0.DirZ = dir.z;

        Send.BulletInfos.Add(bullet_0);

        var size = Send.BulletInfos.Count;

        ProtobufManager.Instance().Send(opcode.CS_FIRE, Send);
        // 속드 줄여버림 state.movSpeed
        //FireMoveHold_ = true;
        // 발사 direction
        //Debug.Log("Dir X: " + dir.x  + ", Dir Z: " + dir.z);
    }
}
