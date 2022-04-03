using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopdownCharacterController;
using UnityEngine.UI;
using System;

public class PlayerBehaviour : CharacterBehaviour
{
	public int maxHealth;
	public Image[] hearts;
	public Sprite fadedHeart;
	public Sprite halfHeart;
	public Sprite fullHeart;
	public bool completed = false;

	private DateTime _timeStart;
	private Animator _animator;

	[System.Serializable]
	public class GameStart
	{
		public string deviceUniqueIdentifier;
		public DateTime dateTime;
		public int eventId;
	}

	[System.Serializable]
	public class GameEnd
	{
		public string deviceUniqueIdentifier;
		public DateTime timeEnded;
		public int eventId;
		public double timePlayed;//time in seconds that they played the game for
		public bool completed;
	}

	[System.Serializable]
	public class PlayerDeath
	{
		public string deviceUniqueIdentifier;
		public string enemyType;
		public string weaponUsed;
		public DateTime timeOfDeath;
		public int eventId;
	}

	public class AsteroidExternalData
    {
		public string deviceUniqueIdentifier;
		public int eventId;
		public int asteroidsDestroyed;
		public int asteroidsSpawned;
		public int asteroidsMissed;
		public int asteroidsCollisions;
    }

	public void HealToFull()
	{
		// Updates the health and the callback updates the visuals.
		Health.HP = maxHealth;
	}

	public bool IsAtFullHealth()
	{
		return Health.HP == maxHealth;
	}

	new void Start()
	{
		base.Start();
		_timeStart = DateTime.UtcNow;
		_animator = GetComponent<Animator>();

		// Disables the behaviour if the required components are null.
		if (!Movement || !RangedAttack || !Health)
			enabled = false;
		
        else
        {
			// Adds the callbacks.
			Health.HealthChangedCallbacks.Add(HealthChangedCallback);
			Health.DeathCallbacks.Add(DeathCallback);

			// Initialises the health to max.
			Health.HP = maxHealth;

			// Sets the attack properties.
			RangedAttack.AttackInfo.Add("weapon_name", "player laser");

			// Posts the game start event.
			GameStart gameStart = new GameStart { dateTime = DateTime.UtcNow, deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier, eventId = 0 };
			string jsonData = JsonUtility.ToJson(gameStart);
			StartCoroutine(AnalyticsManager.PostMethod(jsonData));
		}
	}

	void Update()
	{
		// Horizontal Input.
		if (Input.GetKey(KeyCode.LeftArrow)) Movement.MoveLeft();
		if (Input.GetKey(KeyCode.RightArrow))
        {
			Movement.MoveRight();

			if (_animator != null)
				_animator.SetFloat("RightwardVelocity", 1.0f);
		}
		else if (_animator != null)
			_animator.SetFloat("RightwardVelocity", 0.0f);

		// Vertical Input.
		if (Input.GetKey(KeyCode.UpArrow)) Movement.MoveUp();
		if (Input.GetKey(KeyCode.DownArrow)) Movement.MoveDown();

		if (Input.GetKey(KeyCode.X))
			RangedAttack.Fire(Vector2.right);

		
	}

	private void HealthChangedCallback(float newHealth, Dictionary<string, string> damageInfo)
	{
		int halfHealthIndex = -1;
		if (Mathf.Floor(newHealth) < newHealth)
		{
			halfHealthIndex = Mathf.FloorToInt(newHealth);
		}
		for (int i = 0; i < maxHealth; i++)
		{

			if (i >= newHealth)
				hearts[i].sprite = fadedHeart;
			else if (i == halfHealthIndex)
				hearts[i].sprite = halfHeart;
			else
				hearts[i].sprite = fullHeart;
		}
	}

	private void DeathCallback(Dictionary<string, string> damageInfo)
	{
		if (damageInfo != null)
		{
			if (damageInfo.ContainsKey("weapon_name") && damageInfo.ContainsKey("enemy_type"))
			{
				PlayerDeath playerDeath = new PlayerDeath
				{
					enemyType = damageInfo["enemy_type"],
					weaponUsed = damageInfo["weapon_name"],
					timeOfDeath = DateTime.UtcNow,
					eventId = 3,
					deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier
				};
				string jsonData = JsonUtility.ToJson(playerDeath);
				StartCoroutine(AnalyticsManager.PostMethod(jsonData));
			}
		}
		PostEndGameDataToServer();
	}

	public void PostEndGameDataToServer()
    {
		GameEnd gameEnd = new GameEnd
		{
			timeEnded = DateTime.UtcNow,
			deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier,
			timePlayed = (DateTime.UtcNow - _timeStart).TotalSeconds,
			eventId = 1,
			completed = completed
		};
		string jsonData = JsonUtility.ToJson(gameEnd);
		StartCoroutine(AnalyticsManager.PostMethod(jsonData));

		AsteroidExternalData asteroidData = new AsteroidExternalData
		{
			eventId = 6,
			deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier,
			asteroidsDestroyed = AsteroidData.asteroidsDestroyed,
			asteroidsMissed = AsteroidData.numberAsteroidsMissed,
			asteroidsSpawned = AsteroidData.asteroidsSpawned,
			asteroidsCollisions = AsteroidData.asteroidCollisions
		};
		string asteroidJson = JsonUtility.ToJson(asteroidData);
		StartCoroutine(AnalyticsManager.PostMethod(asteroidJson));

		if (!Application.isEditor)
			Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLScQSZOf3EBjnzuc8Aw1kmCWVHfrod-ccsLGcIxlj7hfG0kH-Q/viewform?usp=pp_url&entry.1452894741=" + SystemInfo.deviceUniqueIdentifier);
	}

	private void OnApplicationQuit()
	{
		PostEndGameDataToServer();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
			AsteroidData.asteroidCollisions += 1;
		}
    }
}