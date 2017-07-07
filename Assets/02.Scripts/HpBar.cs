using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour {
    public GameObject healthBarPrefab; //체력게이지프리팹을담음
    public GameObject healthBarObj;//프리팹으로 생성한 인스턴스를 담음
    public float currHealth;//현재의체력
    public float maxHealth;//최대체력
    private HpBar2 hpBar2;
    public Sprite emptyHp;
    public Sprite fullHp;

    // Use this for initialization
    void Start()
    {
        healthBarObj = Instantiate(healthBarPrefab, transform.position, transform.rotation) as GameObject;
        hpBar2 = healthBarObj.GetComponent<HpBar2>();
        hpBar2.nickname.text = gameObject.name;
    }
	
	// Update is called once per frame
	void Update () {
            healthBarObj.transform.position = Camera.main.WorldToViewportPoint(transform.position);//월드좌표에서 뷰포트 좌표로 변환
            Vector3 pos = transform.position;
            pos.y += 10f;
            healthBarObj.transform.position = pos;
            healthBarObj.transform.eulerAngles = new Vector3(50, 0, 0);

            maxHealth = GetComponent<Tank_State>().maxHp;
            currHealth = GetComponent<Tank_State>().hp;

            float healthPercent = currHealth / maxHealth;
        
            if (healthPercent > 0 && healthPercent <= 0.2)
            {
                hpBar2.healthBarPrefab[0].GetComponent<Image>().sprite = fullHp;
                hpBar2.healthBarPrefab[1].GetComponent<Image>().sprite = emptyHp;
                hpBar2.healthBarPrefab[2].GetComponent<Image>().sprite = emptyHp;
                hpBar2.healthBarPrefab[3].GetComponent<Image>().sprite = emptyHp;
                hpBar2.healthBarPrefab[4].GetComponent<Image>().sprite = emptyHp;
            }
            else if (healthPercent > 0.2 && healthPercent <= 0.4)
            {
                hpBar2.healthBarPrefab[0].GetComponent<Image>().sprite = fullHp;
                hpBar2.healthBarPrefab[1].GetComponent<Image>().sprite = fullHp;
                hpBar2.healthBarPrefab[2].GetComponent<Image>().sprite = emptyHp;
                hpBar2.healthBarPrefab[3].GetComponent<Image>().sprite = emptyHp;
                hpBar2.healthBarPrefab[4].GetComponent<Image>().sprite = emptyHp;
            }
            else if (healthPercent > 0.4 && healthPercent <= 0.6)
            {
                hpBar2.healthBarPrefab[0].GetComponent<Image>().sprite = fullHp;
                hpBar2.healthBarPrefab[1].GetComponent<Image>().sprite = fullHp;
                hpBar2.healthBarPrefab[2].GetComponent<Image>().sprite = fullHp;
                hpBar2.healthBarPrefab[3].GetComponent<Image>().sprite = emptyHp;
                hpBar2.healthBarPrefab[4].GetComponent<Image>().sprite = emptyHp;
            }
            else if (healthPercent > 0.6 && healthPercent <= 0.8)
            {
                hpBar2.healthBarPrefab[0].GetComponent<Image>().sprite = fullHp;
                hpBar2.healthBarPrefab[1].GetComponent<Image>().sprite = fullHp;
                hpBar2.healthBarPrefab[2].GetComponent<Image>().sprite = fullHp;
                hpBar2.healthBarPrefab[3].GetComponent<Image>().sprite = fullHp;
                hpBar2.healthBarPrefab[4].GetComponent<Image>().sprite = emptyHp;
            }
            else if (healthPercent > 0.8 && healthPercent < 1)
            {
                hpBar2.healthBarPrefab[0].GetComponent<Image>().sprite = fullHp;
                hpBar2.healthBarPrefab[1].GetComponent<Image>().sprite = fullHp;
                hpBar2.healthBarPrefab[2].GetComponent<Image>().sprite = fullHp;
                hpBar2.healthBarPrefab[3].GetComponent<Image>().sprite = fullHp;
                hpBar2.healthBarPrefab[4].GetComponent<Image>().sprite = fullHp;
            }
        }
    
}
