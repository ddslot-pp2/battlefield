using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInput : MonoBehaviour {


	protected RaycastHit TFire;
	Vector3 Click;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown(0))
		{
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out TFire);
		
			Click = TFire.point;
			Click.y = transform.position.y;
		}
	}


}
