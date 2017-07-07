using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour {
    private float rotSpreed;
	// Use this for initialization
	void Start () {
        rotSpreed = -20;
	}

	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate(new Vector3(0, 0, rotSpreed * Time.deltaTime));

	}
}
