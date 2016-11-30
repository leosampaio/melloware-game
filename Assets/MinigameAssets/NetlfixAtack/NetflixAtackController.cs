using UnityEngine;
using System.Collections;

public class NetflixAtackController : MonoBehaviour {

	// All Minigames must have these variables
	public int totalSeconds;
	private int currentTimeCount;
	public GameObject startInstructions;

	public Sprite[] netflixWindows;
	public GameObject netflixWindowPrefab;

	// safety control
	public float safeWindowSize;
	public int countFallen;
	private float safeWindowStart;

	private GameController gameController;

	private int difficulty;

	// Use this for initialization
	void Start () {
		gameController = (GameController) GameObject.Find("GameController").GetComponent<GameController>();

		difficulty = gameController.getDifficulty (3);

		totalSeconds = 5;
		currentTimeCount = totalSeconds+1;
		StartCoroutine (showSplashInstructions());
		TimeCountdown ();
		InvokeRepeating ("TimeCountdown", 1.0f, 1.0f);

		float delay = 1.0f;
		if (difficulty == 1) {delay = 0.7f; safeWindowSize = 5.2f;}
		else if (difficulty == 2) {delay = 0.65f; safeWindowSize = 5.1f;}
		else {delay = 0.6f; safeWindowSize = 5f;}
		InvokeRepeating("popUpWindows", 0, delay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void TimeCountdown()
	{
		currentTimeCount -= 1;
		gameController.setTime (currentTimeCount);
		if (currentTimeCount == 0) {
			gameController.wonMinigame (20*difficulty);
		}
	}

	void popUpWindows()
	{
		// change safe window every n falling netflixes
		if (countFallen%5 == 0) {
			// limits of gameplay - safewindowsize - 3 to keep window away from border
			safeWindowStart = Random.Range (-63f+1, -50f - safeWindowSize - 1f);
		}
		countFallen++;

		// these magical numbers define where we can place the window given the player must hit the X
		int leftOrRightOfSafeWindow = Random.Range(0, 2);
		Vector3 windowPlacement;
		if (leftOrRightOfSafeWindow == 0)
			windowPlacement = new Vector3(Random.Range(-63f, safeWindowStart), -7, 1);
		else
			windowPlacement = new Vector3(Random.Range(safeWindowStart+safeWindowSize, -50f), -7, 1);
		int movieKind = Random.Range (0, netflixWindows.Length);
		GameObject newPage = Instantiate(netflixWindowPrefab, transform) as GameObject;
		newPage.transform.localPosition = windowPlacement;
		newPage.GetComponent<SpriteRenderer> ().sprite = netflixWindows [movieKind];
	}

	IEnumerator showSplashInstructions()
	{
		// show splash!
		startInstructions.SetActive(true);
		yield return new WaitForSeconds(1);
		currentTimeCount++;
		startInstructions.SetActive(false);
	}
}
