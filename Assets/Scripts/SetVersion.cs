using UnityEngine;
using TMPro;

namespace Barji
{
    [ExecuteAlways]
    public class SetVersion : MonoBehaviour
    {
        void Awake() => GetComponent<TMP_Text>().text = Application.version;
    }
}
