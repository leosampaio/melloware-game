using UnityEngine;
using System.Collections;

public class FacebookAtackController : MonoBehaviour {

	// All Minigames must have these variables
	public int totalSeconds;
	private int currentTimeCount;
	public GameObject startInstructions;

	public GameObject facebookPage;
	public int nPagesMax;
	private int nPages;
	public int windowCount;

	private GameController gameController;
	private int difficulty;

	// Use this for initialization
	void Start () {
		gameController = (GameController) GameObject.Find("GameController").GetComponent<GameController>();

		difficulty = gameController.getDifficulty (3);

		nPages = Random.Range (difficulty, difficulty*2+1);
		windowCount = nPages;
		StartCoroutine (popUpWindows ());

		if (difficulty == 1) {totalSeconds = 3;}
		if (difficulty == 2) {totalSeconds = 4;}
		if (difficulty == 3) {totalSeconds = 5;}
		currentTimeCount = totalSeconds+1;

		StartCoroutine (showSplashInstructions());

		TimeCountdown ();
		InvokeRepeating ("TimeCountdown", 1.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TimeCountdown()
	{
		currentTimeCount -= 1;
		gameController.setTime (currentTimeCount);
		if (currentTimeCount == 0) {
			gameController.lostMinigame ();
		}
	}

	IEnumerator popUpWindows()
	{
		for (int i = 0; i < nPages; i++) {
			// these magical numbers define where we can place the window given the player must hit the X
			Vector3 windowPlacement = new Vector3(Random.Range(-47f, -59.75f), Random.Range(-15.0f, -22f), 1);
			GameObject newPage = Instantiate(facebookPage, transform) as GameObject;
			newPage.transform.localPosition = windowPlacement;
			yield return new WaitForSeconds(0.5f);
		}
	}

	public void countDownWindows()
	{
		windowCount--;
		if (windowCount == 0) {
			gameController.wonMinigame(20*difficulty);
		}
	}

	IEnumerator showSplashInstructions()
	{
		// show splash!
		startInstructions.SetActive(true);
		yield return new WaitForSeconds(1);
		startInstructions.SetActive(false);
	}
}
