using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour {
    private GameManager gameManager;
	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 30 *Time.deltaTime, 0);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Tank")
        {
            gameManager.addExperience(10);
            Destroy(gameObject);
        }
    }
}
