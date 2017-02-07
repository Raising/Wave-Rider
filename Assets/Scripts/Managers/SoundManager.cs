using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SoundManager : Singleton<SoundManager> {
	private static string _AUDIO_ROOT_DIRECTORY = "./Audio/";
	private static string _SOUND_ROOT_DIRECTORY = _AUDIO_ROOT_DIRECTORY + "Sounds/";
	private static string _MUSIC_ROOT_DIRECTORY = _AUDIO_ROOT_DIRECTORY + "Music/";

	[SerializeField] private const float _MASTER_VOLUME_ = 1;

	private Dictionary<string, AudioClip> soundResources = new Dictionary<string, AudioClip> ();
	private Dictionary<string, AudioClip> musicResources = new Dictionary<string, AudioClip> ();

	private List<string> validExtensions = new List<string> {".wav"};
	private FileInfo[] audioFiles;

	private AudioSource audioSource;

	public void SoundResource(string name, AudioClip audioClip) {
		soundResources.Add (name, audioClip);
	}

	void Awake() {
		if (Application.isEditor) {
			_AUDIO_ROOT_DIRECTORY = "Assets/Audio/";
			_SOUND_ROOT_DIRECTORY = _AUDIO_ROOT_DIRECTORY + "Sounds/";
			_MUSIC_ROOT_DIRECTORY = _AUDIO_ROOT_DIRECTORY + "Music/";
		}
		LoadAudioResources (_MUSIC_ROOT_DIRECTORY, ref  musicResources);
		LoadAudioResources (_SOUND_ROOT_DIRECTORY,  ref soundResources);
		audioSource = gameObject.GetComponent<AudioSource> ();
		SetVolume(_MASTER_VOLUME_);
	}

	public void LoadAudioResources(string path,  ref Dictionary<string, AudioClip> audioResource) {
		var info = new DirectoryInfo(path);
		audioFiles = info.GetFiles ()
			.Where(f => isValidSoundFile(f.Name))
			.ToArray ();

		foreach (FileInfo audioFile in audioFiles) {
			StartCoroutine (LoadSoundFile (audioFile.FullName, audioFile.Name, audioResource));
		}
	}

	IEnumerator LoadSoundFile(string path, string name, Dictionary<string, AudioClip> audioResource) {
		
		WWW www = new WWW("file://"+path);
		print ("Loading " + path);
		yield return www;
		if (!string.IsNullOrEmpty(www.error)) {
			Debug.Log(www.error);
		}

		print ("Done Loading: " + name);
		audioResource.Add(name, www.audioClip);
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
			//StartCoroutine (SoundManager.Instance.playMainMenuMusic ());
		} else {
			//SoundManager.Instance.playMusic ("NIVEL GENERICO NUEVO");
		}
	}
	IEnumerator playMainMenuMusic() {
		SoundManager.Instance.playMusic ("CABECERA", false);
		yield return new WaitForSeconds (musicResources ["CABECERA"].length);
		SoundManager.Instance.playMusic ("NIVEL 1 + MENÚ");
	}

	public void Play(AudioClip audio, bool canLoop = false) {
		audioSource.clip = audio;
		audioSource.loop = canLoop;
		audioSource.Play ();
	}

	public void playSound(string soundKey) {   
		if (soundResources.ContainsKey(soundKey)) {
			AudioClip soundClip = soundResources [soundKey];
			Play (soundClip);
		}
	}

	public void playMusic(string musicKey, bool canLoop = true) { 
		while (!musicResources.ContainsKey (musicKey)) {

		}
		if (musicResources.ContainsKey(musicKey)) {
			AudioClip soundClip = soundResources [musicKey];
			Play (soundClip, canLoop);
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
