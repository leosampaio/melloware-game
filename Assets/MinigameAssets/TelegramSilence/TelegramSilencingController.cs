using UnityEngine;
using System.Collections;

public class TelegramSilencingController : MonoBehaviour
{

	// All Minigames must have these variables
	public int totalSeconds;
	private int currentTimeCount;
	public GameObject startInstructions;

	public GameObject[] messageSpots;
	public int maxMessages;
	public float advisorPopInterval;
	public float partyPopInterval;

	public GameObject PartyMessageNoti;
	public GameObject AdvisorMessageNoti;
	public GameObject AdvisorMessageMain;
	public GameObject telegramPage;

	public Sprite[] partyMessageSprites;

	public int currentAdvisorNotiPosition;
	private int countPartyMessages = 1; // we already have the advisor's messages

	private bool pausePop = false;

	private int advisorMessageCount = 0;

	private string[] advisorPossibleMessages = 
	{"WORK HARD", "Where is my report?", "What are you doing\nNot writing?", "Did you submit the paper?",
			"Did you grade the tests?", "KEEP WORKING", "Why are you on Telegram?"
	};

	private GameController gameController;
	private int difficulty;

	public AudioSource notiSound;

	// Use this for initialization
	void Start ()
	{
		gameController = (GameController) GameObject.Find("GameController").GetComponent<GameController>();
		difficulty = gameController.getDifficulty (3);

		notiSound = GetComponent<AudioSource> ();

		totalSeconds = 5;
		currentTimeCount = totalSeconds+1;
		StartCoroutine (showSplashInstructions());
		TimeCountdown ();
		InvokeRepeating ("TimeCountdown", 1.0f, 1.0f);

		advisorPopInterval = 2f-difficulty/3;
		partyPopInterval = advisorPopInterval / 2f + 0.1f;

		currentAdvisorNotiPosition = 0;
		updateNotificationsView ();

		InvokeRepeating ("popAdvisorMessage", 0, advisorPopInterval);
		InvokeRepeating ("popPartyMessage", 0, partyPopInterval);
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void popAdvisorMessage ()
	{
		if (!pausePop) {
			notiSound.Play ();

			// get it to top
			sendNotiToTop (currentAdvisorNotiPosition);

			// get and set text
			int selectedText = Random.Range(0, advisorPossibleMessages.Length);
			string newText = advisorPossibleMessages [selectedText];
			AdvisorMessageNoti.transform.FindChild ("Message").GetComponent<TextMesh> ().text = newText;

			// add new message message thread
			GameObject newMessage = Instantiate (AdvisorMessageMain, telegramPage.transform) as GameObject;
			newMessage.transform.localScale = new Vector3 (1.62959f, 1.62959f, 1.62959f);
			newMessage.transform.localPosition = new Vector3 (0, 10 - 1.4f * advisorMessageCount, -3);
			newMessage.transform.FindChild ("Message").GetComponent<TextMesh> ().text = newText;
			advisorMessageCount++;
		}
	}
		
	void popPartyMessage()
	{
		if (!pausePop) {

			notiSound.Play ();

			// fill with messages
			if (countPartyMessages < maxMessages) {
				GameObject newMessage = Instantiate (PartyMessageNoti, telegramPage.transform) as GameObject;
				newMessage.transform.localScale = new Vector3 (1, 1, 1);
				int messageSprite = Random.Range (0, partyMessageSprites.Length);
				newMessage.GetComponent<SpriteRenderer> ().sprite = partyMessageSprites [messageSprite];
				messageSpots [countPartyMessages] = newMessage;
				sendNotiToTop (countPartyMessages);
				countPartyMessages++;
			}

			// or pop new notifications
			else {
				int selected = -1;
				do {
					selected = Random.Range (0, maxMessages);
				} while (selected == currentAdvisorNotiPosition);
				messageSpots [selected].GetComponentInChildren<PartyMessageTextCreator> ().changeMessage ();
				sendNotiToTop (selected);
			}
		}
	}

	void updateNotificationsView ()
	{
		for (int i = 0; i < messageSpots.Length; i++) {
			if (messageSpots [i] != null) {
				float newY = 3 - 3 * i;
				messageSpots [i].transform.localPosition = new Vector3 (-5.019f, newY, -4);
			}
		}
	}

	void swapNotifications (int a, int b)
	{
		GameObject aux = messageSpots [a];
		messageSpots [a] = messageSpots [b];
		messageSpots [b] = aux;

		// check for advisor position change
		if (b == currentAdvisorNotiPosition) {
			currentAdvisorNotiPosition = a;
		} else if (a == currentAdvisorNotiPosition) {
			currentAdvisorNotiPosition = b;
		}
	}

	void sendNotiToTop(int a) {
		for (int i = a; i > 0; i--) {
			swapNotifications (i, i - 1);
		}
		updateNotificationsView ();
	}

	public void silencedANotification ()
	{
		pausePop = true;
		bool foundSilenced = false;
		for (int i = 0; i < maxMessages; i++) {
			if (foundSilenced) {
				swapNotifications (i - 1, i);
				print (i);
			}
			else if (!messageSpots[i].activeSelf) {
				foundSilenced = true;
				print ("Deactivated");
				print (i);
			}
		}
		updateNotificationsView ();
		countPartyMessages -= 1;
		maxMessages -= 1;
		if (maxMessages == 1) {
			gameController.wonMinigame (difficulty * 20);
		} else {
			pausePop = false;
		}
	}

	void TimeCountdown()
	{
		currentTimeCount -= 1;
		gameController.setTime (currentTimeCount);
		if (currentTimeCount == 0) {
			gameController.lostMinigame ();
		}
	}

	IEnumerator showSplashInstructions()
	{
		// show splash!
		startInstructions.SetActive(true);
		yield return new WaitForSeconds(1);
		currentTimeCount++;
		startInstructions.SetActive(false);
	}
}
