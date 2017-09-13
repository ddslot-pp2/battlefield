using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PakerTank : Tank {

	public Transform firePos_p1;

	public MeshRenderer muzzleFlash_1;

	public Transform firePos_p2;

	public MeshRenderer muzzleFlash_2;


	protected override void Init () {

		base.Init();

        muzzleFlash_1.enabled = false;
        muzzleFlash_2.enabled = false;
        Debug.Log ("init");
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
        base.Update();
        //EntityUpdate ();
    }


	public override void Fire()
	{
        base.Fire();
        StartCoroutine(this.ShowMuzzleFlash());
    }

	/*
	void CreateBullet()
	{
        //Bullet 프리팹을 동적으로 생성
        GameObject bulletLocalSize = Instantiate(state.bullet, firePos_p1.position, firePos_p1.rotation);
        bulletLocalSize.transform.localScale = new Vector3(bulletLocalSize.transform.localScale.x * state.bulletSize, bulletLocalSize.transform.localScale.y * state.bulletSize, bulletLocalSize.transform.localScale.z * state.bulletSize);
        bulletLocalSize.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.gameObject, state.range, state.bulletSpeed);
	}
	*/

	IEnumerator ShowMuzzleFlash()
	{
        
		//MuzzleFlash 스케일을 불규칙하게 변경
		float scale = Random.Range(2.0f, 4.0f);
		muzzleFlash_1.transform.localScale = Vector3.one * scale;
		muzzleFlash_2.transform.localScale = Vector3.one * scale;

		//활성화해서 보이게 함
		muzzleFlash_1.enabled = true;
		muzzleFlash_2.enabled = true;
		//불규칙적인 시간 동안 Delay한 다음 MeshRenderer를 비활성화
		yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));

		//비활성화해서 보이지 않게 함
		muzzleFlash_1.enabled = false;
		muzzleFlash_2.enabled = false;
    }

    public override Transform GetFirePosition()
    {
        return firePos_p1;
    }

    public override Vector3[] GetFireDirs(Vector3 NormalizedDir)
    {
        //var dir = (new Vector3(TouchDir.x, 0.0f, TouchDir.z) - transform.position).normalized;
        //return new Vector3[] { NormalizedDir };

        
		return new Vector3[] { 
			Quaternion.AngleAxis (5.0f, Vector3.up) * NormalizedDir
			,Quaternion.AngleAxis (-5.0f, Vector3.up) * NormalizedDir

		};
        
    }
}
