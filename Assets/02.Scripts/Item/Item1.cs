using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item1 : MonoBehaviour {
    public GameObject[] bullet1;

    void start()
    {

    }

    void OnTriggerEnter(Collider other)
    {        //충돌한 게임오브젝트의 태그값 비교
        if (other.transform.tag == "Tank")
        {
            Tank_State Ts = other.GetComponent<Tank_State>();
            Debug.Log(other.transform.name);
            if(Ts.bulletAttribute == 1)
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
            }else
            {
                Ts.bulletAttribute = 1;
                Ts.bullet = bullet1[Ts.tankType - 1];
                Ts.bulletSize = 1.0f;
                Ts.damage = Ts.firstDamage;
            }
            Destroy(gameObject);

        }
       

    }
}
