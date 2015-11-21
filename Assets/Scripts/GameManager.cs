using UnityEngine;
using System.Collections;

public class GameManager : SingletonScript<GameManager> {

	public enum GameStates {Intro,MainMenu,Pause,InGame,GameOver,EndLevel,EndGame,Other}
	[SerializeField] private GameStates currentState;
	private GameStates _lastState;

	private int currentLevelNumber;
	private string currentLevelName;
	[SerializeField] private int numberOfLevels;

	public float healthPoint;
	public float timeSpan;

	private bool alreadyInScene;

	[SerializeField] private GameObject _prefabsPlayer1;
	[SerializeField] private GameObject _prefabsPlayer2;
	[SerializeField] private GameObject _prefabsCredit;
	private GameObject _credit;
	
	#region Properties
	
	public GameStates CurrentState
	{
		get { return currentState; }
	}
	
	public GameObject PrefabsPlayer1
	{
		get { return _prefabsPlayer1; }
	}
	
	public GameObject PrefabsPlayer2
	{
		get { return _prefabsPlayer2; }
	}
	
	#endregion

	// Use this for initialization
	void Start ()
	{
		healthPoint = timeSpan;

		currentLevelNumber = Application.loadedLevel;
		currentLevelName = Application.loadedLevelName;

		CheckGameState();

	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Joystick2Button7))
		{
			Pause();
		}
	}

	public void ShowCredit(bool state)
	{
		if (_credit == null && !state) return;
		
		if (_credit == null && state)
		{
			_credit = Instantiate(_prefabsCredit);
			_credit.SetActive(state);
		}
		else if (!state)
		{
			Destroy(_credit);
		}
		
	}
	
	public void Pause()
	{
		if (currentState == GameStates.MainMenu || currentState == GameStates.InGame)
		{
			_lastState = currentState;
			currentState = GameStates.Pause;
		}
		else if (currentState == GameStates.Pause)
		{
			currentState = _lastState;
			_lastState = currentState;
		}
	}

	public void CheckNextLevel()
	{
		if (currentLevelNumber < numberOfLevels && currentState != GameStates.EndLevel)
		{
			currentState = GameStates.EndLevel;
			currentLevelNumber ++;

			if(currentLevelNumber >= 6)
				currentLevelNumber = 0;

			if(currentLevelNumber != numberOfLevels)
			{
				Debug.Log(currentLevelNumber);
				Application.LoadLevel(currentLevelNumber);
			}
			else
				Application.LoadLevel("EndGameTest");

			CheckGameState();
		}
	}

	public void GameOver()
	{
		if(currentState != GameStates.GameOver)
		{
			currentState = GameStates.GameOver;
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	void OnLevelWasLoaded()
	{
		if (currentState == GameStates.EndLevel)
		{
			if(currentLevelName != Application.loadedLevelName)
			{
				currentLevelName = Application.loadedLevelName;
				currentState = GameStates.InGame;
				healthPoint = timeSpan;
			}
		}

		if(currentState == GameStates.GameOver)
		{
			currentState = GameStates.InGame;
			healthPoint = timeSpan;
		}
	}

	void CheckGameState()
	{
		if (currentLevelNumber == 0)
			currentState = GameStates.Intro;
		else if (currentLevelNumber == 1)
			currentState = GameStates.MainMenu;
		else if (currentLevelNumber == 5)
			currentState = GameStates.EndGame;
		else
			currentState = GameStates.InGame;

		SoundManagerEvent.Emit(currentLevelNumber);
	}
}
