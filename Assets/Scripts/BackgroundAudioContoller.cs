using UnityEngine;
using System.Collections;

public class BackgroundAudioContoller : MonoBehaviour {

	public AudioClip menuBackground;
	public AudioClip gameBackground;
	public AudioClip startCountdown;
	public AudioClip won;
	public AudioClip lost;
	public AudioClip gameOver;
	public AudioClip lostLife;

	private AudioSource source;
	private AudioSource effects;

	// Use this for initialization
	void Start () {
		source = GetComponents<AudioSource> ()[0];
		effects = GetComponents<AudioSource> ()[1];
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void playGameBackground() {
		source.clip = gameBackground;
		source.pitch = 1.1f;
		source.Play ();
	}

	public void playMenuBackground() {
		source.clip = menuBackground;
		source.pitch = 1;
		source.Play ();
	}

	public void accelerateGameAudio(int amount) {
		source.pitch += amount/100;
	}

	public void pause() {
		source.Pause ();
	}
		
	public void playStartCountdown() {
		effects.clip = startCountdown;
		effects.volume = 0.5f;
		effects.Play ();
	}

	public void playWon(){
		effects.clip = won;
		effects.Play ();
	}

	public void playLost(){
		effects.clip = lost;
		effects.Play ();
	}

	public void playLostLife() {
		effects.clip = lostLife;
		effects.Play ();
	}

	public void playGameOver() {
		source.clip = gameOver;
		source.Play ();
	}
}
