using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleGameManager : GameManager {

    // Use this for initialization
    void Start()
    {
        playerState = GameObject.Find("Main Camera").GetComponent<GameLoad>().userTank.GetComponent<Tank_State>();
        levelText.GetComponent<Text>().text = "Lv." + playerState.level;
    }

    // Update is called once per frame
    void Update()
    {
        if (leftTime > 0)
        {
            leftTime -= Time.deltaTime;
            float ratio = 1.0f - (leftTime / coolTime);

            coolTimer.GetComponent<Image>().fillAmount = ratio;
        }

    }

    public override void GameOver()
    {
        gameOver.SetActive(true);
    }
}
