using UnityEngine;
using System.Collections;

public class PartyMessageTextCreator : MonoBehaviour {

	private string[] possibleMessages = {
		"When is the party?",
		"Are you coming?",
		"GET READY!",
		"IT WILL BE\nAWESOME",
		"SEE YOU THERE",
		"Bring the beer!",
		"Who got the\nVodka?",
		"Where are you?",
	};

	// Use this for initialization
	void Start () {
		int message = Random.Range (0, possibleMessages.Length);
		transform.GetComponent<TextMesh> ().text = possibleMessages [message];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeMessage() {
		int message = Random.Range (0, possibleMessages.Length);
		transform.GetComponent<TextMesh> ().text = possibleMessages [message];
	}
}
