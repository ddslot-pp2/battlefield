using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRotate : MonoBehaviour {

	float y = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		y = y + 0.5f;
		transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, y, transform.rotation.eulerAngles.z));
	}
}
