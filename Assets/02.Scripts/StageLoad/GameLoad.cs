using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoad : MonoBehaviour
{
    public Vector3[] Pos;
    public int SelectTank;
    public UserManager usm;
    public GameObject userTank;
    public GameObject[] Tank;
    public int[] startRot;
    public int[] endRot;
    public GameObject[] useItem;
    public GameObject[] btnItem;
    public GameObject Canvas;
    private void Awake()
    {
		Debug.Log ("objectname :" + gameObject.name);
        usm = GameObject.Find("UserManager").GetComponent<UserManager>();
        SelectTank = usm.selectTank;

        ResPon();
        ItemUse();
        ItemUse2();
    }
  
    public void ResPon()
    {
        int i = Random.Range(0, 4);
        userTank = Instantiate(Tank[SelectTank - 1], Pos[i], transform.rotation);
        userTank.transform.name = usm.nickName;
        userTank.transform.rotation = Quaternion.Euler(0, Random.Range(startRot[i], endRot[i]), 0);
    }

    public void ItemUse()
    {
        for(int i = 0; i < usm.useItem.Count; i++)
        {
            if(usm.useItem[i] == 1)
            {
                CreateItemImage(i, useItem[0]);
            }else if(usm.useItem[i] == 5)
            {
                CreateItemImage(i, useItem[1]);
            }
            else if (usm.useItem[i] == 7)
            {
                CreateItemImage(i, useItem[2]);
            }
        }
    }

    public void ItemUse2()
    {
        for (int i = 0; i < usm.TouchItem.Count; i++)
        {
            if (usm.TouchItem[i] == 2)
            {
                Debug.Log("asf");
                CreateItemBtn(i, btnItem[0]);
            }
            else if (usm.TouchItem[i] == 3)
            {
                CreateItemBtn(i, btnItem[1]);
            }
            else if (usm.TouchItem[i] == 4)
            {
                CreateItemBtn(i, btnItem[2]);
            }
            else if (usm.TouchItem[i] == 6)
            {
                CreateItemBtn(i, btnItem[3]);
            }
        }
    }
    public void CreateItemImage(int i, GameObject img)
    {
        GameObject item = Instantiate(img);
        item.transform.SetParent(Canvas.transform);
        item.transform.localPosition = new Vector2(-480 + (i * 95), 250);
    }

    public void CreateItemBtn(int i, GameObject img)
    {
        GameObject item = Instantiate(img);
        item.transform.SetParent(Canvas.transform);
        item.transform.localPosition = new Vector2(600, -80 + (i * 110));
    }

}
