using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MusicManager : Singleton<MusicManager> {
	[SerializeField]
	private const float _MASTER_VOLUME_ = 10;

	private const string _MUSIC_ROOT_DIRECTORY = "Assets/Music/";

	private AudioSource audioSource;

	[SerializeField]
	private Dictionary<string, AudioClip> AudioResourcesCollection;


	void Awake() {
		LoadAudioResources();
	}
	// Use this for initialization
	void Start() {
		audioSource = gameObject.GetComponent<AudioSource> ();
		SetVolume(_MASTER_VOLUME_);
		playLevelAudio (0);
	}

	public void LoadAudioResources() {

	}

	void OnLevelWasLoaded(int nivel) {
		SetVolume(_MASTER_VOLUME_);
		playLevelAudio ("ThemeNivel"+nivel);
	}

	private void playLevelAudio(string audioIndex) {   
		if (AudioResourcesCollection.ContainsKey(audioIndex)) {
			AudioClip thisLevelAudio = AudioResourcesCollection [audioIndex];
			audioSource.clip = thisLevelAudio;
			audioSource.loop = true;
			audioSource.Play ();
		}
	}

	private void playLevelAudio(int audioIndex) {   
		if (AudioResourcesCollection.ContainsKey(AudioResourcesCollection.Keys.ElementAt(audioIndex))) {
			AudioClip thisLevelAudio = AudioResourcesCollection.Values.ElementAt (audioIndex);
			audioSource.clip = thisLevelAudio;
			audioSource.loop = true;
			audioSource.Play ();
		}
	}


	public void SetVolume(float volume) {
		audioSource.volume = volume;
	}
}
