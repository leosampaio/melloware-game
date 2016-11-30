using UnityEngine;
using System.Collections;

public class ChangeMugController : MonoBehaviour {

	public GameObject[] otherMugs;
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
		GetComponent<AudioSource> ().Play ();
		if (allowedMugs != alternativeMugs.Length)
			StartCoroutine (showUnlockNewMugs());
	}

	void stopRotating() {
		isRotating = false;
		transform.rotation = finalRotation;
		int itsOver8000 = 1;
		if (gameController.currentHighscore >= 8001)
			itsOver8000 = 0;
		allowedMugs = Mathf.Min(alternativeMugs.Length-itsOver8000, gameController.currentHighscore / 200);
		currentMug = (currentMug + 1) % allowedMugs;
		updateMugs ();
	}

	void updateMugs() {
		GetComponent<SpriteRenderer> ().sprite = alternativeMugs [currentMug];
		for (int i = 0; i < otherMugs.Length; i++)
			otherMugs[i].GetComponent<SpriteRenderer> ().sprite = alternativeMugs [currentMug];
	}

	IEnumerator showUnlockNewMugs()
	{
		unlockNewMugsText.SetActive (true);
		yield return new WaitForSeconds(2);
		unlockNewMugsText.SetActive (false);
	}
}
