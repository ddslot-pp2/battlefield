using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject coolTimer;
    public GameObject expBar;
    public GameObject levelText;
    public GameObject levelPointPanel;
    public GameObject gameOver;

    [HideInInspector]
    public float leftTime;
    [HideInInspector]
    public Tank_State playerState;

    public float coolTime;

    public void CoolTimeCounter(float coolTime)
    {
        this.leftTime = coolTime;
        this.coolTime = coolTime;
        coolTimer.GetComponent<Image>().fillAmount = 0;
    }

    public void addExperience(int exp)
    {
        playerState.exp += exp;

        if (playerState.exp == 100)
        {
            playerState.level += 1;
            playerState.exp = 0;
            LevelUpPoint();
        }

        levelText.GetComponent<Text>().text = "Lv." + playerState.level;
        float ratio = ((float)playerState.exp / 100.0f);
        Debug.Log(ratio);
        expBar.GetComponent<Image>().fillAmount = ratio;
    }

    public void addExperience(GameObject attacker, int exp)
    {
        attacker.GetComponent<Tank_State>().exp += exp;

        if (attacker.GetComponent<Tank_State>().exp == 100)
        {
            attacker.GetComponent<Tank_State>().level += 1;
            attacker.GetComponent<Tank_State>().exp = 0;
        }

        levelText.GetComponent<Text>().text = "Lv." + playerState.level;
        float ratio = 1.0f - ((float)playerState.exp / 100.0f);
        Debug.Log(ratio);
        expBar.GetComponent<Image>().fillAmount = ratio;
    }

    public void LevelUpPoint()
    {
        levelPointPanel.GetComponent<LevelPointManager>().LevelUpPoint();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void exit()
    {
        SceneManager.LoadScene(1);
    }

    public virtual void GameOver()
    {
    }
}
