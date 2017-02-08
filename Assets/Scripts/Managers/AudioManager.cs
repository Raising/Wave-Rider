using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager> {
	
	// Variables de PATH para los recursos de sonido
	private static string _AUDIO_ROOT_DIRECTORY = "Assets/Audio/";
	private static string _SOUND_ROOT_DIRECTORY = _AUDIO_ROOT_DIRECTORY + "Sounds/";
	private static string _MUSIC_ROOT_DIRECTORY = _AUDIO_ROOT_DIRECTORY + "Music/";

	// Componentes de audio y configuración.
	private AudioSource audioSource;
	[SerializeField] private const float _MASTER_VOLUME_ = 1;

	// Recursos de audio, formatos permitidos y todo lo necesario para recuperarlos.
	[SerializeField] private AudioClip[] serializedMusicResources;
	private Dictionary<string, AudioClip> soundResources = new Dictionary<string, AudioClip> ();
	private Dictionary<string, AudioClip> musicResources = new Dictionary<string, AudioClip> ();
	private List<string> validExtensions = new List<string> {".wav"};
	private FileInfo[] audioFiles;

	void Awake() {
		LoadMusicResources ();
		LoadSoundResources (_SOUND_ROOT_DIRECTORY);
		audioSource = gameObject.GetComponent<AudioSource> ();
		SetVolume(_MASTER_VOLUME_);
	}

	public void SoundResource(string name, AudioClip audioClip) {
		soundResources.Add (name, audioClip);
	}

	public void LoadMusicResources() {
		foreach (AudioClip audio in serializedMusicResources) {
			musicResources.Add (audio.name, audio);
		}
	}
		

	public void LoadSoundResources(string path) {
		var info = new DirectoryInfo(path);
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
		SoundResource(name, www.audioClip);
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
			StartCoroutine (AudioManager.Instance.playMainMenuMusic ());
		} else {
			//SoundManager.Instance.playMusic ("NIVEL GENERICO NUEVO");
		}
	}
	IEnumerator playMainMenuMusic() {
		AudioManager.Instance.playMusic ("CABECERA", false);
		yield return new WaitForSeconds (musicResources ["CABECERA"].length);
		AudioManager.Instance.playMusic ("NIVEL 1 + MENÚ");
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
		if (musicResources.ContainsKey(musicKey)) {
			AudioClip soundClip = musicResources [musicKey];
			Play (soundClip, canLoop);
		}
	}
		
	public void SetVolume(float volume) {
		audioSource.volume = volume;
	}
}
