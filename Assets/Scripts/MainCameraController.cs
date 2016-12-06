using UnityEngine;
using System.Collections;

public class MainCameraController : MonoBehaviour {

	private Vector3 gameCameraInitialPos;
	private float gameCameraInitialSize;
	private Vector3 menuCameraInitialPos;
	private float menuCameraInitialSize;

	private Vector3 creditsCameraPos;
	private float creditsCameraSize;

	public float smoothTime = 0.3F;
	private Vector3 velocity = Vector3.zero;
	private float zoomVelocity = 0f;

	private bool focusingOnGame = false;
	private bool focusingOutOfGame = false;
	private bool focusingOnCredits = false;
	private bool focusingOutOfCredits = false;

	public GameObject creditsObjects;

	public GameController gameController;

	// Use this for initialization
	void Start () {
		menuCameraInitialPos = new Vector3 (16f, 52.6f, -10f);
		menuCameraInitialSize = 94.29031f;
		transform.position = menuCameraInitialPos;
		Camera.main.orthographicSize = menuCameraInitialSize;

		gameCameraInitialPos = new Vector3 (0.05f, 0.2f, -10f);
		gameCameraInitialSize = 10.7841f;

		creditsCameraPos = new Vector3 (44f, 1.5f, -10f);
		creditsCameraSize = 14f;
	}
	
	// Update is called once per frame
	void Update () {
		if (focusingOnGame) {
			transform.position = Vector3.SmoothDamp (transform.position, gameCameraInitialPos, ref velocity, smoothTime);
			Camera.main.orthographicSize = Mathf.SmoothDamp (Camera.main.orthographicSize, gameCameraInitialSize, ref zoomVelocity, smoothTime);
			if (transform.position == gameCameraInitialPos) {
				focusingOnGame = false;
				gameController.startGame ();
			}
		} else if (focusingOutOfGame || focusingOutOfCredits) {
			transform.position = Vector3.SmoothDamp (transform.position, menuCameraInitialPos, ref velocity, smoothTime);
			Camera.main.orthographicSize = Mathf.SmoothDamp (Camera.main.orthographicSize, menuCameraInitialSize, ref zoomVelocity, smoothTime);
			if (transform.position == menuCameraInitialPos) {
				focusingOutOfGame = false;
				focusingOutOfCredits = false;
			}
		} else if (focusingOnCredits) {
			transform.position = Vector3.SmoothDamp (transform.position, creditsCameraPos, ref velocity, smoothTime);
			Camera.main.orthographicSize = Mathf.SmoothDamp (Camera.main.orthographicSize, creditsCameraSize, ref zoomVelocity, smoothTime);
			if (transform.position == creditsCameraPos) {
				focusingOnCredits = false;
			}
		}
	}
	 
	public void focusOnGame() {
		focusingOnGame = true;
		focusingOutOfGame = false;
		focusingOnCredits = false;
		focusingOutOfCredits = false;
	}

	public void focusOutOfGame() {
		focusingOutOfGame = true;
		focusingOnGame = false;
		focusingOnCredits = false;
		focusingOutOfCredits = false;
	}

	public void focusOnCredits() {
		focusingOnGame = false;
		focusingOutOfGame = false;
		focusingOnCredits = true;
		focusingOutOfCredits = false;
		creditsObjects.SetActive(true);
	}

	public void focusOutOfCredits() {
		focusingOnCredits = false;
		focusingOutOfCredits = true;
		creditsObjects.SetActive(false);
	}
}
