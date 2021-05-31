using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wildflare.Audio {
    public sealed class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        void Awake()
        {
            instance = this;
        }
    }
}
