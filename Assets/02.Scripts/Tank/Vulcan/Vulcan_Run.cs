﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vulcan_Run : MonoBehaviour {
    //접근해야 하는 컴포넌트는 반드시 변수에 할당한 후 사용
    private Transform playertank;
    private float h, v;
    private Controller controller;
    //이동 속도 변수 (public으로 선언되어 Inspector에 노출됨)
    //회전 속도 변수
    Vulcan_State state;
    public Vector3 lookDirection;

    void Start()
    {
        controller = GameObject.Find("BackgroundJoyStick").GetComponent<Controller>();
        //스트립트 처음에 Transform 컴포넌트 할당
        playertank = GetComponent<Transform>();
        state = gameObject.GetComponent<Vulcan_State>();
    }

    void Update()
    {
        if (controller.dir.x != 0 && controller.dir.y != 0)
        {
            lookDirection = controller.dir.x * Vector3.right + controller.dir.y * Vector3.forward;
            playertank.rotation = Quaternion.LookRotation(state.direct * lookDirection);
            if (controller.dir != new Vector3(0, 0, 0))
                playertank.Translate(state.forward * state.moveSpeed * Time.deltaTime);

            Debug.Log(Time.deltaTime);
        }
    }
}
