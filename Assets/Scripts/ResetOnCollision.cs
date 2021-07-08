using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wildflare.Services;

namespace Wildflare
{
    public class ResetOnCollision : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            print('e');
            if (other.gameObject.tag != "Player") return;
            print("p");
            GameplayManager.singleton.RestartLevel();
        }
    }
}
