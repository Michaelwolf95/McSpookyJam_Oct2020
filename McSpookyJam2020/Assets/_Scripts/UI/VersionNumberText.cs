using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionNumberText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpText = null;
    void Start()
    {
        string versionNumber = Application.version;
#if DEVELOPMENT_BUILD
        versionNumber += "-DEBUG";
#endif
        tmpText.text = versionNumber;
    }
}
