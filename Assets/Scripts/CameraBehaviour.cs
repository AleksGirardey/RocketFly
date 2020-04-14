using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
	[SerializeField] private Transform target = null;
	[SerializeField] private float speed = 5;
	[SerializeField] private Vector3 offset = Vector3.zero;

	private void FixedUpdate()
	{
		transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
	}
}
