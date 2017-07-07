using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBullet : MonoBehaviour {
    //총알의 파괴력
    public int damage;
    //총알 초기 위치
    public Vector3 first;
    //총알 발사 속도
    public float speed = 1f;
    public GameObject ExpEffect;
    public bool up;
    public bool down;
  
    public Vector3 pos;
    public float radius;
    public GameObject attacker;
    public bool start;

    public void GetDamageType(int damage, Vector3 pos, Vector3 first, GameObject attacker)
    {
        this.damage = damage;
        this.pos = pos;
        this.first = first;
        this.attacker = attacker;    }
}
