using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Text timeText;
	public int totalSeconds;
	private int endTime;
	private int currentTimeCount;

	public int numberOfMinigames;
	private int currentMinigame;

	// Use this for initialization
	void Start () {
		currentTimeCount = totalSeconds+1;
		endTime = 60;
		TimeCountdown ();
		InvokeRepeating ("TimeCountdown", 1.0f, 1.0f);

		currentMinigame = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	void TimeCountdown()
	{
		currentTimeCount -= 1;
		int currentTime = endTime - currentTimeCount;
		timeText.text = "23:" + currentTime.ToString();

		//Check if we've collected all 12 pickups. If we have...
		if (currentTimeCount == 0) {
			timeText.text = "00:00";
			changeMinigame ();
			currentTimeCount = totalSeconds;
		}
	}

	void changeMinigame()
	{
//		GameObject currentMinigame = GameObject.Find("MiniGameRoot");
//		Destroy (currentMinigame);

		// unload current minigame
		Application.UnloadLevel(currentMinigame);

		// randomly select next and load it
		currentMinigame = Random.Range(1, numberOfMinigames);
		Application.LoadLevelAdditive(currentMinigame);
	}
}
