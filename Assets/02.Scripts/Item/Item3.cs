using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item3 : MonoBehaviour {

    public GameObject[] bullet3;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Tank")
        {
            Tank_State Ts = other.GetComponent<Tank_State>();
            Debug.Log(other.transform.name);
            if (Ts.bulletAttribute == 3)
            {
                if (Ts.bulletSize >= 1.8)
                {

                }

                else
                {
                    Ts.bulletSize += 0.2f;
                    Ts.damage += 1;
                    Debug.Log(Ts.damage);
                    Destroy(gameObject);
                }
            }
            else
            {
                Ts.bulletAttribute = 3;
                Ts.bullet = bullet3[Ts.tankType - 1];
                Ts.bulletSize = 1.0f;
                Ts.damage = Ts.firstDamage;
            }
            Destroy(gameObject);
        }
    }
}
