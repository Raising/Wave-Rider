using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {  
    
    public static bool CheckUserInput() {
        //Check if we are running either in the Unity editor or in a standalone build.
        #if UNITY_STANDALONE || UNITY_WEBPLAYER
        if (Input.GetButtonDown("Fire1")) {
            return true;
        } else {
            return false;
        }

        //Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
        #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            return true;
        } else {
            return false;
        }

        #endif //End of mobile platform dependendent compilation section started above with #elif

        return false;
    }

    public static Vector3 GetUserInputPosition() {
        Vector3 inputPosition = new Vector3();

    #if UNITY_STANDALONE || UNITY_WEBPLAYER
        inputPosition = Input.mousePosition;

#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        inputPosition = Input.GetTouch(0).position;

#endif //End of mobile platform dependendent compilation section started above with #elif

        return inputPosition;



    }
}