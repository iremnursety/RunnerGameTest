using TMPro;
using UnityEngine;

namespace UI
{
    public class HitCountController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private Animation textAnimation;

        private void OnValidate()
        {
            textMesh = GetComponent<TextMeshProUGUI>();
            textAnimation = GetComponent<Animation>();
        }

        public string UpdateCount
        {
            get => textMesh.text;
            set
            {
                textMesh.text = "Hit Count: " + value;
                textAnimation.Play();
            }
        }
    }
}