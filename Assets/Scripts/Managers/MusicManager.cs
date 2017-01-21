using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MusicManager : Singleton<MusicManager> {
	[SerializeField]
	private const float _MASTER_VOLUME_ = 10;

	private List<string> validExtensions = new List<string> {".wav"};

	private string _MUSIC_ROOT_DIRECTORY = "./Music/";

	private FileInfo[] audioFiles;

	private AudioSource audioSource;

	[SerializeField]
	private Dictionary<string, AudioClip> AudioResourcesCollection;


	void Awake() {
		AudioResourcesCollection = new Dictionary<string, AudioClip> ();
	}
	// Use this for initialization
	void Start() {
		if (Application.isEditor) _MUSIC_ROOT_DIRECTORY = "Assets/Music/";
		LoadAudioResources();
		audioSource = gameObject.GetComponent<AudioSource> ();
		SetVolume(_MASTER_VOLUME_);

	}

	public void LoadAudioResources() {
		var info = new DirectoryInfo(_MUSIC_ROOT_DIRECTORY);
		audioFiles = info.GetFiles ()
			.Where(f => isValidAudioFile(f.Name))
			.ToArray ();

		foreach (FileInfo audioFile in audioFiles) {
			StartCoroutine (LoadFile (audioFile.FullName));
		}
	}

	IEnumerator LoadFile(string path) {
		
		WWW www = new WWW("file://"+path);
		print ("loading " + path);
		yield return www;
		if (!string.IsNullOrEmpty(www.error)) {
			Debug.Log(www.error);
		}

		print ("done loading");
		AudioResourcesCollection.Add (www.audioClip.name, www.audioClip);

		playLevelAudio (0);
	}

	bool isValidAudioFile(string audioFile) {
		return validExtensions.Contains(Path.GetExtension(audioFile));
	}

	/*void OnLevelWasLoaded(int nivel) {
		SetVolume(_MASTER_VOLUME_);
		playLevelAudio ("ThemeNivel"+nivel);
	}*/

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
			Debug.Log (thisLevelAudio);
			audioSource.Play ();
		}
	}


	public void SetVolume(float volume) {
		audioSource.volume = volume;
	}
}
