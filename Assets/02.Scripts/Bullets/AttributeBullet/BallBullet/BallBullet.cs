using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBullet : MonoBehaviour {
    //총알의 파괴력
    public int damage;
    public Vector3 first = new Vector3(1, 1, 1);
    //총알 발사 속도
    public float speed;
    //폭발 반경 범위
    public float radius;
    public GameObject ExpEffect;
   
    public int BoombCount;
    public float distance;
    public List<GameObject> hitTankPlayer = new List<GameObject>();
    public GameObject attacker;

    public void GetDamageType(int damage, int BoombCount, GameObject attacker, float distance, float speed)
    {
        this.damage = damage;
        this.BoombCount = BoombCount;
        this.attacker = attacker;
        this.distance = distance;
        this.speed = speed;
    }
}
