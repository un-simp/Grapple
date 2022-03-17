using UnityEngine;
using UnityEngine.SceneManagement;
using Barji.Services;

namespace Barji
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField] private new string name;

        public void LoadScenes()
        {
            GameplayManager.singleton = null;
            SceneManager.LoadSceneAsync(name);
        }
    }
}