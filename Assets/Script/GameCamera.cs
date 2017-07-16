using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour {


	private  Transform  targetObject;
	public Transform cameraObject;

	static GameCamera myslf;

	public float offsetX;
	public float offsetY;
	public float offsetZ;

	public float followSpeed = 0.25f;

	Vector3 cameraPosition;



	Misc_Timer shakeTimer = new Misc_Timer ();


	void Start()
	{
		myslf = this;
		//targetObject = BattleLib.Instance.
	}

	void LateUpdate()
	{
		
		if (targetObject == null)
		return;

		shakeTimer.UpdateTimer ();
		if (shakeTimer.IsActive())
			UpdateShake ();
		
		cameraPosition = new Vector3 (targetObject.position.x + offsetX, targetObject.position.y + offsetY, targetObject.position.z + offsetZ);
		transform.position = Vector3.Lerp(transform.position, cameraPosition, followSpeed * Time.deltaTime);
	}

	public void SetTarget(Transform target)
	{
		targetObject = target;
	}

	float shakeDelay=0.03f,lastShakeTime=float.MinValue;

	public void UpdateShake()
	{
		
		if (lastShakeTime + shakeDelay < Time.time) {
			Vector3 shakePosition = Vector3.zero;
			shakePosition.x += Random.Range (-0.5f, 0.5f);
			shakePosition.y += Random.Range (-0.5f, 0.5f);
			cameraObject.transform.Translate(shakePosition);
			//cameraObject.transform.localPosition = shakePosition+cameraObject.transform.localPosition;
			lastShakeTime=Time.time;

		}
	}

	public static void ToggleShake(float shakeTime){
		
		myslf.shakeTimer.StartTimer (shakeTime);
		//	myslf.shakeActive = toggleValue;
		//if (!toggleValue) {
		//	myslf.targetCamera.transform.localPosition=myslf.camLocalPos;
		//}
	}
}
