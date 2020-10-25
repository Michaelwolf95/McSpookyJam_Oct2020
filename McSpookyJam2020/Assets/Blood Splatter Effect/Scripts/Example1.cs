using UnityEngine;
using System.Collections;

public class Example1 : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (BloodSplatterEffect.instance != null)
            {
                BloodSplatterEffect.instance.BloodSplatter();
            }
        }
    }
}
