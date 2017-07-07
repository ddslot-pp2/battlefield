using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGunBullet : MonoBehaviour {
    //총알의 파괴력
    public int damage;
    //총알 초기 위치
    public Vector3 first;
    //총알 발사 속도
    public float speed;
    public float distance;
    public GameObject ExpEffect;
    public int bulletType;
   
    GameObject attacker;

    void Start()
    {
        first = gameObject.transform.position;
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
      
    }

    void Update()
    {
        if(Vector3.Distance(first, gameObject.transform.position) > distance)
        {
            GameObject exp = Instantiate(ExpEffect, transform.position, transform.rotation);
            Destroy(exp, 1.0f);
			gameObject.Recycle();
        }
    }

    public void GetDamageType(int Damage, GameObject attacker, float distance, float speed)
    {
        this.damage = Damage;
        this.attacker = attacker;
        this.distance = distance;
        this.speed = speed;
    }

    void OnTriggerEnter(Collider other)
    {
        //충돌한 게임오브젝트의 태그값 비교
        if (other.transform.tag == "Tank" || other.transform.tag == "EnemyTank" || other.transform.tag == "Soldier")
        {
            BulletDamageManager.Instance.GetDamage(damage, other.gameObject, attacker);
        }
        else if (other.transform.tag == "DestroyObject")
        {
            other.GetComponent<DestroyObject>().hit -= 1;
        }
        exp();
    }
    public void exp()
    {
        GameObject exp = Instantiate(ExpEffect, transform.position, transform.rotation);
        Destroy(exp, 1.0f);
    }
}
