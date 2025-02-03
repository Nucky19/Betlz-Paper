using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    public float inputHorizontal;
    public bool jump;
    public bool firstTransformation;

    void HandleInput(){
        inputHorizontal=Input.GetAxisRaw("Horizontal");
        jump=Input.GetButtonDown("Jump");
        firstTransformation=Input.GetKeyDown("z");  
    }
    
    void Update(){
        HandleInput();
    }

}
