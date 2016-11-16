using UnityEngine;
using System.Collections;

public class CloseButtonController : MonoBehaviour {

	private bool pressedCloseOnce = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void didPressClose() {
		transform.parent.parent.GetComponent<FacebookAtackController> ().countDownWindows ();
		transform.parent.gameObject.SetActive (false);
	}

	void OnMouseDown() {
		if (!pressedCloseOnce) {
			didPressClose ();
			pressedCloseOnce = true;
		}
	}
}
