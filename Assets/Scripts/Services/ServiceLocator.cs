using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wildflare.Audio;

public static class ServiceLocator
{
    public static AudioManager audioManager => AudioManager.instance;
}
