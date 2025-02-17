using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action<int> OnScreen;
    [SerializeField] int currentScreen;

    void Start(){
        OnScreen(1);
    }
    
}
