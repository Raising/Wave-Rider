using UnityEngine;
using UnityEngine.Advertisements;

public class AdsController : MonoBehaviour {
    public void ShowAd() {
        if (Advertisement.IsReady()) {
            Advertisement.Show();
        }
    }
}