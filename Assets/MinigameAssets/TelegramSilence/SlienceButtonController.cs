using UnityEngine;
using System.Collections;

public class SlienceButtonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		transform.parent.gameObject.SetActive(false);
		transform.parent.parent.parent.GetComponent<TelegramSilencingController> ().silencedANotification ();
	}
}
