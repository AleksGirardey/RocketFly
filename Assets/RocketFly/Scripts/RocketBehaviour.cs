using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketBehaviour : MonoBehaviour
{
	[SerializeField] private Rigidbody rb = null;
	[SerializeField] private Transform spawnPoint = null;
	[SerializeField] private Transform trailPos = null;
	[SerializeField] private GameObject rocketCloud = null;
	[SerializeField] private GameObject explosion = null;
	[SerializeField] private GameObject trail = null;
	[SerializeField] private Color trailErrorColor = Color.white;
	[SerializeField] private ParticleSystem vfx = null;
	[SerializeField] private MeshRenderer mesh = null;
	[SerializeField] private float thrusterPower = 100;
	[SerializeField] private float sideThrusterPower = 100;
	[SerializeField] private float maxSpeed = 10;
	[SerializeField] private float boostPower = 1;
	[SerializeField] private float boostRate = 5;
	[SerializeField] private Image boostImage;

	private Transform currentTrail;
	private Quaternion initRot;
	private bool activePower;
	private float direction;
	private bool isDestroyed = false;
	private bool detectionActive = false;
	private float boostTime;
	private bool hasLeftSpawn;


	private void Start()
	{
		GameController.Instance.OnTimeIsUp += DestroyRocket;

		initRot = transform.rotation;
		StartRocket();
	}

	private void StartRocket()
	{
		isDestroyed = false;
		detectionActive = false;
		hasLeftSpawn = false;
		GameObject instance = Instantiate(trail, trailPos.position, Quaternion.identity);
		currentTrail = instance.transform;
		StartCoroutine(StartDetectionDelay());
	}
	private IEnumerator StartDetectionDelay()
	{
		yield return new WaitForSeconds(1);
		detectionActive = true;
	}

	private void FixedUpdate()
	{
		if(activePower && rb.velocity.z < maxSpeed)
		{
			rb.AddRelativeForce(Vector3.up * thrusterPower, ForceMode.VelocityChange);
		}

		rb.AddRelativeTorque(transform.forward * direction * sideThrusterPower, ForceMode.VelocityChange);
	}

	private void Update()
	{
		if(currentTrail != null)
		{
			currentTrail.position = trailPos.position;
		}

		if (isDestroyed) return;

		activePower = Input.GetAxis("Vertical") >= 1;

		direction = -Input.GetAxis("Horizontal");

		if(boostTime > 0) boostTime -= Time.deltaTime;

		boostImage.fillAmount = Mathf.Abs((boostTime / boostRate) - 1);

		if(Input.GetKeyDown(KeyCode.Space) && boostTime <= 0)
		{
			boostTime = boostRate;
			rb.AddRelativeForce(Vector3.up * boostPower, ForceMode.Impulse);
		}

		if(activePower || direction != 0)
		{
			if(!vfx.isPlaying) vfx.Play();
		}
		else
		{
			vfx.Stop();
		}	
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Obstacle") && !isDestroyed)
		{
			DestroyRocket();
		} else if (other.CompareTag("SpawnPoint")) {
			hasLeftSpawn = false;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("SpawnPoint")) {
			Debug.Log($"COUCOU {hasLeftSpawn}");
			hasLeftSpawn = true;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!detectionActive) return;

		if(!isDestroyed)
		{
			DestroyRocket();
		}
	}

	private void DestroyRocket()
	{
		isDestroyed = true;
		StartCoroutine(DestructionDelay());
	}

	private void DestroyCloud(GameObject obj)
	{
		Destroy(obj);
	}

	private IEnumerator DestructionDelay()
	{
		GameObject newTrail = Instantiate(trail, trailPos.position, Quaternion.identity);
		currentTrail = newTrail.transform;
		newTrail.GetComponent<TrailRenderer>().startColor = trailErrorColor;
		newTrail.GetComponent<TrailRenderer>().endColor = trailErrorColor;

		yield return new WaitForSeconds(1);

		if (hasLeftSpawn) {
			GameObject newCloud = Instantiate(rocketCloud, transform.position, Quaternion.identity);
			newCloud.GetComponent<ActionDelay>().OnStartAction += DestroyCloud;
		}
		Instantiate(explosion, transform.position, Quaternion.identity);
		currentTrail = null;
		vfx.Stop();
		rb.velocity = Vector3.zero;
		rb.isKinematic = true;		
		mesh.enabled = false;

		yield return new WaitForSeconds(1.5f);

		rb.isKinematic = false;
		mesh.enabled = true;
		transform.position = spawnPoint.position;
		transform.rotation = initRot;
		StartRocket();
		GameController.Instance.ResetTime();
	}
}
