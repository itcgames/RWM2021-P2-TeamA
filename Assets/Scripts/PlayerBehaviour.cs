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
	public Sprite fullHeart;

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

		// Disables the behaviour if the required components are null.
		if (!Movement || !RangedAttack || !Health)
			enabled = false;

		// Adds the callbacks.
		Health.HealthChangedCallbacks.Add(HealthChangedCallback);
		Health.DeathCallbacks.Add(DeathCallback);

		// Initialises the health to max.
		Health.HP = maxHealth;

		// Sets the attack properties.
		RangedAttack.AttackInfo.Add("weapon_name", "sword projectile");

		// Posts the game start event.
		GameStart gameStart = new GameStart { dateTime = DateTime.UtcNow, deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier, eventId = 0 };
		string jsonData = JsonUtility.ToJson(gameStart);
		StartCoroutine(AnalyticsManager.PostMethod(jsonData));
	}

	void Update()
	{
		// Horizontal Input.
		if (Input.GetKey(KeyCode.LeftArrow)) Movement.MoveLeft();
		if (Input.GetKey(KeyCode.RightArrow)) Movement.MoveRight();

		// Vertical Input.
		if (Input.GetKey(KeyCode.UpArrow)) Movement.MoveUp();
		if (Input.GetKey(KeyCode.DownArrow)) Movement.MoveDown();

		if (!Movement.DiagonalMovement)
		{
			// Checks for which direction was pressed last.
			if (Input.GetKeyDown(KeyCode.LeftArrow)
					|| Input.GetKeyDown(KeyCode.RightArrow))
				Movement.PreferHorizontal = true;

			else if (Input.GetKeyDown(KeyCode.UpArrow)
					|| Input.GetKeyDown(KeyCode.DownArrow))
				Movement.PreferHorizontal = false;
		}

		if (Input.GetKeyDown(KeyCode.X))
			RangedAttack.Fire(Movement.Direction);
	}

	private void HealthChangedCallback(float newHealth, Dictionary<string, string> damageInfo)
	{
		for (int i = 0; i < maxHealth; i++)
		{
			if (i >= (int)newHealth)
				hearts[i].sprite = fadedHeart;
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
	}

	private void OnApplicationQuit()
	{
		GameEnd gameEnd = new GameEnd { timeEnded = DateTime.UtcNow, deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier };
		string jsonData = JsonUtility.ToJson(gameEnd);
		StartCoroutine(AnalyticsManager.PostMethod(jsonData));
	}
}