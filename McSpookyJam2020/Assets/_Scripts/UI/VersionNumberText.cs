using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionNumberText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpText = null;
    void Start()
    {
        tmpText.text = Application.version;
    }
}
