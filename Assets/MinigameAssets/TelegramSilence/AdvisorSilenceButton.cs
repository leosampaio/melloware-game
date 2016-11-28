using UnityEngine;
using System.Collections;

public class AdvisorSilenceButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		GameController gameController = (GameController) GameObject.Find("GameController").GetComponent<GameController>();
		gameController.lostMinigame ();
	}
}
