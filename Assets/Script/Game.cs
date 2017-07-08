using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	protected RaycastHit TFire;
	Vector3 Click;

	float lastSendTime;

	// Use this for initialization
	void Start () 
	{
		SendLoadingCompelete();

		// 테스트용 생성
		EnterUser (1,0,"aaa", false, new Vector3(0.0f,0.0f,0.0f));
		EnterUser (2,1,"bbb", false, new Vector3(10.0f,0.0f,0.0f));
		EnterUser (3,2,"bbb", false, new Vector3(20.0f,0.0f,0.0f));

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(0))
		{
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out TFire);

			//Debug.Log ("TFire:" + TFire.transform.gameObject.tag);

			Click = TFire.point;
			Click.y = transform.position.y;

			bool attack = false;
			if (TFire.transform.gameObject.tag == "Tank") 
			{
				attack = true;
			}

			SendUserClickInfo(Click.x, Click.z, attack);
		}

		BattleLib.Instance.ProgressBattle();

		if (lastSendTime + 0.1f < Time.time)
		{
			SendUserPos();
			lastSendTime = Time.time;
		}
	}

	public void SendLoadingCompelete()
	{
		BattleLib.Instance.GameStart();
	}

	public void SendUserPos()
	{
		// 내위치 얻어오기
		Vector3 pos = BattleLib.Instance.GetMyEntityPos();

		// 서버 샌드 로직
	}

	public void SendUserClickInfo(float posX, float posZ, bool attack)
	{
		// 서버로 send

		// 움직임 테스트를 위해 바로 받음
		ReceiveUserClickInfo(0, posX, posZ, attack);
	}

	public void ReceiveUserClickInfo(int index, float posX, float posZ, bool attack )
	{
		BattleLib.Instance.ReceiveInput(index, posX, posZ, attack);
	}

	public void EnterUser( int type, int index, string name, bool my, Vector3 pos)
	{
		BattleLib.Instance.CreateEntity (type, index, name, my, pos);
	}

	public void ReceiveUserPos(int index, float posX, float posZ)
	{
		BattleLib.Instance.ReceivePos(index, posX, posZ );
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


}
