using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : DestroyObject {
    // Use this for initialization
    void Start()
    {
        hit = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit <= 0)
        {
			ObjectManager.Instance.CreateObject(transform.position, transform.rotation, 6);
            //objectMng.GetComponent<ObjectManager>().CreateObject(transform.position, transform.rotation, 6);

            Destroy(gameObject);
        }
    }
}
