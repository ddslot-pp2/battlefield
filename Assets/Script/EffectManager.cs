using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

	public GameObject DustEffect;


	static 	EffectManager _Instance;


	public static EffectManager Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = GameObject.FindObjectOfType(typeof(EffectManager)) as EffectManager;

				if (_Instance == null)
				{
					GameObject kNewObject = GameObject.Instantiate( Resources.Load("Prefab/Global/EffectManager") ) as GameObject;
					kNewObject.name = "EffectManager";

					DontDestroyOnLoad( kNewObject );

					_Instance = kNewObject.GetComponent<EffectManager>();
				}
			}

			return _Instance;
		}
	}


}
