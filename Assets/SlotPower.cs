using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotPower : SlotItemUse, IPointerDownHandler
{
      
    public void OnPointerDown(PointerEventData data)
    {
		BulletDamageManager.Instance.UsePowerItem();
        Destroy(gameObject);
    }
}
