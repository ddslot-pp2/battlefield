using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_State : Tank_State {
    void Awake()
    {
        level = 1;
        exp = 0;
        moveSpeed = 10.0f;
        maxHp = 100;
        hp = 100;
        tankType = 4;
        tankDefensive = 1;
        bulletAttribute = 1;
        bulletSize = 1.0f;
        damage = 10;
        firstDamage = 10;
        fireRate = 0.5f;
        direct = 1f;
        forward = Vector3.forward;
        attAply = 0;
        range = 10.0f;

        soldier = new GameObject[4];

    }
  
}
