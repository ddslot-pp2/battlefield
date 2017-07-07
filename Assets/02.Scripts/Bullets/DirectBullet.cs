using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectBullet : MonoBehaviour {
    //총알의 파괴력
    public int damage;
    //총알 초기 위치
    public Vector3 first;
    //총알 발사 속도
    public float speed;
    public float distance;
    public GameObject ExpEffect;
    public float hitCount;
    private int hitCountDown;
    public int bulletType;
   
    GameObject attacker;
	Rigidbody rigidbody;
	Transform myTransform;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		myTransform = transform;
	}

	void OnEnable()
	{
		hitCountDown = 0;
		first = gameObject.transform.position;
		rigidbody.velocity = Vector3.zero;
	}

    void Update()
    {
        if (Vector3.Distance(first, transform.position) >= distance)
        {
			GameObject exp = Instantiate(ExpEffect, myTransform.position, myTransform.rotation);
            Destroy(exp, 1.0f);
            //Destroy(gameObject);
			gameObject.Recycle ();
        }
    }

    public void GetDamageType(int damage, int hitCount, GameObject attacker, float distance, float speed)
    {
        this.damage = damage;
        this.hitCount = hitCount;
        this.attacker = attacker;
        this.distance = distance;
        this.speed = speed;

		rigidbody.AddForce(transform.forward * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        //충돌한 게임오브젝트의 태그값 비교
        if (other.transform.tag == "Tank" || other.transform.tag == "EnemyTank" ||other.transform.tag == "Soldier")
        {
            BulletDamageManager.Instance.GetDamage(damage, other.gameObject, attacker);
        }
        else if (other.transform.tag == "DestroyObject")
        {
            Debug.Log("aa");

            other.GetComponent<DestroyObject>().hit -= 1;
        }
        else
        {
        }

        hitCountDown++;

        exp();

        if (hitCountDown == hitCount)
        {
			gameObject.Recycle ();
           // Destroy(gameObject);
        }
    }

    public void exp()
    {
		GameObject exp = Instantiate(ExpEffect, myTransform.position, myTransform.rotation);
        Destroy(exp, 1.0f);

    }
}
