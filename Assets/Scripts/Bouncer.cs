using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Barji.UI.Settings;

namespace Barji
{
    public class Bouncer : MonoBehaviour
    {
        [SerializeField] private float bounceForce = 100;
        [SerializeField] private AudioClip sound;
        private void OnTriggerEnter(Collider other)
        {
            Rigidbody rb = other.transform.GetComponent<Rigidbody>();
            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, -rb.velocity.y, rb.velocity.z);
                AudioSource.PlayClipAtPoint(sound, transform.position, SettingsManager.GetSFXVolume());
            }
            
        }
    }
}
