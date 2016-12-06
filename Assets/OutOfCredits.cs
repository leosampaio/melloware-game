using UnityEngine;
using System.Collections;

public class OutOfCredits : MonoBehaviour {

	public Camera camera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		camera.GetComponent<MainCameraController> ().focusOutOfCredits ();
	}
}
