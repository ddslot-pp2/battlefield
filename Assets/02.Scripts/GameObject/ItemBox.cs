using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : DestroyObject {
    public GameObject goldScrew;
    GameObject makeScrew;
    
    // Use this for initialization
    void Start()
    {

        hit = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit <= 0)
        {
            int RandomItemCount = Random.Range(3, 5);

            Debug.Log(RandomItemCount);
            for (int i = 0; i < RandomItemCount; i++)
            {
                makeScrew = Instantiate(goldScrew, transform.position, Quaternion.Euler(0, 0, -45));
                makeScrew.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), ForceMode.Impulse);
            }

            int Item = Random.Range(1, 3);
            if (Item == 1)
            {

            }
            else if(Item == 2)
            {
              
            }else if(Item == 3)
            {

            }
			ObjectManager.Instance.CreateObject(transform.position, transform.rotation, 3);

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
