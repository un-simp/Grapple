using UnityEngine;

namespace Wildflare.Services
{
    public class GameplayManager : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 240;
        }
    }
}