using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustLaserBullet : LaserBullet {

    void Update()
    {
        if (attack == true)
        {
            if (Time.time <= delayFire)
            {
                Physics.Raycast(firstPos.transform.position, transform.forward, out hitPoint);

                lineDraw.SetPosition(0, firstPos.transform.position);
                lineDraw.SetPosition(1, new Vector3(hitPoint.point.x, firstPos.transform.position.y, hitPoint.point.z));

            }
            else
            {
				gameObject.Recycle();
            }
        }
    }

    void DamegeCheck()
    {
        try
        {
            if (hitPoint.transform.tag == "Tank" || hitPoint.transform.tag == "EnemyTank" || hitPoint.transform.tag == "Soldier")
            {
                BulletDamageManager.Instance.GetDamage(damage, hitPoint.transform.gameObject, attacker);
                BulletDamageManager.Instance.GetDustEffect(hitPoint.transform.gameObject);
            }
            else if (hitPoint.transform.tag == "DestroyObject")
            {
                Debug.Log("aa");
                hitPoint.transform.GetComponent<DestroyObject>().hit -= 1;
            }
            else
            {
            }
        }
        catch (NullReferenceException)
        {

        }
    }
}
