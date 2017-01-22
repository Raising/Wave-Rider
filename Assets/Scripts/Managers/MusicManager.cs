using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MusicManager : Singleton<MusicManager> {
	[SerializeField]
	private const float _MASTER_VOLUME_ = 1;

	private List<string> validExtensions = new List<string> {".wav"};

	private string _MUSIC_ROOT_DIRECTORY = "./Audio/Sounds/";

	private FileInfo[] audioFiles;

	private AudioSource audioSource;

	[SerializeField]
	private Dictionary<string, AudioClip> SoundResources = new Dictionary<string, AudioClip> ();
	[SerializeField]
	private AudioClip[] MusicResources;


	void Awake() {
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

	void OnEnable()
	{
		//Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable()
	{
		//Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("Level Loaded");
		Debug.Log(scene.name);
		Debug.Log(mode);

		string sceneName = scene.name;
		Debug.Log (sceneName);

		if (sceneName == "MenuPrincipal") {
			StartCoroutine (MusicManager.Instance.playMainMenuMusic ());
		} else {
			MusicManager.Instance.playMusic (2);
		}
	}
	IEnumerator playMainMenuMusic() {
		MusicManager.Instance.playMusic (0, false);
		yield return new WaitForSeconds (MusicResources [0].length);
		MusicManager.Instance.playMusic (1);
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

	private void playMusic(int musicKey, bool canLoop = true) {
		if (MusicResources[musicKey]) {
			AudioClip musicClip = MusicResources [musicKey];
			Play (musicClip, canLoop);
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
