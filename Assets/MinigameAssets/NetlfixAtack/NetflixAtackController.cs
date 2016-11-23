using UnityEngine;
using System.Collections;

public class NetflixAtackController : MonoBehaviour {

	// All Minigames must have these variables
	public int totalSeconds;
	private int currentTimeCount;

	public Sprite[] netflixWindows;
	public GameObject netflixWindowPrefab;

	private GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = (GameController) GameObject.Find("GameController").GetComponent<GameController>();

		int difficulty = gameController.getDifficulty ();

		totalSeconds = 5;
		currentTimeCount = totalSeconds+1;		
		TimeCountdown ();
		InvokeRepeating ("TimeCountdown", 1.0f, 1.0f);

		InvokeRepeating("popUpWindows", 0, 1f/difficulty);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void TimeCountdown()
	{
		currentTimeCount -= 1;
		gameController.setTime (currentTimeCount);
		if (currentTimeCount == 0) {
			gameController.wonMinigame (20);
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
