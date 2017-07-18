using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTime : MonoBehaviour {

	public float EffectDelay;

	float startTime = 0.0f;

	void OnEnable()
	{
		//Debug.Log ("OnEnable");
		startTime = Time.time;
		StartCoroutine(this.EffectRecycle());
	}
		
	IEnumerator EffectRecycle()
	{
		yield return new WaitForSeconds(EffectDelay);
		gameObject.Recycle();
	}
}
