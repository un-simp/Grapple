using UnityEngine;
using UnityEngine.SceneManagement;

namespace Barji
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