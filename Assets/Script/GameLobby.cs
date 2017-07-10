using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLobby : MonoBehaviour {

	public void OnConnect(bool result)
	{
		if (result)
		{
			Debug.Log("접속 성공");
		}
		else
		{
			Debug.Log("접속 실패");
		}
	}

	void OnDisconnect()
	{
		Debug.Log("접속 끊김");
	}

	void Connect()
	{
		
	}

	void Disconnect()
	{
		
	}

	public void onClickConnectButton()
	{
		Debug.Log("접속 시도 버튼 클릭");
		Connect();
	}

	public void onClickDisconnectButton()
	{
		Debug.Log("연결 끊김 버튼 클릭");
		Disconnect();
	}

	public void onClickChangeSceneButton()
	{
		Debug.Log("로드 sample2 씬");
		UnityEngine.SceneManagement.SceneManager.LoadScene("GAME");
	}

	void RegisterProcessorHandler()
	{
		
	}

	void Start()
	{
	
		
	}

	void Update()
	{
	

	}

	void Destroy()
	{

	}

	public void onClickGameStart()
	{

	}

	
}
