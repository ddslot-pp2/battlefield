using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotPosition : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private Vector3 first;
    private Vector3 pos;
    private Vector3 dir;

    public float xPos1 = 770;
    public float xPos2 = -730;
    private float distance;

    // Use this for initialization
    void Start () {

	}

    public void OnDrag(PointerEventData ped)
    {
        distance = first.x - ped.position.x;
        if ((gameObject.transform.localPosition.x + -distance ) <= xPos1 && (transform.localPosition.x + -distance) >= xPos2)
        {
            transform.localPosition = new Vector3(transform.localPosition.x + -distance, transform.localPosition.y, transform.localPosition.z);
        }

        first = ped.position;
    }

    public void OnPointerDown(PointerEventData ped)
    {
        first = ped.position;
    }
}
