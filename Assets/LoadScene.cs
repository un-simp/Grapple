using UnityEngine.SceneManagement;
using UnityEngine;

namespace Wildflare
{
    public class LoadScene : MonoBehaviour {
        [SerializeField] private string name;
        public void LoadScenes() {
            SceneManager.LoadSceneAsync(name);
        }
    }
}
