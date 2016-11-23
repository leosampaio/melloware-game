using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

	private GameController gameController;
	public Camera minigameCamera;

	public float mouseMoveSpeed;
	public float minX, maxX, minY, maxY;

	// Use this for initialization
	void Start () {
		gameController = (GameController) GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 screenPoint = minigameCamera.WorldToScreenPoint(gameObject.transform.position);
		Vector3 mousePosition = minigameCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
//		transform.position = mousePosition;
		transform.position = new Vector3(Mathf.Clamp(mousePosition.x, -168, -153), Mathf.Clamp(mousePosition.y, -17, -8), 1);

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
		if (other.gameObject.CompareTag("FallingNetflix"))
		{
			gameController.lostMinigame();
			print("lost");
		}
	}
}
