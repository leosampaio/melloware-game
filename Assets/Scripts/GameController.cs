using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Text timeText;
	public int totalSeconds;
	private int endTime;
	private int currentTimeCount;

	public Text startCountdownText;
	private int startCountdown = 5;

	public int numberOfMinigames;
	private int currentMinigame;

	private int score;
	private int lifeCount;

	public GameObject[] lifeSprites;
	public Text scoreText;

	private bool mustChangeMinigame; // flag to change minigame

	// Use this for initialization
	void Start () {
		currentTimeCount = totalSeconds+1;
		endTime = 60;
		score = 0;
		lifeCount = 4;
		mustChangeMinigame = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (mustChangeMinigame) {
			changeMinigame ();
			mustChangeMinigame = false;
		}
	}

	public void startGame() {
		TimeCountdown ();
		InvokeRepeating ("TimeCountdown", 1.0f, 1.0f);
		currentMinigame = 1;
		changeMinigame ();
		updateScoreText ();
	}
		
	void TimeCountdown()
	{
		currentTimeCount -= 1;
		int currentTime = endTime - currentTimeCount;
		timeText.text = "23:" + currentTime.ToString ();

		//Check if we've collected all 12 pickups. If we have...
		if (currentTimeCount == 0) {
			timeText.text = "00:00";
			changeMinigame ();
			currentTimeCount = totalSeconds;
		}
	}

	public void wonMinigame(int minigameScore)
	{
		if (score >= 99999) {
			score = 0;
		}
		score += minigameScore;
		print ("WON YAY");
		updateScoreText ();
		mustChangeMinigame = true;
	}

	public void lostMinigame()
	{
		lifeCount--;
		print ("LOOOSER");
		updateLifeCount ();
		mustChangeMinigame = true;
	}

	void changeMinigame()
	{
		// unload current minigame
		Application.UnloadLevel(currentMinigame);

		// randomly select next and load it
		currentMinigame = Random.Range(1, numberOfMinigames);
		Application.LoadLevelAdditive(currentMinigame);
	}

	void updateLifeCount ()
	{
		int totalLives = lifeSprites.GetLength(0);

		if (lifeCount == 0) {
			endGame ();
		}

		for (int i = 0; i < lifeCount; i++) {
			lifeSprites [i].gameObject.SetActive (true);
		}
		for (int i = lifeCount; i < totalLives; i++) {
			lifeSprites [i].gameObject.SetActive (false);
		}

	}

	void endGame()
	{
		print ("GAME OVER... SEU BURRO");
	}

	void updateScoreText()
	{
		scoreText.text = score.ToString ("D5");
	}

	void StartCountdown()
	{
		startCountdown--;
		startCountdownText.text = startCountdown.ToString ();
		if (startCountdown == 0) {
			CancelInvoke ("StartCountdown");
			startCountdownText.gameObject.SetActive (false);
		}
	}

	public void StartStartCountdown()
	{
		startCountdownText.gameObject.SetActive (true);
		InvokeRepeating ("StartCountdown", 1.0f, 1.0f);
	}
}
