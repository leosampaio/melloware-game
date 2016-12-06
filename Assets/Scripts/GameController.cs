using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	// minigame countdown
	public Text timeText;

	public TextAsset highscoreFile;
	public Text highscoreLabel;
	public int currentHighscore;
	public Text gameOverFinalScore;

	// properties of countdown that triggers just before game start
	// (5, 4, 3...)
	public Text startCountdownText;
	private int startCountdown = 5;

	// minigame control properties
	public int numberOfMinigames;
	private int currentMinigame;

	// player control properties
	private int score;
	public int lifeCount;
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

	public GameObject audioSource;
	private BackgroundAudioContoller audioController;

	public GameObject[] gameUIObjects;

	// Use this for initialization
	void Start () {
		audioController = audioSource.GetComponent<BackgroundAudioContoller>();
		currentMinigame = 1;
		lifeCount = 4;
		score = 0;
		canUnloadMinigame = false;
		wonTextTemplate = splashWonText.text;
		loseTextTemplate = splashLoseText.text;
		audioController.playMenuBackground ();
		checkHighScore ();
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
		score = 0;
		audioController.playGameBackground ();
		updateLifeCount ();
		updateScoreText ();
		changeMinigame ();
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
		currentMinigame = Random.Range(2, numberOfMinigames+1);
//		currentMinigame = 5;
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
		int totalLives = lifeSprites.Length;

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
		updateHighscore ();
		checkHighScore ();
		StartCoroutine(showSplashGameOver ());
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
		activateAllGameUIObjects ();
		audioController.playStartCountdown ();
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
		audioController.playWon ();

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
		audioController.accelerateGameAudio (score);
		changeMinigame ();
		updateScoreText ();
	}

	IEnumerator showSplashLose()
	{
		controlSplashLose (true);
		canUnloadMinigame = true;
		audioController.playLost ();

		yield return new WaitForSeconds(2f);

		controlSplashLose (false);
		changeMinigame ();
	}

	IEnumerator showSplashGameOver()
	{
		controlSplashGameOver (true);
		canUnloadMinigame = true;
		gameOverFinalScore.text = "GAME OVER\nSCORED " + score.ToString();
		audioController.playGameOver ();

		yield return new WaitForSeconds(2);
		unloadAllScenesAndHideGameUI ();

		mainCamera.GetComponent<MainCameraController> ().focusOutOfGame ();

		yield return new WaitForSeconds(0.5f);

		controlSplashGameOver (false);
		audioController.playMenuBackground ();
	}

	// game difficulty
	public int getDifficulty(int max)
	{
		return (int) Mathf.Min(1 + (int) score / 100, max);
	}

	// HIGH SCORE and implications of its existance
	private void checkHighScore() {
		try {
			currentHighscore = PlayerPrefs.GetInt("highscore");
		}
		catch (System.Exception e) {
			PlayerPrefs.SetInt ("highscore", 0);
			currentHighscore = 0;
		}

		highscoreLabel.text = currentHighscore.ToString("D5");
	}

	private void updateHighscore() {
		if (score > currentHighscore) {
			File.WriteAllText (Application.persistentDataPath + "/highscore.txt", score.ToString ());
			PlayerPrefs.SetInt ("highscore", score);
		}
	}

	private void unloadAllScenesAndHideGameUI() {
		if (SceneManager.sceneCount > 1) {
			for (int i = 1; i < SceneManager.sceneCount; i++) {
				Scene toBeUnloaded = SceneManager.GetSceneAt (i);
				if (toBeUnloaded.name != "GameInterface")
					SceneManager.UnloadScene (toBeUnloaded);
			}
		}
		for (int i = 0; i < gameUIObjects.Length; i++) {
			gameUIObjects [i].SetActive (false);
		}
	}

	private void activateAllGameUIObjects() {
		for (int i = 0; i < gameUIObjects.Length; i++) {
			gameUIObjects [i].SetActive (true);
		}
	}
}