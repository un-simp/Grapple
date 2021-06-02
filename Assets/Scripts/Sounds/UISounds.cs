using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Wildflare.Audio;
using Wildflare.Services;

namespace Wildflare.Sounds
{
    [RequireComponent(typeof(Animator))]
    public class UISounds : MonoBehaviour {
        private AudioSource[] sources = new AudioSource[2];
        [SerializeField] private AudioClip hover;
        [SerializeField] private AudioClip select;
        [SerializeField] private AudioMixerGroup mixerGroup;
        
        private float volume = 0;

        void Awake() {
            sources[0] = gameObject.AddComponent<AudioSource>();
            sources[1] = gameObject.AddComponent<AudioSource>();
            sources[0].clip = hover;
            sources[1].clip = select;

            foreach (var source in sources) {
                source.playOnAwake = false;
                source.outputAudioMixerGroup = mixerGroup;
                source.volume = 0.1f;
                AudioManager.sources.Add(source);
            }
        }

        public void PlayHover() 
        {
            sources[0].Play();
        }
        
        public void PlaySelect() 
        {
            sources[1].Play();
        }
        
        public void SliderSound(float _newVol) {
            if (_newVol > volume + 0.1f || _newVol < volume - 0.1f) {
                volume = _newVol;
                sources[0].Play();
            }
        }
    }
}
