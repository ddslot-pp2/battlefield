using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item4 : MonoBehaviour {
    public GameObject[] bullet4;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Tank")
        {
            Tank_State Ts = other.GetComponent<Tank_State>();
            Debug.Log(other.transform.name);
            if (Ts.bulletAttribute == 4)
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
                Ts.bulletAttribute = 4;
                Ts.bullet = bullet4[Ts.tankType - 1];
                Ts.bulletSize = 1.0f;
                Ts.damage = Ts.firstDamage;
            }
            Destroy(gameObject);

        }


    }
}
