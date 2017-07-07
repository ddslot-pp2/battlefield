using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTypeBallBullet : BallBullet {
    void Start()
    {
        radius = 10;
        first = gameObject.transform.position;
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    void Update()
    {
        if (Vector3.Distance(first, transform.position) >= distance)
        {
            GameObject exp = Instantiate(ExpEffect, transform.position, transform.rotation);
            Destroy(exp, 1.0f);
            for (int i = 0; i < BoombCount; i++)
            {
                RaiusDamage(gameObject.transform.position + transform.forward * (i * radius));
            }
            
			gameObject.Recycle();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //충돌한 게임오브젝트의 태그값 비교
        if (other.transform.tag == "Tank" || other.transform.tag == "Soldier" || other.transform.tag == "DestroyObject" || other.transform.tag == "EnemyTank")
        {
            if (other.transform.tag == "Tank" || other.transform.tag == "Soldier" || other.transform.tag == "EnemyTank")
            {
                BulletDamageManager.Instance.GetDamage(damage, other.gameObject, attacker);
            }
            else if (other.transform.tag == "DestroyObject")
            {
                other.GetComponent<DestroyObject>().hit -= 1;
            }

            hitTankPlayer.Add(other.gameObject);
            for (int i = 0; i < BoombCount; i++)
            {
                RaiusDamage(gameObject.transform.position + transform.forward * (i * radius));
            }
            
			gameObject.Recycle();
        }
        else if (other.transform.tag == "Object")
        {
            GameObject exp = Instantiate(ExpEffect, transform.position, transform.rotation);
            Destroy(exp, 1.0f);
            Destroy(gameObject);
        }
        else
        {
            GameObject exp = Instantiate(ExpEffect, transform.position, transform.rotation);
            Destroy(exp, 1.0f);
            
			gameObject.Recycle();
        }

    }

    void RaiusDamage(Vector3 Pos)
    {
        GameObject exp = Instantiate(ExpEffect, Pos, transform.rotation);
        Destroy(exp, 1.0f);
        Collider[] colliders = Physics.OverlapSphere(Pos, radius);

        foreach (Collider col in colliders)
        {
            if (col.transform.tag == "Tank" || col.transform.tag == "DestroyObject" || col.transform.tag == "Soldier" || col.transform.tag == "EnemyTank")
            {
                if (!hitTankPlayer.Contains(col.gameObject))
                {
                    if (col.transform.tag == "Tank" || col.transform.tag == "Soldier" || col.transform.tag == "EnemyTank")
                    {
                        BulletDamageManager.Instance.GetDamage((int)(damage * 0.3), col.gameObject, attacker);
                        Debug.Log(damage * 0.3);
                        hitTankPlayer.Add(col.gameObject);
                    }
                    else if (col.transform.tag == "DestroyObject")
                    {
                        col.GetComponent<DestroyObject>().hit -= 1;
                    }
                }
            }
            else
            {

            }
        }
    }
}
