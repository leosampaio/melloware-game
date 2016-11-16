using UnityEngine;
using System.Collections;

public class FacebookAtackController : MonoBehaviour {

	public GameObject facebookPage;
	public int nPagesMax;
	private int nPages;
	public int windowCount;

	private GameController gameController;

	// Use this for initialization
	void Start () {
		nPages = Random.Range (1, nPagesMax);
		windowCount = nPages;
		StartCoroutine (popUpWindows ());
	}
	
	// Update is called once per frame
	void Update () {
		
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
			gameController = (GameController) GameObject.Find("GameController").GetComponent<GameController>();
			gameController.wonMinigame(10*nPages);
		}
	}
}
