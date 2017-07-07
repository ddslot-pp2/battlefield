
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Entity {


	public Vector3 lookDirection;

	Transform playertank1;
	float h, v;
	Controller controller;
	protected Tank_State state;

	protected RaycastHit TFire;
	Vector3 Click;

	public float nextfire = 0.0f;


	protected override void Init () {

		base.Init();

		//controller = GameObject.Find("BackgroundJoyStick").GetComponent<Controller>();
		//스트립트 처음에 Transform 컴포넌트 할당
		playertank1 = GetComponent<Transform>();

		state = gameObject.GetComponent<Tank_State>();
	}

	public override void EntityUpdate () {

		CheckFire();

		/*
		if (controller.dir.x != 0 && controller.dir.y != 0)
		{
			lookDirection = controller.dir.x * Vector3.right + controller.dir.y * Vector3.forward;
			playertank1.rotation = Quaternion.LookRotation(state.direct * lookDirection);
			if (controller.dir != new Vector3 (0, 0, 0)) 
			{
				playertank1.Translate (Vector3.forward * state.moveSpeed * Time.deltaTime);
			}
		}
		*/
	}
		
	virtual public void Fire()
	{

	}

	void CheckFire()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out TFire);
			//Debug.Log ("Input.mousePosition: " + Input.mousePosition);

			//Debug.Log ("Input.mousePosition.x:" + Input.mousePosition.x);
			//Debug.Log ("Input.mousePosition.y:" + Input.mousePosition.y);

			// drag 영역 제외
			if (Input.mousePosition.x >= 0 && Input.mousePosition.x < 220 && Input.mousePosition.y >= 0 && Input.mousePosition.y <= 220)
			{
				return;
			}

			Click = TFire.point;
			Click.y = transform.position.y;
			transform.rotation = Quaternion.LookRotation((Click - transform.position).normalized);

			Fire();
		}
	}
}
