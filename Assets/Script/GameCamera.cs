using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour {


	private  Transform  targetObject;

	public float offsetX;
	public float offsetY;
	public float offsetZ;

	public float followSpeed = 0.25f;

	Vector3 cameraPosition;

	void Start()
	{
		//targetObject = BattleLib.Instance.
	}

	void LateUpdate()
	{
		if (targetObject == null)
		return;

		cameraPosition = new Vector3 (targetObject.position.x + offsetX, targetObject.position.y + offsetY, targetObject.position.z + offsetZ);
		transform.position = Vector3.Lerp(transform.position, cameraPosition, followSpeed * Time.deltaTime);
	}
}
