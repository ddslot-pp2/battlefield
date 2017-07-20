using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour, IDisposable {

	public Int64 ObjId;

	protected bool m_my = false;

	void Awake ()
	{
		Init();
	}
		
	public bool IsMyEntity()
	{
		return m_my;
	}

	virtual public void Release()
	{
		
	}

	public void Dispose()
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
