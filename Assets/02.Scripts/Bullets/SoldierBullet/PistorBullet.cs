using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistorBullet : MonoBehaviour {
    //총알의 파괴력
    public int damage;
    //총알 초기 위치
    public Vector3 first;
    //총알 발사 속도
    public float speed = 1000.0f;
    public float distance;
    public GameObject ExpEffect;
    public int bulletType;
   

    void Start()
    {
        first = gameObject.transform.position;
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
       
    }

    void Update()
    {
        //Debug.Log(distance);
        if (Vector3.Distance(first, transform.position) >= distance)
        {
            GameObject exp = Instantiate(ExpEffect, transform.position, transform.rotation);
            Destroy(exp, 1.0f);
			gameObject.Recycle();
        }
    }

    public void GetDamageType(int damage, float distance)
    {
        this.damage = damage;
        this.distance = distance;
    }

    void OnTriggerEnter(Collider other)
    {
        //충돌한 게임오브젝트의 태그값 비교
        if (other.transform.tag == "Tank" || other.transform.tag == "Soldier" || other.transform.tag == "EnemyTank")
        {
            BulletDamageManager.Instance.GetDamage(damage, other.gameObject);
            GameObject exp = Instantiate(ExpEffect, transform.position, transform.rotation);
            Destroy(exp, 1.0f);
        }
        else if (other.transform.tag == "DestroyObject")
        {
            other.gameObject.GetComponent<DestroyObject>().hit -= 1;
        }
        else
        {

        }
		gameObject.Recycle();

    }
}
