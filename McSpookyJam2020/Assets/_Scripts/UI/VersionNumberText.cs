using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionNumberText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpText = null;
    void Start()
    {
	    string versionNumber = "";
#if UNITY_STANDALONE_OSX
	    versionNumber += "Mac_";
#endif
#if UNITY_STANDALONE_WIN
	    versionNumber += "Win_";
#endif
#if UNITY_STANDALONE_LINUX
	    versionNumber += "Lin_";
#endif
        versionNumber = Application.version;
#if DEVELOPMENT_BUILD
        versionNumber += "-DEBUG";
#endif

        tmpText.text = versionNumber;
    }
}
