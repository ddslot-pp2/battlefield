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

	Transform myTransform;
	Tank_State state;

    // Use this for initialization
    void Start()
    {
		myTransform = this.transform;
        healthBarObj = Instantiate(healthBarPrefab, transform.position, transform.rotation) as GameObject;
        hpBar2 = healthBarObj.GetComponent<HpBar2>();
        hpBar2.nickname.text = gameObject.name;
		state = GetComponent<Tank_State>();

		//hpBar2.healthBarImage[0].sprite = fullHp;
		//hpBar2.healthBarImage[1].sprite = emptyHp;
    }

	public void UpdateHpBar()
	{
		maxHealth = state.maxHp;
		currHealth = state.hp;

		float healthPercent = currHealth / maxHealth;
		hpBar2.healthBarImage.fillAmount = healthPercent;

		//hpBar2.healthBarImage[0].sprite = fullHp;
		/*
		if (healthPercent > 0 && healthPercent <= 0.2)
		{
			hpBar2.healthBarImage[0].sprite = fullHp;
			hpBar2.healthBarImage[1].sprite = emptyHp;
			hpBar2.healthBarImage[2].sprite = emptyHp;
			hpBar2.healthBarImage[3].sprite = emptyHp;
			hpBar2.healthBarImage[4].sprite = emptyHp;
		}
		else if (healthPercent > 0.2 && healthPercent <= 0.4)
		{
			hpBar2.healthBarImage[0].sprite = fullHp;
			hpBar2.healthBarImage[1].sprite = fullHp;
			hpBar2.healthBarImage[2].sprite = emptyHp;
			hpBar2.healthBarImage[3].sprite = emptyHp;
			hpBar2.healthBarImage[4].sprite = emptyHp;
		}
		else if (healthPercent > 0.4 && healthPercent <= 0.6)
		{
			hpBar2.healthBarImage[0].sprite = fullHp;
			hpBar2.healthBarImage[1].sprite = fullHp;
			hpBar2.healthBarImage[2].sprite = fullHp;
			hpBar2.healthBarImage[3].sprite = emptyHp;
			hpBar2.healthBarImage[4].sprite = emptyHp;
		}
		else if (healthPercent > 0.6 && healthPercent <= 0.8)
		{
			hpBar2.healthBarImage[0].sprite = fullHp;
			hpBar2.healthBarImage[1].sprite = fullHp;
			hpBar2.healthBarImage[2].sprite = fullHp;
			hpBar2.healthBarImage[3].sprite = fullHp;
			hpBar2.healthBarImage[4].sprite = emptyHp;
		}
		else if (healthPercent > 0.8 && healthPercent < 1)
		{
			hpBar2.healthBarImage[0].sprite = fullHp;
			hpBar2.healthBarImage[1].sprite = fullHp;
			hpBar2.healthBarImage[2].sprite = fullHp;
			hpBar2.healthBarImage[3].sprite = fullHp;
			hpBar2.healthBarImage[4].sprite = fullHp;
		}
		*/
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
			healthBarObj.transform.position = Camera.main.WorldToViewportPoint(myTransform.position);//월드좌표에서 뷰포트 좌표로 변환
			Vector3 pos = myTransform.position;
            pos.y += 10f;
            healthBarObj.transform.position = pos;
            healthBarObj.transform.eulerAngles = new Vector3(50, 0, 0);

           
        }
    
}
