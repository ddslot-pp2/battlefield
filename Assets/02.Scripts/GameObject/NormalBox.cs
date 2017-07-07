using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NormalBox : DestroyObject {
    //private GameObject objectMng;
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
            int Item = Random.Range(1, 100);
            if(Item <= 2)
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

			ObjectManager.Instance.CreateObject(transform.position, transform.rotation, 1);

			gameObject.Recycle();

           // Destroy(gameObject);
        }
    }
}
