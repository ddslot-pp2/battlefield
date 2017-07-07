using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank16Rotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 1000 * Time.deltaTime, 0);
    }
}
