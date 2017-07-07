using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBox : DestroyObject {
    private float radius;
    public GameObject goldScrew;
    GameObject makeScrew;
    private GameObject objectMng;

    // Use this for initialization
    void Start()
    {
        hit = 3;
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
                    BulletDamageManager.Instance.GetFireEffect(colliders[i].gameObject, 3);
                }
            }


            int Item = Random.Range(1, 100);
            if (Item <= 2)
            {

            }
            else
            {
                int RandomItemCount = Random.Range(10, 15);

                Debug.Log(RandomItemCount);
                for (int i = 0; i < RandomItemCount; i++)
                {
                    makeScrew = Instantiate(goldScrew, transform.position, Quaternion.Euler(0, 0, -45));
                    makeScrew.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), ForceMode.Impulse);
                }
            }
            StartCoroutine("Delay");
			ObjectManager.Instance.CreateObject(transform.position, transform.rotation, 2);

            gameObject.SetActive(false);
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(18.0f);
        gameObject.SetActive(true);
        hit = 3;
    }
}
