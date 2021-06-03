using Mirror;
using UnityEngine;

namespace Wildflare.Networking
{
    public class PlayerNetworkManager : MonoBehaviour
    {
        public bool isNetworked;

        [SerializeField] private GameObject[] networkedObjects;
        [SerializeField] private NetworkBehaviour[] networkedScripts;
        [SerializeField] private NetworkIdentity[] networkIdentites;

        private void Awake()
        {
            if (isNetworked) return;
            foreach (var obj in networkedObjects)
                obj.SetActive(false);
            foreach (var behaviour in networkedScripts)
                Destroy(behaviour);
            foreach (var id in networkIdentites)
                Destroy(id);
        }
    }
}