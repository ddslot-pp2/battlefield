using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillyTank : Tank {

	//총알 발사좌표
	public Transform firePos_p1;
	//MuzzleFlash의 MeshRenderer 컴포넌트 연결 변수
	public MeshRenderer muzzleFlash_1;

	protected override void Init () 
	{

		base.Init();

		muzzleFlash_1.enabled = false;

		//Debug.Log ("init");
	}

	public override void EntityUpdate () 
	{

		base.EntityUpdate ();
		//Debug.Log ("EntityUpdate");
	}

	// Use this for initialization
	void Start () 
	{

	}

	void Update()
	{
		//EntityUpdate ();
	}


	public override void Fire()
	{
		if (Time.time >= nextfire)
		{
			nextfire = Time.time + state.fireRate;
			//GameObject.Find("GameManager").GetComponent<GameManager>().CoolTimeCounter(state.fireRate);
			CreateBullet();

			//잠시 기다리는 루틴을 위해 코루틴 함수로 호출
			StartCoroutine(this.ShowMuzzleFlash());
		}
	}

	void CreateBullet()
	{
		//Bullet 프리팹을 동적으로 생성
		GameObject bulletLocalSize = state.bullet.Spawn(firePos_p1.position,firePos_p1.rotation);
		//GameObject bulletLocalSize = Instantiate(state.bullet, firePos_p1.position, firePos_p1.rotation);
		bulletLocalSize.transform.localScale = new Vector3(bulletLocalSize.transform.localScale.x * state.bulletSize, bulletLocalSize.transform.localScale.y * state.bulletSize, bulletLocalSize.transform.localScale.z * state.bulletSize);
		if( state == null ) Debug.Log("state null");

		DirectBullet bullet = bulletLocalSize.GetComponent<DirectBullet> ();

		if (bullet)
		{
			bullet.GetDamageType(state.damage, 1, transform.gameObject, state.range, state.bulletSpeed);
		}

	}

	IEnumerator ShowMuzzleFlash()
	{
		//MuzzleFlash 스케일을 불규칙하게 변경
		float scale = Random.Range(2.0f, 4.0f);
		muzzleFlash_1.transform.localScale = Vector3.one * scale;

		//활성화해서 보이게 함
		muzzleFlash_1.enabled = true;

		//불규칙적인 시간 동안 Delay한 다음 MeshRenderer를 비활성화
		yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));

		//비활성화해서 보이지 않게 함
		muzzleFlash_1.enabled = false;

	}
}

