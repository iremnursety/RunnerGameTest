using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PaintController : MonoBehaviour
    {
        [SerializeField] private Color red = Color.red;
        [SerializeField] private Color blue = Color.blue;
        [SerializeField] private Color yellow = Color.yellow;

        [SerializeField] private Color newColor;
        [SerializeField] private float brushSize;

        [SerializeField] private Scrollbar brushScroll;

        private void Awake()
        {
            newColor = yellow;
            brushSize = 1;
        }

        public void SwitchToRed()
        {
            newColor = red;
        }

        public void SwitchToBlue()
        {
            newColor = blue;
        }

        public void SwitchToYellow()
        {
            newColor = yellow;
        }

        public void ChangeBrushSize()
        {
            brushSize += brushScroll.value;
        }

        private Color NewColor
        {
            get => newColor;
            set => newColor = value;
        }
    }
}