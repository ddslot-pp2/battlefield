﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayUi : MonoBehaviour {

	public Image coolTimer;


	bool CheckCoolTime = false;

	float myCoolTime = 0.0f;
	float progressTime = 0.0f;


	// Use this for initialization
	void Start () 
	{
		BattleLib.Instance.FireDelegate = ResetCoolTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( CheckCoolTime )
		{
			if (progressTime < myCoolTime) {
				progressTime += Time.deltaTime * 1000;
				float ratio = (progressTime / myCoolTime);
				coolTimer.fillAmount = ratio;
			} 
			else 
			{
				CheckCoolTime = false;
			}
		}
	}

	public void ResetCoolTime(float fireRate)
	{
		CheckCoolTime = true;
		coolTimer.fillAmount = 0;
		myCoolTime = fireRate;
		progressTime = 0.0f;
	}
}
