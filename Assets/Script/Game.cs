using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		SendLoadingCompelete();
		EnterUser ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void SendLoadingCompelete()
	{
		
	}

	public void SendUserPos()
	{
		
	}

	public void SendUserClickInfo()
	{

	}

	public void ReceiveUserClickInfo()
	{

	}

	public void EnterUser( )
	{
		int type = 1;
		int index = 0;
		string name = "player1";
		bool my = true;
		Vector3 spawnPos = new Vector3 (0.0f, 0.0f, 0.0f);
		BattleLib.Instance.CreateEntity (type, index, name, my, spawnPos);
	}

	public void ReceiveUserPos()
	{
		
	}


	public void ReceiveUserDamage()
	{

	}

	public void Dead()
	{

	}

	public void Revive()
	{

	}


}
