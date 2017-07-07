using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpeedUp : MonoBehaviour, IPointerDownHandler
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerDown(PointerEventData ped)
    {
        Debug.Log("The mouse click was released");
        transform.parent.GetComponent<LevelPointManager>().exit = true;
        Tank_State playerState = GameObject.Find("GameManager").GetComponent<GameManager>().playerState;
        playerState.moveSpeed = (int)(playerState.moveSpeed * 1.1);
    }
}
