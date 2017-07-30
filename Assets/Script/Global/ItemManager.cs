using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

	public GameObject[] InGameItem;
	public GameObject ItemGainEffect;
	/*
	public GameObject hpItem;
	public GameObject ExpItem;
	public GameObject ShieldItem;
	*/

	static 	ItemManager _Instance;

	public static ItemManager Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = GameObject.FindObjectOfType(typeof(ItemManager)) as ItemManager;

				if (_Instance == null)
				{
					GameObject kNewObject = GameObject.Instantiate( Resources.Load("Prefab/Global/ItemManager") ) as GameObject;
					kNewObject.name = "ItemManager";

					DontDestroyOnLoad( kNewObject );

					_Instance = kNewObject.GetComponent<ItemManager>();
				}
			}

			return _Instance;
		}
	}

	public GameObject CreateItem( int itemType, float posX, float posY, float posZ)
	{
		Vector3 respawnPos = new Vector3 (posX, posY, posZ);
		if (InGameItem [itemType] == null) 
		{
			return null;
		}

		return InGameItem [itemType].Spawn(respawnPos);
	}


	public void ItemGain( Vector3 spawnPos)
	{
		ItemGainEffect.Spawn(spawnPos);
	}
		
}
