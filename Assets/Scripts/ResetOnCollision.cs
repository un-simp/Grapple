using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Barji.Services;

namespace Barji
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
