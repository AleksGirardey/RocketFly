using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDelay : MonoBehaviour
{
	public delegate void ActionDelegate(GameObject gameObject);
	public event ActionDelegate OnStartAction;
	[SerializeField] private float delay = 5;

	private void Start()
	{
		Invoke("DoAction", delay);
	}

	private void DoAction()
	{
		OnStartAction?.Invoke(this.gameObject);
	}
}
