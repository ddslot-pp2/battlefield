using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class everyButton : MonoBehaviour {
    public GameObject itemBtn;
    public GameObject gemBtn;
    public GameObject OptionBtn;
    public GameObject Coupon;
    public InputField nick;

    public GameObject timer;
    public GameObject userManager;
    public GameObject goldTxt;
    public GameObject gemTxt;
    public GameObject stageImg;
    public GameObject titleNameText;
    public GameObject informationText;

    private GameObject parentObj;
    private GameObject tmpObject;
    private GameObject parent;
    private GameObject selectItemSlot;

    public Sprite[] itemImg;
    public Sprite[] unlockStage;
    public Sprite[] lockStage;
    public int selectStage;
    // Use this for initialization
    void Start () {
        selectStage = 0;
        stageImg.GetComponent<Image>().sprite = unlockStage[0];
        informationText.GetComponent<Text>().text = "종합 점수 : " + UserManager.Instance.Maxscore + "  획득 점수 : " + UserManager.Instance.beforScore;
        titleNameText.GetComponent<Text>().text = "초보들의 전쟁터";


        gemTxt.GetComponent<Text>().text = UserManager.Instance.getGem.ToString();
        goldTxt.GetComponent<Text>().text = UserManager.Instance.getGold.ToString();
        nick.text = UserManager.Instance.nickName;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ItemOpen(GameObject obj)
    {
        itemBtn.GetComponent<Canvas>().enabled = true;
        selectItemSlot = obj;
    }

    public void ItemClose()
    {
        itemBtn.GetComponent<Canvas>().enabled = false;
    }

    public void PurchaseGemOpen()
    {
        gemBtn.GetComponent<Canvas>().enabled = true;
    }

    public void PurchaseGemClose()
    {
        gemBtn.GetComponent<Canvas>().enabled = false;
    }

    public void OptionClose()
    {
        OptionBtn.GetComponent<Canvas>().enabled = false;
    }
    public void OptionOpen()
    {
        OptionBtn.GetComponent<Canvas>().enabled = true;
    }

    public void CouponOpen()
    {
        Coupon.SetActive(true);
        OptionBtn.transform.Find("OptionMain").gameObject.SetActive(false);
    }

    public void CancelCoupon()
    {
        Coupon.SetActive(false);
        OptionBtn.transform.Find("OptionMain").gameObject.SetActive(true);
    }

    public void ConfirmCoupon()
    {
        Coupon.SetActive(false);
        OptionBtn.transform.Find("Confirm").gameObject.SetActive(true);

        StartCoroutine("StartRutine");
    }

    IEnumerator StartRutine()
    {
        do
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("A");
                OptionBtn.transform.Find("OptionMain").gameObject.SetActive(true);
                OptionBtn.transform.Find("Confirm").gameObject.SetActive(false);
                OptionBtn.GetComponent<Canvas>().enabled = false;
                break;
            }
            yield return null;

        } while (true);
        
    }

    public void BuyItemBtn(GameObject obj)
    {
        string name = obj.name;
        switch (name)
        {
            case "slot1Btn" :
                if(!GoldPricePaid(500)) break;
                UserManager.Instance.useItem.Add(1);
                selectItemSlot.GetComponent<Button>().enabled = false;
                selectItemSlot.GetComponent<Image>().sprite = itemImg[0];
                objectCreate(obj, 3600);
                break;
            case "slot2Btn":
                if (!GoldPricePaid(500)) break;
                UserManager.Instance.TouchItem.Add(2);
                selectItemSlot.GetComponent<Button>().enabled = false;
                selectItemSlot.GetComponent<Image>().sprite = itemImg[1];
                objectCreate(obj, 3600);
                break;
            case "slot3Btn":
                if (!GoldPricePaid(500)) break;
                UserManager.Instance.TouchItem.Add(3);
                selectItemSlot.GetComponent<Button>().enabled = false;
                selectItemSlot.GetComponent<Image>().sprite = itemImg[2];
                objectCreate(obj, 3600);
                break;
            case "slot4Btn":
                if (!GoldPricePaid(500)) break;
                UserManager.Instance.TouchItem.Add(4);
                selectItemSlot.GetComponent<Button>().enabled = false;
                selectItemSlot.GetComponent<Image>().sprite = itemImg[3];
                objectCreate(obj, 3600);
                break;
            case "slot5Btn":
                if (!GemPricePaid(500)) break;
                UserManager.Instance.useItem.Add(5);
                selectItemSlot.GetComponent<Button>().enabled = false;
                selectItemSlot.GetComponent<Image>().sprite = itemImg[4];
                objectCreate(obj, 3600);
                break;
            case "slot6Btn":
                if (!GemPricePaid(500)) break;
                UserManager.Instance.TouchItem.Add(6);
                selectItemSlot.GetComponent<Button>().enabled = false;
                selectItemSlot.GetComponent<Image>().sprite = itemImg[5];
                objectCreate(obj, 3600);
                break;
            case "slot7Btn":
                if (!GemPricePaid(500)) break;
                UserManager.Instance.useItem.Add(7);
                selectItemSlot.GetComponent<Button>().enabled = false;
                selectItemSlot.GetComponent<Image>().sprite = itemImg[6];
                objectCreate(obj, 3600);
                break;

        }
    }

    public void objectCreate(GameObject obj, float time)
    {
        parent = obj.transform.parent.gameObject;
        tmpObject = GameObject.Instantiate(timer, new Vector3(0, 0, 0), Quaternion.identity);
        tmpObject.transform.SetParent(parent.transform);
        tmpObject.transform.localPosition = new Vector3(0, -109, 0);
        obj.SetActive(false);

        tmpObject.GetComponent<timer>().timeCounter = time;
        tmpObject.GetComponent<timer>().clickItemBtn = obj;
        tmpObject.GetComponent<timer>().itemSlot = selectItemSlot;
    }

    public Boolean GoldPricePaid(int pay)
    {
        if (UserManager.Instance.getGold >= pay)
        {
            UserManager.Instance.getGold -= pay;
            goldTxt.GetComponent<Text>().text = UserManager.Instance.getGold.ToString();
            itemBtn.GetComponent<Canvas>().enabled = false;
            return true;

        }
        else
        {
            return false;
        }
    }

    public Boolean GemPricePaid(int pay)
    {
        if (UserManager.Instance.getGem >= pay)
        {
            UserManager.Instance.getGem -= pay;
            gemTxt.GetComponent<Text>().text = UserManager.Instance.getGem.ToString();
            itemBtn.GetComponent<Canvas>().enabled = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void forward()
    {
        if(selectStage == 0)
        {
            selectStage = 1;
            if (UserManager.Instance.Maxscore >= 25000)
            {
                stageImg.GetComponent<Image>().sprite = unlockStage[selectStage];
                informationText.GetComponent<Text>().text = "종합 생존 시간 : " + UserManager.Instance.aliveTime / 3600 + "시간" + UserManager.Instance.aliveTime % 3600 / 60 + "분" + UserManager.Instance.aliveTime % 3600 % 60 + "초   생존 시간 : " + UserManager.Instance.beforeAliveTime / 3600 + "시간" + UserManager.Instance.beforeAliveTime % 3600 / 60 + "분" + UserManager.Instance.beforeAliveTime % 3600 % 60 +"초";
            }
            else
            {
                informationText.GetComponent<Text>().text = "종합 점수 25,000점 필요";
                stageImg.GetComponent<Image>().sprite = lockStage[selectStage];
            }

            titleNameText.GetComponent<Text>().text = "미스터리 유적지";
        }
        else if(selectStage == 1)
        {
            selectStage = 2;
            if (UserManager.Instance.aliveTime >= 12600)
            {
                stageImg.GetComponent<Image>().sprite = unlockStage[selectStage];
                informationText.GetComponent<Text>().text = "종합 구출 포로 : " + UserManager.Instance.rescue + "명   구출 포로 : " + UserManager.Instance.beforeRescue + "명";
            }
            else
            {
                informationText.GetComponent<Text>().text = "종합 생존 시간 : 3시간 30분 필요.";
                stageImg.GetComponent<Image>().sprite = lockStage[selectStage];
            }
            titleNameText.GetComponent<Text>().text = "포로 구출 작전";

        }
        else if(selectStage == 2)
        {
            selectStage = 3;
            if (UserManager.Instance.rescue >= 800)
            {
                stageImg.GetComponent<Image>().sprite = unlockStage[selectStage];
                informationText.GetComponent<Text>().text = "종합 점수 : " + UserManager.Instance.Maxscore + "  획득점수" + UserManager.Instance.beforScore;
            }
            else
            {
                informationText.GetComponent<Text>().text = "종합 구출 포로 800명이 필요.";
                stageImg.GetComponent<Image>().sprite = lockStage[selectStage];
            }
            titleNameText.GetComponent<Text>().text = "숲 속의 전설";
        }
        else if (selectStage == 3)
        {
            selectStage = 0;
            stageImg.GetComponent<Image>().sprite = unlockStage[0];
            informationText.GetComponent<Text>().text = "종합 점수 : " + UserManager.Instance.Maxscore + "  획득 점수 : " + UserManager.Instance.beforScore;
            titleNameText.GetComponent<Text>().text = "초보들의 전쟁터";
        }
    }

    public void Backward()
    {
        if (selectStage == 0)
        {
            selectStage = 3;
            if (UserManager.Instance.rescue >= 800)
            {
                stageImg.GetComponent<Image>().sprite = unlockStage[selectStage];
                informationText.GetComponent<Text>().text = "종합 점수 : " + UserManager.Instance.Maxscore + "  획득점수" + UserManager.Instance.beforScore;
            }
            else
            {
                informationText.GetComponent<Text>().text = "종합 구출 포로 800명이 필요.";
                stageImg.GetComponent<Image>().sprite = lockStage[selectStage];
            }
            titleNameText.GetComponent<Text>().text = "숲 속의 전설";
        }
        else if (selectStage == 1)
        {
            selectStage = 0;
            stageImg.GetComponent<Image>().sprite = unlockStage[0];
            informationText.GetComponent<Text>().text = "종합 점수 : " + UserManager.Instance.Maxscore + "  획득 점수 : " + UserManager.Instance.beforScore;
            titleNameText.GetComponent<Text>().text = "초보들의 전쟁터";
        }
        else if (selectStage == 2)
        {
            selectStage = 1;
            if (UserManager.Instance.Maxscore >= 25000)
            {
                stageImg.GetComponent<Image>().sprite = unlockStage[selectStage];
                informationText.GetComponent<Text>().text = "종합 생존 시간 : " + UserManager.Instance.aliveTime / 3600 + "시간" + UserManager.Instance.aliveTime % 3600 / 60 + "분" + UserManager.Instance.aliveTime % 3600 % 60 + "초   생존 시간 : " + UserManager.Instance.beforeAliveTime / 3600 + "시간" + UserManager.Instance.beforeAliveTime % 3600 / 60 + "분" + UserManager.Instance.beforeAliveTime % 3600 % 60 + "초";
            }
            else
            {
                informationText.GetComponent<Text>().text = "종합 점수 25,000점 필요";
                stageImg.GetComponent<Image>().sprite = lockStage[selectStage];
            }
            titleNameText.GetComponent<Text>().text = "미스터리 유적지";
        }
        else if (selectStage == 3)
        {
            selectStage = 2;
            if (UserManager.Instance.aliveTime >= 12600)
            {
                stageImg.GetComponent<Image>().sprite = unlockStage[selectStage];
                informationText.GetComponent<Text>().text = "종합 구출 포로 : " + UserManager.Instance.rescue + "명   구출 포로 : " + UserManager.Instance.beforeRescue + "명";
            }
            else
            {
                informationText.GetComponent<Text>().text = "종합 생존 시간 : 3시간 30분 필요.";
                stageImg.GetComponent<Image>().sprite = lockStage[selectStage];
            }
            titleNameText.GetComponent<Text>().text = "포로 구출 작전";
        }
    }

    public void InGame()
    {
		Debug.Log ("InGame");
        if(selectStage == 0)
        {
            SceneManager.LoadScene(UnityEngine.Random.Range(2, 4));
        }else if (selectStage == 1)
        {
            SceneManager.LoadScene(UnityEngine.Random.Range(4, 6));
        }
        else if (selectStage == 2)
        {
            SceneManager.LoadScene(UnityEngine.Random.Range(6, 8));
        }
        else if (selectStage == 3)
        {
            SceneManager.LoadScene(UnityEngine.Random.Range(8, 10));
        }
    }

    public void SingleGame()
    {
        if (selectStage == 0)
        {
            SceneManager.LoadScene(UnityEngine.Random.Range(10, 12));
        }
        else if (selectStage == 1)
        {
            SceneManager.LoadScene(UnityEngine.Random.Range(12, 14));
        }
        else if (selectStage == 2)
        {
            SceneManager.LoadScene(UnityEngine.Random.Range(14, 16));
        }
        else if (selectStage == 3)
        {
            SceneManager.LoadScene(UnityEngine.Random.Range(16, 18));
        }
    }

    public void NickNameInput()
    {
        UserManager.Instance.nickName = nick.text;
    }
}
