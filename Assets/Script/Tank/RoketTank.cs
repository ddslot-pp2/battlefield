using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoketTank : Tank {

	//총알 발사좌표
	public Transform firePos_p1;
	//총알 발사좌표
	public Transform firePos_p2;

	public GameObject BombRangeEffect;
	public GameObject[] haveBullet;

	protected override void Init () {

		base.Init();

		Debug.Log ("init");

		haveBullet = new GameObject[2];
		haveBullet[0] = Instantiate(state.bullet, firePos_p1.position, firePos_p1.rotation);
		haveBullet[0].transform.parent = gameObject.transform;
		haveBullet[1] = Instantiate(state.bullet, firePos_p2.position, firePos_p2.rotation);
		haveBullet[1].transform.parent = gameObject.transform;
	}

	public override void EntityUpdate () {

		base.EntityUpdate ();
		//Debug.Log ("EntityUpdate");
	}

	// Use this for initialization
	void Start () {

	}

	void Update()
	{
		//EntityUpdate ();
	}

	public override void Fire()
	{
		/*
		int l = TFire.transform.gameObject.layer;

		if (l == 8 && (Vector3.Distance(transform.position, TFire.point) <= state.range))
		{
			GameObject bulletLocalSize = Instantiate(BombRangeEffect, TFire.point, gameObject.transform.rotation);
			bulletLocalSize.transform.position = new Vector3(bulletLocalSize.transform.position.x, bulletLocalSize.transform.position.y + 1, bulletLocalSize.transform.position.z);
			StartCoroutine(this.DestroyTargetRange(bulletLocalSize));

			GameObject.Find("GameManager").GetComponent<GameManager>().CoolTimeCounter(state.fireRate);
			StartCoroutine("CreateBullet");
		}
		*/

		if (Time.time >= nextfire)
		{
			nextfire = Time.time + state.fireRate;


			//잠시 기다리는 루틴을 위해 코루틴 함수로 호출
			//StartCoroutine(this.ShowMuzzleFlash());
		}



	}

	/*
	IEnumerator CreateBullet()
	{
		if (haveBullet[0] != null || haveBullet[0].GetComponent<RocketBullet>().start == false)
		{
			haveBullet[0].transform.parent = null;
			haveBullet[0].GetComponent<RocketBullet>().GetDamageType(state.damage, TFire.point, transform.position, transform.gameObject);
			haveBullet[0].GetComponent<RocketBullet>().start = true;

			yield return new WaitForSeconds(nextfire);
			haveBullet[0] = Instantiate(state.bullet, firePos_p1.position, firePos_p1.rotation);
			haveBullet[0].transform.parent = gameObject.transform;
		}
		else if (haveBullet[1] != null || haveBullet[1].GetComponent<RocketBullet>().start == false)
		{
			haveBullet[1].transform.parent = null;
			haveBullet[1].GetComponent<RocketBullet>().GetDamageType(state.damage, TFire.point, transform.position, transform.gameObject);
			haveBullet[1].GetComponent<RocketBullet>().start = true;

			yield return new WaitForSeconds(nextfire);
			haveBullet[1] = Instantiate(state.bullet, firePos_p2.position, firePos_p2.rotation);
			haveBullet[1].transform.parent = gameObject.transform;
		}
	}
	*/

	IEnumerator DestroyTargetRange(GameObject obj)
	{
		yield return new WaitForSeconds(3.0f);
		Destroy(obj);
	}
}
