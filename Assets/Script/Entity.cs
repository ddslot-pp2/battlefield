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


	public void SetMyEntity()
	{
		Debug.Log ("SetMyEntity:" + m_index);
		m_my = true;
	}

	public bool IsMyEntity()
	{
		return m_my;
	}

	public void Dispose()
	{
		
	}
	// Update is called once per frame
	void Update () 
	{
		//EntityUpdate();
	}


	public virtual void ProgressInput(float posX, float posZ, bool attack)
	{

	}

	public virtual void ProgressPos(float posX, float posZ)
	{

	}
		
	virtual public void EntityUpdate()
	{

	}

	virtual protected void Init()
	{
		
	}

	public virtual Vector3 GetEntityPos()
	{
		return transform.position;
	}
}
