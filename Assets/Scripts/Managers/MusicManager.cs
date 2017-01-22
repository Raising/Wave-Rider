using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MusicManager : Singleton<MusicManager> {
	[SerializeField]
	private const float _MASTER_VOLUME_ = 10;

	private List<string> validExtensions = new List<string> {".wav"};

	private string _MUSIC_ROOT_DIRECTORY = "./Audio/Sounds/";

	private FileInfo[] audioFiles;

	private AudioSource audioSource;

	[SerializeField]
	private Dictionary<string, AudioClip> SoundResources = new Dictionary<string, AudioClip> ();
	private Dictionary<string, AudioClip> MusicResources = new Dictionary<string, AudioClip> ();


	void Awake() {
	}
	// Use this for initialization
	void Start() {
		if (Application.isEditor) _MUSIC_ROOT_DIRECTORY = "Assets/Audio/Sounds/";
		LoadSoundResources();
		audioSource = gameObject.GetComponent<AudioSource> ();
		SetVolume(_MASTER_VOLUME_);

	}

	public void LoadSoundResources() {
		var info = new DirectoryInfo(_MUSIC_ROOT_DIRECTORY);
		audioFiles = info.GetFiles ()
			.Where(f => isValidSoundFile(f.Name))
			.ToArray ();

		foreach (FileInfo audioFile in audioFiles) {
			StartCoroutine (LoadSoundFile (audioFile.FullName, audioFile.Name));
		}
	}

	public void LoadMusicResources() {
		//MusicResources.Add("Nivel_1.wav
	}

	IEnumerator LoadSoundFile(string path, string name) {
		
		WWW www = new WWW("file://"+path);
		print ("Loading " + path);
		yield return www;
		if (!string.IsNullOrEmpty(www.error)) {
			Debug.Log(www.error);
		}

		print ("Done Loading: " + name);
		SoundResources.Add (name, www.audioClip);
	}

	bool isValidSoundFile(string audioFile) {
		return validExtensions.Contains(Path.GetExtension(audioFile));
	}

	void OnLevelWasLoaded(int nivel) {
		string sceneName = SceneManager.GetSceneByBuildIndex (nivel).name;
		if(sceneName.Contains("Nivel_")) {
			SetVolume(_MASTER_VOLUME_);
			playSound (sceneName+".wav");
		}
	}

	public void Play(AudioClip audio, bool canLoop = false) {
		audioSource.clip = audio;
		audioSource.loop = canLoop;
		audioSource.Play ();
	}

	public void playSound(string soundKey) {   
		if (SoundResources.ContainsKey(soundKey)) {
			AudioClip soundClip = SoundResources [soundKey];
			Play (soundClip);
		}
	}

	private void playMusic(string musicKey) {
		if (MusicResources.ContainsKey (musicKey)) {
			AudioClip musicClip = MusicResources [musicKey];
			Play (musicClip, true);
		}
	}
		

	/*private void playLevelAudio(int audioIndex) {   
		if (SoundResources.ContainsKey(SoundResources.Keys.ElementAt(audioIndex))) {
			AudioClip thisLevelAudio = SoundResources.Values.ElementAt (audioIndex);
			audioSource.clip = thisLevelAudio;
			audioSource.loop = true;
			Debug.Log (thisLevelAudio);
			audioSource.Play ();
		}
	}*/

	public void SetVolume(float volume) {
		audioSource.volume = volume;
	}
}
