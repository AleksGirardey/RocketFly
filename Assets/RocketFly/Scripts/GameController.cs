using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameController : MonoBehaviour
{
	public static GameController Instance { get; private set; }

	public delegate void GameDelegate();
	public event GameDelegate OnTimeIsUp;
	public event GameDelegate OnAltitudeReached;

	[SerializeField] private float levelTime = 60;
	[SerializeField] private float levelAltitude = 100;
	[SerializeField] private TextMeshProUGUI timerText = null;
	[SerializeField] private TextMeshProUGUI altitudeText = null;
	[SerializeField] private Slider altitudeSlider = null;
	[SerializeField] private GameObject victoryContainer = null;
	[SerializeField] private Transform rocket = null;
	private float timer;
	private bool timeIsUp;
	private bool altitudeReached = false;

	public void ResetTime()
	{
		timer = levelTime;
		timeIsUp = false;
	}

	private void Victory()
	{
		victoryContainer.SetActive(true);
		StartCoroutine(ReloadDelay());
	}

	private void Start()
	{
		victoryContainer.SetActive(false);
		ResetTime();
		OnAltitudeReached += Victory;
	}

	private void Awake()
	{
		DOTween.SetTweensCapacity(1250, 50);
		
		if(Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}

	private void Update()
	{
		if (timeIsUp) return;

		timer -= Time.deltaTime;
		timerText.text = timer.ToString("0:00");
		altitudeText.text = rocket.position.y.ToString("0");
		altitudeSlider.value = rocket.position.y / levelAltitude;

		if (timer <= 0 && !timeIsUp)
		{
			timer = 0;
			timeIsUp = true;
			OnTimeIsUp?.Invoke();
		}

		if(rocket.position.y >= levelAltitude && !altitudeReached)
		{
			altitudeReached = true;
			OnAltitudeReached?.Invoke();
		}
	}

	private IEnumerator ReloadDelay()
	{
		yield return new WaitForSeconds(3);

		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
	}
}
