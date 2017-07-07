using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotHp : MonoBehaviour {
    public GameLoad gameLoad;

    // Use this for initialization
    void Start () {
        gameLoad = GameObject.Find("Main Camera").GetComponent<GameLoad>();
        gameLoad.userTank.GetComponent<Tank_State>().maxHp *= 2;
        gameLoad.userTank.GetComponent<Tank_State>().hp *= 2;
    }
}