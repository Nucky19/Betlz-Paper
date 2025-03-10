using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CheckPoint : MonoBehaviour
{
    public static event Action<Vector3> OnCheckPoint;
    private Vector3 _checkpointPosition;
    void Start(){
        _checkpointPosition = transform.position;
    }

    public void Interact(){
        OnCheckPoint(_checkpointPosition);
    }
}
