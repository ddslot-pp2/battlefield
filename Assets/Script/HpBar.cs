using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour {
    public GameObject healthBarPrefab; //체력게이지프리팹을담음
    public GameObject healthBarObj;//프리팹으로 생성한 인스턴스를 담음
	public Slider hpSlider;
    private HpBar2 hpBar2;
    public Sprite emptyHp;
    public Sprite fullHp;

	Transform myTransform;
	Tank_State state;

    // Use this for initialization
    void Awake()
    {
        myTransform = this.transform;
        healthBarObj = Instantiate(healthBarPrefab, transform.position, transform.rotation) as GameObject;
        hpBar2 = healthBarObj.GetComponent<HpBar2>();
        state = GetComponent<Tank_State>();
    }

    void Start()
    {
        hpBar2.nickname.text = gameObject.name;
    }

	public void DeleteHpobject()
	{
		Destroy(healthBarObj);
	}

	public void UpdateHpBar()
	{
		float maxHealth = state.maxHp;
		float currHealth = state.hp;

		float healthPercent = currHealth / maxHealth;
		hpBar2.healthBarImage.fillAmount = healthPercent;

		if( hpSlider != null)
		hpSlider.value = healthPercent;
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		healthBarObj.transform.position = Camera.main.WorldToViewportPoint(myTransform.position);//월드좌표에서 뷰포트 좌표로 변환
		Vector3 pos = myTransform.position;
        pos.y += 10f;
        healthBarObj.transform.position = pos;
        healthBarObj.transform.eulerAngles = new Vector3(50, 0, 0); 
     }
    
}
