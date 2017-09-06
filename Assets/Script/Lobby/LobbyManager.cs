using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour {

    public GameObject NewTank;

    public Text MedalText;
    public Text CoinText;

    private string uuid_;
    public int CharacterType_ = 0;
	public GameObject garageRoom;
	public GameObject lobbyRoom;
	//public RenderTank[] renderTankList;
	public GarageRoom garageRoomManager;
	public GameObject tankRoot;
	public GameObject renderTank;

    public static bool log_in_ = false;

    // 접속 완료 후 콜백
    public void onConnect()
    {
        Debug.Log("OnConnected called\n");
        Login();
    }

    // 접속 종료시 콜백
    public void onDisconnect(SocketError ErrorCode)
    {
        Debug.Log("OnDisonnected called");
        Debug.Log("ErrorCode: " + ErrorCode);
    }

    // 이번 Scene에서 사용할 패킷 관련 핸들러
    public void handler_SC_LOG_IN(LOBBY.SC_LOG_IN read)
    {
        Debug.Log("recv handler_SC_LOG_IN");
        Debug.Log(read.Result);
        Debug.Log(read.Nickname);

        MedalText.text = read.MedalCount.ToString();
        CoinText.text = read.CoinCount.ToString();

        CharacterType_ = read.CharacterType;

		CreateRenderMainTank (CharacterType_);
        // 필드 리스트 요청
        var Send = new LOBBY.CS_FIELD_LIST();
        ProtobufManager.Instance().Send(opcode.CS_FIELD_LIST, Send);

        log_in_ = true;

		if (log_in_) 
		{
			GetMyInfo ();
			GetMyCharacterList ();
		}
    }

    public void handle_SC_FIELD_LIST(LOBBY.SC_FIELD_LIST read)
    {
        foreach(var field_info in read.FieldInfos)
        {
            Debug.Log(field_info.FieldId);
            Debug.Log(field_info.UserCount);
            Debug.Log(field_info.MaxUserCount);
            Debug.Log("----------------------------");
        }

        // 케릭터 정보
        var Send = new LOBBY.CS_CHARACTER_INFO();
        Send.CharacterType = CharacterType_;
        ProtobufManager.Instance().Send(opcode.CS_CHARACTER_INFO, Send);
    }

    public void handler_SC_ENTER_FIELD(LOBBY.SC_ENTER_FIELD read)
    {
        // 게임씬 로딩 시작
        Debug.Log("게임씬 로딩");
        UnityEngine.SceneManagement.SceneManager.LoadScene("GAME");
    }

    public void handler_SC_LEAVE_FIELD(LOBBY.SC_LEAVE_FIELD read)
    {

    }

    public void handler_SC_PURCHASE_CHARACTER(LOBBY.SC_PURCHASE_CHARACTER read)
    {
        var result = read.Result;

        if (!result)
        {
            Debug.Log("탱크 구매 실패\n");
            var ec = read.Ec;
            Debug.Log(ec);
            return;
        }

        MedalText.text = read.MedalCount.ToString();
        CoinText.text = read.CoinCount.ToString();
        Debug.Log("탱크 구매 성공\n");
		garageRoomManager.RefreshBoxUi ();
		GetMyCharacterList ();
        //NewTank.SetActive(false);
    }

    public void handler_SC_CHARACTER_INFO(LOBBY.SC_CHARACTER_INFO read)
    {
        Debug.Log("recv SC_CHARACTER_INFO\n");
        var max_hp = read.MaxHp;
        var speed = read.Speed;
        var bullet_speed = read.BulletSpeed;
        var bullet_power = read.BulletPower;
        var bullet_distance = read.BulletDistance;
        var reload_time = read.ReloadTime;

        // 100 기준으로 들어오기땜에 그에 맞춰서 필요한 요소만 ui 업데이트 필요
        // ui ratio는 1.0이 max이므로 100 -> 1.0; 즉 20이면 -> 0.2

    }

    public void handler_SC_MY_INFO(LOBBY.SC_MY_INFO read)
    {
		Debug.Log("handler_SC_MY_INFO");

        var result = read.Result;
        if (!result)
        {
            Debug.Log("SC_MY_INFO result is not true");
            return;
        }

        var medal_count = read.MedalCount;
        var coin_count = read.CoinCount;

        Debug.Log("medal_count: " + medal_count);
        Debug.Log("coin_count: "  + coin_count);
    }

    public void handler_SC_MY_CHARACTER_INFO(LOBBY.SC_MY_CHARACTER_INFO read)
    {
		Debug.Log("handler_SC_MY_CHARACTER_INFO");

        var result = read.Result;
        if (!result)
        {
            Debug.Log("SC_MY_INFO result is not true");
            return;
        }

        foreach (var character_info in read.CharacterInfos)
        {
            var tank_type = character_info.Type;
            var max_hp = character_info.MaxHp;
            var speed = character_info.Speed;


			garageRoomManager.renderTank[tank_type].byTank = true;
			garageRoomManager.SetDisableBox (tank_type);
            Debug.Log("tank_type: " + tank_type);
        }
    }

    public void handler_SC_PING(GAME.SC_PING read)
    {
        Debug.Log("핑 받음");
        var Send = new GAME.CS_PING();
        ProtobufManager.Instance().Send(opcode.CS_PING, Send);
    }

    // 사용할 패킷 등록
    public void RegisterPacketHandler()
    {
        // 이번 패킷에 사용할 패킷관련 핸들러를 지정
        ProtobufManager.Instance().SetHandler<LOBBY.SC_LOG_IN>(opcode.SC_LOG_IN, handler_SC_LOG_IN);
        ProtobufManager.Instance().SetHandler<LOBBY.SC_FIELD_LIST>(opcode.SC_FIELD_LIST, handle_SC_FIELD_LIST);
        ProtobufManager.Instance().SetHandler<LOBBY.SC_ENTER_FIELD>(opcode.SC_ENTER_FIELD, handler_SC_ENTER_FIELD);
        ProtobufManager.Instance().SetHandler<LOBBY.SC_LEAVE_FIELD>(opcode.SC_LEAVE_FIELD, handler_SC_LEAVE_FIELD);
        ProtobufManager.Instance().SetHandler<LOBBY.SC_PURCHASE_CHARACTER>(opcode.SC_PURCHASE_CHARACTER, handler_SC_PURCHASE_CHARACTER);
        ProtobufManager.Instance().SetHandler<LOBBY.SC_CHARACTER_INFO>(opcode.SC_CHARACTER_INFO, handler_SC_CHARACTER_INFO);
        ProtobufManager.Instance().SetHandler<LOBBY.SC_MY_INFO>(opcode.SC_MY_INFO, handler_SC_MY_INFO);
        ProtobufManager.Instance().SetHandler<LOBBY.SC_MY_CHARACTER_INFO>(opcode.SC_MY_CHARACTER_INFO, handler_SC_MY_CHARACTER_INFO);
        ProtobufManager.Instance().SetHandler<GAME.SC_PING>(opcode.SC_PING, handler_SC_PING);
    }

    // Use this for initialization
    void Start ()
    {
        uuid_ = SystemInfo.deviceUniqueIdentifier;
        Debug.Log("uuid: " + uuid_);
        //NewTank.SetActive(false);
        RegisterPacketHandler();
        //ProtobufManager.Instance().Connect("127.0.0.1", 3000, onConnect, onDisconnect);
        ProtobufManager.Instance().Connect("112.217.116.82", 3000, onConnect, onDisconnect);

		//garageRoomManager.renderTank[0].byTank = true;
		//garageRoomManager.renderTank[2].byTank = true;
		//garageRoomManager.renderTank[3].byTank = true;

		//garageRoomManager.SetDisableBox (0);
		//garageRoomManager.SetDisableBox (2);
		//garageRoomManager.SetDisableBox (3);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // 업데이트 할때마다 패킷을 처리해 핸들러를 호출
        ProtobufManager.Instance().ProcessPacket();
    }

    void OnDestroy()
    {
        // 섹션 정리
        //ProtobufManager.Instance().Destroy();
    }

    public void onStartButton()
    {
        // [테스트 패킷]
        /*
        var TestSend = new LOBBY.CS_MY_CHARACTER_INFO();
        ProtobufManager.Instance().Send(opcode.CS_MY_CHARACTER_INFO, TestSend);
        return;
        */

        var Send = new LOBBY.CS_ENTER_FIELD();
        Send.FieldId = 0;
        ProtobufManager.Instance().Send(opcode.CS_ENTER_FIELD, Send);
    }

    public void onNewTankButton(bool active)
    {
        NewTank.SetActive(active);
    }

    public void onPurchaseCoinButton()
    {
        Debug.Log("코인 구매하기");
    }

    private void Login()
    {
        var Send = new LOBBY.CS_LOG_IN();

        if (!log_in_)
        {
            Send.Id = uuid_;
            Send.Password = "1234ABCD";
            ProtobufManager.Instance().Send(opcode.CS_LOG_IN, Send);
            return;
        }

        //ProtobufManager.Instance().Send(opcode.CS_MY_INFO, Send);
    }

    public void onPurchaseTankButton(int type)
    {
		int selectType = garageRoomManager.GetSelectIndex();

		if (selectType < 0)
			return;
		
		Debug.Log("탱크 구매하기 버튼 클릭: " + selectType.ToString());
        var Send = new LOBBY.CS_PURCHASE_CHARACTER();
		Send.CharacterType = selectType;
        ProtobufManager.Instance().Send(opcode.CS_PURCHASE_CHARACTER, Send);
    }

	public void GetMyInfo()
	{
		var Send = new LOBBY.CS_MY_INFO();
		ProtobufManager.Instance().Send(opcode.CS_MY_INFO, Send);
	}

	public void GetMyCharacterList()
	{
		var Send = new LOBBY.CS_MY_CHARACTER_INFO();
		ProtobufManager.Instance().Send(opcode.CS_MY_CHARACTER_INFO, Send);
	}

	public void UpgradeTank()
	{
		//var Send = new LOBBY.();
		//ProtobufManager.Instance().Send(opcode.CS_TANK_UPGRADE, Send);
	}

	public void onGarageButton()
	{
		lobbyRoom.SetActive (false);
		garageRoom.SetActive (true);
	}

	public void onGarageCloseButton()
	{
		lobbyRoom.SetActive (true);
		garageRoom.SetActive (false);
	}

	public void onSelectTankButton()
	{
		CharacterType_ = garageRoomManager.GetSelectIndex();

		CreateRenderMainTank (CharacterType_);
		//tankobject.transform.localRotation = Quaternion.Euler( new Vector3( 0.0f, -45.0f, 0.0f));
		//tankobject.transform.localPosition = new Vector3 (0.0f, 2.0f, 0.0f);
		//tankobject.transform.position = new Vector3 (0.0f - 15 * i, 4.0f, 0.0f);
		//renderTankPosX[i] = tankobject.transform.position.x;
	}

	void CreateRenderMainTank(int tankType)
	{
		if( renderTank != null )
		{
			DestroyObject( renderTank );
		}

		Debug.Log ("tankType: " + tankType);
		renderTank = (GameObject)Instantiate(garageRoomManager.renderTank[tankType].gameObject);
		renderTank.transform.parent = tankRoot.transform;
		renderTank.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
		renderTank.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);

	}
}
