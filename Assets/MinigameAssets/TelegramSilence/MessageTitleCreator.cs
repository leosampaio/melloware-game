using UnityEngine;
using System.Collections;

public class MessageTitleCreator : MonoBehaviour {

	private string[] possibleMessages = {
		"Lena's Birthday",
		"TUSCA",
		"Gamenight",
		"Semcomp42",
		"Bananight",
		"CodeClubClub",
		"Ragnarokers",
		"WOW",
		"Monsters BIRL"
	};

	// Use this for initialization
	void Start () {
		int message = Random.Range (0, possibleMessages.Length);
		transform.GetComponent<TextMesh> ().text = possibleMessages [message];
	}
}
