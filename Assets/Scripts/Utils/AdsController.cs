using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class AdsController : MonoBehaviour {
    #if UNITY_ADS
    public void ShowAd() {
        if (Advertisement.IsReady()) {
            Advertisement.Show();
        }
    }
    #endif
}