using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBarrel : DestroyObject {
    private float radius;
   
    public GameObject goldScrew;
    private GameObject makeScrew;
    // Use this for initialization
    void Start()
    {
        hit = 2;
        radius = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit <= 0)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Tank")
                {
                    BulletDamageManager.Instance.GetDamage(3, colliders[i].gameObject);
                }
            }


            for (int i = 0; i < 8; i++)
            {
                makeScrew = Instantiate(goldScrew, transform.position, Quaternion.Euler(0, 0, -45));
                makeScrew.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), ForceMode.Impulse);
            }
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "Bullet")
        {
            hit--;
        }
    }
}
