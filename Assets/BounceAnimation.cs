using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BounceAnimation : MonoBehaviour
{   
    [SerializeField] private ParticleSystem bounceParticles; 
    [SerializeField] private AudioClip bounceSFX; 
    public void Bounce()
    {
        transform.DOKill(true);
        transform.DOPunchPosition(Vector3.down, .5f);
        transform.DOPunchScale(Vector3.one, .5f);
        Instantiate(bounceParticles, transform.position + transform.up, Quaternion.identity);
        GetComponent<AudioSource>().PlayOneShot(bounceSFX);
    }
}
