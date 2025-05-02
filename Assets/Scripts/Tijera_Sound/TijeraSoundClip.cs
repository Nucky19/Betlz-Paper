using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TijeraSoundClip : MonoBehaviour
{
   public AudioSource audioTijera;


   void Awake()
   {
    audioTijera = GetComponent<AudioSource>();
   }

    public void ReproducirSonidoTijera()
    {
        audioTijera.Play();
    }
}
