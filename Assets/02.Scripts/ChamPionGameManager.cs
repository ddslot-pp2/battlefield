using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ChamPionGameManager : GameManager {

    public GameObject gameTimerTxt;
    public GameObject Respon;

    float gameTime;

    // Use this for initialization
    void Start()
    {
        playerState = GameObject.Find("Main Camera").GetComponent<GameLoad>().userTank.GetComponent<Tank_State>();
        levelText.GetComponent<Text>().text = "Lv." + playerState.level;
        gameTime = 300.0f;
        Debug.Log(gameTime);
    }

    // Update is called once per frame
    void Update()
    {
        gameTime -= Time.deltaTime;

        String time = (int)gameTime / 60 + ":" + (int)gameTime % 60;
        gameTimerTxt.GetComponent<Text>().text = time;

        if (gameTime <= 0)
        {
            gameOver.SetActive(true);
        }

        if (leftTime > 0)
        {
            leftTime -= Time.deltaTime;
            float ratio = 1.0f - (leftTime / coolTime);

            coolTimer.GetComponent<Image>().fillAmount = ratio;
        }

    }

    public override void GameOver()
    {
        Respon.SetActive(true);
    }
}
