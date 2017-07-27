using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayUi : MonoBehaviour {

	public Image coolTimer;
	public GameObject BuffRoot_;
	public GameObject BuffRootHide;
	public GameObject BuffRootShow;
	public GameObject RespawnButton_;
	public GameObject UseJoypad;
	//public GameObject JoyStick;

	bool CheckCoolTime = false;

	float myCoolTime = 0.0f;
	float progressTime = 0.0f;

	public bool UseJoystick = false;

    private int max_rank_info = 5;
    public GameObject [] Ranks;
    public GameObject MyRank;

    // Use this for initialization
    void Start () 
	{
        BattleLib.Instance.FireDelegate = ResetCoolTime;
		RespawnButton_.SetActive(false);
		BuffSelectBtnHide();
		UseJoystick = false;
        
        //JoyStick.SetActive (false);
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

	public void ChangeJostick()
	{
		if (UseJoystick) {
			UseJoystick = false;
			//JoyStick.SetActive (false);
		} else 
		{
			UseJoystick = true;
			//JoyStick.SetActive (true);
		}
	}

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


}
