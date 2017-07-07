using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankBuy : MonoBehaviour {
    public GameObject[] tankBuyBtn;
    public Canvas buyPageCanvas;
    public GameObject user;
    public Text Gold;
    public Text Gem;
    // Use this for initialization
    void Start () {

        foreach(var haveTank in UserManager.Instance.haveTank)
        {
            tankBuyBtn[haveTank - 1].transform.Find("slotBtn").gameObject.SetActive(false);
            tankBuyBtn[haveTank - 1].transform.Find("TankSelectBtn").gameObject.SetActive(true);
        }

        tankBuyBtn[UserManager.Instance.selectTank - 1].transform.Find("slotBtn").gameObject.SetActive(false);
        tankBuyBtn[UserManager.Instance.selectTank - 1].transform.Find("selectedBtn").gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TankSelectButton(int btnNum)
    {
		Debug.Log("btnNum: " + btnNum);
        tankBuyBtn[UserManager.Instance.selectTank - 1].transform.Find("TankSelectBtn").gameObject.SetActive(true);
        tankBuyBtn[UserManager.Instance.selectTank - 1].transform.Find("selectedBtn").gameObject.SetActive(false);
        UserManager.Instance.selectTank = btnNum;
        tankBuyBtn[UserManager.Instance.selectTank - 1].transform.Find("selectedBtn").gameObject.SetActive(true);
        tankBuyBtn[UserManager.Instance.selectTank - 1].transform.Find("TankSelectBtn").gameObject.SetActive(false);
    }

    public void BuyTankButton(int btnNum)
    {
        switch (btnNum)
        {
            case 1:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
            case 2:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
            case 3:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
            case 4:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
            case 5:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
            case 6:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
            case 7:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
            case 8:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
            case 9:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
            case 10:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
            case 11:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
            case 12:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
            case 13:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
            case 14:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
            case 15:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
            case 16:
                {
                    if (GoldPriceTankPaid(500))
                        SelectedButtonChange(btnNum);
                    break;
                }
        }
    }

    public Boolean GoldPriceTankPaid(int pay)
    {
        if (UserManager.Instance.getGold >= pay)
        {
            UserManager.Instance.getGold -= pay;
            Gold.text = UserManager.Instance.getGold.ToString();
            return true;

        }
        else
        {
            return false;
        }
    }

    public Boolean GemPriceTankPaid(int pay)
    {
        if (UserManager.Instance.getGold >= pay)
        {
            UserManager.Instance.getGem -= pay;
            Gem.text = UserManager.Instance.getGem.ToString();
            return true;

        }
        else
        {
            return false;
        }
    }

    public void SelectedButtonChange(int btnNum)
    {
        tankBuyBtn[btnNum - 1].transform.Find("slotBtn").gameObject.SetActive(false);
        tankBuyBtn[btnNum - 1].transform.Find("TankSelectBtn").gameObject.SetActive(true);
        UserManager.Instance.haveTank.Add(btnNum);
    }

    public void BuyPageExit()
    {
        buyPageCanvas.enabled = false;
    }

    public void BuyPageOpen()
    {
        buyPageCanvas.enabled = true;
    }
}
