using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;

public class GarageRoom : MonoBehaviour {

	TouchDispatcher _TouchDispatcher = new TouchDispatcher();

	Vector3 BeginPos,LastPos_, ArrivePos, StartPos;

	public GameObject TankRoot;
	public RenderTank[] renderTank;
	public GameObject[] rockBoxList;
	public GameObject StatUi;
	public GameObject UpgradeUi;
	public GameObject BuyUi;

	bool move = false;
	float distance = 9.0f;

	int currentIndex = 0;
	int maxIndex = 16;


	public int GetSelectIndex()
	{
		if (renderTank [currentIndex].byTank == true) 
		{
			return -1;
		}

		return currentIndex;
	}
	// Use this for initialization
	void Start () {

		_TouchDispatcher.BeganDelegate = new TouchDispatcher.TouchDelegate(OnTouchBegan);
		_TouchDispatcher.EndedDelegate = new TouchDispatcher.TouchDelegate(OnTouchEnded);

		StartPos = TankRoot.transform.position;

		/*
		for (int i = 0; i < maxIndex; i++) 
		{
			GameObject initobject = (GameObject)Instantiate(boxSet);
			initobject.transform.parent = TankRoot.transform;
			initobject.transform.localPosition = new Vector3 (0.0f + distance * i, 0.0f, 0.0f);
			initobject.transform.localRotation = Quaternion.Euler( new Vector3( 0.0f, 55.0f, 0.0f));


			GameObject tankobject = (GameObject)Instantiate(renderTank[i]);
			tankobject.transform.parent = initobject.transform;
			tankobject.transform.localRotation = Quaternion.Euler( new Vector3( 0.0f, -45.0f, 0.0f));
			tankobject.transform.localPosition = new Vector3 (0.0f, 2.0f, 0.0f);
			//tankobject.transform.position = new Vector3 (0.0f - 15 * i, 4.0f, 0.0f);
			//renderTankPosX[i] = tankobject.transform.position.x;

		}
		*/
		
	}
	
	// Update is called once per frame
	void Update () {

		if (move) 
		{
			if (Vector3.Distance (TankRoot.transform.position, ArrivePos) > 0.1f) 
			{
				
				Vector3 dir = (ArrivePos - TankRoot.transform.position).normalized;
			
				TankRoot.transform.position = Vector3.Lerp (TankRoot.transform.position, ArrivePos,  0.1f);

			} 
			else 
			{
				if (currentIndex >= 0 && currentIndex < maxIndex) 
				{
					if (renderTank [currentIndex].byTank) 
					{
						StatUi.SetActive(true);
						UpgradeUi.SetActive(true);
						BuyUi.SetActive(false);;
					}
					else 
					{
						StatUi.SetActive(false);
						UpgradeUi.SetActive(false);
						BuyUi.SetActive(true);;
					}
				}
				move = false;
				// 정보 셋팅.
			}
		}
		
	}

	public void RefreshBoxUi()
	{
		StatUi.SetActive(true);
		UpgradeUi.SetActive(true);
		BuyUi.SetActive(false);;
	}

	public void SetDisableBox(int tankNumber)
	{
		rockBoxList [tankNumber].gameObject.SetActive (false);
	}


	void OnTouchBegan(Vector3 pos)
	{
		BeginPos = pos;
	}


	public void ClearBoxUi()
	{
		StatUi.SetActive(false);
		UpgradeUi.SetActive(false);
		BuyUi.SetActive(false);
	}

	void OnTouchEnded(Vector3 pos)
	{
		if (BeginPos.x > pos.x + 3.0f) 
		{
			Debug.Log ("<----");

			if (currentIndex > 0) 
			{
				currentIndex--;
				ArrivePos =   new Vector3 (StartPos.x +  (currentIndex * distance), StartPos.y, StartPos.z);

				ClearBoxUi();

				move = true;
			}
				
		} 
		else if (BeginPos.x < pos.x + 3.0f) 
		{
			Debug.Log ("---->");
			if (currentIndex < maxIndex) 
			{
				currentIndex++;
				ArrivePos =  new Vector3 (StartPos.x +  (currentIndex * distance), StartPos.y, StartPos.z);

				ClearBoxUi();

				move = true;
			}
		}
	}

	void LateUpdate()
	{
		//if (move)
		//	return;
		
		_TouchDispatcher.Update();
	}
}
