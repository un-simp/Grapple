using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wildflare
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField] private new string name;

        public void LoadScenes()
        {
            SceneManager.LoadSceneAsync(name);
        }
    }
}