using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class codeController : MonoBehaviour {

	// All Minigames must have these variables
	public int totalSeconds;
	private int currentTimeCount;
	public GameObject startInstructions;

	private GameController gameController;
	private int difficulty;

	private TextMesh code;
	public TextAsset[] possibleCodes;

	private int typingSpeed;
	private int currentChar = 0;

	private string selectedCode;

	// Use this for initialization
	void Start () {

		gameController = (GameController) GameObject.Find("GameController").GetComponent<GameController>();
		difficulty = gameController.getDifficulty(4);

		code = GetComponent<TextMesh> ();
		int selectedCodeID = Random.Range (0, possibleCodes.Length);
		selectedCode = possibleCodes [selectedCodeID].text;

		if (difficulty == 1) typingSpeed = selectedCode.Length/50;
		else if (difficulty == 2) typingSpeed = selectedCode.Length/55;
		else if (difficulty == 3) typingSpeed = selectedCode.Length/60;
		else if (difficulty == 4) typingSpeed = selectedCode.Length/65;

		totalSeconds = 5;
		currentTimeCount = totalSeconds+1;
		StartCoroutine (showSplashInstructions());
		TimeCountdown ();
		InvokeRepeating ("TimeCountdown", 1.0f, 1.0f);
	}

	private int counter = 0;

	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			counter++;
			currentChar += typingSpeed;
			code.text = selectedCode.Substring (0, Mathf.Min(currentChar, selectedCode.Length));
			if (currentChar >= selectedCode.Length) {
				gameController.wonMinigame (20 * difficulty);
				print("Won");
			}
		}
	}

	void TimeCountdown()
	{
		currentTimeCount -= 1;
		gameController.setTime (currentTimeCount);
		if (currentTimeCount == 0) {
			gameController.lostMinigame ();
			print("Lost");
			print (counter);
		}
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
