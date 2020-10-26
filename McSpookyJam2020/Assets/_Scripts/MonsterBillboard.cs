using System;
using UnityEngine;

public class MonsterBillboard : MonoBehaviour
{
    private Transform target = null;

    private void Awake()
    {
        target = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(target.position, Vector3.up);
    }
}