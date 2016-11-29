using UnityEngine;
using System.Collections;

public class ChangeMugController : MonoBehaviour {

	public GameObject otherMug;
	public Sprite[] alternativeMugs;
	private int allowedMugs;
	public GameObject unlockNewMugsText;

	private Quaternion finalRotation;
	private bool isRotating;
	private GameController gameController;
	private int currentMug;

	// Use this for initialization
	void Start () {
		currentMug = 0;
		gameController = (GameController) GameObject.Find("GameController").GetComponent<GameController>();
		finalRotation = Quaternion.AngleAxis(25, new Vector3(0,0,1));
	}

	// Update is called once per frame
	void Update () {
		if (isRotating) {
			transform.Rotate (new Vector3 (0, 0, -1), Time.deltaTime*1000);
		}
	}

	void OnMouseDown () {
		isRotating = true;
		Invoke ("stopRotating", 0.38f);
		//if (allowedMugs != alternativeMugs.Length)
			StartCoroutine (showUnlockNewMugs());
	}

	void stopRotating() {
		isRotating = false;
		transform.rotation = finalRotation;
		allowedMugs = gameController.currentHighscore / 200;
		currentMug = (currentMug + 1) % Mathf.Min(alternativeMugs.Length, allowedMugs);
		updateMugs ();
	}

	void updateMugs() {
		GetComponent<SpriteRenderer> ().sprite = alternativeMugs [currentMug];
		otherMug.GetComponent<SpriteRenderer> ().sprite = alternativeMugs [currentMug];
	}

	IEnumerator showUnlockNewMugs()
	{
		unlockNewMugsText.SetActive (true);
		yield return new WaitForSeconds(2);
		unlockNewMugsText.SetActive (false);
	}
}
