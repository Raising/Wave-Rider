using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class AdsController : Singleton<AdsController> {

    [SerializeField] private const int LEVELS_PER_AD = 2;
    [SerializeField] private const int PLAYS_PER_AD = 8;

    [SerializeField] private int currentPlayedLevels = 0;
    [SerializeField] private int currentPlays = 0;
    [SerializeField] private string lastSceneName;
    [SerializeField] private string currentSceneName;

#if UNITY_ADS
    public void ShowAd() {
        if (Advertisement.IsReady()) {
            Advertisement.Show();
        }
    }
#endif

    public void CheckAdCondition(string sceneName) {
        if (SceneController.Instance.IsSceneLevel()) {
            if (sceneName == AdsController.Instance.lastSceneName) {
                AdsController.Instance.currentPlays++;
            } else {
                AdsController.Instance.lastSceneName = sceneName;
                AdsController.Instance.currentPlayedLevels++;
                AdsController.Instance.currentPlays++;
            }

            if(AdsController.Instance.currentPlayedLevels >= LEVELS_PER_AD || AdsController.Instance.currentPlays >= PLAYS_PER_AD) {
                AdsController.Instance.ShowAd();
            }
        }
    }
}