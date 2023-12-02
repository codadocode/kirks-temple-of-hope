using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private void Update()
    {
        Vector3 nextPosition = this.transform.position;
        nextPosition.y += this.moveSpeed * Time.deltaTime;
        this.transform.position = nextPosition;
    }
}
