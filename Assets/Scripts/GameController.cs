using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	// minigame countdown
	public Text timeText;

	// properties of countdown that triggers just before game start
	// (5, 4, 3...)
	public Text startCountdownText;
	private int startCountdown = 5;

	// minigame control properties
	public int numberOfMinigames;
	private int currentMinigame;

	// player control properties
	private int score;
	private int lifeCount;
	public GameObject[] lifeSprites;
	public Text scoreText;

	// splash screens that are shown at every minigame change
	public GameObject splashLose;
	public GameObject splashWon;
	public Text splashWonText;
	public Text splashLoseText;
	private string wonTextTemplate;
	private string loseTextTemplate;

	// game over splash
	public GameObject splashGameOver;

	// flag that indicates we should change the minigame
	// see Update() for an explanation on whhy this exists
	private bool canUnloadMinigame;

	public Camera mainCamera;

	// Use this for initialization
	void Start () {
		score = 0;
		canUnloadMinigame = false;
		wonTextTemplate = splashWonText.text;
		loseTextTemplate = splashLoseText.text;
	}
	
	// Update is called once per frame
	void Update () {

		// inneficient way to check if we need to change minigame
		// TODO: find a better way
		// we can't change minigame immediatly when the win and lose methods are called 
		// because they are called from within a minigame that would be destroyed
		// and the called methods would have nowhere to return to, causing a BIG TIME CRASH
		if (canUnloadMinigame) {
			unloadMinigame ();
			canUnloadMinigame = false;
		}
	}

	public void startGame() {

		currentMinigame = 1;
		lifeCount = 4;
		changeMinigame ();
		updateScoreText ();
	}

	// methods for controling minigame countdown

	public void setTime(int currentTime)
	{
		int clockTime = 60 - currentTime;
		timeText.text = "23:" + clockTime.ToString ();
		if (currentTime == 0) {
			timeText.text = "00:00";
		}
	}

	// methods for minigame control

	public void wonMinigame(int minigameScore)
	{
		if (score >= 99999) {
			score = 0;
		}
		score += minigameScore;
		StartCoroutine (showSplashWon (minigameScore));
	}

	public void lostMinigame()
	{
		lifeCount--;
		bool keepPlaying = updateLifeCount ();
		if (keepPlaying) {
			StartCoroutine (showSplashLose ());
		}
	}

	void changeMinigame()
	{
		// randomly select next and load it
		currentMinigame = Random.Range(2, numberOfMinigames);
		Application.LoadLevelAdditive(currentMinigame);
	}

	// unload current minigame
	void unloadMinigame()
	{
		Application.UnloadLevel(currentMinigame);
	}

	// methods to update player control variables (life and score)

	// returns false if game over, true if the user can keep playing
	bool updateLifeCount ()
	{
		int totalLives = lifeSprites.GetLength(0);

		if (lifeCount == 0) {
			endGame ();
			return false;
		}

		// simple algorithm to turn on 0:lifeCount hearts
		// and then turn off lifeCount:total hearts
		for (int i = 0; i < lifeCount; i++) {
			lifeSprites [i].gameObject.SetActive (true);
		}
		for (int i = lifeCount; i < totalLives; i++) {
			lifeSprites [i].gameObject.SetActive (false);
		}

		return true;
	}
		
	void updateScoreText()
	{
		// "D5" keeps the score with at least 5 digits (with trailing zeroes)
		scoreText.text = score.ToString ("D5");
	}

	void endGame()
	{
		CancelInvoke ("TimeCountdown");
		StartCoroutine(showSplashGameOver ());
		mainCamera.GetComponent<MainCameraController> ().focusOutOfGame ();
	}
		
	// control starting countdown (5, 4, 3...) at game start

	void StartCountdown()
	{
		startCountdown--;
		startCountdownText.text = startCountdown.ToString ();
		if (startCountdown == 0) {
			CancelInvoke ("StartCountdown");
			startCountdownText.gameObject.SetActive (false);
			startCountdown = 5;
			startCountdownText.text = startCountdown.ToString ();
		}
	}

	public void StartStartCountdown()
	{
		startCountdownText.gameObject.SetActive (true);
		InvokeRepeating ("StartCountdown", 1.0f, 1.0f);
	}

	// splashes for winning and losing control

	void controlSplashLose(bool show)
	{
		if (show) {
			splashLoseText.text = System.String.Format (loseTextTemplate, lifeCount);
			splashLose.SetActive (true);
		} else {
			splashLose.SetActive (false);
		}
	}

	void controlSplashWon(bool show, int scoreWon)
	{
		if (show) {
			splashWonText.text = System.String.Format (wonTextTemplate, scoreWon);
			splashWon.SetActive (true);
		} else {
			splashWon.SetActive (false);
		}
	}

	void controlSplashGameOver(bool show)
	{
		if (show) {
			splashGameOver.SetActive (true);
		} else {
			splashGameOver.SetActive (false);
		}
	}

	IEnumerator showSplashWon(int score)
	{
		// show splash!
		controlSplashWon (true, score);
		canUnloadMinigame = true;

		// BLINK IT
		float blinkingTime = 0.05f;
		int nBlinks = 10;
		for (int i = 0; i < nBlinks; i++) {
			yield return new WaitForSeconds(blinkingTime);
			splashWonText.enabled = false;
			yield return new WaitForSeconds(blinkingTime);
			splashWonText.enabled = true;
		}

		// hide splash and update game state
		controlSplashWon (false, score);
		changeMinigame ();
		updateScoreText ();
	}

	IEnumerator showSplashLose()
	{
		controlSplashLose (true);
		canUnloadMinigame = true;

		yield return new WaitForSeconds(3f);

		controlSplashLose (false);
		changeMinigame ();
	}

	IEnumerator showSplashGameOver()
	{
		controlSplashGameOver (true);
		canUnloadMinigame = true;

		yield return new WaitForSeconds(3);

		controlSplashGameOver (false);
	}

	// game difficulty

	public int getDifficulty()
	{
		return 1 + (int) score / 100;
	}
}