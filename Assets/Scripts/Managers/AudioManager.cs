using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



/// <summary>
/// Clase encargada de gestionar todos los recursos de audio, así como su reproducción 
/// <remarks> SINGLETON, acceder siempre mediante <see cref="Instance"/> </remarks>
/// </summary>

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance = null;
    /// <summary>
    /// Ruta base de los recursos de audio
    /// </summary>
    private static string _AUDIO_ROOT_DIRECTORY = "Assets/Audio/";
	/// <summary>
	/// Ruta de los recursos de sonido
	/// </summary>
	private static string _SOUND_ROOT_DIRECTORY = _AUDIO_ROOT_DIRECTORY + "Sounds/";
	/// <summary>
	/// Ruta de los recursos de musica
	/// </summary>
	private static string _MUSIC_ROOT_DIRECTORY = _AUDIO_ROOT_DIRECTORY + "Music/";

	/// <summary>
	/// Audiosource que se utiliza para reproducir los sonidos
	/// <remarks> Se marca desde editor></remarks>
	/// </summary>
	private AudioSource audioSource;
	/// <summary>
	/// Volumen maestro (por defecto)
	/// </summary>
	[SerializeField] private const float _MASTER_VOLUME_ = 1;

	/// <summary>
	/// El volumen actual
	/// </summary>
	private float currentVolume;

	/// <summary>
	/// Array de los recursos de musica
	/// <remarks> Se marca desde el editor </remarks>
	/// </summary>
	[SerializeField] private AudioClip[] serializedMusicResources;
	/// <summary>
	/// Diccionario con todos los sonidos
	/// /// <remarks> Se accede a ellos mediante su nombre de fichero </remarks>
	/// </summary>
	private Dictionary<string, AudioClip> soundResources = new Dictionary<string, AudioClip> ();
	/// <summary>
	/// Diccionario con toda la musica
	/// <remarks> Se accede a ellos mediante su nombre de fichero </remarks>
	/// </summary>
	private Dictionary<string, AudioClip> musicResources = new Dictionary<string, AudioClip> ();
	/// <summary>
	/// Extensiones de sonido validas
	/// </summary>
	private List<string> validExtensions = new List<string> {".wav"};
	/// <summary>
	/// Array para recoger los ficheros de audio a cargar.
	/// </summary>
	private FileInfo[] audioFiles;

    /// <summary>
    /// Obtiene o modifica el volumen actual
    /// </summary>
    /// <value>The current volume.</value>
    public float CurrentVolume {
		get {
			return this.currentVolume;
		}
		set {
			currentVolume = value;
		}
	}

	/// <summary>
	/// Permite obtener un recurso de sonido
	/// </summary>
	/// <returns>Retorna el recurso de sonido</returns>
	/// <param name="soundKey">Nombre del fichero de sonido</param>
	public AudioClip SoundResources (string soundKey) {
		if (this.soundResources.ContainsKey (soundKey)) {
			return this.soundResources [soundKey];
		}

		return null;
	}

	void Awake() {
        //Check if instance already exists
        if (Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        // Se cargan los recursos de musica y sonido
        LoadMusicResources ();
		LoadSoundResources (_SOUND_ROOT_DIRECTORY);
        //Recuperamos componente de audio y seteamos configuracion
		audioSource = gameObject.GetComponent<AudioSource> ();
		currentVolume = _MASTER_VOLUME_;
		SetVolume(currentVolume);
	}

    /// <summary>
    /// Carga los recursos de musica en un diccionario a partir del array 
    /// que hay cargado en AudioManager
    /// </summary>
      
	public void LoadMusicResources() {
		foreach (AudioClip audio in serializedMusicResources) {
			musicResources.Add (audio.name, audio);
		}
	}

    /// <summary>
    /// Carga los recursos de sonido dada una ruta
    /// <remarks>Formatos admitidos en <paramref name="validExtensions"/></remarks>
    /// </summary>
    /// <param name="path">Ruta absoluta a los recursos de sonido</param>

    public void LoadSoundResources(string path) {
		var info = new DirectoryInfo(path);
        audioFiles = info.GetFiles("*.*", SearchOption.AllDirectories)
			.Where(f => isValidSoundFile(f.Name))
			.ToArray ();

		foreach (FileInfo audioFile in audioFiles) {
			StartCoroutine (LoadSoundFile (audioFile.FullName, audioFile.Name));
		}
	}

	/// <summary>
	/// Carga un sonido y lo añade a <paramref name="soundResources"/>
	/// </summary>
	/// <param name="path">Ruta del sonido.</param>
	/// <param name="name">Nombre del sonido.</param>

	IEnumerator LoadSoundFile(string path, string name) {
		WWW www = new WWW("file://"+path);
		//print ("Loading " + path);
		yield return www;
		if (!string.IsNullOrEmpty(www.error)) {
			Debug.Log(www.error);
		}

		//print ("Done Loading: " + name);
        soundResources.Add(name, www.GetAudioClip());
	}

	/// <summary>
	/// Compruebo si el fichero de audio tiene una extensión válida
	/// </summary>
	/// <returns><c>true</c>, Si el recurso tiene una extensión valida, <c>false</c> Cualquier otro caso.</returns>
	/// <param name="audioFile">Audio file.</param>
	bool isValidSoundFile(string audioFile) {
		return validExtensions.Contains(Path.GetExtension(audioFile));
	}



	/// <summary>
	/// Reproduce la música de la intro + menú principal
	/// </summary>
	public IEnumerator playMainMenuMusic() { //TODO TRASLADAR TODO ESTO A SCENECONTROLLER
		playMusic ("MainTheme", false);
		yield return new WaitForSeconds (musicResources ["MainTheme"].length);
		playMusic ("NIVEL 1 + MENÚ");
	}

	/// <summary>
	/// Reproduce un audio
	/// <remarks> Reemplazará cualquier otro sonido que hubiese reproduciéndose como principal></remarks>
	/// </summary>
	/// <param name="audio">Audioclip a reproducir.</param>
	/// <param name="canLoop">Indica si el audio se reproduce en bucle<c>true</c> En bucle.</param>
	private void Play(AudioClip audio, bool canLoop = false) {
		audioSource.clip = audio;
		audioSource.loop = canLoop;
		audioSource.Play ();
	}

	/// <summary>
	/// Reproduce un determinado recurso de sonido
	/// <remarks> Respetará cualquier otra fuente de sonido en marcha></remarks>
	/// </summary>
	/// <param name="soundKey">Nombre del sonido.</param>
	public void playSound(string soundKey) {   
		if (soundResources.ContainsKey(soundKey)) {
			AudioClip soundClip = soundResources [soundKey];
			//audioSource.clip = soundClip;

			 //audioSource.Play();
			//audioSource.PlayOneShot (soundClip);
		}
	}

	/// <summary>
	/// Reproduce un determinado recurso de musica
	/// </summary>
	/// <param name="musicKey">Nombre de la musica.</param>
	/// <param name="canLoop">Indica si el audio se reproduce en bucle<c>true</c> En bucle.</param>
	public void playMusic(string musicKey, bool canLoop = true) { 
		if (musicResources.ContainsKey(musicKey)) {
			AudioClip soundClip = musicResources [musicKey];
			Play (soundClip, canLoop);
		}
	}

	/// <summary>
	/// Modifica el valor del volumen en el audioSource
	/// </summary>
	/// <param name="volume">Volume.</param>
	public void SetVolume(float volume) {
		audioSource.volume = volume;
	}
}