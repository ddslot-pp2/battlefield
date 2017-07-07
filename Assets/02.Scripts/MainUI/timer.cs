using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour {

    public float timeCounter;
    public GameObject itemSlot;
    public GameObject clickItemBtn;
    public Sprite emptySlot;
    public GameObject counterTxtBox;
    public GameObject tmpObject;
    public GameObject txtBox;

    private GameObject timerTxt;

    int min;
    int sec;

    // Use this for initialization
    void Start () {
        timerTxt = gameObject.transform.Find("timerTxt").gameObject;
        CreateBtnDownCounter();
    }
	
	// Update is called once per frame
	void Update () {
        timeCounter -= Time.deltaTime;
        Debug.Log(min + " " + sec);

        TimeCount();
        string MinSec = min + ":" + sec;

        timerTxt.GetComponent<Text>().text = MinSec;
        txtBox.GetComponent<Text>().text = MinSec;

        if (min <= 0 && sec <= 0)
        {
            clickItemBtn.SetActive(true);
            itemSlot.GetComponent<Image>().sprite = emptySlot;
            itemSlot.GetComponent<Button>().enabled = true;

            Destroy(tmpObject);
            Destroy(gameObject);
        }
    }


    void TimeCount()
    {
        min = (int)timeCounter / 60;
        sec = (int)timeCounter % 60;
    }

    public void CreateBtnDownCounter()
    {
        tmpObject = GameObject.Instantiate(counterTxtBox, new Vector3(0, 0, 0), Quaternion.identity);
        tmpObject.transform.SetParent(itemSlot.transform);
        tmpObject.transform.localPosition = new Vector3(0, -35, 0);

        txtBox = tmpObject.transform.Find("Text").gameObject;
    }
}
