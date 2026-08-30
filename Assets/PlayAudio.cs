using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField] AudioSource source ;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
     source.Play();   
    }
}
