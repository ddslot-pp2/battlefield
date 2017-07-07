using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTypeRocketBullet : RocketBullet {
    void Start()
    {
        start = false;
        radius = 10f;
        up = true;
        down = false;
        //BulletDamageManager.Instance = GameObject.Find("BulletDamageManager").GetComponent("BulletDamageManager") as BulletDamageManager;
    }

    void Update()
    {
        if (start == true)
        {
            //Debug.Log("as");
            if (up == true)
            {
                transform.Translate(0, speed, 0);
                if (transform.position.y >= 100)
                {
                    Debug.Log("높다");
                    up = false;
                    StartCoroutine(this.ShowMuzzleFlash());
                }
            }
            else if (down == true)
            {
                transform.Translate(0, speed, 0);
                if (transform.position.y <= 0)
                {
                    Bomb();
                    RaiusDamage(transform.position);
					gameObject.Recycle();
                }
            }
        }
    }

    IEnumerator ShowMuzzleFlash()
    {
        //불규칙적인 시간 동안 Delay한 다음 MeshRenderer를 비활성화
        yield return new WaitForSeconds(2.0f);
        transform.position = new Vector3(pos.x, 100f, pos.z);
        transform.Rotate(new Vector3(180, 0, 0), Space.Self);
        down = true;
    }

    public void Bomb()
    {
        int count = Random.Range(8, 10);
        for (int i = 0; i < count; i++)
        {
            GameObject exp = Instantiate(ExpEffect, new Vector3(transform.position.x + Random.Range(-10f, 10f), 2, transform.position.z + Random.Range(-10f, 10f)), transform.rotation);
            Destroy(exp, 1.0f);
        }

    }

    void RaiusDamage(Vector3 Pos)
    {
        Collider[] colliders = Physics.OverlapSphere(Pos, radius);
        foreach (Collider col in colliders)
        {
            if (col.transform.tag == "Tank" || col.transform.tag == "DestroyObject" || col.transform.tag == "Soldier" || col.transform.tag == "EnemyTank")
            {
                if (col.transform.tag == "Tank" || col.transform.tag == "EnemyTank" || col.transform.tag == "Soldier")
                {
                    BulletDamageManager.Instance.GetDamage(damage, col.gameObject, attacker);
                }
                else if (col.transform.tag == "DestroyObject")
                {
                    col.GetComponent<DestroyObject>().hit -= 1;
                }

            }
        }
    }
}
