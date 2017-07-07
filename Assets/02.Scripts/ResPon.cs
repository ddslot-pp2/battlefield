using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResPon : MonoBehaviour {
    int sec;
    public float timeCounter;
    public GameLoad gameLoad;

    public GameObject reponTxt;
    // Use this for initialization

    void Start () {
        timeCounter = 4.0f;
        gameLoad = GameObject.Find("Main Camera").GetComponent<GameLoad>();

    }

    // Update is called once per frame
    void Update () {
        timeCounter -= Time.deltaTime;
        sec = (int)timeCounter % 60;
        string sec2 = "" + sec;
        reponTxt.GetComponent<Text>().text = sec2;
        if(timeCounter <= 1)
        {
            Respon2();
            timeCounter = 3.0f;
            gameObject.SetActive(false);
        }
    }

    public void Respon2()
    {
        gameLoad.ResPon();
    }
}
