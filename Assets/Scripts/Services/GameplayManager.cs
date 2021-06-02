using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wildflare.Services
{
    public class GameplayManager : MonoBehaviour
    {
        void Start() {
            Application.targetFrameRate = 240;
        }
    }
}
