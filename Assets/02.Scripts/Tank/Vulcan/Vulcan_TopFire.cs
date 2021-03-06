﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vulcan_TopFire : MonoBehaviour {

    //총알 발사좌표
    public Transform firePos_p1;
    public Transform firePos_p2;

    //MuzzleFlash의 MeshRenderer 컴포넌트 연결 변수
    public MeshRenderer muzzleFlash_1;
    public MeshRenderer muzzleFlash_2;

    RaycastHit TFire;
    Vector3 Click;
    Quaternion dir;
    public float nextfire = 0.0f;//다음 총알 발사시간
    GameObject bulletLocalSize;
	Tank_State state;

    void Start()
    {
		state = gameObject.GetComponentInParent<Tank_State>();
        //최초에 MuzzleFlash MeshRenderer를 비활성화
        muzzleFlash_1.enabled = false;
        muzzleFlash_2.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out TFire);
            Click = TFire.point;
            Click.y = transform.position.y;
            dir = Quaternion.LookRotation((Click - transform.position).normalized);

            transform.rotation = dir;

            Fire();
        }
    }



    void Fire()
    {
        if (Time.time >= nextfire)
        {
            nextfire = Time.time + state.fireRate;
            GameObject.Find("GameManager").GetComponent<GameManager>().CoolTimeCounter(state.fireRate);
            StartCoroutine("CreateBullet");
        }
    }

    IEnumerator CreateBullet()
    {
        for(int i = 0; i < 20; i++)
        {
            if (i % 2 == 0)
            {
                bulletLocalSize = Instantiate(state.bullet, firePos_p1.position, firePos_p1.rotation);
                bulletLocalSize.transform.localScale = new Vector3(bulletLocalSize.transform.localScale.x * state.bulletSize, bulletLocalSize.transform.localScale.y * state.bulletSize, bulletLocalSize.transform.localScale.z * state.bulletSize);
                bulletLocalSize.GetComponent<BalKanBullet>().GetDamageType(state.damage, transform.parent.gameObject, state.range, state.bulletSpeed);
                StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_1));
                yield return new WaitForSeconds( 0.2f);

            }
            else
            {
                bulletLocalSize = Instantiate(state.bullet, firePos_p2.position, firePos_p2.rotation);
                bulletLocalSize.transform.localScale = new Vector3(bulletLocalSize.transform.localScale.x * state.bulletSize, bulletLocalSize.transform.localScale.y * state.bulletSize, bulletLocalSize.transform.localScale.z * state.bulletSize);
                bulletLocalSize.GetComponent<BalKanBullet>().GetDamageType(state.damage, transform.parent.gameObject, state.range, state.bulletSpeed);
                StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_2));
                yield return new WaitForSeconds(0.2f);


            }
        }
    }

    IEnumerator ShowMuzzleFlash(MeshRenderer muzzleFlash)
    {
        //MuzzleFlash 스케일을 불규칙하게 변경
        float scale = Random.Range(2.0f, 4.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        //활성화해서 보이게 함
        muzzleFlash.enabled = true;

        //불규칙적인 시간 동안 Delay한 다음 MeshRenderer를 비활성화
        yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));

        //비활성화해서 보이지 않게 함
        muzzleFlash.enabled = false;
    }
}
