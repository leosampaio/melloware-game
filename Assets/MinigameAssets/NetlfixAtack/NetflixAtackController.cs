using UnityEngine;
using System.Collections;

public class NetflixAtackController : MonoBehaviour {

	// All Minigames must have these variables
	public int totalSeconds;
	private int currentTimeCount;

	public Sprite[] netflixWindows;
	public GameObject netflixWindowPrefab;

	private GameController gameController;

	private int difficulty;

	// Use this for initialization
	void Start () {
		gameController = (GameController) GameObject.Find("GameController").GetComponent<GameController>();

		difficulty = gameController.getDifficulty (3);

		totalSeconds = 5;
		currentTimeCount = totalSeconds+1;		
		TimeCountdown ();
		InvokeRepeating ("TimeCountdown", 1.0f, 1.0f);

		float delay = 1.0f;
		if (difficulty == 1) {delay = 0.8f;}
		else if (difficulty == 2) {delay = 0.7f;}
		else {delay = 0.6f;}
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
		// these magical numbers define where we can place the window given the player must hit the X
		Vector3 windowPlacement = new Vector3(Random.Range(-63f, -50f), -7, 1);
		int movieKind = Random.Range (0, netflixWindows.Length);
		GameObject newPage = Instantiate(netflixWindowPrefab, transform) as GameObject;
		newPage.transform.localPosition = windowPlacement;
		newPage.GetComponent<SpriteRenderer> ().sprite = netflixWindows [movieKind];
	}
}
