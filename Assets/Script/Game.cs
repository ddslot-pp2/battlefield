using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using GameCore;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour {

	public GameCamera gameCamera;
	public PlayUi playUi;

	TouchDispatcher _TouchDispatcher = new TouchDispatcher();

	protected RaycastHit TFire;
	Vector3 BeginPos,LastPos_ ;

    Int64 MyObjId;
    
    enum BuffType { MaxHpUp, TankSpeedUp, BulletSpeedUp, BulletPowerUp, BulletDistanceUp, BulletReloadTimeDown };


    // 1초에 5번만
    private const float UPDATE_MOVE_INTERVAL = 1.0f / 5.0f;
    // 터치랑 가끔 구별이 안되 길이를 늘림
    private const float DRAG_AS_FIRE_DISTANCE = 80.0f;
    private float LastUpdateMoveTime_ = 0.0f;

    private int BuffCount_ = 0;

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
            BattleInfo.TANK_INFO other_tank_info = new BattleInfo.TANK_INFO(new Vector3(0.0f, 0.0f, 0.0f), other_info.MaxHp, other_info.Hp, other_info.Speed, read.BulletSpeed, read.BulletPower, read.BulletDistance, 0.0f);
            BattleLib.Instance.SetTankInfo(other_obj_id, other_tank_info);
        }

        // index 현재는 줄어들지 않게 함; 
        //IndexInfos_[read.ObjId] = Index;
        //MyIndex = Index;
        var spawn_pos = new Vector3(read.PosX, read.PosY, read.PosZ);
        LastPos_ = spawn_pos;

        EnterUser(read.ObjId, read.TankType, read.Nickname, true, spawn_pos);

        // state가 없어서 디짐
        BattleInfo.TANK_INFO tank_info = new BattleInfo.TANK_INFO(new Vector3(0.0f, 0.0f, 0.0f), read.MaxHp, read.Hp, read.Speed, read.BulletSpeed, read.BulletPower, read.BulletDistance, read.ReloadTime);
        BattleLib.Instance.SetTankInfo(read.ObjId, tank_info);

        MyObjId = read.ObjId;
        
        // 씬 관련 서버에서 정보를 가져옴
        OnLoadingComplete();
    }

    public void handler_SC_NOTI_OTHER_ENTER_FIELD(GAME.SC_NOTI_OTHER_ENTER_FIELD read)
    {
        //IndexInfos_[read.ObjId] = Index;
        EnterUser(read.ObjId, read.TankType, read.Nickname, false, new Vector3(read.PosX, read.PosY, read.PosZ));

        BattleInfo.TANK_INFO other_tank_info = new BattleInfo.TANK_INFO(new Vector3(0.0f, 0.0f, 0.0f), read.MaxHp, read.Hp, read.Speed, read.BulletSpeed, read.BulletPower, read.BulletDistance, 0.0f);
        BattleLib.Instance.SetTankInfo(read.ObjId, other_tank_info);
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
		BattleLib.Instance.ReceivePos(read.ObjId, read.PosX, read.PosZ);
		//ReceiveUserClickInfo(obj_id, read.PosX, read.PosZ, false);
		//Debug.Log("read" + read.ObjId);
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
			playUi.RespawnButton_.SetActive(true);
            return;
        }


        // 휴 다행이 난 아니다 다른 탱크군 후훗 여기부분 처리해주자
        var OtherObjId = read.ObjId;
        
        // bullet destroy는 따로 올테니 일단은 신경안씀; 이 오브젝트 기준으로 bullet을 다 제거 할수도 있지만... 보류
    }

    public void handler_SC_NOTI_RESPAWN_CHARACTER(GAME.SC_NOTI_RESPAWN_CHARACTER read)
    {
        BattleInfo.TANK_INFO tank_info = new BattleInfo.TANK_INFO(new Vector3(read.PosX, read.PosY, read.PosZ), read.MaxHp, read.Hp, read.Speed, read.BulletSpeed, read.BulletPower, read.BulletDistance, read.ReloadTime);
        
        // 내가 respawn 됨
        if (MyObjId == read.ObjId)
        {
            
        }

        BattleLib.Instance.EntityRevive(read.ObjId, tank_info);
    }

    public void handler_SC_SELECT_BUFF(GAME.SC_SELECT_BUFF read)
    {
        BuffCount_ += read.Count;
		playUi.BuffSelectBtnShow();
        // 버프 가능 횟수 증가해줘야함 
        // 유저가 바빠 바로 못하면 2번 3번 쌓았다가 1개씩 처리;
        //ShowBuffButtons();
    }

    public void handler_SC_NOTI_UPDATE_CHARACTER_STATUS(GAME.SC_NOTI_UPDATE_CHARACTER_STATUS read)
    {
        if (read.ObjId == MyObjId && BuffCount_ <= 0)
        {
			playUi.BuffSelectBtnHide();
        }
        else
        {
            Debug.Log("업그레이드 버프 카운트 남아있음: " + BuffCount_);
        }
        BattleInfo.TANK_INFO tank_info = new BattleInfo.TANK_INFO(new Vector3(0.0f, 0.0f, 0.0f), read.MaxHp, read.Hp, read.Speed, read.BulletSpeed, read.BulletPower, read.BulletDistance, read.ReloadTime);
        BattleLib.Instance.SetTankInfo(read.ObjId, tank_info);
    }

    public void handler_SC_NOTI_ACTIVE_ITEM(GAME.SC_NOTI_ACTIVE_ITEM read)
    {
        Debug.Log("아이템 정보 받음");
        // 아이템 잔상 때문에 이렇게 한번에 보냄 기종 item 리스트와 비교해서 업데이트
        foreach (var item_info in read.ItemInfos)
        {
			BattleLib.Instance.CreateItem (item_info.ItemId , item_info.ItemType, item_info.PosX, item_info.PosY, item_info.PosZ);
        }
    }

    public void handler_SC_NOTI_ACQUIRE_ITEM(GAME.SC_NOTI_ACQUIRE_ITEM read)
    {
        Debug.Log("아아템 사용 패킷 받음");

        var item_type = (Item.Type)read.ItemType;

        // 효과 처리
        if (item_type == Item.Type.Hp_Item)
        {
            BattleLib.Instance.DestroyItem(read.ItemId, read.ObjId, (Item.Type)read.ItemType, read.Hp, 0);
        }
        else if (item_type == Item.Type.Shield_Item)
        {
            // 이 시간만큼만 해주고 타이머를 걸어서 효과 제거
            var shield_time = read.ShieldTime;
            BattleLib.Instance.DestroyItem(read.ItemId, read.ObjId, (Item.Type)read.ItemType, read.Hp, shield_time);
        }
    }

    public void handler_SC_NOTI_CREATE_MEDAL_ITEM(GAME.SC_NOTI_CREATE_MEDAL_ITEM read)
    {
        foreach (var medal_item_info in read.MedalItemInfos)
        {
            var item_id = medal_item_info.ItemId;
            //var from_pos = new Vector3(medal_item_info.FromPosX, medal_item_info.FromPosY, medal_item_info.FromPosZ);
			//var to_pos = new Vector3(medal_item_info.ToPosX, medal_item_info.ToPosY, medal_item_info.ToPosZ);

			BattleLib.Instance.CreateAniItem (medal_item_info.ItemId , (int)Item.Type.Medal_Item, 
				medal_item_info.FromPosX, medal_item_info.FromPosY, medal_item_info.FromPosZ, medal_item_info.ToPosX, medal_item_info.ToPosY, medal_item_info.ToPosZ);
			//Debug.Log("from pos: " + from_pos); 
			//Debug.Log("to pos: " + to_pos); 
        }
    }
    public void handler_SC_NOTI_ACQUIRE_PERSIST_ITEM(GAME.SC_NOTI_ACQUIRE_PERSIST_ITEM read)
    {
        var obj_id = read.ObjId;
        var item_id = read.ItemId;
        var count = read.Count;

        // 만약 내가 먹은게 아니라면 그냥 훈장만 삭제한다.
        /*
        if (MyObjId != obj_id)
        {

            return;
        }
        */

        Item.Type itemType = (Item.Type)read.ItemType;
        if (itemType == Item.Type.Medal_Item)
        {
            Debug.Log("메달 아이템 획득\n");
            Debug.Log("현재 매달 갯수: " + count);
        }
        else if (itemType == Item.Type.Coin_Item)
        {
            Debug.Log("코인 아이템 획득\n");
            Debug.Log("현재 코인 갯수: " + count);
        }

        BattleLib.Instance.DestroyItem(read.ItemId, obj_id, itemType, 0, 0);
    }

    public void handler_SC_NOTI_RANK_INFO(GAME.SC_NOTI_RANK_INFO read)
    {
        var index = 0;
        foreach(var rank_info in read.RankInfos)
        {
            var rank = playUi.Ranks[index];
            var name = rank.transform.GetChild(0);
            var textComponent = name.GetComponent<Text>();
            textComponent.text = rank_info.Nickname;

            var point = rank.transform.GetChild(2);
            textComponent = point.GetComponent<Text>();
            textComponent.text = rank_info.Score.ToString();

            ++index;
        }

        const int max_rank_info = 5;
        if (read.RankInfos.Count < max_rank_info)
        {
            var left_over_count = max_rank_info - read.RankInfos.Count + 1;
            for (var i = 1; i < left_over_count; ++i)
            {
                var rank = playUi.Ranks[max_rank_info - i];
                var name = rank.transform.GetChild(0);
                var textComponent = name.GetComponent<Text>();
                textComponent.text = "";

                var point = rank.transform.GetChild(2);
                textComponent = point.GetComponent<Text>();
                textComponent.text = "";
            }
        }
    }

    public void handler_SC_NOTI_RANK(GAME.SC_NOTI_RANK read)
    {
        var rank = playUi.MyRank;
        var name = rank.transform.GetChild(0);
        var textComponent = name.GetComponent<Text>();
        textComponent.text = read.Nickname;

        var rank_count = rank.transform.GetChild(1);
        textComponent = rank_count.GetComponent<Text>();
        textComponent.text = read.Rank.ToString();

        var point = rank.transform.GetChild(2);
        textComponent = point.GetComponent<Text>();
        textComponent.text = read.Score.ToString();
    }
    public void handler_SC_PING(GAME.SC_PING read)
    {
        Debug.Log("핑 받음");
        var Send = new GAME.CS_PING();
        ProtobufManager.Instance().Send(opcode.CS_PING, Send);
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
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_ACTIVE_ITEM>(opcode.SC_NOTI_ACTIVE_ITEM, handler_SC_NOTI_ACTIVE_ITEM);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_ACQUIRE_ITEM>(opcode.SC_NOTI_ACQUIRE_ITEM, handler_SC_NOTI_ACQUIRE_ITEM);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_CREATE_MEDAL_ITEM>(opcode.SC_NOTI_CREATE_MEDAL_ITEM, handler_SC_NOTI_CREATE_MEDAL_ITEM);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_ACQUIRE_PERSIST_ITEM>(opcode.SC_NOTI_ACQUIRE_PERSIST_ITEM, handler_SC_NOTI_ACQUIRE_PERSIST_ITEM);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_RANK_INFO>(opcode.SC_NOTI_RANK_INFO, handler_SC_NOTI_RANK_INFO);
        ProtobufManager.Instance().SetHandler<GAME.SC_NOTI_RANK>(opcode.SC_NOTI_RANK, handler_SC_NOTI_RANK);
        ProtobufManager.Instance().SetHandler<GAME.SC_PING>(opcode.SC_PING, handler_SC_PING);
    }

	void OnTouchBegan(Vector3 pos)
	{
		BeginPos = pos;
	}
		
	/*
	void JoystickModeTouch(Vector3 pos)
	{
		Debug.Log ("JoystickModeTouch");

		if (!playUi.UseJoystick) 
		{
			return;
		}

		if (BattleLib.Instance.GetMyEntityIsDead ())
			return;

		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out TFire);

		BattleLib.Instance.TryFire(MyObjId, TFire.point.x, TFire.point.z);
	}
	*/

	void OnTouchEnded(Vector3 pos)
	{
		
		if (playUi.UseJoystick) 
		{
			return;
		}

		if (BattleLib.Instance.GetMyEntityIsDead ())
			return;


		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out TFire);

		if (Vector3.Distance (BeginPos, pos) > DRAG_AS_FIRE_DISTANCE) 
		{
            BattleLib.Instance.TryFire(TFire.point.x, TFire.point.z);
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
        MyObjId = 0;
        
        //BuffSelectBtnHide();
        //RespawnButton_.enabled = false;

        RegisterPacketHandler();

        SendSyncField();

		_TouchDispatcher.BeganDelegate = new TouchDispatcher.TouchDelegate(OnTouchBegan);
		_TouchDispatcher.EndedDelegate = new TouchDispatcher.TouchDelegate(OnTouchEnded);

		//InputManager.Instance.ClickDelegate += JoystickModeTouch;
		//Controller.Instance.test1dele = Testaaa;
		//Controller.Instance.test2dele = Testbbb;

		//test.Instance.test1dele = Testaaa;
		//test.Instance.test2dele = Testbbb;
		//aatest.test1dele = Testaaa;
		//aatest.test2dele = Testbbb;


		 //Controller.Instance.ClickDelegate += JoystickModeTouch;
    }

	public void Testaaa()
	{
		Debug.Log("Testaaa:");
	}

	public void Testbbb()
	{
		Debug.Log("Testbbb:");
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

		if (obj_id == MyObjId) 
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
		}
    }
		
    public void onRespawnButton()
    {
		playUi.RespawnButton_.SetActive(false);
        var Send = new GAME.CS_RESPAWN_CHARACTER();
        ProtobufManager.Instance().Send(opcode.CS_RESPAWN_CHARACTER, Send);
    }

    public void ShowBuffButtons()
    {
        //BuffButtons_.SetActive(true);
    }

    public void HideBuffButtons()
    {
        //BuffButtons_.SetActive(false);
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

        if (BuffCount_ > 0)
        {
            BuffCount_--;
        }
        // 서버에 전송
        var Send = new GAME.CS_ENHANCE_BUFF();
        Send.BuffType = type;
        ProtobufManager.Instance().Send(opcode.CS_ENHANCE_BUFF, Send);
    }

	/*
	public void BuffSelectBtnShow()
	{
        if (BuffRootHide.activeSelf)
        {
            return;
        }

        Debug.Log ("BuffSelectBtnShow");
		BuffRootHide.SetActive (true);
		BuffRootShow.SetActive (false);
		RectTransform rootRect = BuffRoot_.GetComponent<RectTransform> ();
		BuffRoot_.GetComponent<RectTransform> ().localPosition 
		= new Vector3 (BuffRoot_.GetComponent<RectTransform>().localPosition.x, BuffRoot_.GetComponent<RectTransform> ().localPosition.y + 100, BuffRoot_.GetComponent<RectTransform>().localPosition.z);

	}

	public void BuffSelectBtnHide()
	{
        if (!BuffRootHide.activeSelf)
        {
            return;
        }


        Debug.Log ("BuffSelectBtnHide");
		BuffRootHide.SetActive (false);
		BuffRootShow.SetActive (true);
		RectTransform rootRect = BuffRoot_.GetComponent<RectTransform> ();
		BuffRoot_.GetComponent<RectTransform> ().localPosition 
		= new Vector3 (BuffRoot_.GetComponent<RectTransform>().localPosition.x, BuffRoot_.GetComponent<RectTransform> ().localPosition.y - 100, BuffRoot_.GetComponent<RectTransform>().localPosition.z);

	}
	*/

    private void Reset()
    {
        BuffCount_ = 0;
    }
}
