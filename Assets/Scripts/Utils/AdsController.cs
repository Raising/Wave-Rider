using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class AdsController : MonoBehaviour {
    public static AdsController Instance = null;
    [SerializeField] private const int LEVELS_PER_AD = 2;
    [SerializeField] private const int PLAYS_PER_AD = 8;

    [SerializeField] private int currentPlayedLevels = 0;
    [SerializeField] private int currentPlays = 0;
    [SerializeField] private string lastSceneName;
    [SerializeField] private string currentSceneName;

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
    }

#if UNITY_ADS
    public void ShowAd() {
        if (Advertisement.IsReady()) {
            Advertisement.Show();
        }
    }


    public void CheckAdCondition(string sceneName) {
        if (SceneController.Instance.IsSceneLevel()) {
            if (sceneName == lastSceneName) {
                currentPlays++;
            } else {
                lastSceneName = sceneName;
                currentPlayedLevels++;
                currentPlays++;
            }

            if(currentPlayedLevels >= LEVELS_PER_AD || currentPlays >= PLAYS_PER_AD) {
                ShowAd();
            }
        }
    }
#endif
}