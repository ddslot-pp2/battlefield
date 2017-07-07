using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour, IDisposable {

	public int m_index;

	bool m_my = false;

	void Awake ()
	{
		Init();
	}

	public void ProgressInput()
	{
	}

	public void SetMyEntity()
	{
		m_my = true;
	}

	public void Dispose()
	{
		
	}
	// Update is called once per frame
	void Update () 
	{
		//EntityUpdate();
	}


	virtual public void EntityUpdate()
	{

	}

	virtual protected void Init()
	{
		
	}
}
