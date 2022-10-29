using Managers;
using TMPro;
using UnityEngine;

namespace UI
{

    public class TextRegisterer : MonoBehaviour
    {
        [SerializeField] private int index;

        private void Start()
        {
            LeaderboardManager.Instance.TextRegister(index, gameObject.GetComponent<TextMeshProUGUI>());
        }
    }
}
