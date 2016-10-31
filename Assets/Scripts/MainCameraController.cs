using UnityEngine;
using System.Collections;

public class MainCameraController : MonoBehaviour {

	private Vector3 gameCameraInitialPos;
	private float gameCameraInitialSize;

	public float smoothTime = 0.3F;
	private Vector3 velocity = Vector3.zero;
	private float zoomVelocity = 0f;

	private bool focusingOnGame = false;

	public GameController gameController;

	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (16f, 52.6f, -10f);
		Camera.main.orthographicSize = 94.29031f;

		gameCameraInitialPos = new Vector3 (0.05f, 0.2f, -10f);
		gameCameraInitialSize = 10.7841f;
	}
	
	// Update is called once per frame
	void Update () {
		if (focusingOnGame) {
			transform.position = Vector3.SmoothDamp (transform.position, gameCameraInitialPos, ref velocity, smoothTime);
			Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, gameCameraInitialSize, ref zoomVelocity, smoothTime);
			if (transform.position == gameCameraInitialPos) {
				focusingOnGame = false;
				gameController.startGame ();
			}
		}
	}

	public void focusOnGame() {
		focusingOnGame = true;
	}
}
